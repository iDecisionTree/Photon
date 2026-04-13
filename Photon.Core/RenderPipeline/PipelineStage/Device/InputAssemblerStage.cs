using Photon.Core.Geometry;
using Photon.Core.Geometry.Vertex;
using Photon.Math.Vector;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class InputAssemblerStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            ArgumentNullException.ThrowIfNull(context);

            Parallel.For(0, context.geometryObjects.Count, i =>
            {
                context.geometryObjects[i].primitive = Assemble(context.geometryObjects[i].mesh!);
            });
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private Primitive Assemble(Mesh mesh)
        {
            Vertex[] vertices = new Vertex[mesh.vertices.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 position = mesh.vertices[i];
                Vector2 uv = i < mesh.uvs.Count ? mesh.uvs[i] : Vector2.zero;
                Vector3 normal = i < mesh.normals.Count ? mesh.normals[i] : Vector3.zero;

                vertices[i] = new Vertex(position, uv, normal);
            }

            int[] triangles = mesh.triangles.ToArray();

            return new Primitive(vertices, triangles);
        }
    }
}
