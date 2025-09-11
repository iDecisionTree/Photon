using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Sphere : HitableObject
    {
        public Vector3 position;
        public float radius;

        public Sphere(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            Vector3 oc = ray.origin - position;
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
            if (t1 > 0f)
            {
                t = t1;
            }
            if (t2 > 0f && t2 < t)
            {
                t = t2;
            }

            if (t == float.MaxValue)
            {
                hitInfo = new HitInfo();
                return false;
            }

            Vector3 point = ray.At(t);
            Vector3 normal = Vector3.Normalize(point - position);
            hitInfo = new HitInfo(t, point, normal);

            return true;
        }
    }
}
