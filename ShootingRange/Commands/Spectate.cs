using System;

using CommandSystem;

using Exiled.API.Features;

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
            Player player = Player.Get(sender);

            if (PluginMain.Instance.ActiveRange.HasPlayer(player))
            {
                player.ClearInventory();
                player.SetRole(RoleType.Spectator);
                response = "Command Successful";
                return true;
            }

            response = "You are not on the range, you cannot become a spectator";
            return false;
        }
    }
}
