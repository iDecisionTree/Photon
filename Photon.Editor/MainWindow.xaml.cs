using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Geometry.Importer;
using Photon.Core.RenderPipeline;
using Photon.Core.RenderPipeline.Forward;
using Photon.Core.Scene;
using Photon.Math;
using Photon.Math.Matrix;
using Photon.Math.Vector;
using System;
using Windows.Foundation;

namespace Photon.Editor
{
    public sealed partial class MainWindow : Window
    {
        public const float TARGET_FPS = 120f;

        private CanvasRenderTarget? _renderTarget = null;
        private DispatcherTimer _frameTimer;

        private ForwardRenderPipeline _renderPipeline;
        private SceneObject _cube;
        private Camera _camera;
        private SceneManager _scene;

        public MainWindow()
        {
            InitializeComponent();

            _frameTimer = new DispatcherTimer();
            _frameTimer.Interval = TimeSpan.FromSeconds(1f / TARGET_FPS);

            _renderPipeline = new ForwardRenderPipeline();

            _renderPipeline.Initialize(new Vector2((float)Mathf.Max(1f, (float)Layout.Size.Width), (float)Mathf.Max(1f, (float)Layout.Size.Height)));

            Activated += (s, e) => _frameTimer.Start();
            _frameTimer.Tick += (s, e) => FrameTick(s, e);
            Closed += (s, e) => _frameTimer.Stop();

            _cube = new SceneObject("Cube");
            MeshRenderer meshRenderer = _cube.AddComponent<MeshRenderer>();
            meshRenderer.mesh = MeshPrimitive.CreateCube(2f);

            SceneObject cameraObj = new SceneObject("Camera");
            _camera = cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = new Vector3(0f, 0f, -5f);

            _scene = new SceneManager();
            _scene.sceneObjects.Add(_cube);
            _scene.sceneObjects.Add(cameraObj);
        }

        private void Layout_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int currentWidth = (int)Mathf.Max(1f, (float)Layout.Size.Width);
            int currentHeight = (int)Mathf.Max(1f, (float)Layout.Size.Height);

            if (_renderTarget == null || _renderTarget.SizeInPixels.Width != currentWidth || _renderTarget.SizeInPixels.Height != currentHeight)
            {
                _renderTarget?.Dispose();
                _renderTarget = new CanvasRenderTarget(Layout, currentWidth, currentHeight, 96f);
                _renderPipeline.OnViewportResize(new Vector2(currentWidth, currentHeight));
            }

            Vector2 viewportSize = new Vector2(currentWidth, currentHeight);
            RenderContext context = new RenderContext(_scene, _camera, viewportSize);

            _renderPipeline.RenderFrame(context);

            _renderTarget.SetPixelBytes(context.renderTarget?.GetData());

            args.DrawingSession.DrawImage(_renderTarget);
        }

        private void FrameTick(object? sender, object e)
        {
            if (Layout != null)
            {
                _cube.transform.Rotate(new Vector3(1f, 1f, 1f));
                Layout.Invalidate();
            }
        }
    }
}