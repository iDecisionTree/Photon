using System.Numerics;

namespace Photon.Core
{
    public struct Color
    {
        public byte r;
        public byte g;
        public byte b;

        public Color(byte value)
        {
            r = value;
            g = value;
            b = value;
        }

        public Color(float value)
        {
            r = (byte)(value * 255f);
            g = (byte)(value * 255f);
            b = (byte)(value * 255f);
        }

        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color(float r, float g, float b)
        {
            this.r = (byte)(r * 255f);
            this.g = (byte)(g * 255f);
            this.b = (byte)(b * 255f);
        }

        public Color(Vector3 v)
        {
            r = (byte)(v.X * 255f);
            g = (byte)(v.Y * 255f);
            b = (byte)(v.Z * 255f);
        }

        public static Color FromVector(Vector3 v)
        {
            return new Color((byte)(v.X * 255f), (byte)(v.Y * 255f), (byte)(v.Z * 255f));
        }
    }
}
