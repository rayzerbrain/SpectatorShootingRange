using System;

using Exiled.API.Features;

using ShootingRange.API;

using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace ShootingRange
{
    public class PluginMain : Plugin<Config>
    {
        public override string Author => "rayzer";
        public override string Name => "Spectator Shooting Range";
        public override Version Version => new(3, 0, 1);
        public static PluginMain Singleton { get; private set; }
        public EventHandlers EventHandler { get; private set; }
        public SpectatorRange ActiveRange { get; set; }

        public override void OnEnabled()
        {
            base.OnEnabled();
         
            Singleton = this;   
            EventHandler = new EventHandlers(this);
            RegisterEvents();
            Config.DeathBroadcast.Show = !Config.ForceSpectators;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
         
            UnregisterEvents();
            EventHandler = null;
            Singleton = null;
        }
        public void RegisterEvents()
        {
            Player.Verified += EventHandler.OnVerified;
            Player.Died += EventHandler.OnDied;
            Player.Shooting += EventHandler.OnShooting;
            Player.DroppingItem += EventHandler.OnDroppingItem;
            Server.RoundStarted += EventHandler.OnRoundStarted;
        }
        public void UnregisterEvents()
        {
            Server.RoundStarted -= EventHandler.OnRoundStarted;
            Player.Verified -= EventHandler.OnVerified;
            Player.Died -= EventHandler.OnDied;
            Player.Shooting -= EventHandler.OnShooting;
            Player.DroppingItem -= EventHandler.OnDroppingItem;
        }
    }
}
