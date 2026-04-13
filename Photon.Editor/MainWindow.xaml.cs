using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Lighting;
using Photon.Core.Material.Example;
using Photon.Core.RenderPipeline;
using Photon.Core.RenderPipeline.Forward;
using Photon.Core.Scene;
using Photon.Math;
using Photon.Math.Vector;
using System;
using Windows.Graphics.DirectX;
using Windows.System;
using Windows.UI.Core;

namespace Photon.Editor
{
    public sealed partial class MainWindow : Window
    {
        public const float TARGET_FPS = 120f;

        private CanvasRenderTarget? _renderTarget = null;
        private DispatcherTimer _frameTimer;

        private ForwardRenderPipeline _renderPipeline;
        private RenderContext _renderContext;
        private SceneObject _cube;
        private Camera _camera;
        private Light[] _lights;
        private SceneManager _scene;

        private float _fps;
        private int _frameCount;
        private DateTime _lastTime;

        private readonly float _moveSpeed = 0.5f;
        private readonly float _rotateSpeed = 2f;

        public MainWindow()
        {
            InitializeComponent();

            _frameTimer = new DispatcherTimer();
            _frameTimer.Interval = TimeSpan.FromSeconds(1f / TARGET_FPS);

            _renderPipeline = new ForwardRenderPipeline();
            _renderPipeline.Initialize(new Vector2((float)Mathf.Max(1f, (float)Layout.Size.Width), (float)Mathf.Max(1f, (float)Layout.Size.Height)));

            Activated += (s, e) => _frameTimer.Start();
            Activated += (s, e) => Layout.Focus(FocusState.Programmatic);
            _frameTimer.Tick += (s, e) => FrameTick(s, e);
            Closed += (s, e) => _frameTimer.Stop();

            _cube = new SceneObject("Cube");
            MeshRenderer meshRenderer = _cube.AddComponent<MeshRenderer>();
            meshRenderer.mesh = MeshPrimitive.CreateCube(2f);
            ExampleMaterial material = new ExampleMaterial();
            material.baseColor = new Vector4(0f, 0f, 1f, 1f);
            meshRenderer.material = material;

            SceneObject cameraObj = new SceneObject("Camera");
            _camera = cameraObj.AddComponent<Camera>();
            _camera.far = 1000f;
            cameraObj.transform.position = new Vector3(0f, 3f, -7f);

            _lights = new Light[1];
            SceneObject lightObj = new SceneObject("Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Vector4(1.0f, 0.98f, 0.92f, 1f);
            lightObj.transform.position = new Vector3(2f, 2f, 2f);
            _lights[0] = light;

            _scene = new SceneManager();
            _scene.sceneObjects.Add(_cube);
            _scene.sceneObjects.Add(cameraObj);

            int currentWidth = (int)Mathf.Max(1f, (float)Layout.Size.Width);
            int currentHeight = (int)Mathf.Max(1f, (float)Layout.Size.Height);
            _renderContext = new RenderContext(_scene, _camera, _lights, new Vector2(currentWidth, currentHeight));

            _lastTime = DateTime.Now;
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
                Vector2 newSize = new Vector2(currentWidth, currentHeight);
                _renderTarget?.Dispose();
                _renderTarget = new CanvasRenderTarget(Layout, currentWidth, currentHeight, 96f, DirectXPixelFormat.B8G8R8A8UIntNormalized, CanvasAlphaMode.Ignore);
                _renderPipeline.OnViewportResize(newSize);
                _renderContext.OnViewportResize(newSize);
            }

            _renderPipeline.RenderFrame(_renderContext);
            _renderTarget.SetPixelBytes(_renderContext.renderTarget?.GetData());
            _renderContext.Clear();

            args.DrawingSession.DrawImage(_renderTarget);
        }

        private void FrameTick(object? sender, object e)
        {
            UpdateCameraInput();
            Layout?.Invalidate();
        }

        private void CalculateFps(CanvasControl canvas)
        {
            _frameCount++;
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - _lastTime;

            if (elapsed >= TimeSpan.FromSeconds(0.5))
            {
                _fps = _frameCount / (float)elapsed.TotalSeconds;
                _frameCount = 0;
                _lastTime = now;
            }
        }

        private void UpdateCameraInput()
        {
            bool leftArrow = IsKeyDown(VirtualKey.Left);
            bool rightArrow = IsKeyDown(VirtualKey.Right);
            bool upArrow = IsKeyDown(VirtualKey.Up);
            bool downArrow = IsKeyDown(VirtualKey.Down);

            if (leftArrow)
            {
                _camera.sceneObject!.transform.Rotate(new Vector3(0f, -_rotateSpeed, 0f));
            }
            if (rightArrow)
            {
                _camera.sceneObject!.transform.Rotate(new Vector3(0f, _rotateSpeed, 0f));
            }
            if (upArrow)
            {
                _camera.sceneObject!.transform.Rotate(new Vector3(-_rotateSpeed, 0f, 0f));
            }
            if (downArrow)
            {
                _camera.sceneObject!.transform.Rotate(new Vector3(_rotateSpeed, 0f, 0f));
            }

            bool w = IsKeyDown(VirtualKey.W);
            bool a = IsKeyDown(VirtualKey.A);
            bool s = IsKeyDown(VirtualKey.S);
            bool d = IsKeyDown(VirtualKey.D);
            bool space = IsKeyDown(VirtualKey.Space);
            bool ctrl = IsKeyDown(VirtualKey.Control);

            if (w)
            {
                _camera.sceneObject!.transform.Translate(_camera.sceneObject!.transform.forward * _moveSpeed);
            }
            if (s)
            {
                _camera.sceneObject!.transform.Translate(-_camera.sceneObject!.transform.forward * _moveSpeed);
            }
            if (a)
            {
                _camera.sceneObject!.transform.Translate(-_camera.sceneObject!.transform.right * _moveSpeed);
            }
            if (d)
            {
                _camera.sceneObject!.transform.Translate(_camera.sceneObject!.transform.right * _moveSpeed);
            }
            if (space)
            {
                _camera.sceneObject!.transform.Translate(_camera.sceneObject!.transform.up * _moveSpeed);
            }
            if (ctrl)
            {
                _camera.sceneObject!.transform.Translate(-_camera.sceneObject!.transform.up * _moveSpeed);
            }
        }

        private bool IsKeyDown(VirtualKey key)
        {
            CoreVirtualKeyStates state = InputKeyboardSource.GetKeyStateForCurrentThread(key);
            return (state & CoreVirtualKeyStates.Down) != 0;
        }
    }
}