using Photon.Core.Mathematics;

namespace Photon.Core.Textures;

public interface ITextureCodec
{
    int bytesPerPixel { get; }

    Color Unpack(ReadOnlySpan<byte> pixelData);

    void Pack(Color color, Span<byte> pixelData);

    void UnpackPixels(ReadOnlySpan<byte> pixelData, Span<Color> colors);

    void PackPixels(ReadOnlySpan<Color> colors, Span<byte> pixelData);
}
