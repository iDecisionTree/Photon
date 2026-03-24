using Photon.Math;
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline.PipelineStage.Rasterization
{
    public static class LineRasterizer
    {
        public static void RasterizeLine(FrameBuffer frameBuffer, Vector2 p0, Vector2 p1, Vector4 color)
        {
            int x0 = (int)Mathf.Round(p0.x);
            int y0 = (int)Mathf.Round(p0.y);
            int x1 = (int)Mathf.Round(p1.x);
            int y1 = (int)Mathf.Round(p1.y);

            if (x0 == x1 && y0 == y1)
            {
                frameBuffer.SetColor(x0, y0, color);
            }

            int dx = (int)Mathf.Abs(x1 - x0);
            int dy = (int)Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                frameBuffer.SetColor(x0, y0, color);

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public static void RasterizeThickLine(FrameBuffer frameBuffer, Vector2 p0, Vector2 p1, Vector4 color, int thickness = 2)
        {
            if (thickness <= 1)
            {
                RasterizeLine(frameBuffer, p0, p1, color);
                return;
            }

            Vector2 dir = new Vector2(p1.x - p0.x, p1.y - p0.y);
            dir = Vector2.Normalize(dir);
            Vector2 normal = new Vector2(-dir.y, dir.x);

            for (int i = -thickness / 2; i < thickness / 2; i++)
            {
                Vector2 offset = normal * i;
                RasterizeLine(frameBuffer, p0 + offset, p1 + offset, color);
            }
        }
    }
}
