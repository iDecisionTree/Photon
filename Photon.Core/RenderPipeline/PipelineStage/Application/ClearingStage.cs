
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Application
{
    public class ClearingStage : PipelineStageBase
    {
        public override void Initialize()
        {
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

            frameBuffer.Clear(new Vector4(0f, 0f, 0f, 1));
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
