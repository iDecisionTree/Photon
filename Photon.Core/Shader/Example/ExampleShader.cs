using Photon.Core.Material.Example;
using Photon.Core.Shader.Generator;
using Photon.Math.Matrix;
using Photon.Math.Vector;

namespace Photon.Core.Shader.Example
{
    public partial class ExampleShader : ShaderBase
    {
        public ExampleShader(ExampleMaterial material) : base(material)
        {
        }

        [ShaderBinding]
        public struct VertexInput : IVertexInput
        {
            public Vector3 positionOS;
            public Vector3 normalOS;
        }

        [ShaderBinding]
        public struct VertexToFragment : IVertexToFragment
        {
            public Vector3 positionWS;
            public Vector3 normalWS;
        }

        public override void VertexShader(IVertexInput input, out IVertexToFragment output)
        {
            VertexInput i = (VertexInput)input;
            VertexToFragment o = new VertexToFragment();
            o.positionWS = Matrix4x4.TransformPoint(u_Matrix_M.matrix4x4Value, i.positionOS);
            o.normalWS = Matrix4x4.TransformVector(u_Matrix_M.matrix4x4Value, i.normalOS);
            output = o;
        }

        public override void FragmentShader(IVertexToFragment input, out Vector4 color)
        {
            color = u_baseColor.vector4Value;
        }
    }
}
