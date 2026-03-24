using System.Diagnostics.CodeAnalysis;

namespace Photon.Math.Vector
{
    public readonly struct Vector2 : IEquatable<Vector2>
    {
        public readonly float x;
        public readonly float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float length => Length(this);
        public float lengthSquared => LengthSquared(this);
        public Vector2 normalized => Normalize(this);

        public static readonly Vector2 zero = new Vector2(0f, 0f);
        public static readonly Vector2 one = new Vector2(1f, 1f);
        public static readonly Vector2 unitX = new Vector2(1f, 0f);
        public static readonly Vector2 unitY = new Vector2(0f, 1f);

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator +(Vector2 a, float b)
        {
            return new Vector2(a.x + b, a.y + b);
        }

        public static Vector2 operator +(float a, Vector2 b)
        {
            return new Vector2(a + b.x, a + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a, float b)
        {
            return new Vector2(a.x - b, a.y - b);
        }

        public static Vector2 operator -(float a, Vector2 b)
        {
            return new Vector2(a - b.x, a - b.y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.x, -v.y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(a * b.x, a * b.y);
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            return new Vector2(a.x / b, a.y / b);
        }

        public static Vector2 operator /(float a, Vector2 b)
        {
            if (Mathf.Approximately(b.x, 0f) || Mathf.Approximately(b.y, 0f))
            {
                throw new DivideByZeroException($"{b.ToString()}不能做除数");
            }

            return new Vector2(a / b.x, a / b.y);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (a - b).length;
        }

        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            return (a - b).lengthSquared;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float Length(Vector2 v)
        {
            return Mathf.Sqrt(v.lengthSquared);
        }

        public static float LengthSquared(Vector2 v)
        {
            return v.x * v.x + v.y * v.y;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector2 Normalize(Vector2 v)
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
            return obj is Vector2 v && this == v;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return $"Vector2({x}, {y})";
        }
    }
}
