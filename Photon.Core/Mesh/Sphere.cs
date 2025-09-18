using System.Numerics;

namespace Photon.Core
{
    public class Sphere : HitableObject
    {
        public Sphere(string name = "New Sphere") : base(false, name)
        {
            
        }

        public override bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            float radius = (transform.scale.X + transform.scale.Y + transform.scale.Z) / 3f;

            Vector3 oc = ray.origin - transform.position;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = 2f * Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = b * b - 4f * a * c;

            if (discriminant < 0)
            {
                hitInfo = new HitInfo();
                return false;
            }

            float sqrtDiscriminant = MathF.Sqrt(discriminant);
            float t1 = (-b - sqrtDiscriminant) / (2f * a);
            float t2 = (-b + sqrtDiscriminant) / (2f * a);
            float t = float.MaxValue;
            if (t1 > Mathf.Epsilon)
            {
                t = t1;
            }
            if (t2 > Mathf.Epsilon && t2 < t)
            {
                t = t2;
            }

            if (t == float.MaxValue)
            {
                hitInfo = new HitInfo();
                return false;
            }

            Vector3 point = ray.At(t);
            Vector3 normal = Vector3.Normalize(point - transform.position);
            hitInfo = new HitInfo(t, point, normal);

            return true;
        }
    }
}
