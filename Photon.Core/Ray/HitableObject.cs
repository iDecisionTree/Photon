namespace Photon.Core
{
    public interface HitableObject
    {
        public abstract bool Intersect(Ray ray, out HitInfo hitInfo);
    }
}
