using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Core.Scene;
using Photon.Core.Texture;
using Photon.Math.Matrix;
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline
{
    public class RenderContext : IDisposable
    {
        public SceneManager sceneManager { get; set; }
        public Camera camera { get; set; }
        public Vector2 viewport { get; set; }
        public List<GeometryObject> geometryObjects { get; set; }
        public Matrix4x4 viewMatrix { get; set; }
        public Matrix4x4 projectionMatrix { get; set; }
        public Texture2D? renderTarget { get; set; } = null;


        public RenderContext(SceneManager sceneManager, Camera camera, Vector2 viewport)
        {
            this.sceneManager = sceneManager;
            this.camera = camera;
            this.camera.aspect = viewport.x / viewport.y;
            this.viewport = viewport;
            geometryObjects = new List<GeometryObject>();
            renderTarget = new Texture2D((int)viewport.x, (int)viewport.y, TextureFormat.B8G8R8A8_UNorm);
        }

        public void OnViewportResize(Vector2 newSize)
        {
            viewport = newSize;
            camera.aspect = newSize.x / newSize.y;

            renderTarget?.Dispose();
            renderTarget = new Texture2D((int)newSize.x, (int)newSize.y, TextureFormat.B8G8R8A8_UNorm);
        }

        public void Clear()
        {
            geometryObjects.Clear();
        }

        public void Dispose()
        {
            geometryObjects.Clear();
            renderTarget?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
