using SuperfastBlur;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class BevelEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.FromArgb(100, Color.White);
        public Color ColorShadow { get; set; } = Color.FromArgb(100, Color.Black);
        public int Blur { get; set; } = 2;
        public int Distance { get; set; } = 5;
        public PointF Direction { get; set; } = new PointF(0.7f, 1);

        public override void Render(Graphics gr, GraphicsPath path)
        {
            var state = gr.Save();
            var padding = Blur * 2 + 1 + Distance;
            var rect = path.GetBounds();
            var offset = new PointF(Direction.X * Distance, Direction.Y * Distance);
            var color1 = Color.Transparent;
            var color2 = Color;
            var color3 = ColorShadow;
            Func<float, float> func = null;// (c) => ((c - 0.5f) * 2);

            gr.SetClip(path);

            var w = (int)(rect.Width + padding * 2 + 4);
            var h = (int)(rect.Height + padding * 2 + 4);

            using (var bmp = new Bitmap(w, h))
            using (var bmpGr = Graphics.FromImage(bmp))
            {
                bmpGr.SmoothingMode = SmoothingMode.AntiAlias;
                bmpGr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                bmpGr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bmpGr.TranslateTransform(-rect.Location.X + padding, -rect.Location.Y + padding);
                bmpGr.Clear(Color.Black);
                bmpGr.FillPath(Color.White.Brush(), path);

                var blur = new GaussianBlurOneChannel(bmp);
                using (var bmp2 = blur.Process(Blur, color1, color3, func))
                    gr.DrawImage(bmp2, rect.Location.X - padding - padding / 10 - offset.X, rect.Location.Y - padding - offset.Y);
                using (var bmp2 = blur.Process(Blur, color1, color2, func))
                    gr.DrawImage(bmp2, rect.Location.X - padding - padding / 10 + offset.X, rect.Location.Y - padding + offset.Y);
            }

            gr.Restore(state);
            gr.ResetClip();
        }
    }
}
