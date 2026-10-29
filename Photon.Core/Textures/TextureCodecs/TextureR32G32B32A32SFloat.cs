using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR32G32B32A32SFloat : TextureCodec
{
    public override int bytesPerPixel => 16;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        if (BitConverter.IsLittleEndian)
        {
            return MemoryMarshal.Read<Color>(pixelData);
        }

        return new Color(
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[..4]),
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[4..8]),
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[8..12]),
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[12..16]));
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        if (BitConverter.IsLittleEndian)
        {
            MemoryMarshal.Write(pixelData, in color);
            return;
        }

        BinaryPrimitives.WriteSingleLittleEndian(pixelData[..4], color.r);
        BinaryPrimitives.WriteSingleLittleEndian(pixelData[4..8], color.g);
        BinaryPrimitives.WriteSingleLittleEndian(pixelData[8..12], color.b);
        BinaryPrimitives.WriteSingleLittleEndian(pixelData[12..16], color.a);
    }

    public override void UnpackPixels(ReadOnlySpan<byte> pixelData, Span<Color> colors)
    {
        if (!BitConverter.IsLittleEndian)
        {
            base.UnpackPixels(pixelData, colors);
            return;
        }

        int count = ValidateUnpackPixels(pixelData, colors);
        pixelData.CopyTo(MemoryMarshal.AsBytes(colors[..count]));
    }

    public override void PackPixels(ReadOnlySpan<Color> colors, Span<byte> pixelData)
    {
        if (!BitConverter.IsLittleEndian)
        {
            base.PackPixels(colors, pixelData);
            return;
        }

        ValidatePackPixels(colors, pixelData);
        MemoryMarshal.AsBytes(colors).CopyTo(pixelData);
    }
}
