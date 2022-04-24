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
 
//config data here
