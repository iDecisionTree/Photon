using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR8UNorm : TextureCodec
{
    public override int bytesPerPixel => 1;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            pixelData[0] / 255f,
            0f,
            0f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        pixelData[0] = TextureCodecUtility.PackUNorm8(color.r);
    }
}
