using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using System;

namespace Photon.Editor
{
    public sealed partial class MainWindow : Window
    {
        public const float TARGET_FPS = 60f;

        private CanvasRenderTarget? _renderTarget = null;
        private DispatcherTimer _frameTimer;

        public MainWindow()
        {
            InitializeComponent();

            _frameTimer = new DispatcherTimer();
            _frameTimer.Interval = TimeSpan.FromSeconds(1f / TARGET_FPS);

            Activated += (s, e) => _frameTimer.Start();
            _frameTimer.Tick += (s, e) => FrameTick(s, e);
            Closed += (s, e) => _frameTimer.Stop();
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            _renderTarget?.Dispose();
            _renderTarget = new CanvasRenderTarget(Layout, (float)AppWindow.Size.Width, (float)AppWindow.Size.Height, 96f);
        }

        private void Layout_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            if (_renderTarget == null)
            {
                _renderTarget = new CanvasRenderTarget(sender, (float)AppWindow.Size.Width, (float)AppWindow.Size.Height, 96f);
            }
        }

        private void Layout_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_renderTarget == null)
            {
                return;
            }

            // 预留Draw接口

            args.DrawingSession.DrawImage(_renderTarget);
        }

        private void FrameTick(object? sender, object e)
        {
            if (Layout != null)
            {
                Layout.Invalidate();
            }
        }
    }
}
