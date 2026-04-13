using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;
using Photon.Core.Component;
using Photon.Core.RenderPipeline;
using Photon.Core.RenderPipeline.PipelineStage.Application;
using Photon.Core.Scene;
using Photon.Math.Vector;

namespace BenchmarkSuite1
{
    [CPUUsageDiagnoser]
    public class PresentationStageBenchmark
    {
        private static readonly Vector2 Viewport = new Vector2(512f, 512f);

        private RenderContext _context = null!;
        private FrameBuffer _frameBuffer = null!;
        private PresentationStage _presentationStage = null!;

        [GlobalSetup]
        public void Setup()
        {
            _context = CreateContext(Viewport);
            _frameBuffer = new FrameBuffer((int)Viewport.x, (int)Viewport.y);
            _presentationStage = new PresentationStage();
            _presentationStage.Initialize();

            _frameBuffer.Clear(new Vector4(0.8f, 0.6f, 0.4f, 1f));
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _presentationStage.Dispose();
            _frameBuffer.Dispose();
            _context.Dispose();
        }

        [Benchmark(Baseline = true)]
        public void Present()
        {
            _presentationStage.Execute(_context, _frameBuffer);
        }

        private static RenderContext CreateContext(Vector2 viewport)
        {
            SceneManager sceneManager = new SceneManager();
            SceneObject cameraObject = new SceneObject("BenchmarkCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            cameraObject.transform.position = new Vector3(0f, 2f, -10f);

            return new RenderContext(sceneManager, camera, [], viewport);
        }
    }
}
