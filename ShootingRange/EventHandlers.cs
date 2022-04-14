using System.Collections.Generic;

using UnityEngine;

using MEC;

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Features.Items;

using ShootingRange.API;

namespace ShootingRange 
{
    public class EventHandlers
    {
        private PluginMain _plugin;
        public List<Player> FreshlyDead { get; private set; } = new List<Player>();

        public EventHandlers(PluginMain plugin)
        {
            _plugin = plugin;
        }
        
        public void OnRoundStarted()
        {
            if (!PluginMain.Instance.Config.UseDefaultRange)
            {
                int randNum = Random.Range(0, PluginMain.Instance.Config.OtherRangeLocations.Count - 1);
                _plugin.ActiveRange = new SpectatorRange(PluginMain.Instance.Config.OtherRangeLocations[randNum]);
            }
            else
            {
                _plugin.ActiveRange = new SpectatorRange();
                _plugin.ActiveRange.SpawnTargets();
            }

            if (_plugin.Config.UseCollider)
                _plugin.ActiveRange.SpawnCollider();

            Timing.RunCoroutine(WaitForRespawnCoroutine());
        }
        public void OnVerified(VerifiedEventArgs ev) => Timing.CallDelayed(10f, () => ev.Player.Broadcast(PluginMain.Instance.Config.DeathBroadcast));
        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer.Role.Type == RoleType.Scp049)
            {
                FreshlyDead.Add(ev.Target);
                Timing.CallDelayed(10f, () => FreshlyDead.Remove(ev.Target));
            }
               
        }
        public void OnLeft(LeftEventArgs ev)
        {
            if (_plugin.ActiveRange.HasPlayer(ev.Player))
                ev.Player.ClearInventory(true);
        }
        public void OnShooting(ShootingEventArgs ev)
        {
            if (_plugin.ActiveRange.HasPlayer(ev.Shooter))
            {
                Firearm gun = (Firearm)ev.Shooter.CurrentItem;
                gun.Ammo = gun.MaxAmmo;
            }
        }
        public void OnDroppingItem(DroppingItemEventArgs ev) => ev.IsAllowed = !_plugin.ActiveRange.HasPlayer(ev.Player);
        public IEnumerator<float> WaitForRespawnCoroutine()
        {
            for (;;)
            {
                if (Respawn.TimeUntilRespawn < 20)
                    _plugin.ActiveRange.RemovePlayers();

                yield return Timing.WaitForSeconds(15f);
                
                if (!Round.IsStarted)
                    break;
            }
        }
    }
}
