using SuperfastBlur;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class GlowEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.FromArgb(100, Color.White);
        public int Blur { get; set; } = 5;
        public bool OuterGlow { get; set; } = false;

        public override void Render(Graphics gr, GraphicsPath path)
        {
            var state = gr.Save();
            var padding = Blur * 2 + 1;
            var rect = path.GetBounds();
            var color1 = Color.Transparent;
            var color2 = Color;
            Func<float, float> func = (c) => ((c - 0.5f) * 2);

            if (OuterGlow)
            {
                color1 = Color;
                color2 = Color.Transparent;
                func = (c) => (c * 2);
                padding = Blur * 3 + 1;
            }

            var w = (int)(rect.Width + padding * 2 + 4);
            var h = (int)(rect.Height + padding * 2 + 4);

            using (var bmp = new Bitmap(w, h))
            using (var bmpGr = Graphics.FromImage(bmp))
            {
                bmpGr.SmoothingMode = SmoothingMode.AntiAlias;
                bmpGr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                bmpGr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bmpGr.TranslateTransform(-rect.Location.X + padding , -rect.Location.Y + padding);
                if (Blur > 0)
                {
                    bmpGr.Clear(Color.Black);
                    bmpGr.FillPath(Color.White.Brush(), path);

                    var blur = new GaussianBlurOneChannel(bmp);
                    using (var bmp2 = blur.Process(Blur, color1, color2, func))
                    {
                        if (OuterGlow)
                            gr.ExcludeClip(new Region(path));
                        else
                            gr.SetClip(path);

                        gr.DrawImage(bmp2, rect.Location.X - padding - padding / 10, rect.Location.Y - padding);
                    }
                }
            }

            gr.Restore(state);
            gr.ResetClip();
        }
    }
}
