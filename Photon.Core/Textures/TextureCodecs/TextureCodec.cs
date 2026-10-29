using System.Runtime.InteropServices;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public abstract class TextureCodec : ITextureCodec
{
    public abstract int bytesPerPixel { get; }

    public Color Unpack(ReadOnlySpan<byte> pixelData)
    {
        if (pixelData.Length < bytesPerPixel)
        {
            throw new ArgumentException("The source does not contain a complete pixel.", nameof(pixelData));
        }

        return UnpackCore(pixelData);
    }

    public void Pack(Color color, Span<byte> pixelData)
    {
        if (pixelData.Length < bytesPerPixel)
        {
            throw new ArgumentException("The destination cannot contain a complete pixel.", nameof(pixelData));
        }

        PackCore(color, pixelData);
    }

    public virtual void UnpackPixels(ReadOnlySpan<byte> pixelData, Span<Color> colors)
    {
        int count = ValidateUnpackPixels(pixelData, colors);
        int stride = bytesPerPixel;
        for (int i = 0; i < count; i++)
        {
            colors[i] = UnpackCore(pixelData.Slice(i * stride, stride));
        }
    }

    public virtual void PackPixels(ReadOnlySpan<Color> colors, Span<byte> pixelData)
    {
        ValidatePackPixels(colors, pixelData);
        int stride = bytesPerPixel;
        for (int i = 0; i < colors.Length; i++)
        {
            PackCore(colors[i], pixelData.Slice(i * stride, stride));
        }
    }

    protected int ValidateUnpackPixels(ReadOnlySpan<byte> pixelData, Span<Color> colors)
    {
        int stride = bytesPerPixel;
        if (pixelData.Length % stride != 0)
        {
            throw new ArgumentException("The source must contain complete pixels.", nameof(pixelData));
        }

        int count = pixelData.Length / stride;
        if (colors.Length < count)
        {
            throw new ArgumentException("The destination is too small.", nameof(colors));
        }

        if (pixelData.Overlaps(MemoryMarshal.AsBytes(colors[..count])))
        {
            throw new ArgumentException("Source and destination must not overlap.", nameof(colors));
        }

        return count;
    }

    protected void ValidatePackPixels(ReadOnlySpan<Color> colors, Span<byte> pixelData)
    {
        long requiredSize = (long)colors.Length * bytesPerPixel;
        if (pixelData.Length < requiredSize)
        {
            throw new ArgumentException("The destination is too small.", nameof(pixelData));
        }

        if (MemoryMarshal.AsBytes(colors).Overlaps(pixelData[..(int)requiredSize]))
        {
            throw new ArgumentException("Source and destination must not overlap.", nameof(pixelData));
        }
    }

    protected abstract Color UnpackCore(ReadOnlySpan<byte> pixelData);

    protected abstract void PackCore(Color color, Span<byte> pixelData);
}
