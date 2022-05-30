using System.Collections.Generic;

using MEC;

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Features.Items;

using ShootingRange.API;
using System.Linq;

namespace ShootingRange 
{
    public class EventHandlers
    {
        private readonly PluginMain _plugin;
        public List<Player> FreshlyDead { get; } = new List<Player>();

        public EventHandlers(PluginMain plugin)
        {
            _plugin = plugin;
        }
        
        public void OnRoundStarted()
        {
            SpectatorRange range = _plugin.Config.UseRangeLocation ? new SpectatorRange(_plugin.Config.RangeLocation) : new SpectatorRange();
            range.SpawnTargets();
            range.SpawnBench();

            if (_plugin.Config.UsePrimitives)
                range.SpawnPrimitives();

            _plugin.ActiveRange = range;

            Timing.RunCoroutine(WaitForRespawnCoroutine());
        }
        public void OnVerified(VerifiedEventArgs ev)
        {
            Timing.CallDelayed(10f, () =>
            {
                if (ev.Player.IsDead)
                    ev.Player.Broadcast(PluginMain.Singleton.Config.DeathBroadcast);
            });
        }
            
        public void OnDied(DiedEventArgs ev) => Timing.RunCoroutine(OnDiedCoroutine(ev.Target, ev.Killer != null && ev.Killer.Role == RoleType.Scp049));
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
                {
                    foreach (Player plyr in Player.List.Where((plyr) => _plugin.ActiveRange.HasPlayer(plyr)))
                    {
                        _plugin.ActiveRange.RemovePlayer(plyr);
                        plyr.Broadcast(PluginMain.Singleton.Config.RespawnBroadcast, true);
                    }
                }

                yield return Timing.WaitForSeconds(15f);
                
                if (!Round.IsStarted)
                    break;
            }
        }
    }
}
