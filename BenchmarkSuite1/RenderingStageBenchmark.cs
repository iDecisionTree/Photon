using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;
using Photon.Core.Component;
using Photon.Core.Geometry;
using Photon.Core.Material.Example;
using Photon.Core.RenderPipeline;
using Photon.Core.RenderPipeline.PipelineStage.Application;
using Photon.Core.Scene;
using Photon.Math.Vector;

namespace BenchmarkSuite1
{
    [CPUUsageDiagnoser]
    public class RenderingStageBenchmark
    {
        public const int OBJECT_COUNT = 256;

        private static readonly Vector2 Viewport = new Vector2(256f, 256f);

        private RenderContext _context = null!;
        private FrameBuffer _frameBuffer = null!;
        private RenderingStage _renderingStage = null!;

        [GlobalSetup]
        public void Setup()
        {
            _context = CreateContext(OBJECT_COUNT, Viewport);
            _frameBuffer = new FrameBuffer((int)Viewport.x, (int)Viewport.y);
            _renderingStage = new RenderingStage();
            _renderingStage.Initialize();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _renderingStage.Dispose();
            _frameBuffer.Dispose();
            _context.Dispose();
        }

        [Benchmark(Baseline = true)]
        public void Render()
        {
            _frameBuffer.Clear(Vector4.zero);
            _renderingStage.Execute(_context, _frameBuffer);
        }

        private static RenderContext CreateContext(int objectCount, Vector2 viewport)
        {
            SceneManager sceneManager = new SceneManager();
            SceneObject cameraObject = new SceneObject("BenchmarkCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            cameraObject.transform.position = new Vector3(0f, 2f, -10f);

            RenderContext context = new RenderContext(sceneManager, camera, [], viewport);
            for (int i = 0; i < objectCount; i++)
            {
                SceneObject sceneObject = new SceneObject($"Cube_{i}");
                float x = (i % 18 - 9) * 2.0f;
                float z = (i / 18) * 2.5f;
                sceneObject.transform.position = new Vector3(x, 0f, z + 5f);

                Mesh mesh = MeshPrimitive.CreateCube(1f);
                ExampleMaterial material = new ExampleMaterial();
                GeometryObject geometryObject = new GeometryObject(mesh, material);
                geometryObject.Initialize(sceneObject.transform.worldMatrix, camera.viewMatrix, camera.projectionMatrix);
                context.geometryObjects.Add(geometryObject);
            }

            return context;
        }
    }
}