namespace Photon.Core
{
    public class MeshRenderer : Component
    {
        public Mesh mesh
        {
            get => mesh;
            set => mesh = value;
        }

        private Mesh _mesh;

        public MeshRenderer()
        {

        }


    }
}
