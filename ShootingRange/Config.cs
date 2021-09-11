using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ShootingRange
{
    public class Config : IConfig
    {
        [Description("Indicates if the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("Player broadcast that appears when a player dies or joins as a spectator")]
        public string Death_greeting { get; set; } = "To join the shooting range, type .range into your console(`)!";

        [Description("Time in seconds that the death_greeting is broadcasted for (default is 5)")]
        public int Death_greeting_time { get; set; } = 5;

        [Description("Player broadcast that appears when a player enters the shooting range")]
        public string Range_greeting { get; set; } = "Welcome to the shooting range! Type .spectate into your console to return to spectator";

        [Description("Time in seconds that Range_greeting is broadcasted for (default is 5)")]
        public int Range_greeting_time { get; set; } = 5;

        [Description("Player broadcast that appears when everyone in the range is returned to spectator to spawn")]
        public string Returning_for_spawn_message { get; set; } = "You will be respawning soon!";

        [Description("Distance each set of targets are away from each other (default is 16)")]
        public int Relative_target_distance { get; set; } = 16;

        [Description("Distance all targets are away from the shooting zone (default is 7)")]
        public int Absolute_target_distance { get; set; } = 7;

        [Description("Determines whether players with Remote Admin access are immune to the shooting range bounds (default is true)")]
        public bool RA_bounds_immunity { get; set; } = true;

        
    }
}
