using Photon.Core.Geometry;
using Photon.Math.Matrix;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class FragmentShaderStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            for (int i = 0; i < context.fragments.Count; i++)
            {
                Fragment fragment = context.fragments[i];

                Vector3 positionOS = fragment.attributes["positionOS"].vector3Value;
                fragment.color = new Vector4(positionOS, 1f);

                context.fragments[i] = fragment;
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
