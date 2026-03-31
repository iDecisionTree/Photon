namespace Photon.Core.Geometry
{
    public readonly struct Primitive
    {
        public readonly Vertex[] vertices;
        public readonly int[] triangles;

        public Primitive(Vertex[] vertices, int[] triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }
    }
}
