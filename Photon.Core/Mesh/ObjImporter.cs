using System.Globalization;
using System.Numerics;

namespace Photon.Core
{
    public class ObjImporter
    {
        public static Mesh Import(string path, bool useObjNormals = true)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var meshTriangles = new List<int>();
            var meshNormals = new List<Vector3>();

            foreach (var line in File.ReadLines(path))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("v "))
                {
                    var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Vector3(x, y, z));
                }
                else if (trimmed.StartsWith("vn "))
                {
                    var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    normals.Add(Vector3.Normalize(new Vector3(x, y, z)));
                }
                else if (trimmed.StartsWith("f "))
                {
                    var parts = trimmed.Substring(2).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 3) continue;
                    int[] vIdx = new int[3];
                    int[] nIdx = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        var indices = parts[i].Split('/');
                        vIdx[i] = int.Parse(indices[0]) - 1;
                        if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2]))
                            nIdx[i] = int.Parse(indices[2]) - 1;
                        else
                            nIdx[i] = -1;
                    }
                    meshTriangles.Add(vIdx[0]);
                    meshTriangles.Add(vIdx[1]);
                    meshTriangles.Add(vIdx[2]);
                    for (int i = 0; i < 3; i++)
                    {
                        if (nIdx[i] >= 0 && nIdx[i] < normals.Count)
                            meshNormals.Add(normals[nIdx[i]]);
                        else
                            meshNormals.Add(Vector3.Zero);
                    }
                }
            }

            Vector3[] finalNormals = new Vector3[vertices.Count];
            for (int i = 0; i < meshTriangles.Count; i++)
            {
                int vIdx = meshTriangles[i];
                if (i < meshNormals.Count)
                    finalNormals[vIdx] = meshNormals[i];
            }

            return new Mesh(vertices.ToArray(), meshTriangles.ToArray(), finalNormals);
        }
    }
}
