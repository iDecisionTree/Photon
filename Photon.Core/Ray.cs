using Photon.Math;
using System.Runtime.CompilerServices;

namespace Photon.Core
{
    public struct Ray
    {
        private Vector3 _origin;
        private Vector3 _direction;

        public Vector3 origin
        {
            get => _origin;
            set => _origin = value;
        }

        public Vector3 direction
        {
            get => _direction;
            set => _direction = value;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Marching(float t)
        {
            return _origin + _direction * t;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_origin, _direction);
        }

        public override string ToString()
        {
            return $"Ray(origin:{_origin}, direction:{_direction})";
        }
    }
}
