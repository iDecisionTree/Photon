using System.Runtime.InteropServices;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures;

public abstract class Texture
{
    private readonly ITextureCodec m_codec;
    private readonly byte[] m_data;
    private readonly TextureMipLayout[] m_mips;

    protected Texture(int width, int height, int depth, TextureFormat format, bool mipChain, bool linear)
    {
        m_codec = TextureFormatInfo.GetCodec(format);
        bytesPerPixel = m_codec.bytesPerPixel;
        m_mips = TextureMipLayout.Create(width, height, depth, bytesPerPixel, mipChain);
        TextureMipLayout last = m_mips[^1];
        m_data = new byte[last.offset + last.size];
        this.width = width;
        this.height = height;
        this.format = format;
        baseDepth = depth;
        isDataSRGB = !linear;
    }

    public int width { get; }
    public int height { get; }
    public TextureFormat format { get; }
    public bool isDataSRGB { get; }
    public int mipmapCount => m_mips.Length;
    public int bytesPerPixel { get; }

    public int dataSize => m_data.Length;
    public abstract TextureDimension dimension { get; }

    protected int baseDepth { get; }

    public (int width, int height, int depth) GetMipDimensions(int mipLevel)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        return (mip.width, mip.height, mip.depth);
    }

    public ReadOnlySpan<byte> GetPixelData(int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        return m_data.AsSpan(mip.offset, mip.size);
    }

    public ReadOnlySpan<byte> GetRawTextureData()
    {
        return m_data;
    }

    public void SetPixelData(ReadOnlySpan<byte> pixelData, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        if (pixelData.Length != mip.size)
        {
            throw new ArgumentException("The data size must match the mip level exactly.", nameof(pixelData));
        }

        pixelData.CopyTo(m_data.AsSpan(mip.offset, mip.size));
    }

    public void LoadRawTextureData(ReadOnlySpan<byte> pixelData)
    {
        if (pixelData.Length != m_data.Length)
        {
            throw new ArgumentException("The data size must match the entire texture exactly.", nameof(pixelData));
        }

        pixelData.CopyTo(m_data);
    }

    public Color[] GetPixels(int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        Color[] colors = new Color[mip.pixelCount];
        m_codec.UnpackPixels(m_data.AsSpan(mip.offset, mip.size), colors);
        return colors;
    }

    public void GetPixels(Span<Color> colors, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        if (colors.Length < mip.pixelCount)
        {
            throw new ArgumentException("The destination is too small for the mip level.", nameof(colors));
        }

        m_codec.UnpackPixels(m_data.AsSpan(mip.offset, mip.size), colors);
    }

    public void SetPixels(ReadOnlySpan<Color> colors, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        if (colors.Length != mip.pixelCount)
        {
            throw new ArgumentException("The color count must match the mip level exactly.", nameof(colors));
        }

        m_codec.PackPixels(colors, m_data.AsSpan(mip.offset, mip.size));
    }

    public Color32[] GetPixels32(int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        Color32[] colors = new Color32[mip.pixelCount];
        GetPixels32(colors.AsSpan(), mipLevel);
        return colors;
    }

    public void GetPixels32(Span<Color32> colors, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        if (colors.Length < mip.pixelCount)
        {
            throw new ArgumentException("The destination is too small for the mip level.", nameof(colors));
        }

        ReadOnlySpan<byte> source = m_data.AsSpan(mip.offset, mip.size);
        Span<byte> destination = MemoryMarshal.AsBytes(colors[..mip.pixelCount]);
        if (source.Overlaps(destination))
        {
            throw new ArgumentException("Source and destination must not overlap.", nameof(colors));
        }

        if (format == TextureFormat.R8G8B8A8_UNorm)
        {
            source.CopyTo(destination);
            return;
        }

        for (int i = 0; i < mip.pixelCount; i++)
        {
            colors[i] = (Color32)m_codec.Unpack(source.Slice(i * bytesPerPixel, bytesPerPixel));
        }
    }

    public void SetPixels32(ReadOnlySpan<Color32> colors, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        if (colors.Length != mip.pixelCount)
        {
            throw new ArgumentException("The color count must match the mip level exactly.", nameof(colors));
        }

        ReadOnlySpan<byte> source = MemoryMarshal.AsBytes(colors);
        Span<byte> destination = m_data.AsSpan(mip.offset, mip.size);
        if (source.Overlaps(destination))
        {
            throw new ArgumentException("Source and destination must not overlap.", nameof(colors));
        }

        if (format == TextureFormat.R8G8B8A8_UNorm)
        {
            source.CopyTo(destination);
            return;
        }

        for (int i = 0; i < mip.pixelCount; i++)
        {
            m_codec.Pack(colors[i], destination.Slice(i * bytesPerPixel, bytesPerPixel));
        }
    }

    public void Fill(Color color, int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        Span<byte> data = m_data.AsSpan(mip.offset, mip.size);
        m_codec.Pack(color, data[..bytesPerPixel]);
        int written = bytesPerPixel;
        while (written < data.Length)
        {
            int count = Math.Min(written, data.Length - written);
            data[..count].CopyTo(data.Slice(written, count));
            written += count;
        }
    }

    public void CopyFrom(Texture source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (source.dimension != dimension || source.width != width || source.height != height ||
            source.baseDepth != baseDepth || source.format != format || source.mipmapCount != mipmapCount ||
            source.isDataSRGB != isDataSRGB)
        {
            throw new ArgumentException("Texture dimensions, format, mip levels and color space must match.", nameof(source));
        }

        source.m_data.AsSpan().CopyTo(m_data);
    }

    public void GenerateMipmaps()
    {
        for (int level = 1; level < m_mips.Length; level++)
        {
            TextureMipLayout source = m_mips[level - 1];
            TextureMipLayout destination = m_mips[level];
            for (int z = 0; z < destination.depth; z++)
            {
                double zStart = (double)z * source.depth / destination.depth;
                double zEnd = (double)(z + 1) * source.depth / destination.depth;
                for (int y = 0; y < destination.height; y++)
                {
                    double yStart = (double)y * source.height / destination.height;
                    double yEnd = (double)(y + 1) * source.height / destination.height;
                    for (int x = 0; x < destination.width; x++)
                    {
                        double xStart = (double)x * source.width / destination.width;
                        double xEnd = (double)(x + 1) * source.width / destination.width;
                        double r = 0d;
                        double g = 0d;
                        double b = 0d;
                        double a = 0d;
                        double weightSum = 0d;
                        int zLimit = Math.Min(source.depth, (int)Math.Ceiling(zEnd));
                        int yLimit = Math.Min(source.height, (int)Math.Ceiling(yEnd));
                        int xLimit = Math.Min(source.width, (int)Math.Ceiling(xEnd));
                        for (int sourceZ = (int)zStart; sourceZ < zLimit; sourceZ++)
                        {
                            double zWeight = Math.Min(sourceZ + 1d, zEnd) - Math.Max(sourceZ, zStart);
                            for (int sourceY = (int)yStart; sourceY < yLimit; sourceY++)
                            {
                                double yWeight = Math.Min(sourceY + 1d, yEnd) - Math.Max(sourceY, yStart);
                                for (int sourceX = (int)xStart; sourceX < xLimit; sourceX++)
                                {
                                    double xWeight = Math.Min(sourceX + 1d, xEnd) - Math.Max(sourceX, xStart);
                                    double weight = xWeight * yWeight * zWeight;
                                    Color color = ReadLinearPixel(source, sourceX, sourceY, sourceZ);
                                    r += color.r * weight;
                                    g += color.g * weight;
                                    b += color.b * weight;
                                    a += color.a * weight;
                                    weightSum += weight;
                                }
                            }
                        }

                        Color average = new Color((float)(r / weightSum),
                            (float)(g / weightSum),
                            (float)(b / weightSum),
                            (float)(a / weightSum));
                        WritePixel(destination, x, y, z, isDataSRGB ? average.gamma : average);
                    }
                }
            }
        }
    }

    protected Span<byte> GetDataSpan(int mipLevel = 0)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        return m_data.AsSpan(mip.offset, mip.size);
    }

    protected Color GetPixelCore(int x, int y, int z, int mipLevel)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        ValidateCoordinates(mip, x, y, z);
        return ReadPixel(mip, x, y, z);
    }

    protected void SetPixelCore(int x, int y, int z, Color color, int mipLevel)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        ValidateCoordinates(mip, x, y, z);
        WritePixel(mip, x, y, z, color);
    }

    protected void CopyPixelsTo(Texture destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        if (destination.dimension != dimension || destination.width != width || destination.height != height ||
            destination.baseDepth != baseDepth || destination.mipmapCount != mipmapCount || destination.isDataSRGB != isDataSRGB)
        {
            throw new ArgumentException("Texture dimensions, mip levels and color space must match.", nameof(destination));
        }

        if (destination.format == format)
        {
            destination.CopyFrom(this);
            return;
        }

        for (int level = 0; level < m_mips.Length; level++)
        {
            TextureMipLayout sourceMip = m_mips[level];
            TextureMipLayout destinationMip = destination.m_mips[level];
            for (int i = 0; i < sourceMip.pixelCount; i++)
            {
                Color color = m_codec.Unpack(m_data.AsSpan(sourceMip.offset + i * bytesPerPixel, bytesPerPixel));
                destination.m_codec.Pack(color,
                    destination.m_data.AsSpan(
                        destinationMip.offset + i * destination.bytesPerPixel,
                        destination.bytesPerPixel));
            }
        }
    }

    protected Color SamplePointCore(float u, float v, float w, int mipLevel, TextureWrapMode wrapMode)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        u = WrapCoordinate(u, wrapMode, nameof(u));
        v = WrapCoordinate(v, wrapMode, nameof(v));
        w = WrapCoordinate(w, wrapMode, nameof(w));
        int x = Math.Min((int)(u * mip.width), mip.width - 1);
        int y = Math.Min((int)(v * mip.height), mip.height - 1);
        int z = Math.Min((int)(w * mip.depth), mip.depth - 1);
        return ReadLinearPixel(mip, x, y, z);
    }

    protected Color SampleLinearCore(float u, float v, float w, int mipLevel, TextureWrapMode wrapMode)
    {
        TextureMipLayout mip = GetMip(mipLevel);
        float x = WrapCoordinate(u, wrapMode, nameof(u)) * mip.width - 0.5f;
        float y = WrapCoordinate(v, wrapMode, nameof(v)) * mip.height - 0.5f;
        float z = WrapCoordinate(w, wrapMode, nameof(w)) * mip.depth - 0.5f;
        int x0 = (int)MathF.Floor(x);
        int y0 = (int)MathF.Floor(y);
        int z0 = (int)MathF.Floor(z);
        float tx = x - x0;
        float ty = y - y0;
        float tz = z - z0;
        int left = AddressPixel(x0, mip.width, wrapMode);
        int right = AddressPixel(x0 + 1, mip.width, wrapMode);
        int bottom = AddressPixel(y0, mip.height, wrapMode);
        int top = AddressPixel(y0 + 1, mip.height, wrapMode);
        int front = AddressPixel(z0, mip.depth, wrapMode);
        Color first = Color.LerpUnclamped(
            Color.LerpUnclamped(ReadLinearPixel(mip, left, bottom, front), ReadLinearPixel(mip, right, bottom, front), tx),
            Color.LerpUnclamped(ReadLinearPixel(mip, left, top, front), ReadLinearPixel(mip, right, top, front), tx),
            ty);
        if (mip.depth == 1)
        {
            return first;
        }

        int back = AddressPixel(z0 + 1, mip.depth, wrapMode);
        Color second = Color.LerpUnclamped(
            Color.LerpUnclamped(ReadLinearPixel(mip, left, bottom, back), ReadLinearPixel(mip, right, bottom, back), tx),
            Color.LerpUnclamped(ReadLinearPixel(mip, left, top, back), ReadLinearPixel(mip, right, top, back), tx),
            ty);
        return Color.LerpUnclamped(first, second, tz);
    }

    protected Color SampleLodCore(float u, float v, float w, float lod, TextureWrapMode wrapMode)
    {
        if (!float.IsFinite(lod))
        {
            throw new ArgumentOutOfRangeException(nameof(lod));
        }

        lod = Math.Clamp(lod, 0f, mipmapCount - 1f);
        int level = (int)lod;
        Color first = SampleLinearCore(u, v, w, level, wrapMode);
        if (level == mipmapCount - 1 || lod == level)
        {
            return first;
        }

        return Color.LerpUnclamped(first, SampleLinearCore(u, v, w, level + 1, wrapMode), lod - level);
    }

    private TextureMipLayout GetMip(int mipLevel)
    {
        if ((uint)mipLevel >= (uint)m_mips.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(mipLevel));
        }

        return m_mips[mipLevel];
    }

    private Color ReadPixel(TextureMipLayout mip, int x, int y, int z)
    {
        int offset = mip.offset + ((z * mip.height + y) * mip.width + x) * bytesPerPixel;
        return m_codec.Unpack(m_data.AsSpan(offset, bytesPerPixel));
    }

    private Color ReadLinearPixel(TextureMipLayout mip, int x, int y, int z)
    {
        Color color = ReadPixel(mip, x, y, z);
        return isDataSRGB ? color.linear : color;
    }

    private void WritePixel(TextureMipLayout mip, int x, int y, int z, Color color)
    {
        int offset = mip.offset + ((z * mip.height + y) * mip.width + x) * bytesPerPixel;
        m_codec.Pack(color, m_data.AsSpan(offset, bytesPerPixel));
    }

    private static void ValidateCoordinates(TextureMipLayout mip, int x, int y, int z)
    {
        if ((uint)x >= (uint)mip.width)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        if ((uint)y >= (uint)mip.height)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if ((uint)z >= (uint)mip.depth)
        {
            throw new ArgumentOutOfRangeException(nameof(z));
        }
    }

    private static float WrapCoordinate(float value, TextureWrapMode wrapMode, string parameterName)
    {
        if (!float.IsFinite(value))
        {
            throw new ArgumentOutOfRangeException(parameterName);
        }

        switch (wrapMode)
        {
            case TextureWrapMode.Clamp:
                return Math.Clamp(value, 0f, 1f);
            case TextureWrapMode.Repeat:
                return value - MathF.Floor(value);
            case TextureWrapMode.Mirror:
                float position = value % 2f;
                if (position < 0f)
                {
                    position += 2f;
                }

                return position <= 1f ? position : 2f - position;
            default:
                throw new ArgumentOutOfRangeException(nameof(wrapMode));
        }
    }

    private static int AddressPixel(int coordinate, int size, TextureWrapMode wrapMode)
    {
        if (wrapMode != TextureWrapMode.Repeat)
        {
            return Math.Clamp(coordinate, 0, size - 1);
        }

        int position = coordinate % size;
        return position < 0 ? position + size : position;
    }
}
