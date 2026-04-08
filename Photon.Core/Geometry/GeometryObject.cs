using Photon.Core.Geometry.Vertex;
using Photon.Core.Material;
using Photon.Core.Shader;
using Photon.Math.Matrix;

namespace Photon.Core.Geometry
{
    public class GeometryObject
    {
        public Mesh mesh { get; set; }
        public MaterialBase material { get; set; }
        public Primitive primitive { get; set; }
        public Dictionary<string, int> propertyIndexMap { get; set; }
        public GeometryProperty[][] properties { get; set; }

        public GeometryObject(Mesh mesh, MaterialBase material)
        {
            this.mesh = mesh;
            this.material = material;
            propertyIndexMap = new Dictionary<string, int>();
            properties = new GeometryProperty[(int)BuildinGeometryPropertyType.Count + mesh.vertexProperties.Count][];
        }

        public void Initialize(Matrix4x4 worldMatrix, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
        {
            int currentIndex = 0;
            foreach (BuildinGeometryPropertyType type in Enum.GetValues<BuildinGeometryPropertyType>())
            {
                if (type == BuildinGeometryPropertyType.Count)
                {
                    continue;
                }

                string name = Enum.GetName(type) ?? type.ToString();
                propertyIndexMap.TryAdd(name, currentIndex);
                properties[currentIndex++] = new GeometryProperty[mesh.vertices.Count];
            }
            foreach (KeyValuePair<string, GeometryProperty[]> kvp in mesh.vertexProperties)
            {
                if (propertyIndexMap.TryAdd(kvp.Key, currentIndex))
                {
                    currentIndex++;
                    properties[currentIndex] = kvp.Value;
                }
            }

            Matrix4x4 mvp = projectionMatrix * viewMatrix * worldMatrix;

            currentIndex = 0;
            foreach (BuildinShaderUniformType type in Enum.GetValues<BuildinShaderUniformType>())
            {
                if (type == BuildinShaderUniformType.Count)
                {
                    continue;
                }

                string name = Enum.GetName(type) ?? type.ToString();
                material.propertyIndexMap.TryAdd(name, currentIndex++);
            }
            material.shaderUniforms![(int)BuildinShaderUniformType.Matrix_M] = new ShaderUniform(worldMatrix);
            material.shaderUniforms![(int)BuildinShaderUniformType.Matrix_V] = new ShaderUniform(viewMatrix);
            material.shaderUniforms![(int)BuildinShaderUniformType.Matrix_P] = new ShaderUniform(projectionMatrix);
            material.shaderUniforms![(int)BuildinShaderUniformType.Matrix_MVP] = new ShaderUniform(mvp);

            material.BindUniform();
        }
    }
}
