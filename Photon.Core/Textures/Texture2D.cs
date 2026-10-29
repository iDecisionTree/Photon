using Photon.Core.Mathematics;

namespace Photon.Core.Textures;

public sealed class Texture2D : Texture
{
    public Texture2D(int width, int height, TextureFormat format, bool mipChain = false, bool linear = true)
        : base(width, height, 1, format, mipChain, linear)
    {
    }

    public override TextureDimension dimension => TextureDimension.Tex2D;

    public Color GetPixel(int x, int y, int mipLevel = 0)
    {
        return GetPixelCore(x, y, 0, mipLevel);
    }

    public void SetPixel(int x, int y, Color color, int mipLevel = 0)
    {
        SetPixelCore(x, y, 0, color, mipLevel);
    }

    public Color GetPixelPoint(float u, float v, int mipLevel = 0, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SamplePointCore(u, v, 0.5f, mipLevel, wrapMode);
    }

    public Color GetPixelBilinear(float u, float v, int mipLevel = 0, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SampleLinearCore(u, v, 0.5f, mipLevel, wrapMode);
    }

    public Color Sample(float u, float v, float lod = 0f, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SampleLodCore(u, v, 0.5f, lod, wrapMode);
    }

    public Texture2D Clone()
    {
        Texture2D result = new Texture2D(width, height, format, mipmapCount > 1, !isDataSRGB);
        result.CopyFrom(this);
        return result;
    }

    public Texture2D ConvertFormat(TextureFormat format)
    {
        Texture2D result = new Texture2D(width, height, format, mipmapCount > 1, !isDataSRGB);
        CopyPixelsTo(result);
        return result;
    }
}
