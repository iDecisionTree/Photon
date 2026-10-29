using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR16G16SFloat : TextureCodec
{
    public override int bytesPerPixel => 4;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[..2]),
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[2..4]),
            0f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[..2], (Half)color.r);
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[2..4], (Half)color.g);
    }
}
