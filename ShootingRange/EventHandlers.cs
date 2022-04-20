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
        private readonly PluginMain _plugin;
        public List<Player> FreshlyDead { get; private set; } = new List<Player>();

        public EventHandlers(PluginMain plugin)
        {
            _plugin = plugin;
        }
        
        public void OnRoundStarted()
        {
            SpectatorRange range = _plugin.Config.RangeLocation == null ? new SpectatorRange() : new SpectatorRange(_plugin.Config.RangeLocation);
            range.SpawnTargets();

            if (_plugin.Config.UsePrimitives)
                range.SpawnPrimitives();

            _plugin.ActiveRange = range;

            Timing.RunCoroutine(WaitForRespawnCoroutine());
        }
        public void OnVerified(VerifiedEventArgs ev) => 
            Timing.CallDelayed(10f, () => ev.Player.Broadcast(PluginMain.Instance.Config.DeathBroadcast));
        public void OnDied(DiedEventArgs ev) => 
            Timing.RunCoroutine(OnDiedCoroutine(ev.Target, ev.Killer.Role.Type == RoleType.Scp049));
        private IEnumerator<float> OnDiedCoroutine(Player plyr, bool byDoctor)
        {
            if (byDoctor)
            {
                FreshlyDead.Add(plyr);
                yield return Timing.WaitForSeconds(10f);
                FreshlyDead.Remove(plyr);
            }

            if (_plugin.Config.ForceSpectators)
            {
                yield return Timing.WaitForSeconds(0.5f);
                _plugin.ActiveRange.TryAdmit(plyr);
            }
            else
            {
                yield return Timing.WaitForSeconds(5f);
                plyr.Broadcast(_plugin.Config.DeathBroadcast);
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
        public void OnDroppingItem(DroppingItemEventArgs ev) => 
            ev.IsAllowed = !_plugin.ActiveRange.HasPlayer(ev.Player);
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
