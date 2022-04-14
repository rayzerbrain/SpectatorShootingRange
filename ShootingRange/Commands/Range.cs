using System;

using CommandSystem;

using Exiled.API.Features;

namespace ShootingRange.Commands
{
    [CommandHandler(typeof (ClientCommandHandler))]
    public class Range : ICommand
    {
        public string Command { get; } = "range";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description { get; } = "Transports you to the shooting range";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!PluginMain.Instance.ActiveRange.TryAdmit(player))
            {
                response = "Error, either you are not a spectator or the round has not started yet";
                return false;
            }

            response = "Command Successful";
            return true;
        }
    }
}
