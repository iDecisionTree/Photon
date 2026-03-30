using Photon.Core.RenderPipeline.PipelineStage.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Application
{
    public class RenderingStage : PipelineStageBase
    {
        private readonly InputAssemblerStage _inputAssemblerStage;
        private readonly VertexShaderStage _vertexShaderStage;
        private readonly RasterizationStage _rasterizationStage;
        private readonly FragmentShaderStage _fragmentShaderStage;
        private readonly OutputMergeStage _outputMergeStage;

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
            if (frameBuffer == null)
            {
                throw new ArgumentNullException(nameof(frameBuffer), "帧缓冲不能为空");
            }

            _inputAssemblerStage.Execute(context);
            _vertexShaderStage.Execute(context);
            _rasterizationStage.Execute(context, frameBuffer);
            _fragmentShaderStage.Execute(context);
            _outputMergeStage.Execute(context, frameBuffer);
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
