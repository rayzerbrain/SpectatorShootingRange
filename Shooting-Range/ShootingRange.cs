using Exiled.API.Features;
using Exiled.Events;
using Exiled;
using Exiled.API.Enums;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using ShootingRange.EventHandlers;

namespace ShootingRange
{
    public class ShootingRange : Plugin<Config>
    {
        private static readonly ShootingRange Singleton = new ShootingRange();
        public static ShootingRange Instance => Singleton;
        private ShootingRange()
        {
        }

        public EventHandlers.EventHandlers EventHandler;

        public override PluginPriority Priority { get; } = PluginPriority.High;

        public override void OnEnabled()
        {
            EventHandler = new EventHandlers.EventHandlers();
            Player.Joined += EventHandler.OnJoined;
            Player.Died += EventHandler.OnDied;
            Player.Shooting += EventHandler.OnShooting;
            Server.RoundStarted += EventHandler.OnRoundStarted;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            EventHandler = null;
            Player.Joined -= EventHandler.OnJoined;
            Player.Died -= EventHandler.OnDied;
            Player.Shooting -= EventHandler.OnShooting;
            Server.RoundStarted -= EventHandler.OnRoundStarted;

            base.OnDisabled();
        }
    }
}
