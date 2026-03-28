using Photon.Core.RenderPipeline.PipelineStage.Application;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.Forward
{
    public class ForwardRenderPipeline : RenderPipelineBase
    {
        private readonly PreparationStage _preparationStage;
        private readonly ClearingStage _clearingStage;
        private readonly RenderingStage _renderingStage;
        private readonly PresentationStage _presentationStage;

        public ForwardRenderPipeline()
        {
            _preparationStage = new PreparationStage();
            _clearingStage = new ClearingStage();
            _renderingStage = new RenderingStage();
            _presentationStage = new PresentationStage();
        }

        public override void Initialize(Vector2 viewportSize)
        {
            base.Initialize(viewportSize);

            _preparationStage.Initialize();
            _clearingStage.Initialize();
            _renderingStage.Initialize();
            _presentationStage.Initialize();
        }

        public override void OnViewportResize(Vector2 newSize)
        {
            base.OnViewportResize(newSize);
        }

        public override void RenderFrame(RenderContext context)
        {
            base.RenderFrame(context);

            _preparationStage.Execute(context);
            _clearingStage.Execute(context, _frameBuffer);
            _renderingStage.Execute(context, _frameBuffer);
            _presentationStage.Execute(context, _frameBuffer);
        }

        public override void Dispose()
        {
            base.Dispose();

            _preparationStage.Dispose();
            _clearingStage.Dispose();
            _renderingStage.Dispose();
            _presentationStage.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
