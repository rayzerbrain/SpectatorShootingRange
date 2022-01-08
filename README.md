# Spectator-Shooting-Range
An SCP:SL Exiled 3.0 (3.0 ONLY) plugin implementing a shooting range for spectators. The range is located outside of the wall perpendicular to the escape tunnel, that is, the wall the MTF helicopter flies over.

### To install
If you are unfamiliar with installing Exiled plugins, do the following: on this page navigate to the releases link on the right side of the screen and download the latest .dll file. Then, Put this file in your Exiled\Plugins file with any other plugins you might have. 

# In-depth information
The shooting range can be accessed whenever someone that is spectating uses the .range command in their console. This teleports them as a tutorial class to the range and gives them one of each weapon (excluding the com-15, as it causes the inventory limit to exceed 8). They are constricted to within a fairly small boundary to practice shooting targets located at varying distances from the range starting area.

Commands to utilize-

.range (client console command)- as a spectator anyone can use this command to enter the shooting range

.spectate (client console command)- anyone in the shooting range can use this command to return to spectator (they will be returned automatically for respawn events)

### Disclaimers
This plugin is not intended to affect gameplay and is only intending to provide spectators with an alternate time-killer while waiting to spawn. However, certain aspects of the plugin may affect natural gameplay. This includes: hearing spectators shooting guns near the escape on surface, and being able to faintly hear and talk to said spectators (if the configuration allows). In addition, make sure no other plugins you have installed conflict with this one, for example one that makes it difficult to use the tutorial class.

2.0 does not allow workbenches to spawn due to their functionality being removed lately or something. 

### Configuration
Make sure to keep the smae data type when changing the configuration, i.e. don't set is_enabled to yes. Set text values to nothing if you wish no message to be displayed for the specific event, and remember that players can remove targets that are too close to the shooting area.


The configurations that will appear in your [port number]-config.yml file allows you to do/change the following (values already present are default values upon implementation)
:
 
//config data here
