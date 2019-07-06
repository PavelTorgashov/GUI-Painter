using SuperfastBlur;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class InnerGlowEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.FromArgb(100, Color.Black);
        public int Size { get; set; } = 5;
        public PointF Direction { get; set; } = new PointF(1, 0.7f);

        public override void Render(Graphics gr, GraphicsPath path)
        {
            var state = gr.Save();
            var padding = Size * 2 + 1;
            var rect = path.GetBounds();
            var w = (int)(rect.Width + padding * 2 + 4);
            var h = (int)(rect.Height + padding * 2 + 4);

            using (var bmp = new Bitmap(w, h))
            using (var bmpGr = Graphics.FromImage(bmp))
            {
                bmpGr.SmoothingMode = SmoothingMode.AntiAlias;
                bmpGr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                bmpGr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bmpGr.TranslateTransform(-rect.Location.X + padding , -rect.Location.Y + padding);
                if (Size > 0)
                {
                    bmpGr.Clear(Color.Black);
                    bmpGr.FillPath(Color.White.Brush(), path);

                    var blur = new GaussianBlurOneChannel(bmp);
                    Func<float, float> func = (c) => ((c - 0.5f) * 2);
                    using (var bmp2 = blur.Process(Size, Color.Transparent, Color, func))
                    {
                        gr.SetClip(path);
                        gr.DrawImage(bmp2, rect.Location.X - padding - padding / 8, rect.Location.Y - padding);
                    }
                }
            }

            gr.Restore(state);
            gr.ResetClip();
        }
    }
}
