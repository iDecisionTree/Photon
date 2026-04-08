using Photon.Core.Shader.Example;
using Photon.Math.Vector;

namespace Photon.Core.Material.Example
{
    [Material]
    public partial class ExampleMaterial : MaterialBase
    {
        [MaterialProperty]
        public Vector4 baseColor { get; set; }

        public ExampleMaterial() : base()
        {
            shader = new ExampleShader(this);
            InitializeUniform();
        }
    }
}
