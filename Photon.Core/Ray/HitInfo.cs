using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public struct HitInfo
    {
        public float t;
        public Vector3 point;
        public Vector3 normal;

        public HitInfo(float t, Vector3 point, Vector3 normal)
        {
            this.t = t;
            this.point = point;
            this.normal = normal;
        }
    }
}
