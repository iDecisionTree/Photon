using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR16G16B16A16SFloat : TextureCodec
{
    public override int bytesPerPixel => 8;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[..2]),
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[2..4]),
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[4..6]),
            (float)BinaryPrimitives.ReadHalfLittleEndian(pixelData[6..8]));
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[..2], (Half)color.r);
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[2..4], (Half)color.g);
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[4..6], (Half)color.b);
        BinaryPrimitives.WriteHalfLittleEndian(pixelData[6..8], (Half)color.a);
    }
}
