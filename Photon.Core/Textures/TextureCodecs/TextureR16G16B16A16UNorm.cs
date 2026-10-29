using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR16G16B16A16UNorm : TextureCodec
{
    public override int bytesPerPixel => 8;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[..2]) / 65535f,
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[2..4]) / 65535f,
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[4..6]) / 65535f,
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[6..8]) / 65535f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[..2], TextureCodecUtility.PackUNorm16(color.r));
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[2..4], TextureCodecUtility.PackUNorm16(color.g));
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[4..6], TextureCodecUtility.PackUNorm16(color.b));
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[6..8], TextureCodecUtility.PackUNorm16(color.a));
    }
}
