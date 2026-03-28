using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Scene;
using Photon.Core.Texture;
using Photon.Math.Matrix;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline
{
    public class RenderContext
    {
        public SceneManager sceneManager { get; set; }
        public Camera camera { get; set; }
        public Vector2 viewportSize { get; set; }
        public List<GeometryObject> geometryObjects { get; set; }
        public Matrix4x4 viewMatrix { get; set; }
        public Matrix4x4 projectionMatrix { get; set; }
        public Texture2D? renderTarget { get; set; } = null;

        public RenderContext(SceneManager sceneManager, Camera camera, Vector2 viewportSize)
        {
            this.sceneManager = sceneManager;
            this.camera = camera;
            this.camera.aspect = viewportSize.x / viewportSize.y;
            this.viewportSize = viewportSize;
            geometryObjects = new List<GeometryObject>();
        }
    }
}
