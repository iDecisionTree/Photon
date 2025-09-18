using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Plane : HitableObject
    {
        public Plane(string name = "New Plane") : base(false, name)
        {

        }

        public override bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            Vector3 normal = transform.up;

            float t = Vector3.Dot(transform.position - ray.origin, normal) / Vector3.Dot(ray.direction, normal);
            if (t > Mathf.Epsilon)
            {
                Vector3 point = ray.At(t);
                hitInfo = new HitInfo(t, point, normal);

                return true;
            }

            hitInfo = new HitInfo();
            return false;
        }
    }
}
