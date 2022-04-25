# Spectator-Shooting-Range
An SCP:SL Exiled 5.0 plugin implementing a shooting range for spectators. The range is located outside of the wall perpendicular to the escape tunnel, that is, the wall the MTF helicopter flies over. This location can be changed inside of the configuration if deemed necessary

### To install
If you are unfamiliar with installing Exiled plugins, do the following: on this page navigate to the releases link on the right side of the screen and download the latest .dll file. Then, Put this file in your Exiled\Plugins file with any other plugins you might have. 

# In-depth information
The shooting range can be accessed whenever someone that is spectating uses the .range command in their console. This teleports them as a tutorial class to the range and gives them one of each weapon (excluding the com-15, as it causes the inventory limit to exceed 8). They are constricted to within a fairly small boundary to practice shooting targets located at varying distances from the range starting area.

### Commands to utilize-

.range (client console command)- as a spectator anyone can use this command to enter the shooting range

.spectate (client console command)- anyone in the shooting range can use this command to return to spectator (they will be returned automatically for respawn events)


### Configuration
The configurations that will appear in your [port number]-config.yml file allows you to change the following (values already present are default values upon implementation)
:
 
|Config Name|Data Type|Description|Default Value|
|-----------|---------|-----------|-------------|
|is_enabled|bool|Determines whether the plugin is enabled or not|true|
|force_specators|bool|Determines whether spectators will be AUTOMATICALLY transferred to the shooting range upon death|true|
|use_primitives|bool|Determines if primitives will be used to form the boundaries of the shooting range (recommended)|true|
|require_permission|bool|Determines if the ".range" permission is required to use the .range command (this will not affect automatically teleported players)|false|
|range_location|Vector4|Location of a possible non default shooting range. the "w" of the vector will determine the radius of the cube that forms the boundaries|Vector4.zero|
|use_range_location|bool|Determines whether the alternative range location will be used or not. The default range location will be used if this is set to false|false|
|death_broadcast|Broadcast|The broadcast that will appear for a player when they die or join as a spectator|N/A|
|range_greeting|Broadcast|The broadcast for when players enter the shooting range|N/A|
|respawn_broadcast|Broadcast|The broadcast for when players are forced off the shooting range because they are respawning|N/A|
|relative_target_distance|int|Distance (meters) that shooting targets will be from <b>each other</b>|24|
|absolute_target_distance|int|Distance (meters) that shooting targets will be from <b>the range</b>|7|

