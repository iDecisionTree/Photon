using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

public sealed class TextureR8G8B8UNorm : TextureCodec
{
    public override int bytesPerPixel => 3;

    protected override Color UnpackCore(ReadOnlySpan<byte> pixelData)
    {
        return new Color(
            pixelData[0] / 255f,
            pixelData[1] / 255f,
            pixelData[2] / 255f);
    }

    protected override void PackCore(Color color, Span<byte> pixelData)
    {
        pixelData[0] = TextureCodecUtility.PackUNorm8(color.r);
        pixelData[1] = TextureCodecUtility.PackUNorm8(color.g);
        pixelData[2] = TextureCodecUtility.PackUNorm8(color.b);
    }
}
