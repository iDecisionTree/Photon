using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage
{
    public abstract class PipelineStageBase : IDisposable
    {
        public abstract void Initialize();
        public abstract void Execute(RenderContext context, FrameBuffer? frameBuffer = null);
        public abstract void Dispose();
    }
}
