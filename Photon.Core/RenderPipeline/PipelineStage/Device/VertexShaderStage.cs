using Photon.Core.Geometry;
using Photon.Core.Shader;
using Photon.Math;
using Photon.Math.Matrix;
using Photon.Math.Vector;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class VertexShaderStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            ArgumentNullException.ThrowIfNull(context);

            Parallel.For(0, context.geometryObjects.Count, i =>
            {
                GeometryObject geometryObject = context.geometryObjects[i];
                ShaderBase? shader = geometryObject.material.shader;
                if (shader == null)
                {
                    return;
                }

                lock (shader)
                {
                    shader.material = geometryObject.material;
                    geometryObject.material.BindUniform();

                    ShaderUniform[]? uniforms = geometryObject.material.shaderUniforms;
                    int positionWSIndex = GetGeometryPropertyIndex(geometryObject, "positionWS");
                    int positionSSIndex = GetGeometryPropertyIndex(geometryObject, BuildinGeometryPropertyType.PositionSS.ToString());
                    int depthIndex = GetGeometryPropertyIndex(geometryObject, BuildinGeometryPropertyType.Depth.ToString());
                    Matrix4x4? viewProjection = null;
                    if (positionWSIndex >= 0 && uniforms != null)
                    {
                        viewProjection = uniforms[(int)BuildinShaderUniformType.Matrix_P].matrix4x4Value * uniforms[(int)BuildinShaderUniformType.Matrix_V].matrix4x4Value;
                    }

                    for (int j = 0; j < geometryObject.primitive.vertices.Length; j++)
                    {
                        shader.BindVertexInput(geometryObject, j, out IVertexInput input);
                        shader.VertexShader(input, out IVertexToFragment output);
                        shader.BindVertexToFragment(geometryObject, j, output);
                        WriteBuiltInGeometryProperties(context, geometryObject, j, uniforms, viewProjection, positionWSIndex, positionSSIndex, depthIndex);
                    }
                }
            });
        }

        private static void WriteBuiltInGeometryProperties(RenderContext context, GeometryObject geometryObject, int vertexIndex, ShaderUniform[]? uniforms, Matrix4x4? viewProjection, int positionWSIndex, int positionSSIndex, int depthIndex)
        {
            if (uniforms == null)
            {
                return;
            }

            Vector4 positionCS;
            if (positionWSIndex >= 0)
            {
                Vector3 positionWS = geometryObject.properties[positionWSIndex][vertexIndex].vector3Value;
                positionCS = Matrix4x4.Transform(viewProjection!.Value, new Vector4(positionWS, 1f));
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
