using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Mesh
{
    public class Mesh
    {
        public Vector3[] vertices
        {
            get => _vertices;
            set => _vertices = value;
        }

        public int[] triangles
        {
            get => _triangles;
            set => _triangles = value;
        }
        
        public Vector3[] normals
        {
            get => _normals;
            set => _normals = value;
        }

        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector3[] _normals;

        public Mesh(Vector3[] vertices, int[] triangles, Vector3[] normals)
        {
            _vertices = vertices;
            _triangles = triangles;
            _normals = normals;
        }
    }
}
