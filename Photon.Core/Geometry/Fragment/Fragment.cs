using Photon.Core.Material;
using Photon.Math.Vector;

namespace Photon.Core.Geometry.Fragment
{
    public struct Fragment
    {
        public readonly Vector2 positionSS;
        public Vector4 color;
        public readonly GeometryProperty[] properties;
        public readonly Dictionary<string, int> propertyIndexMap;
        public readonly MaterialBase material;

        public Fragment(Vector2 positionSS, Vector4 color, GeometryProperty[] properties, Dictionary<string, int> propertyIndexMap, MaterialBase material)
        {
            this.positionSS = positionSS;
            this.color = color;
            this.properties = properties;
            this.propertyIndexMap = propertyIndexMap;
            this.material = material;
        }

        public GeometryProperty this[string name]
        {
            get
            {
                if (propertyIndexMap.TryGetValue(name, out int index))
                {
                    return properties[index];
                }

                throw new ArgumentException($"几何属性{name}不存在");
            }
        }
    }
}
