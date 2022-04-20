using System.Linq;

using UnityEngine;

using MEC;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Extensions;

using Object = UnityEngine.Object;

namespace ShootingRange.API
{
    public class SpectatorRange
    {
        private Primitive[] _primitives = new Primitive[5];
        private Vector3 _smallBound = new Vector3(202, 970, -54);
        private Vector3 _bigBound = new Vector3(237, 1000, -28);
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
            const float dx = 2.5f;
            const int startX = 235;
            const int startZ = -53;
            const float y = 996.63f;
            Vector3 rot = new Vector3(0, 1, 0);

            for (int i = 0; i < 3; i++)
            {
                float xOffset = dx * i;
                float z = startZ - absZOffset - relZOffset * i;

                ShootingTargetToy.Create(ShootingTargetType.Sport, new Vector3(startX - xOffset, y, z), rot);
                ShootingTargetToy.Create(ShootingTargetType.ClassD, new Vector3(startX - xOffset * 3, y, z), rot);
                ShootingTargetToy.Create(ShootingTargetType.Binary, new Vector3(startX - xOffset * 6, y, z), rot);
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
            Color color = Color.clear;
            Vector3 dif = _bigBound - _smallBound;
            Vector3 center = (_bigBound + _smallBound) / 2;

            _primitives[0] = Primitive.Create(new Vector3(center.x, center.y, _bigBound.z), null, new Vector3(dif.x, dif.y, thick));
            _primitives[1] = Primitive.Create(_primitives[0].Position - new Vector3(0, 0, dif.z), null, _primitives[0].Scale);
            _primitives[2] = Primitive.Create(new Vector3(_bigBound.x, center.y, center.z), null, new Vector3(thick, dif.y, dif.z));
            _primitives[3] = Primitive.Create(_primitives[2].Position - new Vector3(dif.x, 0, 0), null, _primitives[2].Scale);
            _primitives[4] = Primitive.Create(new Vector3(center.x, _smallBound.y, center.z), null, new Vector3(dif.x, thick, dif.z));

            _primitives[0].Color = color;
            for (int i = 1; i < 4; i++)
            {
                _primitives[i].Base.NetworkScale = _primitives[i].Scale;
                _primitives[i].Color = color;
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
