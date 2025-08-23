using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Math
{
    public struct Quaternion : IEquatable<Quaternion>
    {
        private float _x;
        private float _y;
        private float _z;
        private float _w;

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

        public float w
        {
            get => _w;
            set
            {
                _w = value;
                _isDirty = true;
            }
        }

        public float length => GetLength();
        public Quaternion normalized => Normalize(this);
        public Quaternion conjugated => Conjugate(this);
        public Quaternion inverse => Inverse(this);

        public static readonly Quaternion zero = new Quaternion(0f, 0f, 0f, 0f);
        public static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);

        public Quaternion()
        {
            _x = 0f;
            _y = 0f;
            _z = 0f;
            _w = 1f;
            _isDirty = false;
            _cachedLength = 1f;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
            _isDirty = true;
            _cachedLength = 0f;
        }

        public static Quaternion operator +(Quaternion x, Quaternion y) => new Quaternion(x._x + y._x, x._y + y._y, x._z + y._z, x._w + y._w);
        public static Quaternion operator -(Quaternion x) => new Quaternion(-x._x, -x._y, -x._z, -x._w);
        public static Quaternion operator -(Quaternion x, Quaternion y) => new Quaternion(x._x - y._x, x._y - y._y, x._z - y._z, x._w - y._w);
        
        public static Quaternion operator *(Quaternion x, Quaternion y)
        {
            return new Quaternion(
                x._w * y._x + x._x * y._w + x._y * y._z - x._z * y._y,
                x._w * y._y - x._x * y._z + x._y * y._w + x._z * y._x,
                x._w * y._z + x._x * y._y - x._y * y._x + x._z * y._w,
                x._w * y._w - x._x * y._x - x._y * y._y - x._z * y._z
            );
        }

        public static Quaternion operator *(float x, Quaternion y) => new Quaternion(x * y._x, x * y._y, x * y._z, x * y._w);
        public static Quaternion operator *(Quaternion x, float y) => new Quaternion(x._x * y, x._y * y, x._z * y, x._w * y);
        public static Quaternion operator /(Quaternion x, float y) => new Quaternion(x._x / y, x._y / y, x._z / y, x._w / y);
        public static bool operator ==(Quaternion x, Quaternion y) => x.Equals(y);
        public static bool operator !=(Quaternion x, Quaternion y) => !x.Equals(y);

        public static float Dot(Quaternion x, Quaternion y)
        {
            return x._x * y._x + x._y * y._y + x._z * y._z + x._w * y._w;
        }

        public static Quaternion FromEulerAngleDegree(float pitch, float yaw, float roll)
        {
            float halfX = pitch * Mathf.Deg2Rad * 0.5f;
            float halfY = yaw * Mathf.Deg2Rad * 0.5f;
            float halfZ = roll * Mathf.Deg2Rad * 0.5f;

            float cx = Mathf.Cos(halfX);
            float sx = Mathf.Sin(halfX);
            float cy = Mathf.Cos(halfY);
            float sy = Mathf.Sin(halfY);
            float cz = Mathf.Cos(halfZ);
            float sz = Mathf.Sin(halfZ);

            float x = sx * cy * cz + cx * sy * sz;
            float y = cx * sy * cz - sx * cy * sz;
            float z = cx * cy * sz + sx * sy * cz;
            float w = cx * cy * cz - sx * sy * sz;

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion FromEulerAngleRadians(float pitch, float yaw, float roll)
        {
            float halfX = pitch * 0.5f;
            float halfY = yaw * 0.5f;
            float halfZ = roll * 0.5f;

            float cx = Mathf.Cos(halfX);
            float sx = Mathf.Sin(halfX);
            float cy = Mathf.Cos(halfY);
            float sy = Mathf.Sin(halfY);
            float cz = Mathf.Cos(halfZ);
            float sz = Mathf.Sin(halfZ);

            float x = sx * cy * cz + cx * sy * sz;
            float y = cx * sy * cz - sx * cy * sz;
            float z = cx * cy * sz + sx * sy * cz;
            float w = cx * cy * cz - sx * sy * sz;

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion FromAxisAngleRadians(Vector3 axis, float angle)
        {
            float halfAngle = angle * 0.5f;
            float sinHalf = Mathf.Sin(halfAngle);
            float cosHalf = Mathf.Cos(halfAngle);

            return new Quaternion(axis.x * sinHalf, axis.y * sinHalf, axis.z * sinHalf, cosHalf).normalized;
        }

        public static Quaternion FromAxisAngleDegree(Vector3 axis, float angle)
        {
            float halfAngle = angle * Mathf.Deg2Rad * 0.5f;
            float sinHalf = Mathf.Sin(halfAngle);
            float cosHalf = Mathf.Cos(halfAngle);

            return new Quaternion(axis.x * sinHalf, axis.y * sinHalf, axis.z * sinHalf, cosHalf).normalized;
        }

        public static Quaternion Normalize(Quaternion x)
        {
            float length = x.GetLength();
            if (length < Mathf.Epsilon) 
            {
                return identity;
            }

            float invLength = 1f / length;

            return new Quaternion(x._x * invLength, x._y * invLength, x._z * invLength, x._w * invLength);
        }

        public static Quaternion Conjugate(Quaternion x)
        {
            return new Quaternion(-x._x, -x._y, -x._z, x._w);
        }

        public static Quaternion Inverse(Quaternion x)
        {
            float length = x.length;
            float lengthSquared = length * length;

            if (lengthSquared < Mathf.Epsilon * Mathf.Epsilon)
            {
                return identity;
            }

            float invLengthSquared = 1f / lengthSquared;

            return new Quaternion(-x._x * invLengthSquared, -x._y * invLengthSquared, -x._z * invLengthSquared, x._w * invLengthSquared);
        }

        public static Quaternion Lerp(Quaternion x, Quaternion y, float t)
        {
            return new Quaternion(Mathf.Lerp(x._x, y._x, t), Mathf.Lerp(x._y, y._y, t), Mathf.Lerp(x._z, y._z, t), Mathf.Lerp(x._w, y._w, t));
        }

        public static Quaternion LerpUnclamped(Quaternion x, Quaternion y, float t)
        {
            return new Quaternion(Mathf.LerpUnclamped(x._x, y._x, t), Mathf.LerpUnclamped(x._y, y._y, t), Mathf.LerpUnclamped(x._z, y._z, t), Mathf.LerpUnclamped(x._w, y._w, t));
        }

        public static Quaternion SmoothStep(Quaternion x, Quaternion y, float t)
        {
            return new Quaternion(Mathf.SmoothStep(x._x, y._x, t), Mathf.SmoothStep(x._y, y._y, t), Mathf.SmoothStep(x._z, y._z, t), Mathf.SmoothStep(x._w, y._w, t));
        }

        public static Quaternion Slerp(Quaternion x, Quaternion y, float t)
        {
            t = Mathf.Clamp01(t);
            float dot = Dot(x, y);

            Quaternion temp = y;
            if (dot < 0f)
            {
                dot = -dot;
                temp = -y;
            }

            if (dot > 1f - Mathf.Epsilon)
            {
                return Lerp(x, temp, t);
            }

            float theta = Mathf.Acos(dot);
            float sinTheta = Mathf.Sin(theta);

            float s1 = Mathf.Sin((1f - t) * theta) / sinTheta;
            float s2 = Mathf.Sin(t * theta) / sinTheta;

            return new Quaternion(s1 * x._x + s2 * temp._x, s1 * x._y + s2 * temp._y, s1 * x._z + s2 * temp._z, s1 * x._w + s2 * temp._w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetLength()
        {
            if (_isDirty)
            {
                _cachedLength = Mathf.Sqrt(_x * _x + _y * _y + _z * _z + _w * _w);
                _isDirty = false;
            }

            return _cachedLength;
        }

        public bool Equals(Quaternion other)
        {
            return _x == other._x && _y == other._y && _z == other._z && _w == other._w;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Quaternion other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _x.GetHashCode();
                hash = hash * 23 + _y.GetHashCode();
                hash = hash * 23 + _z.GetHashCode();
                hash = hash * 23 + _w.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return $"Quaternion({_x}, {_y}, {_z}, {_w})";
        }
    }
}
