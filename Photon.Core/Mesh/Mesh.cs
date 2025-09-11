using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Mesh
    {
        public Triangle[] triangles
        {
            get => _triangles;
            set => _triangles = value;
        }

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

        public Mesh(Vector3[] vertices, int[] triangles, Vector3[] normals)
        {
            _triangles = new Triangle[triangles.Length / 3];

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++) 
            {
                Vertex v0 = new Vertex(vertices[i], normals[i]);
                Vertex v1 = new Vertex(vertices[i + 1], normals[i + 1]);
                Vertex v2 = new Vertex(vertices[i + 2], normals[i + 2]);
                _triangles[j] = new Triangle(v0, v1, v2);
            }
        }
    }
}
