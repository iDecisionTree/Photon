using System.Numerics;

namespace Photon.Core
{
    public class Mesh : HitableObject
    {
        public string name
        {
            get => _name;
            set => _name = value;
        }

        public Triangle[] triangles
        {
            get => _triangles;
            set => _triangles = value;
        }

        private string _name;
        private Triangle[] _triangles;

        public Triangle this[int index]
        {
            get
            {
                return triangles[index];
            }
            set
            {
                triangles[index] = value;
            }
        }

        public int triangleCount => triangles.Length;

        public Mesh(Triangle[] triangles, string name = "New Mesh")
        {
            _triangles = triangles;
            _name = name;
        }

        public Mesh(Vector3[] vertices, int[] triangles, Vector3[] normals, string name = "New Mesh")
        {
            _triangles = new Triangle[triangles.Length / 3];
            _name = name;

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                Vertex v0 = new Vertex(vertices[triangles[i]], normals[triangles[i]]);
                Vertex v1 = new Vertex(vertices[triangles[i + 1]], normals[triangles[i + 1]]);
                Vertex v2 = new Vertex(vertices[triangles[i + 2]], normals[triangles[i + 2]]);
                _triangles[j] = new Triangle(v0, v1, v2);
            }
        }

        public bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            hitInfo = new HitInfo();
            hitInfo.t = float.MaxValue;

            bool isHit = false;

            for (int i = 0; i < triangles.Length; i++)
            {
                if (triangles[i].Intersect(ray, out HitInfo info) && info.t < hitInfo.t)
                {
                    hitInfo = info;
                    isHit = true;
                }
            }

            return isHit;
        }
    }
}
