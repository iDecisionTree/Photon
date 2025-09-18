using System.Numerics;

namespace Photon.Core
{
    public struct Ray
    {
        public Vector3 origin;
        public Vector3 direction;

        /// <summary>
        /// 传入的方向必须已经归一化
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

        public Ray WorldToObject(Matrix4x4 worldToObjectMatrix)
        {
            Vector4 positionV4 = Vector4.Transform(new Vector4(origin.X, origin.Y, origin.Z, 1f), worldToObjectMatrix);
            Vector4 directionV4 = Vector4.Transform(new Vector4(direction.X, direction.Y, direction.Z, 0f), worldToObjectMatrix);
        
            return new Ray(new Vector3(positionV4.X, positionV4.Y, positionV4.Z), Vector3.Normalize(new Vector3(directionV4.X, directionV4.Y, directionV4.Z)));
        }
    }
}
