using System.Collections.Generic;
using System.ComponentModel;

using Exiled.API.Interfaces;

using UnityEngine;

using PlayerBroadcast = Exiled.API.Features.Broadcast;

namespace ShootingRange
{
    public class Config : IConfig
    {
        [Description("Indicates if the plugin is enabled or not.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Determines whether spectators will automatically be teleported to the shooting range upon death")]
        public bool ForceSpectators { get; set; } = true;
        [Description("Determines if primitives will be used to prevent players from going outside the bounds of the range")]
        public bool UsePrimitives { get; set; } = true;
        [Description("Determines whether the \".range\" permission is required to use the .range command (this will not affect automatically teleported players)")]
        public bool RequirePermission { get; set; } = false;
        [Description("Alternative range location. The \"w\" will determine the radius of the sphere (cube) that forms the boundaries.")]
        public Vector4 RangeLocation { get; set; } = default;
        [Description("Determines if the above location will be used or not")]
        public bool UseRangeLocation { get; set; } = false;
        [Description("The items one will spawn with on the range (may be ItemType or the name of a CustomItem")]
        public List<string> RangerInventory { get; set; } = new()
        {
            "GunAK",
            "GunCOM18",
            "GunCrossvec",
            "GunE11SR",
            "GunFSP9",
            "GunLogicer",
            "GunRevolver",
            "GunShotgun",
        };
        [Description("Player broadcast that appears when a player dies or joins as a spectator (This will not show if force_spectators is true)")]
        public PlayerBroadcast DeathBroadcast { get; set; } = new()
        {
            Duration = 5,
            Content = "Type .range to join the shooting range",
            Show = true
        };
        [Description("Player broadcast that appears when a player enters the shooting range")]
        public PlayerBroadcast RangeGreeting { get; set; } = new()
        {
            Duration = 5,
            Content = "Welcome to the shooting range! Type .spectate into your console to return (you will be returned automatically for respawns, but you may be affected by an afk detecter)",
            Show = true
        };
        [Description("Player broadcast that appears when everyone in the range is returned to spectator to spawn")]
        public PlayerBroadcast RespawnBroadcast { get; set; } = new()
        {
            Duration = 5,
            Content = "You will be respawning soon!",
            Show = true
        };
        [Description("Distance each set of targets are away from each other (default is 24)")]
        public int RelativeTargetDistance { get; set; } = 24;
        [Description("Distance all targets are away from the shooting zone (default is 7)")]
        public int AbsoluteTargetDistance { get; set; } = 7;
    }
}
