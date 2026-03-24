namespace Photon.Math
{
    public static class Mathf
    {
        public const float E = 2.71828183f;

        public const float PI = 3.14159265f;
        public const float TWO_PI = 2f * PI;
        public const float FORE_PI = 2f * PI;
        public const float HALF_PI = 0.5f * PI;
        public const float INV_PI = 1f / PI;

        public const float DEG2RAD = PI / 180f;
        public const float RAD2DEG = 180f / PI;

        public const float Epsilon = 1e-6f;

        public static float Abs(float x)
        {
            return MathF.Abs(x);
        }

        public static float Acos(float x)
        {
            return MathF.Acos(x);
        }

        public static bool Approximately(float a, float b)
        {
            return Abs(a - b) < Epsilon;
        }

        public static float Asin(float x)
        {
            return MathF.Asin(x);
        }

        public static float Atan(float x)
        {
            return MathF.Atan(x);
        }

        public static float Atan2(float y, float x)
        {
            return MathF.Atan2(y, x);
        }

        public static float Ceiling(float x)
        {
            return MathF.Ceiling(x);
        }

        public static float Clamp(float x, float min, float max)
        {
            x = x < min ? min : x;
            x = x > max ? max : x;
            return x;
        }

        public static float Clamp01(float x)
        {
            return Clamp(x, 0f, 1f);
        }

        public static float Cos(float x)
        {
            return MathF.Cos(x);
        }

        public static float Cosh(float x)
        {
            return MathF.Cosh(x);
        }

        public static float DegreeToRadians(float x)
        {
            return x * DEG2RAD;
        }

        public static float Exp(float x)
        {
            return MathF.Exp(x);
        }

        public static float Floor(float x)
        {
            return MathF.Floor(x);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Log(float x)
        {
            return MathF.Log(x);
        }

        public static float Log(float x, float y)
        {
            return MathF.Log(x, y);
        }

        public static float Log10(float x)
        {
            return MathF.Log10(x);
        }

        public static float Max(float x, float y)
        {
            return MathF.Max(x, y);
        }

        public static float Min(float x, float y)
        {
            return MathF.Min(x, y);
        }

        public static float Pow(float x, float y)
        {
            return MathF.Pow(x, y);
        }

        public static float RadiansToDegree(float x)
        {
            return x * RAD2DEG;
        }

        public static float Round(float x)
        {
            return MathF.Round(x);
        }

        public static float Sign(float x)
        {
            return MathF.Sign(x);
        }

        public static float Sin(float x)
        {
            return MathF.Sin(x);
        }

        public static float Sinh(float x)
        {
            return MathF.Sinh(x);
        }

        public static float Smoothstep(float x, float t1, float t2)
        {
            x = Clamp((x - t1) / (t2 - t1), 0f, 1f);
            return x * x * (3f - 2f * x);
        }

        public static float Sqrt(float x)
        {
            return MathF.Sqrt(x);
        }

        public static float Tan(float x)
        {
            return MathF.Tan(x);
        }

        public static float Tanh(float x)
        {
            return MathF.Tanh(x);
        }

        public static float Truncate(float x)
        {
            return MathF.Truncate(x);
        }
    }
}
