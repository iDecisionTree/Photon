using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Math.Vector;

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

                Vector3 positionOS = fragment.attributes[(int)BuildinGeometryAttributeType.positionOS].vector3Value;
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
