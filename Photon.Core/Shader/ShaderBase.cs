using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Core.Material;
using Photon.Math.Vector;

namespace Photon.Core.Shader
{
    public abstract class ShaderBase
    {
        public MaterialBase material { get; set; }

        protected ShaderBase(MaterialBase material)
        {
            this.material = material;
        }

        public abstract void VertexShader(IVertexInput input, out IVertexToFragment output);
        public abstract void FragmentShader(IVertexToFragment input, out Vector4 color);

        public abstract void BindVertexInput(GeometryObject geometryObject, int vertexIndex, out IVertexInput input);
        public abstract void BindVertexToFragment(GeometryObject geometryObject, int vertexIndex, IVertexToFragment output);
        public abstract void BindFragmentInput(Fragment fragment, out IVertexToFragment input);
    }
}
