using System.Linq;

using UnityEngine;

using MEC;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Extensions;

namespace ShootingRange.API
{
    public class SpectatorRange
    {
        private Vector3 _smallBound = new Vector3(202, 997, -54);
        private Vector3 _bigBound = new Vector3(237, 1015, -28);
        public Vector3 Spawn { get; } = new Vector3(218.5f, 999.1f, -43.0f);
        public bool IsOpen => Round.IsStarted && Respawn.TimeUntilRespawn > 20;
        public SpectatorRange() { }
        public SpectatorRange(Vector4 v4)
        {
            Spawn = new Vector3(v4.x, v4.y, v4.z);
            Vector3 offset = new Vector3(v4.w, v4.w, v4.w);

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
            if (!(IsOpen && player.IsDead && !PluginMain.Instance.EventHandler.FreshlyDead.Contains(player)))
                return false;

            player.SetRole(RoleType.Tutorial);
            player.Broadcast(PluginMain.Instance.Config.RangeGreeting);
            Timing.CallDelayed(0.5f, () =>
            {
                player.Position = Spawn;
                player.AddItem(PluginMain.Instance.Config.RangerInventory);
                player.Health = 100000;
                player.ChangeAppearance(RoleType.ChaosConscript);
            });
            return true;
        }
        public void SpawnTargets()
        {
            int absZOffset = PluginMain.Instance.Config.AbsoluteTargetDistance;
            int relZOffset = PluginMain.Instance.Config.RelativeTargetDistance;
            float centerX = (_bigBound.x + _smallBound.x) / 2;
            Vector3 rot = new Vector3(0, 90, 0);
            ShootingTargetToy[] targets = new ShootingTargetToy[9];

            for (int i = 0; i < 3; i++)
            {
                float xOffset = 2.5f * (i + 1);
                float z = _smallBound.z - absZOffset - relZOffset * i;

                targets[i * 3] = ShootingTargetToy.Create(ShootingTargetType.Sport, new Vector3(_bigBound.x - xOffset, _smallBound.y, z), rot);
                targets[1 + i * 3] = ShootingTargetToy.Create(ShootingTargetType.ClassD, new Vector3(centerX  - xOffset, _smallBound.y, z), rot);
                targets[2 + i * 3] = ShootingTargetToy.Create(ShootingTargetType.Binary, new Vector3(_smallBound.x + 10 - xOffset, _smallBound.y, z), rot);
            }
            foreach(ShootingTargetToy target in targets)
            {
                target.Scale = target.Scale;
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
            const float frontHeight = 2f;
            Color color = Color.clear;
            Vector3 dif = _bigBound - _smallBound;
            Vector3 center = (_bigBound + _smallBound) / 2;
            Primitive[] primitives = new Primitive[5];

            primitives[0] = Primitive.Create(new Vector3(center.x, center.y, _bigBound.z), null, new Vector3(dif.x, dif.y, thick));
            primitives[1] = Primitive.Create(new Vector3(center.x, _smallBound.y + frontHeight / 2, _smallBound.z), null, new Vector3(dif.x, frontHeight, thick));
            primitives[2] = Primitive.Create(new Vector3(_bigBound.x, center.y, center.z), null, new Vector3(thick, dif.y, dif.z));
            primitives[3] = Primitive.Create(primitives[2].Position - new Vector3(dif.x, 0, 0), null, primitives[2].Scale);
            primitives[4] = Primitive.Create(new Vector3(center.x, _smallBound.y, center.z), null, new Vector3(dif.x, thick, dif.z));

            for (int i = 0; i < 5; i++)
            {
                primitives[i].Base.NetworkScale = primitives[i].Scale;
                primitives[i].Color = color;
                primitives[i].Type = PrimitiveType.Cube;
            }
        }
        //public void UnspawnCollider() => Object.Destroy(_collider);
        public void RemovePlayers()
        {
            foreach (Player plyr in Player.List.Where((plyr) => HasPlayer(plyr)))
            {
                plyr.ClearInventory();
                plyr.SetRole(RoleType.Spectator);
                plyr.Broadcast(PluginMain.Instance.Config.RespawnBroadcast, true);
            }
        }
    }
}
