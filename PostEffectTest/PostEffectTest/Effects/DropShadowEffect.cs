using SuperfastBlur;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostEffectTest.Effects
{
    public class DropShadowEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.FromArgb(100, Color.Black);
        public int Size { get; set; } = 5;
        public int Distance { get; set; } = 10;
        public PointF Direction { get; set; } = new PointF(1, 0.7f);

        public override void Render(Graphics gr, GraphicsPath path)
        {
            var padding = Size * 3 + 2;
            var offset = new PointF(Direction.X * Distance, Direction.Y * Distance);
            var rect = path.GetBounds();
            var w = (int)(rect.Width + padding * 2 + 4);
            var h = (int)(rect.Height + padding * 2 + 4);

            using (var bmp = new Bitmap(w, h))
            using (var bmpGr = Graphics.FromImage(bmp))
            {
                bmpGr.SmoothingMode = SmoothingMode.AntiAlias;
                bmpGr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                bmpGr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bmpGr.TranslateTransform(-rect.Location.X + padding, -rect.Location.Y + padding);
                if (Size > 0)
                {
                    bmpGr.Clear(Color.Black);
                    bmpGr.FillPath(Color.White.Brush(), path);

                    var blur = new GaussianBlurOneChannel(bmp);
                    using (var bmp2 = blur.Process(Size, Color))
                        gr.DrawImage(bmp2, rect.Location.X - padding + offset.X, rect.Location.Y - padding + offset.Y);
                } else
                {
                    bmpGr.FillPath(Color.Brush(), path);
                    gr.DrawImage(bmp, rect.Location.X - padding + offset.X, rect.Location.Y - padding + offset.Y);
                }
            }
        }
    }
}
