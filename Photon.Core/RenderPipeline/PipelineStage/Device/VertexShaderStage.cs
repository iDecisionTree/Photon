using Photon.Core.Geometry;
using Photon.Core.Shader;
using Photon.Math;
using Photon.Math.Matrix;
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class VertexShaderStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                GeometryObject geometryObject = context.geometryObjects[i];
                ShaderBase? shader = geometryObject.material.shader;
                if (shader == null)
                {
                    continue;
                }

                shader.material = geometryObject.material;
                geometryObject.material.BindUniform();

                for (int j = 0; j < context.geometryObjects[i].primitive.vertices.Length; j++)
                {
                    shader.BindVertexInput(geometryObject, j, out IVertexInput input);
                    shader.VertexShader(input, out IVertexToFragment output);
                    shader.BindVertexToFragment(geometryObject, j, output);
                    WriteBuiltInGeometryProperties(context, geometryObject, j);
                }
            }
        }

        private static void WriteBuiltInGeometryProperties(RenderContext context, GeometryObject geometryObject, int vertexIndex)
        {
            ShaderUniform[]? uniforms = geometryObject.material.shaderUniforms;
            if (uniforms == null)
            {
                return;
            }

            Vector4 positionCS;
            int positionWSIndex = GetGeometryPropertyIndex(geometryObject, "positionWS");
            if (positionWSIndex >= 0)
            {
                Vector3 positionWS = geometryObject.properties[positionWSIndex][vertexIndex].vector3Value;
                Matrix4x4 view = uniforms[(int)BuildinShaderUniformType.Matrix_V].matrix4x4Value;
                Matrix4x4 projection = uniforms[(int)BuildinShaderUniformType.Matrix_P].matrix4x4Value;
                positionCS = Matrix4x4.Transform(projection * view, new Vector4(positionWS, 1f));
            }
            else
            {
                Vector3 positionOS = geometryObject.primitive.vertices[vertexIndex].position;
                Matrix4x4 mvp = uniforms[(int)BuildinShaderUniformType.Matrix_MVP].matrix4x4Value;
                positionCS = Matrix4x4.Transform(mvp, new Vector4(positionOS, 1f));
            }

            float invW = Mathf.Approximately(positionCS.w, 0f) ? 0f : 1f / positionCS.w;
            float ndcX = positionCS.x * invW;
            float ndcY = positionCS.y * invW;
            float ndcZ = positionCS.z * invW;

            float positionSSX = (ndcX * 0.5f + 0.5f) * (context.viewport.x - 1f);
            float positionSSY = (1f - (ndcY * 0.5f + 0.5f)) * (context.viewport.y - 1f);

            int positionSSIndex = GetGeometryPropertyIndex(geometryObject, BuildinGeometryPropertyType.PositionSS.ToString());
            int depthIndex = GetGeometryPropertyIndex(geometryObject, BuildinGeometryPropertyType.Depth.ToString());

            if (positionSSIndex >= 0)
            {
                geometryObject.properties[positionSSIndex][vertexIndex] = new GeometryProperty(new Vector4(positionSSX, positionSSY, ndcZ, positionCS.w));
            }

            if (depthIndex >= 0)
            {
                geometryObject.properties[depthIndex][vertexIndex] = new GeometryProperty(ndcZ);
            }
        }

        private static int GetGeometryPropertyIndex(GeometryObject geometryObject, string propertyName)
        {
            if (geometryObject.propertyIndexMap.TryGetValue(propertyName, out int index))
            {
                return index;
            }

            foreach (KeyValuePair<string, int> pair in geometryObject.propertyIndexMap)
            {
                if (string.Equals(pair.Key, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Value;
                }
            }

            return -1;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
