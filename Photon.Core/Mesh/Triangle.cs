using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public struct Triangle : HitableObject
    {
        public Vertex v0;
        public Vertex v1;
        public Vertex v2;

        public Triangle(Vertex v0, Vertex v1, Vertex v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        public bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            hitInfo = default;

            Vector3 e1 = v1.position - v0.position;
            Vector3 e2 = v2.position - v0.position;
            Vector3 s1 = Vector3.Cross(ray.direction, e2);
            float det = Vector3.Dot(s1, e1);
            if (Mathf.Abs(det) < Mathf.Epsilon)
            {
                return false;
            }
            float invDet = 1f / det;

            Vector3 s = ray.origin - v0.position;
            float u = Vector3.Dot(s1, s) * invDet;
            if (u < 0f || u > 1f)
            {
                return false;
            }

            Vector3 s2 = Vector3.Cross(s, e1);
            float v = Vector3.Dot(s2, ray.direction) * invDet;
            if (v < 0f || u + v > 1f)
            {
                return false;
            }

            float t = Vector3.Dot(s2, e2) * invDet;
            if (t < Mathf.Epsilon)
            {
                return false;
            }

            Vector3 point = ray.At(t);
            Vector3 normal = Vector3.Normalize((1f - u - v) * v0.normal + u * v1.normal + v * v2.normal);
            hitInfo.t = t;
            hitInfo.point = point;
            hitInfo.normal = normal;

            return true;
        }
    }
}
