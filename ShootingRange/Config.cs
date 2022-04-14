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
        [Description("Determines whether the default range will be used. If false, one of the below coordinates will be chosen randomly each round")]
        public bool UseDefaultRange { get; set; } = true;
        [Description("Determines if a collider will be used to prevent players from going outside the bounds of the range")]
        public bool UseCollider { get; set; } = true;
        [Description("Determines whether the \".range\" permission is required to use the .range command (this will not affect automatically teleported players)")]
        public bool RequirePermission { get; set; } = false; 
        [Description("List of alternative range locations. The \"w\" will determine the radius of the sphere (cube) that forms the boundaries  (note: targets will not be spawned at this location")]
        public List<Vector4> OtherRangeLocations { get; set; } = new List<Vector4>()
        {
            {
                Vector4.one
            }
        };
        public List<ItemType> RangerInventory = new List<ItemType>()
        {
            ItemType.GunAK,
            ItemType.GunCOM18,
            ItemType.GunCrossvec,
            ItemType.GunE11SR,
            ItemType.GunFSP9,
            ItemType.GunLogicer,
            ItemType.GunRevolver,
            ItemType.GunShotgun
        };

        [Description("Player broadcast that appears when a player dies or joins as a spectator (This will not show if force_spectators is true)")]
        public PlayerBroadcast DeathBroadcast { get; set; } = new PlayerBroadcast()
        {
            Duration = 5,
            Content = "Type .range to join the shooting range",
            Show = true
        };

        [Description("Player broadcast that appears when a player enters the shooting range")]
        public PlayerBroadcast RangeGreeting { get; set; } = new PlayerBroadcast()
        {
            Duration = 5,
            Content = "Welcome to the shooting range! Type .spectate into your console to return (you will be returned automatically for respawns, but you may be affected by an afk detecter)",
            Show = true
        };

        [Description("Player broadcast that appears when everyone in the range is returned to spectator to spawn")]
        public PlayerBroadcast RespawnBroadcast { get; set; } = new PlayerBroadcast
        {
            Duration = 5,
            Content = "You will be respawning soon!",
            Show = true
        };

        [Description("Distance each set of targets are away from each other (default is 16)")]
        public int RelativeTargetDistance { get; set; } = 16;

        [Description("Distance all targets are away from the shooting zone (default is 7)")]
        public int AbsoluteTargetDistance { get; set; } = 7;
    }
}
