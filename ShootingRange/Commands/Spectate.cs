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

        public string Description { get;} = "Returns you to spectating if you are on the range";

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

            response = "Error, you are not on the shooting range";
            return false;
        }
    }
}
