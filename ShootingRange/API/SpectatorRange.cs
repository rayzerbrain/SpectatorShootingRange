using UnityEngine;

using MEC;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Extensions;
using Mirror;

namespace ShootingRange.API
{
    public class SpectatorRange
    {
        private Vector3 _smallBound = new(205, 997.25f, -52);
        private Vector3 _bigBound = new(237, 1015, -37);
        public Vector3 Spawn { get; } = new(218.5f, 999.1f, -43.0f);
        public bool IsOpen => Round.IsStarted && Respawn.TimeUntilRespawn > 20;

        public SpectatorRange() { }
        public SpectatorRange(Vector4 v4)
        {
            Spawn = new(v4.x, v4.y, v4.z);
            Vector3 offset = new(v4.w, v4.w, v4.w);

            _smallBound = Spawn - offset;
            _bigBound = Spawn + offset;
        }

        public bool HasPlayer(Player plyr)
        {
            for (int i = 0; i < 3; i++)
            {
                float pos = plyr.Position[i];
                if (pos > _bigBound[i] || pos < _smallBound[i])
                    return false;
            }
            return true;
        }
        public bool TryAdmit(Player player)
        {
            if (!(IsOpen && player.IsDead && !PluginMain.Singleton.EventHandler.FreshlyDead.Contains(player)))
                return false;

            player.SetRole(RoleType.Tutorial);
            Timing.CallDelayed(0.5f, () =>
            {
                player.Position = Spawn;
                player.AddItem(PluginMain.Singleton.Config.RangerInventory);
                player.Health = 100000;
                player.ChangeAppearance(RoleType.ChaosConscript);
                player.Broadcast(PluginMain.Singleton.Config.RangeGreeting);
            });
            return true;
        }
        public void SpawnTargets()
        {
            int absZOffset = PluginMain.Singleton.Config.AbsoluteTargetDistance;
            int relZOffset = PluginMain.Singleton.Config.RelativeTargetDistance;
            float centerX = (_bigBound.x + _smallBound.x) / 2;
            float xDist = 2.25f;
            Vector3 rot = new(0, 90, 0);
            ShootingTargetToy[] targets = new ShootingTargetToy[9];

            for (int i = 0; i < 3; i++)
            {
                float xOffset = xDist * (i + 1);
                float z = _smallBound.z - absZOffset - relZOffset * i;
                int index = i * 3;

                targets[index] = ShootingTargetToy.Create(ShootingTargetType.Sport, new(_bigBound.x - xOffset, _smallBound.y, z), rot);
                targets[index + 1] = ShootingTargetToy.Create(ShootingTargetType.ClassD, new(centerX  - xOffset, _smallBound.y, z), rot);
                targets[index + 2] = ShootingTargetToy.Create(ShootingTargetType.Binary, new(_smallBound.x + (xDist * 3) - xOffset, _smallBound.y, z), rot);
            }

            //0 rotation = towards gate a
            //+1 rotation to turn clockwise 90
            //for targets at least
            //x smaller as going to A
            //z bigger going to escape
        }
        public void SpawnPrimitives()
        {
            const float thick = 0.1f;
            const float frontHeight = 1.9f;
            Color color = Color.clear;
            Vector3 dif = _bigBound - _smallBound;
            Vector3 center = (_bigBound + _smallBound) / 2;
            Primitive[] prims = new Primitive[5];

            prims[0] = Primitive.Create(new(center.x, center.y, _bigBound.z), null, new(dif.x, dif.y, thick));
            prims[1] = Primitive.Create(new(center.x, _smallBound.y + frontHeight / 2, _smallBound.z), null, new(dif.x, frontHeight, thick));
            prims[2] = Primitive.Create(new(_bigBound.x, center.y, center.z), null, new(thick, dif.y, dif.z));
            prims[3] = Primitive.Create(prims[2].Position - new Vector3(dif.x, 0, 0), null, prims[2].Scale);
            prims[4] = Primitive.Create(new(center.x, _smallBound.y, center.z), null, new(dif.x, thick, dif.z));

            for (int i = 0; i < 5; i++)
            {
                prims[i].Color = color;
                prims[i].Type = PrimitiveType.Cube;
            }
        }
        public void SpawnBench()
        {
            Quaternion rot = Quaternion.Euler(0, 180, 0);
            Vector3 pos = new((_bigBound.x + _smallBound.x) / 2, _smallBound.y + 0.25f, _bigBound.z - 1);

            GameObject benchPrefab = NetworkClient.prefabs[System.Guid.Parse("307eb9b0-d080-9dc4-78e6-673847876412")];
            NetworkServer.Spawn(Object.Instantiate(benchPrefab, pos, rot));
        }
        public void RemovePlayer(Player plyr)
        {
            plyr.ClearInventory();
            plyr.SetRole(RoleType.Spectator);
        }
    }
}
