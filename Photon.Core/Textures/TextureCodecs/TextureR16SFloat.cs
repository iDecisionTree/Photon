using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR16SFloat : TextureCodec
{
    public override int bytesPerPixel => 2;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[..2]),
            0f,
            0f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[..2], (Half)color.r);
    }
}
