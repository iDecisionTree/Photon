using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math
{
    public struct Vector2 : IEquatable<Vector2>
    {
        private float _x;
        private float _y;

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
        public float length => GetLength();
        public Vector2 normalized => Normalize(this);

        public static readonly Vector2 zero = new Vector2(0f, 0f);
        public static readonly Vector2 one = new Vector2(1f, 1f);
        public static readonly Vector2 unitX = new Vector2(1f, 0f);
        public static readonly Vector2 unitY = new Vector2(0f, 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2()
        {
            _x = 0f;
            _y = 0f;
            _isDirty = false;
            _cachedLength = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float value)
        {
            _x = value;
            _y = value;
            _isDirty = true;
            _cachedLength = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
            _isDirty = true;
            _cachedLength = 0f;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 x, Vector2 y) => new Vector2(x._x + y._x, x._y + y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(float x, Vector2 y) => new Vector2(x + y._x, x + y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 x, float y) => new Vector2(x._x + y, x._y + y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 x) => new Vector2(-x._x, -x._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 x, Vector2 y) => new Vector2(x._x - y._x, x._y - y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(float x, Vector2 y) => new Vector2(x - y._x, x - y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 x, float y) => new Vector2(x._x - y, x._y - y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(float x, Vector2 y) => new Vector2(x * y._x, x * y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 x, float y) => new Vector2(x._x * y, x._y * y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(float x, Vector2 y) => new Vector2(x / y._x, x / y._y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 x, float y) => new Vector2(x._x / y, x._y / y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2 x, Vector2 y) => x.Equals(y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2 x, Vector2 y) => !x.Equals(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2 x, Vector2 y)
        {
            return x._x * y._x + x._y * y._y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2 x, Vector2 y)
        {
            return x._x * y._y - x._y * y._x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleDegree(Vector2 x, Vector2 y)
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
        public static float AngleRadians(Vector2 x, Vector2 y)
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
        public static float SignedAngleDegree(Vector2 a, Vector2 b)
        {
            float cross = Cross(a, b);
            float dot = Dot(a, b);

            return Mathf.Atan2(cross, dot) * Mathf.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngleRadians(Vector2 a, Vector2 b)
        {
            float cross = Cross(a, b);
            float dot = Dot(a, b);

            return Mathf.Atan2(cross, dot);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Project(Vector2 x, Vector2 y)
        {
            float denom = y._x * y._x + y._y * y._y;
            if (denom < Mathf.Epsilon)
            {
                return zero;
            }
            float t = Dot(x, y) / denom;

            return new Vector2(y._x * t, y._y * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Reflect(Vector2 dir, Vector2 normal)
        {
            float denom = normal._x * normal._x + normal._y * normal._y;
            if (denom < Mathf.Epsilon)
            {
                return dir;
            }
            float k = 2f * Dot(dir, normal) / denom;

            return new Vector2(dir._x - normal._x * k, dir._y - normal._y * k);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 x, Vector2 y)
        {
            float dx = x._x - y._x;
            float dy = x._y - y._y;

            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector2 x, Vector2 y)
        {
            float dx = x._x - y._x;
            float dy = x._y - y._y;

            return dx * dx + dy * dy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normalize(Vector2 x)
        {
            float length = x.length;
            if (length < Mathf.Epsilon)
            {
                return zero;
            }

            return new Vector2(x._x / length, x._y / length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Abs(Vector2 x)
        {
            return new Vector2(Mathf.Abs(x._x), Mathf.Abs(x._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Min(Vector2 x, Vector2 y)
        {
            return new Vector2(Mathf.Min(x._x, y._x), Mathf.Min(x._y, y._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Max(Vector2 x, Vector2 y)
        {
            return new Vector2(Mathf.Max(x._x, y._x), Mathf.Max(x._y, y._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Floor(Vector2 x)
        {
            return new Vector2(Mathf.Floor(x._x), Mathf.Floor(x._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Ceiling(Vector2 x)
        {
            return new Vector2(Mathf.Ceiling(x._x), Mathf.Ceiling(x._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Round(Vector2 x)
        {
            return new Vector2(Mathf.Round(x._x), Mathf.Round(x._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp(Vector2 x, float min, float max)
        {
            return new Vector2(Mathf.Clamp(x._x, min, max), Mathf.Clamp(x._y, min, max));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp01(Vector2 x)
        {
            return new Vector2(Mathf.Clamp01(x._x), Mathf.Clamp01(x._y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 x, Vector2 y, float t)
        {
            return new Vector2(Mathf.Lerp(x._x, y._x, t), Mathf.Lerp(x._y, y._y, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LerpUnclamped(Vector2 x, Vector2 y, float t)
        {
            return new Vector2(Mathf.LerpUnclamped(x._x, y._x, t), Mathf.LerpUnclamped(x._y, y._y, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SmoothStep(Vector2 x, Vector2 y, float t)
        {
            return new Vector2(Mathf.SmoothStep(x._x, y._x, t), Mathf.SmoothStep(x._y, y._y, t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetLength()
        {

            if (_isDirty)
            {
                if (_x == 0f && _y == 0f)
                {
                    _cachedLength = 0f;
                }
                else
                {
                    _cachedLength = Mathf.Sqrt(_x * _x + _y * _y);
                }

                _isDirty = false;
            }


            return _cachedLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2 other)
        {
            return _x == other._x && _y == other._y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector2 other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _x.GetHashCode();
                hash = hash * 23 + _y.GetHashCode();

                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Vector2({_x}, {_y})";
        }
    }
}
