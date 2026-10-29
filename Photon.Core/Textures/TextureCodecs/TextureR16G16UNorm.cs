using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR16G16UNorm : TextureCodec
{
    public override int bytesPerPixel => 4;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[..2]) / 65535f,
            BinaryPrimitives.ReadUInt16LittleEndian(pixelData[2..4]) / 65535f,
            0f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[..2], TextureCodecUtility.PackUNorm16(color.r));
        BinaryPrimitives.WriteUInt16LittleEndian(pixelData[2..4], TextureCodecUtility.PackUNorm16(color.g));
    }
}
