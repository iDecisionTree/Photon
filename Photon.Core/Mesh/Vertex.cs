using System.Numerics;

namespace Photon.Core
{
    public struct Vertex
    {
        public Vector3 position;
        public Vector3 normal;

        public Vertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
}
