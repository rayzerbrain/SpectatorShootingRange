using System;
using RemoteAdmin;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Extensions;
using MEC;
using System.Collections.Generic;
using Mirror;

namespace ShootingRange.Commands
{
    [CommandHandler(typeof (ClientCommandHandler))]
    class Range : ICommand
    {
        public string Command { get; } = "range";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description { get; } = "Transports you to the shooting range";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            
            Player player = Player.Get((CommandSender)sender);
            if(player.Team.Equals(Team.RIP)&&Round.IsStarted)
            {
                player.SetRole(RoleType.Tutorial);
                player.Broadcast((ushort)ShootingRange.Instance.Config.Range_greeting_time, ShootingRange.Instance.Config.Range_greeting);
                Timing.RunCoroutine(EnteringRangeCoroutine(player));
            }
            else
            {
                response = "Error, either you are not a spectator or the round has not started yet";
                return false;
            }
            response = "Command Successful";
            return true;

            IEnumerator<float> EnteringRangeCoroutine(Player ranger)
            {
                yield return Timing.WaitForSeconds(.5f);
                ranger.Position = new UnityEngine.Vector3((float)218.5, (float)999.1, (float)-43.0);
                ranger.AddItem(ItemType.GunAK);
                ranger.AddItem(ItemType.GunCOM18);
                ranger.AddItem(ItemType.GunCrossvec);
                ranger.AddItem(ItemType.GunE11SR);
                ranger.AddItem(ItemType.GunFSP9);
                ranger.AddItem(ItemType.GunLogicer);
                ranger.AddItem(ItemType.GunRevolver);
                ranger.AddItem(ItemType.GunShotgun);
                ranger.Health = 100000;

            }
        }
    }
}