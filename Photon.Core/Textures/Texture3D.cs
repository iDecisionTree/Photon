using Photon.Core.Mathematics;

namespace Photon.Core.Textures;

public sealed class Texture3D : Texture
{
    public Texture3D(int width, int height, int depth, TextureFormat format, bool mipChain = false, bool linear = true)
        : base(width, height, depth, format, mipChain, linear)
    {
    }

    public int depth => baseDepth;
    public override TextureDimension dimension => TextureDimension.Tex3D;

    public Color GetPixel(int x, int y, int z, int mipLevel = 0)
    {
        return GetPixelCore(x, y, z, mipLevel);
    }

    public void SetPixel(int x, int y, int z, Color color, int mipLevel = 0)
    {
        SetPixelCore(x, y, z, color, mipLevel);
    }

    public Color GetPixelPoint(float u, float v, float w, int mipLevel = 0, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SamplePointCore(u, v, w, mipLevel, wrapMode);
    }

    public Color GetPixelTrilinear(float u, float v, float w, int mipLevel = 0, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SampleLinearCore(u, v, w, mipLevel, wrapMode);
    }

    public Color Sample(float u, float v, float w, float lod = 0f, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        return SampleLodCore(u, v, w, lod, wrapMode);
    }

    public Texture3D Clone()
    {
        Texture3D result = new Texture3D(width, height, depth, format, mipmapCount > 1, !isDataSRGB);
        result.CopyFrom(this);
        return result;
    }

    public Texture3D ConvertFormat(TextureFormat format)
    {
        Texture3D result = new Texture3D(width, height, depth, format, mipmapCount > 1, !isDataSRGB);
        CopyPixelsTo(result);
        return result;
    }
}
