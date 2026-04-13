using Photon.Core.Geometry.Fragment;
using Photon.Core.RenderPipeline.PipelineStage.Device;
using Photon.Math;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Application
{
    public class RenderingStage : PipelineStageBase
    {
        private static readonly ParallelOptions s_parallelOptions = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        private readonly InputAssemblerStage _inputAssemblerStage;
        private readonly VertexShaderStage _vertexShaderStage;
        private readonly RasterizationStage _rasterizationStage;
        private readonly FragmentShaderStage _fragmentShaderStage;
        private readonly OutputMergeStage _outputMergeStage;
        private readonly object _tileLocksSync = new();

        private object[] _tileLocks = [];

        public RenderingStage()
        {
            _inputAssemblerStage = new InputAssemblerStage();
            _vertexShaderStage = new VertexShaderStage();
            _rasterizationStage = new RasterizationStage();
            _fragmentShaderStage = new FragmentShaderStage();
            _outputMergeStage = new OutputMergeStage();
        }

        public override void Initialize()
        {
            _inputAssemblerStage.Initialize();
            _vertexShaderStage.Initialize();
            _rasterizationStage.Initialize();
            _fragmentShaderStage.Initialize();
            _outputMergeStage.Initialize();
        }

        /// <summary>
        /// 需要帧缓冲
        /// </summary>
        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (frameBuffer == null)
            {
                throw new ArgumentNullException(nameof(frameBuffer), "帧缓冲不能为空");
            }

            _inputAssemblerStage.Execute(context);
            _vertexShaderStage.Execute(context);

            const int tileShift = 4;
            const int tileSize = 1 << tileShift;
            int frameWidth = frameBuffer.width;
            int frameHeight = frameBuffer.height;
            int tileCountX = (frameWidth + tileSize - 1) / tileSize;
            int tileCountY = (frameHeight + tileSize - 1) / tileSize;

            object[] tileLocks = GetOrCreateTileLocks(tileCountX * tileCountY);

            Parallel.For(0, context.geometryObjects.Count, s_parallelOptions, geometryIndex =>
            {
                _rasterizationStage.Execute(context, context.geometryObjects[geometryIndex], fragment =>
                {
                    Fragment shadedFragment = _fragmentShaderStage.Execute(context, fragment);

                    int pixelX = Mathf.Clamp((int)shadedFragment.positionSS.x, 0, frameWidth - 1);
                    int pixelY = Mathf.Clamp((int)shadedFragment.positionSS.y, 0, frameHeight - 1);

                    int tileX = pixelX >> tileShift;
                    int tileY = pixelY >> tileShift;
                    int tileIndex = tileY * tileCountX + tileX;

                    lock (tileLocks[tileIndex])
                    {
                        _outputMergeStage.Execute(shadedFragment, frameBuffer, pixelX, pixelY);
                    }
                });
            });
        }

        private object[] GetOrCreateTileLocks(int tileCount)
        {
            if (_tileLocks.Length == tileCount)
            {
                return _tileLocks;
            }

            lock (_tileLocksSync)
            {
                if (_tileLocks.Length == tileCount)
                {
                    return _tileLocks;
                }

                object[] tileLocks = new object[tileCount];
                for (int i = 0; i < tileLocks.Length; i++)
                {
                    tileLocks[i] = new object();
                }

                _tileLocks = tileLocks;
                return _tileLocks;
            }
        }

        public override void Dispose()
        {
            _inputAssemblerStage.Dispose();
            _vertexShaderStage.Dispose();
            _rasterizationStage.Dispose();
            _fragmentShaderStage.Dispose();
            _outputMergeStage.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
