using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Math.Vector;
using System.Buffers;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class FragmentShaderStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            throw new NotSupportedException("未实现的方法");
        }
        
        public Fragment Execute(RenderContext context, Fragment fragment)
        {
            Vector3 positionOS = fragment.attributes[(int)BuildinGeometryAttributeType.positionOS].vector3Value;
            fragment.color = new Vector4(positionOS, 1f);
            
            return fragment;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
