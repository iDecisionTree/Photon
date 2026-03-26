using Photon.Math.Vector;
using System.Globalization;

namespace Photon.Core.Geometry.Importer
{
    public static class ObjImporter
    {
        public static Mesh Import(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("OBJ文件不存在", filePath);
            }

            Mesh mesh = new Mesh(Path.GetFileNameWithoutExtension(filePath));
            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector2> tempUVs = new List<Vector2>();
            List<Vector3> tempNormals = new List<Vector3>();
            Dictionary<Tuple<int, int, int>, int> vertexMap = new Dictionary<Tuple<int, int, int>, int>();

            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = trimmed.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                {
                    continue;
                }

                switch (parts[0])
                {
                    case "v":
                        ParseVertex(parts, tempVertices);
                        break;
                    case "vt":
                        ParseUV(parts, tempUVs);
                        break;
                    case "vn":
                        ParseNormal(parts, tempNormals);
                        break;
                    case "o":
                        if (parts.Length > 1)
                        {
                            mesh.name = parts[1];
                        }
                        break;
                    case "g":
                        break;
                    case "f":
                        ParseFace(parts, tempVertices, tempUVs, tempNormals, mesh, vertexMap);
                        break;
                }
            }

            mesh.CalculateBoundingBox();

            bool allNormalsZero = true;
            foreach (Vector3 n in mesh.normals)
            {
                if (n != Vector3.zero)
                {
                    allNormalsZero = false;
                    break;
                }
            }
            if (allNormalsZero)
            {
                mesh.CalculateNormals();
            }

            return mesh;
        }

        private static void ParseVertex(string[] parts, List<Vector3> vertices)
        {
            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
            vertices.Add(new Vector3(x, y, z));
        }

        private static void ParseUV(string[] parts, List<Vector2> uvs)
        {
            float u = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float v = float.Parse(parts[2], CultureInfo.InvariantCulture);
            uvs.Add(new Vector2(u, v));
        }

        private static void ParseNormal(string[] parts, List<Vector3> normals)
        {
            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
            normals.Add(new Vector3(x, y, z));
        }

        private static void ParseFace(string[] parts, List<Vector3> tempVertices, List<Vector2> tempUVs, List<Vector3> tempNormals, Mesh mesh, Dictionary<Tuple<int, int, int>, int> vertexMap)
        {
            if (parts.Length < 4)
            {
                return;
            }

            for (int i = 2; i < parts.Length - 1; i++)
            {
                AddFaceVertex(parts[1], tempVertices, tempUVs, tempNormals, mesh, vertexMap);
                AddFaceVertex(parts[i], tempVertices, tempUVs, tempNormals, mesh, vertexMap);
                AddFaceVertex(parts[i + 1], tempVertices, tempUVs, tempNormals, mesh, vertexMap);
            }
        }

        private static void AddFaceVertex(string vertexData, List<Vector3> tempVertices, List<Vector2> tempUVs, List<Vector3> tempNormals, Mesh mesh, Dictionary<Tuple<int, int, int>, int> vertexMap)
        {
            string[] parts = vertexData.Split('/');
            int vIdx = -1;
            if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0]))
            {
                vIdx = int.Parse(parts[0]) - 1;
            }
            int vtIdx = -1;
            if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
            {
                vtIdx = int.Parse(parts[1]) - 1;
            }
            int vnIdx = -1;
            if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
            {
                vnIdx = int.Parse(parts[2]) - 1;
            }

            Tuple<int, int, int> key = Tuple.Create(vIdx, vtIdx, vnIdx);
            int existingIdx;
            if (vertexMap.TryGetValue(key, out existingIdx))
            {
                mesh.triangles.Add(existingIdx);
                return;
            }

            if (vIdx >= 0)
            {
                mesh.vertices.Add(tempVertices[vIdx]);
            }
            else
            {
                mesh.vertices.Add(Vector3.zero);
            }
            if (vtIdx >= 0)
            {
                mesh.uvs.Add(tempUVs[vtIdx]);
            }
            else
            {
                mesh.uvs.Add(Vector2.zero);
            }
            if (vnIdx >= 0)
            {
                mesh.normals.Add(tempNormals[vnIdx]);
            }
            else
            {
                mesh.normals.Add(Vector3.zero);
            }

            int newIdx = mesh.vertices.Count - 1;
            mesh.triangles.Add(newIdx);
            vertexMap[key] = newIdx;
        }
    }
}