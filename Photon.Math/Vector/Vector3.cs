using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math
{
    public struct Vector3 : IEquatable<Vector3>
    {
        private float _x;
        private float _y;
        private float _z;

        private bool _isDirty;
        private float _cachedLength;

        public float x
        {
            get => _x;
            set
            {
                _x = value;
                _isDirty = true;
            }
        }

        public float y
        {
            get => _y;
            set
            {
                _y = value;
                _isDirty = true;
            }
        }

        public float z
        {
            get => _z;
            set
            {
                _z = value;
                _isDirty = true;
            }
        }

        public float length => GetLength();
        public Vector3 normalized => Normalize(this);

        public static readonly Vector3 zero = new Vector3(0f, 0f, 0f);
        public static readonly Vector3 one = new Vector3(1f, 1f, 1f);
        public static readonly Vector3 unitX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 unitY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 unitZ = new Vector3(0f, 0f, 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3()
        {
            _x = 0f;
            _y = 0f;
            _z = 0f;
            _isDirty = false;
            _cachedLength = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float value)
        {
            _x = value;
            _y = value;
            _z = value;
            _isDirty = true;
            _cachedLength = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
            _isDirty = true;
            _cachedLength = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 x, Vector3 y) => new Vector3(x._x + y._x, x._y + y._y, x._z + y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(float x, Vector3 y) => new Vector3(x + y._x, x + y._y, x + y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 x, float y) => new Vector3(x._x + y, x._y + y, x._z + y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 x) => new Vector3(-x._x, -x._y, -x._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 x, Vector3 y) => new Vector3(x._x - y._x, x._y - y._y, x._z - y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(float x, Vector3 y) => new Vector3(x - y._x, x - y._y, x - y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 x, float y) => new Vector3(x._x - y, x._y - y, x._z - y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(float x, Vector3 y) => new Vector3(x * y._x, x * y._y, x * y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 x, float y) => new Vector3(x._x * y, x._y * y, x._z * y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(float x, Vector3 y) => new Vector3(x / y._x, x / y._y, x / y._z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 x, float y) => new Vector3(x._x / y, x._y / y, x._z / y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3 x, Vector3 y) => x.Equals(y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3 x, Vector3 y) => !x.Equals(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector3 x, Vector3 y)
        {
            return x._x * y._x + x._y * y._y + x._z * y._z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Cross(Vector3 x, Vector3 y)
        {
            return new Vector3(
                x._y * y._z - x._z * y._y,
                x._z * y._x - x._x * y._z,
                x._x * y._y - x._y * y._x
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleDegree(Vector3 x, Vector3 y)
        {
            float denom = x.length * y.length;
            if (denom < Mathf.Epsilon)
            {
                return 0f;
            }

            float cos = Dot(x, y) / denom;
            cos = Mathf.Clamp(cos, -1f, 1f);

            return Mathf.Acos(cos) * Mathf.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleRadians(Vector3 x, Vector3 y)
        {
            float denom = x.length * y.length;
            if (denom < Mathf.Epsilon)
            {
                return 0f;
            }

            float cos = Dot(x, y) / denom;
            cos = Mathf.Clamp(cos, -1f, 1f);

            return Mathf.Acos(cos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Project(Vector3 x, Vector3 y)
        {
            float denom = y._x * y._x + y._y * y._y + y._z * y._z;
            if (denom < Mathf.Epsilon)
            {
                return zero;
            }
            float t = Dot(x, y) / denom;

            return new Vector3(y._x * t, y._y * t, y._z * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Reflect(Vector3 dir, Vector3 normal)
        {
            float denom = normal._x * normal._x + normal._y * normal._y + normal._z * normal._z;
            if (denom < Mathf.Epsilon)
            {
                return dir;
            }
            float k = 2f * Dot(dir, normal) / denom;

            return new Vector3(dir._x - normal._x * k, dir._y - normal._y * k, dir._z - normal._z * k);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3 x, Vector3 y)
        {
            float dx = x._x - y._x;
            float dy = x._y - y._y;
            float dz = x._z - y._z;

            return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector3 x, Vector3 y)
        {
            float dx = x._x - y._x;
            float dy = x._y - y._y;
            float dz = x._z - y._z;

            return dx * dx + dy * dy + dz * dz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Normalize(Vector3 x)
        {
            float length = x.length;
            if (length < Mathf.Epsilon)
            {
                return zero;
            }

            return new Vector3(x._x / length, x._y / length, x._z / length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Abs(Vector3 x)
        {
            return new Vector3(Mathf.Abs(x._x), Mathf.Abs(x._y), Mathf.Abs(x._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Min(Vector3 x, Vector3 y)
        {
            return new Vector3(Mathf.Min(x._x, y._x), Mathf.Min(x._y, y._y), Mathf.Min(x._z, y._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Max(Vector3 x, Vector3 y)
        {
            return new Vector3(Mathf.Max(x._x, y._x), Mathf.Max(x._y, y._y), Mathf.Max(x._z, y._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Floor(Vector3 x)
        {
            return new Vector3(Mathf.Floor(x._x), Mathf.Floor(x._y), Mathf.Floor(x._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Ceiling(Vector3 x)
        {
            return new Vector3(Mathf.Ceiling(x._x), Mathf.Ceiling(x._y), Mathf.Ceiling(x._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Round(Vector3 x)
        {
            return new Vector3(Mathf.Round(x._x), Mathf.Round(x._y), Mathf.Round(x._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Clamp(Vector3 x, float min, float max)
        {
            return new Vector3(Mathf.Clamp(x._x, min, max), Mathf.Clamp(x._y, min, max), Mathf.Clamp(x._z, min, max));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Clamp01(Vector3 x)
        {
            return new Vector3(Mathf.Clamp01(x._x), Mathf.Clamp01(x._y), Mathf.Clamp01(x._z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 x, Vector3 y, float t)
        {
            return new Vector3(Mathf.Lerp(x._x, y._x, t), Mathf.Lerp(x._y, y._y, t), Mathf.Lerp(x._z, y._z, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 LerpUnclamped(Vector3 x, Vector3 y, float t)
        {
            return new Vector3(Mathf.LerpUnclamped(x._x, y._x, t), Mathf.LerpUnclamped(x._y, y._y, t), Mathf.LerpUnclamped(x._z, y._z, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SmoothStep(Vector3 x, Vector3 y, float t)
        {
            return new Vector3(Mathf.SmoothStep(x._x, y._x, t), Mathf.SmoothStep(x._y, y._y, t), Mathf.SmoothStep(x._z, y._z, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetLength()
        {
            if (_isDirty)
            {
                _cachedLength = Mathf.Sqrt(_x * _x + _y * _y + _z * _z);
                _isDirty = false;
            }

            return _cachedLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3 other)
        {
            return _x == other._x && _y == other._y && _z == other._z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _x.GetHashCode();
                hash = hash * 23 + _y.GetHashCode();
                hash = hash * 23 + _z.GetHashCode();

                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Vector3({_x}, {_y}, {_z})";
        }
    }
}