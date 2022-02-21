using CommandSystem;
using System;
using Exiled.API.Features;
using MEC;
using Exiled.API.Extensions;
using System.Collections.Generic;

namespace ShootingRange.Commands
{
    
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Spectate : ICommand
    {
        public string Command { get;} = "spectate";

        public string[] Aliases { get;} = Array.Empty<string>();

        public string Description { get;} = "returns you to spectator if you are a tutorial class";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            IEnumerator<float> SettingSpectatorCoroutine(Player spectatee)
            {
                yield return Timing.WaitForSeconds(.5f);
                PluginMain.Instance.EventHandler.rangerList.Remove(spectatee);
                spectatee.SetRole(RoleType.Spectator);
                spectatee.IsMuted = false;
                if(!PluginMain.Instance.Config.RangersCanTalk && !PluginMain.Instance.EventHandler.mutedPlayers.IsEmpty())
                {
                    foreach (Player plyr in PluginMain.Instance.EventHandler.mutedPlayers)
                    {
                        if(plyr.Equals(spectatee))
                        {
                            spectatee.IsMuted = true;
                            break;
                        }
                    }
                }
            }
            Player player = Player.Get((CommandSender)sender);
            if(PluginMain.Instance.EventHandler.curBounds.IsOnRange(player))
            {
                player.ClearInventory(true);
                Timing.RunCoroutine(SettingSpectatorCoroutine(player));
                
                response = "Command Successful";
                return true;
            }
            response = "You are not on the range, you cannot become a spectator";
            return false;
        }
    }
}
