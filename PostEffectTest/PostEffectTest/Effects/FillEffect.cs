using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class FillEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.White;

        public override int Priority => 0;

        public override void Render(Graphics gr, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Color))
            {
                gr.FillPath(brush, path);
            }
        }
    }
}
