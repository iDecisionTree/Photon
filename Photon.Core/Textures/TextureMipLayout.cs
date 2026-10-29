using System.Numerics;

namespace Photon.Core.Textures;

internal readonly struct TextureMipLayout
{
    internal TextureMipLayout(int width, int height, int depth, int offset, int size)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.offset = offset;
        this.size = size;
    }

    internal int width { get; }
    internal int height { get; }
    internal int depth { get; }
    internal int offset { get; }
    internal int size { get; }
    internal int pixelCount => width * height * depth;

    internal static TextureMipLayout[] Create(int width, int height, int depth, int stride, bool mipChain)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stride);

        int maxDimension = Math.Max(Math.Max(width, height), depth);
        int count = mipChain ? BitOperations.Log2((uint)maxDimension) + 1 : 1;
        TextureMipLayout[] result = new TextureMipLayout[count];
        int offset = 0;
        for (int level = 0; level < count; level++)
        {
            int mipWidth = Math.Max(1, width >> level);
            int mipHeight = Math.Max(1, height >> level);
            int mipDepth = Math.Max(1, depth >> level);
            long sliceSize = (long)mipWidth * mipHeight;
            long availablePixels = (Array.MaxLength - (long)offset) / mipDepth / stride;
            if (sliceSize > availablePixels)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "The combined texture dimensions exceed the supported storage size.");
            }

            int size = checked((int)(sliceSize * mipDepth * stride));
            result[level] = new TextureMipLayout(mipWidth, mipHeight, mipDepth, offset, size);
            offset += size;
        }

        return result;
    }
}
