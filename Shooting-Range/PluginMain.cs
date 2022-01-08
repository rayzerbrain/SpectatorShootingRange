using Exiled.API.Features;
using Exiled.Events;
using Exiled;
using Exiled.API.Enums;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using System;

namespace ShootingRange
{
    public class PluginMain : Plugin<Config>
    {
        private static PluginMain Singleton;
        public static PluginMain Instance => Singleton;

        public override string Author => "rayzer";
        public override string Name => "Spectator Shooting Range";
        public override Version Version => new Version(2, 0, 0);

        public EventHandlers EventHandler;


        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        public override void OnEnabled()
        {
            Singleton = this;
            
            EventHandler = new EventHandlers();
            Player.Verified += EventHandler.OnVerified;
            Player.Died += EventHandler.OnDied;
            Player.Shooting += EventHandler.OnShooting;
            Player.ChangingMuteStatus += EventHandler.OnChangingMuteStatus;
            Player.DroppingItem += EventHandler.OnDroppingItem;
            Server.RoundStarted += EventHandler.OnRoundStarted;
            Player.Shot += EventHandler.OnShot;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            
            Server.RoundStarted -= EventHandler.OnRoundStarted;
            Player.Verified -= EventHandler.OnVerified;
            Player.Died -= EventHandler.OnDied;
            Player.Shooting -= EventHandler.OnShooting;
            Player.ChangingMuteStatus -= EventHandler.OnChangingMuteStatus;
            Player.DroppingItem -= EventHandler.OnDroppingItem;
            Player.Shot -= EventHandler.OnShot;
            EventHandler = null;
            
            Singleton = null;
            base.OnDisabled();
        }

    }
}
