# Spectator-Shooting-Range
An SCP:SL Exiled 3.0 plugin implementing a shooting range for spectators. The range is located outside of the wall perpendicular to the escape tunnel, that is, the wall the MTF helicopter flies over.

# To install
If you are unfamiliar with installing Exiled plugins, do the following: on this page navigate to the releases link on the right side of the screen and download the latest .dll file. Then, Put this file in your Exiled\Plugins file with any other plugins you might have. 

# In-depth information
The shooting range can be accessed whenever someone is spectating using the .range command in their console. This teleports them as a tutorial class to the range and gives them one of each weapon (excluding the com-15, as it causes the inventory limit to exceed 8). They are constricted to within a fairly small boundary to practice shooting targets located at varying distances from the range starting area.

Commands to utilize-

.range (client console command)- as a spectator anyone can use this command to enter the shooting range

.spectate (client console command)- anyone in the shooting range can use this command to return to spectator (they will be returned automatically for respawn events)

(not yet implemented) ToggleRange (server console command)- a command for those with access to the server console to easily disable the shooting range without altering the config file

# Disclaimer
This plugin is not intended to affect gameplay and is only intending to provide spectators with an alternate time-killer while waiting to spawn. However, certain aspects of the plugin may affect natural gameplay. This includes: hearing spectators shooting guns near the escape on surface, and being able to faintly hear and talk to said spectators (if the configuration allows)

# Configuration
The configurations that will appear in your [port number]-config.yml file allows you to change the following:
 
Enable or disable the plugin 

  is_enabled: true

Player broadcast that appears when a player dies or joins as a spectator
  
  death_greeting: To join the shooting range, type .range into your console(`)!

Time in seconds that the death_greeting is broadcasted for (default is 5)
  
  death_greeting_time: 5

Player broadcast that appears when a player enters the shooting range
  
  range_greeting: Welcome to the shooting range! Type .spectate into your console to return to spectator

Time in seconds that Range_greeting is broadcasted for (default is 5)
  
  range_greeting_time: 5

Player broadcast that appears when everyone in the range is returned to spectator to spawn
  
  returning_for_spawn_message: You will be respawning soon!

Distance each set of targets are away from each other (default is 16)
  
  relative_target_distance: 16

Distance all targets are away from the shooting zone (default is 7)
  
  absolute_target_distance: 7

Determines whether players with Remote Admin access are immune to the shooting range bounds (default is true)
  
  ra_bounds_immunity: true

Determines whether players can talk in the shooting range (if enabled it is possible for spectators in the range to hear and talk to living players on the other side of the wall)
  
  rangers_can_talk: true
