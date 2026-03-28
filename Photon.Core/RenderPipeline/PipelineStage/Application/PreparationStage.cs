using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Math.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    GeometryObject geometry = new GeometryObject();
                    geometry.mesh = mr.mesh;
                    geometry.worldMatrix = mr.sceneObject!.transform.worldMatrix;
                    geometry.Initialize();

                    context.geometryObjects.Add(geometry);
                }
            }

            context.viewMatrix = context.camera.viewMatrix;
            context.projectionMatrix = context.camera.projectionMatrix;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
