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

            if (PluginMain.Singleton.ActiveRange.HasPlayer(player))
            {
                PluginMain.Singleton.ActiveRange.RemovePlayer(player);
                response = "Command Successful";
                return true;
            }

            response = "Error, you are not on the shooting range";
            return false;
        }
    }
}
