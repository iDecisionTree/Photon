using Photon.Core.Geometry.Fragment;
using Photon.Core.Shader;
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
            ArgumentNullException.ThrowIfNull(context);

            ShaderBase? shader = fragment.material.shader;
            if (shader == null)
            {
                throw new InvalidOperationException("Fragment 对应材质未绑定 Shader");
            }

            shader.material = fragment.material;
            shader.BindFragmentInput(fragment, out IVertexToFragment input);
            shader.FragmentShader(input, out Vector4 color);
            fragment.color = color;

            return fragment;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
