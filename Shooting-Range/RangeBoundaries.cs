using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShootingRange.API
{
    public class RangeBoundaries
    {
        public Vector3 spawn = new Vector3(218.5f, 999.1f, -43.0f);
        int bigXBound = 237;
        int smallXBound = 202;
        int bigZBound = -28;
        int smallZBound = -54;
        int smallYBound = 970;
        int bigYBound = 1000;

        public RangeBoundaries() { }
        public RangeBoundaries(int smallX, int smallZ, int bigX, int bigZ, int smallY, int bigY)
        {
            smallXBound = smallX;
            smallZBound = smallZ;
            bigXBound = bigX;
            bigZBound = bigZ;
            smallYBound = smallY;
            bigYBound = bigY;
        }
        public RangeBoundaries(Vector4 spawn)
        {
            smallXBound = (int)(spawn.x - spawn.w);
            smallZBound = (int)(spawn.z - spawn.w);
            smallYBound = (int)(spawn.y - spawn.w);
            bigXBound = (int)(spawn.x + spawn.w);
            bigZBound = (int)(spawn.z + spawn.w);
            bigYBound = (int)(spawn.y + spawn.w);

            this.spawn = new Vector3(spawn.x, spawn.y, spawn.z);
        }

        public bool IsOnRange(Player plyr)
        {
            return plyr.Position.x < bigXBound
            && plyr.Position.x > smallXBound
            && plyr.Position.z > smallZBound
            && plyr.Position.z < bigZBound
            && plyr.Position.y < bigYBound
            && plyr.Position.y > smallYBound;
        }

    }
}
