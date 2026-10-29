using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR32G32SFloat : TextureCodec
{
    public override int bytesPerPixel => 8;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[..4]),
            BinaryPrimitives.ReadSingleLittleEndian(pixelData[4..8]),
            0f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteSingleLittleEndian(pixelData[..4], color.r);
        BinaryPrimitives.WriteSingleLittleEndian(pixelData[4..8], color.g);
    }
}
