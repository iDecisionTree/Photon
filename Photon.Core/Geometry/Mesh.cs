using Photon.Math.Vector;

namespace Photon.Core.Geometry
{
    public class Mesh
    {
        public string name { get; set; }
        public List<Vector3> vertices { get; set; }
        public List<int> triangles { get; set; }
        public List<Vector2> uvs { get; set; }
        public List<Vector3> normals { get; set; }
        public Dictionary<string, GeometryAttribute[]> vertexAttributes { get; set; }
        public BoundingBox boundingBox { get; set; }

        public Mesh(string name)
        {
            this.name = name;
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
            normals = new List<Vector3>();
            vertexAttributes = new Dictionary<string, GeometryAttribute[]>();
            boundingBox = new BoundingBox();
        }

        public void CalculateBoundingBox()
        {
            if (vertices.Count == 0)
            {
                boundingBox = new BoundingBox();
                return;
            }

            Vector3 min = vertices[0];
            Vector3 max = vertices[0];

            foreach (Vector3 vertex in vertices)
            {
                min = Vector3.Min(min, vertex);
                max = Vector3.Max(max, vertex);
            }

            boundingBox = new BoundingBox(min, max);
        }

        public void CalculateNormals()
        {
            if (triangles.Count % 3 != 0)
            {
                throw new InvalidOperationException("三角形索引必须是3的倍数");
            }

            normals.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                normals.Add(Vector3.zero);
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                int i0 = triangles[i];
                int i1 = triangles[i + 1];
                int i2 = triangles[i + 2];

                Vector3 v0 = vertices[i0];
                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];

                Vector3 edge1 = v1 - v0;
                Vector3 edge2 = v2 - v0;
                Vector3 normal = Vector3.Cross(edge1, edge2);
                normal = Vector3.Normalize(normal);

                normals[i0] += normal;
                normals[i1] += normal;
                normals[i2] += normal;
            }

            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = Vector3.Normalize(normals[i]);
            }
        }

        public void Clear()
        {
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            normals.Clear();
            boundingBox = new BoundingBox();
        }
    }
}
