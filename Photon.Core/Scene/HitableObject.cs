namespace Photon.Core
{
    public class HitableObject : SceneObject
    {
        public MeshRenderer meshRenderer => _meshRenderer;

        private bool _isMesh;
        private MeshRenderer _meshRenderer;

        public HitableObject(bool isMesh, string name = "New HitableObject") : base(name)
        {
            if (isMesh)
            {
                _meshRenderer = AddComponent<MeshRenderer>();
            }
        }

        public virtual bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            if (_isMesh)
            {
                return _meshRenderer.mesh.Intersect(ray, out hitInfo);
            }

            hitInfo = new HitInfo();
            return false;
        }
    }
}
