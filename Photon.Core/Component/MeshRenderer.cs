namespace Photon.Core
{
    public class MeshRenderer : Component
    {
        public Mesh mesh
        {
            get => _mesh;
            set => _mesh = value;
        }

        private Mesh _mesh;

        public MeshRenderer()
        {
            _mesh = new Mesh();
        }

        internal override void Init(SceneObject sceneObject)
        {
            base.Init(sceneObject);
        }
    }
}
