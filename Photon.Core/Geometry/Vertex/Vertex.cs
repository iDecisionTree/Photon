using Photon.Math.Vector;

namespace Photon.Core.Geometry.Vertex
{
    public readonly struct Vertex
    {
        public readonly Vector3 position;
        public readonly Vector2 uv;
        public readonly Vector3 normal;
        public readonly Vector4 clippedPosition;

        public Vertex(Vector3 position, Vector2 uv, Vector3 normal)
        {
            this.position = position;
            this.uv = uv;
            this.normal = normal;
        }
    }
}
