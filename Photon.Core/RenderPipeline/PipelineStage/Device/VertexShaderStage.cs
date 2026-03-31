using Photon.Core.Geometry;
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
            Matrix4x4 vpMatrix = context.projectionMatrix * context.viewMatrix;
			for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                Matrix4x4 worldMatrix = context.geometryObjects[i].worldMatrix;
                Matrix4x4 mvpMatrix = vpMatrix * context.geometryObjects[i].worldMatrix;
                for (int j = 0; j < context.geometryObjects[i].primitive.vertices.Length; j++)
                {
                    Vector3 positionOS = context.geometryObjects[i].primitive.vertices[j].position;

                    Vector3 positionWS = Matrix4x4.TransformPoint(worldMatrix, positionOS);
                    Vector4 positionCS = Matrix4x4.Transform(mvpMatrix, new Vector4(positionOS, 1f));
                    Vector3 positionNDC = new Vector3(positionCS.x, positionCS.y, positionCS.z) / positionCS.w;
                    Vector2 positionSS = new Vector2((positionNDC.x + 1f) * 0.5f * context.viewport.x, (1f - (positionNDC.y + 1f) * 0.5f) * context.viewport.y);

                    Vector3 normalOS = context.geometryObjects[i].primitive.vertices[j].normal;
                    Vector3 normalWS = Matrix4x4.TransformVector(worldMatrix, normalOS);

                    float depth = positionNDC.z;   

                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.positionOS][j] = new GeometryAttribute(positionOS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.positionWS][j] = new GeometryAttribute(positionWS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.positionCS][j] = new GeometryAttribute(positionCS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.positionNDC][j] = new GeometryAttribute(positionNDC);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.positionSS][j] = new GeometryAttribute(positionSS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.normalOS][j] = new GeometryAttribute(normalOS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.normalWS][j] = new GeometryAttribute(normalWS);
                    context.geometryObjects[i].attributes![(int)BuildinGeometryAttributeType.depth][j] = new GeometryAttribute(depth);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
