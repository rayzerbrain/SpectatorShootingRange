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
                spectatee.SetRole(RoleType.Spectator);
            }
            Player player = Player.Get((CommandSender)sender);
            if(ShootingRange.Instance.EventHandler.IsOnRange(player))
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
