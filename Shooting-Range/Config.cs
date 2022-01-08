using Exiled.API.Interfaces;
using Broadcast = Exiled.API.Features.Broadcast;
using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;

namespace ShootingRange
{
    public class Config : IConfig
    {
        
        [Description("Indicates if the plugin is enabled or not. THIS IS FOR SPAWNING BENCHES, NOT ACTUAL SHOOTING RANGE")]
        public bool IsEnabled { get; set; } = true;
        [Description("Determines whether the default range will be used. If false, one of the below random coordinates will be chosen each round")]
        public bool UseDefaultRange { get; set; } = true;
        [Description("List of alternative range locations. The \"w\" will determine the radius of the sphere (rectangle) that forms the boundaries  (one has been provided as an example, please don't actually use it)")]
        public List<Vector4> OtherRangeLocations { get; set; } = new List<Vector4>()
        {
            {
                Vector4.one
            }
        };


        [Description("Player broadcast that appears when a player dies or joins as a spectator")]
        public Exiled.API.Features.Broadcast DeathBroadcast { get; set; } = new Exiled.API.Features.Broadcast()
        {
            Duration = 5,
            Content = "Type .range to join the shooting range",
            Show = true
        };

        [Description("Player broadcast that appears when a player enters the shooting range")]
        public Exiled.API.Features.Broadcast RangeGreeting { get; set; } = new Exiled.API.Features.Broadcast()
        {
            Duration = 5,
            Content = "Welcome to the shooting range! Type .spectate into your console to return (you will be returned automatically for respawns, but you may be affected by an afk detecter)",
            Show = true
        };

        [Description("Player broadcast that appears when everyone in the range is returned to spectator to spawn")]
        public Exiled.API.Features.Broadcast RespawnBroadcast { get; set; } = new Exiled.API.Features.Broadcast
        {
            Duration = 5,
            Content = "You will be respawning soon!",
            Show = true
        };

        [Description("Distance each set of targets are away from each other (default is 16)")]
        public int Relative_target_distance { get; set; } = 16;

        [Description("Distance all targets are away from the shooting zone (default is 7)")]
        public int Absolute_target_distance { get; set; } = 7;

        [Description("Determines whether players with Remote Admin access are immune to the shooting range bounds (default is true)")]
        public bool Ra_bounds_immunity { get; set; } = false;

        [Description("Determines whether players can talk in the shooting range (if enabled it is possible for them to hear and talk to living players on the other side of the wall, players with Remote Admin access are exempt from this)")]
        public bool Rangers_can_talk { get; set; } = true;
        
    }
}
