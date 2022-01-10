using MEC;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using Exiled.API.Features.Items;
using Exiled.API.Enums;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;
using ShootingRange.API;

namespace ShootingRange 
{
    public class EventHandlers
    {
        public RangeBoundaries curBounds = new RangeBoundaries();
        public List<Player> mutedPlayers = new List<Player>();
        public List<Player> freshlyDead = new List<Player>();
        public List<Player> rangerList = new List<Player>();
        
        public void BringRangers()
        {
            List<Player> toRemoveList = new List<Player>();
            foreach (Player plyr in rangerList)
            {
                toRemoveList.Add(plyr);
                plyr.Broadcast(PluginMain.Instance.Config.RespawnBroadcast);
                plyr.ClearInventory(true);
                plyr.SetRole(RoleType.Spectator);
            }
            foreach(Player plyr in toRemoveList)
            {
                rangerList.Remove(plyr);
            }
        }
        
        public void OnRoundStarted()
        {
            int randNum = Random.Range(0, PluginMain.Instance.Config.OtherRangeLocations.Count - 1);
            if (!PluginMain.Instance.Config.UseDefaultRange) curBounds = new RangeBoundaries(PluginMain.Instance.Config.OtherRangeLocations[randNum]);
            Timing.RunCoroutine(WaitForRespawnCoroutine());
            Timing.RunCoroutine(SpawnTargetsCoroutine());
            Timing.RunCoroutine(CheckBoundsCoroutine());
        }
        public void OnVerified(VerifiedEventArgs ev)
        {
            if (Round.IsStarted)
            {
                Timing.RunCoroutine(DyingCoroutine(ev.Player));
            }
        }
        public void OnDied(DiedEventArgs ev)
        {
            freshlyDead.Add(ev.Target);
            Timing.RunCoroutine(DyingCoroutine(ev.Target));
        }
        public void OnLeft(LeftEventArgs ev)
        {
            if (rangerList.Contains(ev.Player))
            {
                ev.Player.ClearInventory(true);
                rangerList.Remove(ev.Player);
            }
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            if (rangerList.Contains(ev.Shooter))
            {
                Firearm weapon = (Firearm)ev.Shooter.CurrentItem;
                weapon.Ammo = weapon.MaxAmmo;
            }
        }
        public void OnShot(ShotEventArgs ev)
        {
            if(rangerList.Contains(ev.Shooter) && ev.Target != null && !rangerList.Contains(ev.Target))
            {
                ev.CanHurt = false;
            }
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if(rangerList.Contains(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnChangingMuteStatus(ChangingMuteStatusEventArgs ev)
        {
            Timing.RunCoroutine(MuteChangeCoroutine());
            
            IEnumerator<float> MuteChangeCoroutine()
            {
                yield return Timing.WaitForSeconds(2f);
                if (ev.IsMuted)
                {
                    mutedPlayers.Add(ev.Player);
                }
                else if (!ev.IsMuted && !mutedPlayers.IsEmpty())
                {
                    foreach (Player player in mutedPlayers)
                    {
                        if (player.Equals(ev.Player))
                        {
                            mutedPlayers.Remove(player);
                        }
                    }
                }
            }
        }
        public IEnumerator<float> DyingCoroutine(Player deadGuy)
        {
            yield return Timing.WaitForSeconds(10f);
            freshlyDead.Remove(deadGuy);
            deadGuy.Broadcast(PluginMain.Instance.Config.DeathBroadcast);
        }
        public IEnumerator<float> WaitForRespawnCoroutine()
        {
            for (;;)
            {
                if (Respawn.TimeUntilRespawn < 20)
                {
                    BringRangers();
                }
                yield return Timing.WaitForSeconds(17f);
                if(!Round.IsStarted)
                {
                    break;
                }
            }
        }
        public IEnumerator<float> CheckBoundsCoroutine()
        {
            for (;;)
            {
                foreach (Player plyr in Player.List)
                {
                    if (!curBounds.IsOnRange(plyr) && rangerList.Contains(plyr))
                    {
                        if(PluginMain.Instance.Config.Ra_bounds_immunity&&plyr.RemoteAdminAccess)
                        {
                            continue;
                        }
                        plyr.ClearInventory();
                        plyr.SetRole(RoleType.Spectator);
                        plyr.Broadcast(3, "You went out of bounds");
                        rangerList.Remove(plyr);
                    }
                }
                yield return Timing.WaitForSeconds(0.8f);
                if (!Round.IsStarted)
                {
                    break;
                }
            }
        }
        public IEnumerator<float> SpawnTargetsCoroutine()
        {
            yield return Timing.WaitForSeconds(4f);
            int distance = PluginMain.Instance.Config.Absolute_target_distance;
            int distanceZbetween = PluginMain.Instance.Config.Relative_target_distance;

            const float distanceXBetweeen = 3.5f;
            const int startTargetsX = 235;
            const float height = 996.63f;
            Quaternion rot = new Quaternion(0, 1, 0, 1);

            for (var i=0;i<3;i++)
            {
                ShootingTarget.Spawn(ShootingTargetType.Sport, new Vector3(startTargetsX - i * distanceXBetweeen, height, -53 - distance - distanceZbetween * i), rot);
                ShootingTarget.Spawn(ShootingTargetType.ClassD, new Vector3(startTargetsX - 1 - distanceXBetweeen * 3 - distanceXBetweeen * i, height, -53 - distance - distanceZbetween * i), rot);
                ShootingTarget.Spawn(ShootingTargetType.Binary, new Vector3(startTargetsX - 2 - distanceXBetweeen * 6 - distanceXBetweeen * i, height, -53 - distance - distanceZbetween * i), rot);
            }

            //0 rotation = towards gate a
            //+1 rotation to turn clockwise 90
            //for targets at least
            //x smaller as going to A
            //z bigger going to escape
        
        }
    }
}
