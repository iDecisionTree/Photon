using Photon.Math.Vector;
using System.Collections.Generic;

namespace Photon.Core.Geometry
{
    public static class MeshPrimitive
    {
        public static Mesh CreateCube(float size = 1.0f)
        {
            Mesh mesh = new Mesh("Cube");
            float half = size * 0.5f;

            mesh.vertices = new List<Vector3>
            {
                new Vector3(-half, -half, half),
                new Vector3(half, -half, half),
                new Vector3(half, half, half),
                new Vector3(-half, half, half),

                new Vector3(-half, -half, -half),
                new Vector3(-half, half, -half),
                new Vector3(half, half, -half),
                new Vector3(half, -half, -half),

                new Vector3(-half, half, -half),
                new Vector3(-half, half, half),
                new Vector3(half, half, half),
                new Vector3(half, half, -half),

                new Vector3(-half, -half, -half),
                new Vector3(half, -half, -half),
                new Vector3(half, -half, half),
                new Vector3(-half, -half, half),

                new Vector3(half, -half, -half),
                new Vector3(half, half, -half),
                new Vector3(half, half, half),
                new Vector3(half, -half, half),

                new Vector3(-half, -half, -half),
                new Vector3(-half, -half, half),
                new Vector3(-half, half, half),
                new Vector3(-half, half, -half)
            };

            mesh.triangles = new List<int>
            {
                0, 2, 1, 0, 3, 2,
                4, 6, 5, 4, 7, 6,
                8, 10, 9, 8, 11, 10,
                12, 14, 13, 12, 15, 14,
                16, 18, 17, 16, 19, 18,
                20, 22, 21, 20, 23, 22
            };

            mesh.uvs = new List<Vector2>
            {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
            };

            mesh.CalculateNormals();
            mesh.CalculateBoundingBox();

            return mesh;
        }
    }
}