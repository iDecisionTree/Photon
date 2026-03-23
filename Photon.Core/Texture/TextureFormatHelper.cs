using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Photon.Core.Texture
{
    public static class TextureFormatHelper
    {
        private static readonly Dictionary<TextureFormat, TextureFormatInfo> _formatMap = new Dictionary<TextureFormat, TextureFormatInfo>()
        {
            { TextureFormat.R8G8B8A8_UNorm, new Texture_R8G8B8A8_UNorm() },
            { TextureFormat.R11G11B10_UFloat, new Texture_R11G11B10_UFloat() },
        };

        public static bool TryGetFormatInfo(TextureFormat format, out TextureFormatInfo formatInfo)
        {
            return _formatMap.TryGetValue(format, out formatInfo!);
        }
    }
}
