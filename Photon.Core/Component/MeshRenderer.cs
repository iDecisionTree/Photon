using Photon.Core.Geometry;

namespace Photon.Core.Component
{
    public class MeshRenderer : ComponentBase
    {
        public Mesh mesh
        {
            get => _mesh;
            set => _mesh = value;
        }

        private Mesh _mesh;

        public MeshRenderer()
        {
            _mesh = new Mesh("Mesh");
        }
    }
}
