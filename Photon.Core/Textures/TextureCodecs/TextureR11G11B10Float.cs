using System.Buffers.Binary;
using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR11G11B10Float : TextureCodec
{
    public override int bytesPerPixel => 4;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        uint value = BinaryPrimitives.ReadUInt32LittleEndian(pixelData);
        return new Color(
            TextureCodecUtility.UnpackUnsignedFloat(value & 0x7FFu, 6),
            TextureCodecUtility.UnpackUnsignedFloat((value >> 11) & 0x7FFu, 6),
            TextureCodecUtility.UnpackUnsignedFloat(value >> 22, 5));
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        uint r = TextureCodecUtility.PackUnsignedFloat(color.r, 6);
        uint g = TextureCodecUtility.PackUnsignedFloat(color.g, 6);
        uint b = TextureCodecUtility.PackUnsignedFloat(color.b, 5);
        BinaryPrimitives.WriteUInt32LittleEndian(pixelData, r | (g << 11) | (b << 22));
    }
}
