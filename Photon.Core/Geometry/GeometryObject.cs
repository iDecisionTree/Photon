using Photon.Core.Geometry.Vertex;
using Photon.Math.Matrix;

namespace Photon.Core.Geometry
{
    public class GeometryObject
    {
        public Mesh? mesh { get; set; } = null;
        public Primitive primitive { get; set; }
        public Matrix4x4 worldMatrix { get; set; }
        public Dictionary<string, int> propertyIndexMap { get; set; }
        public GeometryAttribute[][]? attributes { get; set; } = null;

        public GeometryObject()
        {
            propertyIndexMap = new Dictionary<string, int>();
        }

        public void Initialize()
        {
            if (mesh == null)
            {
                return;
            }

            int currentIndex = 0;
            List<GeometryAttribute[]> attributeList = new List<GeometryAttribute[]>();
            foreach (BuildinGeometryAttributeType type in Enum.GetValues<BuildinGeometryAttributeType>())
            {
                if (type == BuildinGeometryAttributeType.count)
                {
                    continue;
                }

                string name = Enum.GetName(type) ?? type.ToString();
                propertyIndexMap.TryAdd(name, currentIndex++);

                attributeList.Add(new GeometryAttribute[mesh.vertices.Count]);
            }
            foreach (KeyValuePair<string, GeometryAttribute[]> kvp in mesh.vertexAttributes)
            {
                if (propertyIndexMap.TryAdd(kvp.Key, currentIndex))
                {
                    currentIndex++;
                    attributeList.Add(kvp.Value);
                }
            }

            attributes = attributeList.ToArray();
        }
    }
}
