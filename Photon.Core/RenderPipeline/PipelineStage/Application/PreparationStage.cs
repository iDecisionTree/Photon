using Photon.Core.Component;
using Photon.Core.Geometry;

namespace Photon.Core.RenderPipeline.PipelineStage.Application
{
    public class PreparationStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            for (int i = 0; i < context.sceneManager.sceneObjects.Count; i++)
            {
                if (context.sceneManager.sceneObjects[i].isActive && context.sceneManager.sceneObjects[i].GetComponent<MeshRenderer>() is MeshRenderer mr)
                {
                    GeometryObject geometry = new GeometryObject(mr.mesh, mr.material);
                    geometry.mesh = mr.mesh;
                    geometry.material = mr.material;
                    geometry.Initialize(mr.sceneObject!.transform.worldMatrix, context.camera.viewMatrix, context.camera.projectionMatrix);

                    context.geometryObjects.Add(geometry);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
