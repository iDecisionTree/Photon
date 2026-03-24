using System.Diagnostics.CodeAnalysis;

namespace Photon.Math.Vector
{
    public readonly struct Vector3 : IEquatable<Vector3>
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector2 v, float z) : this(v.x, v.y, z)
        {
        }

        public float length => Length(this);
        public float lengthSquared => LengthSquared(this);
        public Vector3 normalized => Normalize(this);

        public static readonly Vector3 zero = new Vector3(0f, 0f, 0f);
        public static readonly Vector3 one = new Vector3(1f, 1f, 1f);
        public static readonly Vector3 unitX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 unitY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 unitZ = new Vector3(0f, 0f, 1f);

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator +(Vector3 a, float b)
        {
            return new Vector3(a.x + b, a.y + b, a.z + b);
        }

        public static Vector3 operator +(float a, Vector3 b)
        {
            return new Vector3(a + b.x, a + b.y, a + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a, float b)
        {
            return new Vector3(a.x - b, a.y - b, a.z - b);
        }

        public static Vector3 operator -(float a, Vector3 b)
        {
            return new Vector3(a - b.x, a - b.y, a - b.z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(float a, Vector3 b)
        {
            return new Vector3(a * b.x, a * b.y, a * b.z);
        }

        public static Vector3 operator /(Vector3 a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b;
            return new Vector3(a.x * inv, a.y * inv, a.z * inv);
        }

        public static Vector3 operator /(float a, Vector3 b)
        {
            if (Mathf.Approximately(b.x, 0f) || Mathf.Approximately(b.y, 0f) || Mathf.Approximately(b.z, 0f))
            {
                throw new DivideByZeroException($"{b.ToString()}不能做除数");
            }

            return new Vector3(a / b.x, a / b.y, a / b.z);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        public bool Equals(Vector3 other)
        {
            return this == other;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).length;
        }

        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            return (a - b).lengthSquared;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static float Length(Vector3 v)
        {
            return Mathf.Sqrt(v.lengthSquared);
        }

        public static float LengthSquared(Vector3 v)
        {
            return v.x * v.x + v.y * v.y + v.z * v.z;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float length = v.length;
            if (Mathf.Approximately(length, 0f))
            {
                return zero;
            }
            return v / length;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector3 v && this == v;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"Vector3({x}, {y}, {z})";
        }
    }
}
