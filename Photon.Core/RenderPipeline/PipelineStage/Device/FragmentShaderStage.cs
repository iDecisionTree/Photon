using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Math;
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
            throw new NotSupportedException("未实现的方法");
        }

        public Fragment Execute(RenderContext context, Fragment fragment)
        {
            Light light = context.lights[0];

            Vector3 positionWS = fragment.attributes[(int)BuildinGeometryAttributeType.positionWS].vector3Value;
            Vector3 normalWS = fragment.attributes[(int)BuildinGeometryAttributeType.normalWS].vector3Value;

            float lambert = Vector3.Dot(normalWS, -light.GetLightDirection(positionWS));
            lambert = Mathf.Max(lambert, 0f);
            float halfLambert = Mathf.Pow(lambert * 0.5f + 0.5f, 2f);

            float attenuation = light.GetAttenuation(positionWS);

            fragment.color = new Vector4(halfLambert, halfLambert, halfLambert, 1f);

            return fragment;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
