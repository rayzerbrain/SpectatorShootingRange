using System;
using RemoteAdmin;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Extensions;
using MEC;
using System.Collections.Generic;
using Exiled.Permissions.Extensions;

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
            Player player = Player.Get(sender);

            if(!sender.CheckPermission("range"))
            {
                response = "You don't have permission to run this command!";
                return false;
            }

            if(player.Role.Equals(RoleType.Spectator)&&Round.IsStarted
                && !PluginMain.Instance.EventHandler.freshlyDead.Contains(player))
            {
                player.SetRole(RoleType.Tutorial, Exiled.API.Enums.SpawnReason.None);
                player.Broadcast(PluginMain.Instance.Config.RangeGreeting);
                Timing.RunCoroutine(EnteringRangeCoroutine(player));
                if (!PluginMain.Instance.Config.RangersCanTalk) //&& !player.RemoteAdminAccess)
                {   
                    player.IsMuted = true;
                }

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
                ranger.Position = PluginMain.Instance.EventHandler.curBounds.spawn;
                PluginMain.Instance.EventHandler.rangerList.Add(ranger);
                ranger.AddItem(ItemType.GunAK);
                ranger.AddItem(ItemType.GunCOM18);
                ranger.AddItem(ItemType.GunCrossvec);
                ranger.AddItem(ItemType.GunE11SR);
                ranger.AddItem(ItemType.GunFSP9);
                ranger.AddItem(ItemType.GunLogicer);
                ranger.AddItem(ItemType.GunRevolver);
                ranger.AddItem(ItemType.GunShotgun);
                ranger.Health = 100000;
                ranger.ChangeAppearance(RoleType.ChaosConscript);
                Log.Debug($"Plaer {ranger.Nickname} has entered the range. range list count: {PluginMain.Instance.EventHandler.rangerList.Count}", PluginMain.Instance.Config.DebugMode);
            }
        }
    }
   
}
