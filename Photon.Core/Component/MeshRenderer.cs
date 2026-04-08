using Photon.Core.Geometry;
using Photon.Core.Material;
using Photon.Core.Material.Example;

namespace Photon.Core.Component
{
    public class MeshRenderer : ComponentBase
    {
        public Mesh mesh
        {
            get => _mesh;
            set => _mesh = value;
        }
        public MaterialBase material
        {
            get => _material;
            set => _material = value;
        }

        private Mesh _mesh;
        private MaterialBase _material;

        public MeshRenderer()
        {
            _mesh = new Mesh("Mesh");
            _material = new ExampleMaterial();
        }
    }
}
