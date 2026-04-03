using Photon.Math.Vector;

namespace Photon.Core.Geometry.Fragment
{
    public struct Fragment
    {
        public readonly Vector2 positionSS;
        public Vector4 color;
        public readonly GeometryAttribute[] attributes;
        public readonly Dictionary<string, int> propertyIndexMap;

        public Fragment(Vector2 positionSS, Vector4 color, GeometryAttribute[] attributes, Dictionary<string, int> propertyIndexMap)
        {
            this.positionSS = positionSS;
            this.color = color;
            this.attributes = attributes;
            this.propertyIndexMap = propertyIndexMap;
        }

        public GeometryAttribute this[string name]
        {
            get
            {
                if (propertyIndexMap.TryGetValue(name, out int index))
                {
                    return attributes[index];
                }

                throw new ArgumentException($"片元属性{name}不存在");
            }
        }
    }
}
