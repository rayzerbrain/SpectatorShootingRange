using MEC;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using Exiled.API.Features.Items;
using Exiled.API.Enums;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShootingRange.EventHandlers 
{
    public class EventHandlers
    {
        const int bigXBound = 237;
        const int smallXBound = 202;
        const int bigZBound = -28;
        const int smallZBound = -54;
        const int YBound = 1000;

        public bool IsOnRange(Player player)
        {
            if (player.Position.x < bigXBound
            && player.Position.x > smallXBound
            && player.Position.z > smallZBound
            && player.Position.z < bigZBound
            && player.Position.y < YBound)
            {
                return true;
            }
            return false;
        }
        public void BringRangers()
        {
            foreach (Player player in Player.List)
            {
                if (IsOnRange(player))
                {
                    player.Broadcast(5, ShootingRange.Instance.Config.Returning_for_spawn_message);
                    player.ClearInventory(true);
                    player.Role = RoleType.Spectator;
                }
            }
        }
        public static void SpawnPrefab(float x, float y, float z, float rotation, int prefabId)
        {
            GameObject obj = Object.Instantiate(NetworkManager.singleton.spawnPrefabs[prefabId]);
            obj.transform.position = new Vector3(x, y, z);
            obj.transform.rotation = new Quaternion(0, rotation, 0, 1);
            NetworkServer.Spawn(obj);
        }
        
        public void OnRoundStarted()
        {
            Timing.RunCoroutine(WaitForRespawnCoroutine());
            Timing.RunCoroutine(SpawnTargetsCoroutine());
            Timing.RunCoroutine(CheckBoundsCoroutine());
        }
        public void OnJoined(JoinedEventArgs ev)
        {
            if (Round.IsStarted)
            {
                Timing.RunCoroutine(DyingCoroutine(ev.Player));
            }
        }
        public void OnDied(DiedEventArgs ev)
        {
            Timing.RunCoroutine(DyingCoroutine(ev.Target));
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            if (IsOnRange(ev.Shooter))
            {
                Firearm weapon = (Firearm)ev.Shooter.CurrentItem;
                weapon.Ammo = 2;
            }
        }

        public IEnumerator<float> DyingCoroutine(Player deadGuy)
        {
            yield return Timing.WaitForSeconds(5f);
            deadGuy.Broadcast((ushort)ShootingRange.Instance.Config.Death_greeting_time, ShootingRange.Instance.Config.Death_greeting);
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
                foreach (Player player in Player.List)
                {
                    const int bufferSize = 5;

                    if ((player.Position.z < smallZBound && player.Position.z > smallZBound - bufferSize
                        || player.Position.x < smallXBound && player.Position.x > smallXBound - bufferSize 
                        || player.Position.z > bigZBound && player.Position.z< bigZBound + bufferSize
                        || player.Position.x > bigXBound && player.Position.x< bigXBound + bufferSize
                        || player.Position.y > YBound)
                        && player.Position.x > smallXBound - bufferSize &&player.Position.x < bigXBound +bufferSize
                        && player.Position.z < bigZBound + bufferSize &&player.Position.z > smallZBound - bufferSize
                        && player.Zone.Equals(ZoneType.Surface))
                    {
                        if(ShootingRange.Instance.Config.RA_bounds_immunity&&player.RemoteAdminAccess)
                        {
                            continue;
                        }
                        player.ClearInventory(true);
                        player.Kill();
                        player.Broadcast(3, "You went out of bounds");
                    }
                }
                yield return Timing.WaitForSeconds((float)0.8);
                if (!Round.IsStarted)
                {
                    break;
                }
            }
        }
        public IEnumerator<float> SpawnTargetsCoroutine()
        {
            
            yield return Timing.WaitForSeconds(4f);
            int distance = ShootingRange.Instance.Config.Absolute_target_distance;
            int distanceZbetween = ShootingRange.Instance.Config.Relative_target_distance;
            const float distanceXBetweeen = 3.5f;
            const int startTargetsX = 235;
            const float height = 996.63f;
            for (var i=0;i<3;i++)
            {
                SpawnPrefab(startTargetsX - i*distanceXBetweeen, height, -53 - distance - distanceZbetween*i, 1, 21);
                SpawnPrefab(startTargetsX - 1 - distanceXBetweeen*3 - distanceXBetweeen*i, height, -53 - distance - distanceZbetween*i, 1,22);
                SpawnPrefab(startTargetsX - 2 - distanceXBetweeen*6 - distanceXBetweeen*i, height, -53 - distance - distanceZbetween*i, 1, 23);
                SpawnPrefab(startTargetsX+3, height, -30 - 4.0f * i, -1f, 2);
                SpawnPrefab(startTargetsX+3, height, -53 + 4.0f * i, -1f, 2);
            }
            SpawnPrefab(startTargetsX + 3, height, -41.5f, -1f, 2);
            SpawnPrefab(204f, height+1.5f, -36, 3, 2);
            SpawnPrefab(203.5f, height + 0.2f, -36, 3, 2);

            //0 rotation = towards gate a
            //+1 rotation to turn clockwise 90
            //for targets at least
            //x smaller as going to A
            //z bigger going to escape

        }
    }
}
