using Photon.Math.Vector;

namespace Photon.Core.Geometry
{
    public struct Fragment
    {
        public readonly Vector2 positionSS;
        public Vector4 color;
        public readonly Dictionary<string, FragmentAttribute> attributes;

        public Fragment(Vector2 positionSS, Vector4 color, Dictionary<string, FragmentAttribute> attributes)
        {
            this.positionSS = positionSS;
            this.color = color;
            this.attributes = attributes;
        }
    }
}
