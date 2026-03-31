using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.RenderPipeline;
using Photon.Core.RenderPipeline.Forward;
using Photon.Core.Scene;
using Photon.Math;
using Photon.Math.Vector;
using System;
using Windows.Graphics.DirectX;

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

        private double _fps;
        private int _frameCount;
        private DateTime _lastTime;

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
            _camera.far = 1000f;
            cameraObj.transform.position = new Vector3(0f, 0f, -5f);

            _scene = new SceneManager();
            _scene.sceneObjects.Add(_cube);
            _scene.sceneObjects.Add(cameraObj);
        }

        private void Layout_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            CalculateFps(sender);

            DispatcherQueue.TryEnqueue(() =>
            {
                FpsText.Text = $"FPS: {_fps:F1}";
            });

            int currentWidth = (int)Mathf.Max(1f, (float)Layout.Size.Width);
            int currentHeight = (int)Mathf.Max(1f, (float)Layout.Size.Height);

            if (_renderTarget == null || _renderTarget.SizeInPixels.Width != currentWidth || _renderTarget.SizeInPixels.Height != currentHeight)
            {
                _renderTarget?.Dispose();
                _renderTarget = new CanvasRenderTarget(Layout, currentWidth, currentHeight, 96f, DirectXPixelFormat.B8G8R8A8UIntNormalized, CanvasAlphaMode.Ignore);
                _renderPipeline.OnViewportResize(new Vector2(currentWidth, currentHeight));
            }

            Vector2 viewportSize = new Vector2(currentWidth, currentHeight);
            RenderContext context = new RenderContext(_scene, _camera, viewportSize);

            _renderPipeline.RenderFrame(context);

            _renderTarget.SetPixelBytes(context.renderTarget?.GetData());
            context.Dispose();

            args.DrawingSession.DrawImage(_renderTarget);
        }

        private void FrameTick(object? sender, object e)
        {
            if (Layout != null)
            {
                _cube.transform.Rotate(new Vector3(5f, 5f, 5f));
                Layout.Invalidate();
            }
        }

        private void CalculateFps(CanvasControl canvas)
        {
            _frameCount++;
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - _lastTime;

            if (elapsed >= TimeSpan.FromSeconds(0.5))
            {
                _fps = _frameCount / elapsed.TotalSeconds;
                _frameCount = 0;
                _lastTime = now;
            }
        }
    }
}