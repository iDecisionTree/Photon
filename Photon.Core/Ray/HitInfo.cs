using System.Numerics;

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
