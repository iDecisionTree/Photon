using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Photon.Core.Textures.TextureCodecs;

namespace Photon.Core.Textures;

public static class TextureFormatInfo
{
    private static readonly FrozenDictionary<TextureFormat, ITextureCodec> s_codecs =
        new Dictionary<TextureFormat, ITextureCodec>
        {
            { TextureFormat.R11G11B10_Float, new TextureR11G11B10Float() },
            { TextureFormat.R8G8B8A8_UNorm, new TextureR8G8B8A8UNorm() },
            { TextureFormat.R32G32B32A32_SFloat, new TextureR32G32B32A32SFloat() },
            { TextureFormat.R8_UNorm, new TextureR8UNorm() },
            { TextureFormat.R8G8_UNorm, new TextureR8G8UNorm() },
            { TextureFormat.R8G8B8_UNorm, new TextureR8G8B8UNorm() },
            { TextureFormat.B8G8R8A8_UNorm, new TextureB8G8R8A8UNorm() },
            { TextureFormat.R16_UNorm, new TextureR16UNorm() },
            { TextureFormat.R16G16_UNorm, new TextureR16G16UNorm() },
            { TextureFormat.R16G16B16A16_UNorm, new TextureR16G16B16A16UNorm() },
            { TextureFormat.R16_SFloat, new TextureR16SFloat() },
            { TextureFormat.R16G16_SFloat, new TextureR16G16SFloat() },
            { TextureFormat.R16G16B16A16_SFloat, new TextureR16G16B16A16SFloat() },
            { TextureFormat.R32_SFloat, new TextureR32SFloat() },
            { TextureFormat.R32G32_SFloat, new TextureR32G32SFloat() },
            { TextureFormat.R32G32B32_SFloat, new TextureR32G32B32SFloat() }
        }.ToFrozenDictionary();

    public static IReadOnlyDictionary<TextureFormat, ITextureCodec> formatInfoMap => s_codecs;

    public static ITextureCodec GetCodec(TextureFormat format)
    {
        if (!s_codecs.TryGetValue(format, out ITextureCodec? codec))
        {
            throw new ArgumentOutOfRangeException(nameof(format), format, "The texture format is not supported.");
        }

        return codec;
    }

    public static bool TryGetCodec(TextureFormat format, [NotNullWhen(true)] out ITextureCodec? codec)
    {
        return s_codecs.TryGetValue(format, out codec);
    }

    public static int GetBytesPerPixel(TextureFormat format)
    {
        return GetCodec(format).bytesPerPixel;
    }

    public static int GetDataSize(int width, int height, TextureFormat format, int depth = 1, bool mipChain = false)
    {
        TextureMipLayout[] levels = TextureMipLayout.Create(width, height, depth, GetBytesPerPixel(format), mipChain);
        TextureMipLayout last = levels[^1];
        return last.offset + last.size;
    }
}
