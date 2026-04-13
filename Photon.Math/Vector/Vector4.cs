using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math.Vector
{
    public readonly struct Vector4 : IEquatable<Vector4>
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;
        public readonly float w;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Vector3 v, float w) : this(v.x, v.y, v.z, w)
        {
        }

        public float length => Length(this);
        public float lengthSquared => LengthSquared(this);
        public Vector4 normalized => Normalize(this);

        public static readonly Vector4 zero = new Vector4(0f, 0f, 0f, 0f);
        public static readonly Vector4 one = new Vector4(1f, 1f, 1f, 1f);
        public static readonly Vector4 unitX = new Vector4(1f, 0f, 0f, 0f);
        public static readonly Vector4 unitY = new Vector4(0f, 1f, 0f, 0f);
        public static readonly Vector4 unitZ = new Vector4(0f, 0f, 1f, 0f);
        public static readonly Vector4 unitW = new Vector4(0f, 0f, 0f, 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator +(Vector4 a, float b)
        {
            return new Vector4(a.x + b, a.y + b, a.z + b, a.w + b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator +(float a, Vector4 b)
        {
            return new Vector4(a + b.x, a + b.y, a + b.z, a + b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 a, float b)
        {
            return new Vector4(a.x - b, a.y - b, a.z - b, a.w - b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(float a, Vector4 b)
        {
            return new Vector4(a - b.x, a - b.y, a - b.z, a - b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4(-v.x, -v.y, -v.z, -v.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 a, float b)
        {
            return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(float a, Vector4 b)
        {
            return new Vector4(a * b.x, a * b.y, a * b.z, a * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(Vector4 a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b;
            return new Vector4(a.x * inv, a.y * inv, a.z * inv, a.w * inv);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(float a, Vector4 b)
        {
            if (Mathf.Approximately(b.x, 0f) || Mathf.Approximately(b.y, 0f) || Mathf.Approximately(b.z, 0f) || Mathf.Approximately(b.w, 0f))
            {
                throw new DivideByZeroException($"{b.ToString()}不能做除数");
            }

            return new Vector4(a / b.x, a / b.y, a / b.z, a / b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z) && Mathf.Approximately(a.w, b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4 other)
        {
            return this == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector4 a, Vector4 b)
        {
            return (a - b).length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector4 a, Vector4 b)
        {
            return (a - b).lengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector4 a, Vector4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(Vector4 v)
        {
            return Mathf.Sqrt(v.lengthSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LengthSquared(Vector4 v)
        {
            return v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
        {
            return a + (b - a) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Max(Vector4 a, Vector4 b)
        {
            return new Vector4(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z), Mathf.Max(a.w, b.w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Min(Vector4 a, Vector4 b)
        {
            return new Vector4(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z), Mathf.Min(a.w, b.w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Normalize(Vector4 v)
        {
            float length = v.length;
            if (Mathf.Approximately(length, 0f))
            {
                return zero;
            }
            return v / length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector4 v && this == v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Vector4({x}, {y}, {z}, {w})";
        }
    }
}
