using System.Numerics;

namespace Photon.Core
{
    public struct Ray
    {
        public Vector3 origin;
        public Vector3 direction;

        /// <summary>
        /// 传入的方向默认已经归一化
        /// </summary>
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Vector3 At(float t)
        {
            return origin + t * direction;
        }
    }
}
