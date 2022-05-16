using System;

using CommandSystem;

using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ShootingRange.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Range : ICommand
    {
        public string Command { get; } = "range";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description { get; } = "Transports you to the shooting range";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (PluginMain.Singleton.Config.RequirePermission && !sender.CheckPermission("range"))
            {
                response = "Error, you do not have permission to use this command";
                return false;
            }

            if (!PluginMain.Singleton.ActiveRange.TryAdmit(player))
            {
                response = "Error, either you are not a spectator or the range is currently unavailable";
                return false;
            }

            response = "Command Successful";
            return true;
        }
    }
}
