using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class StrokeEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.Black;
        public int Distance { get; set; } = 0;

        public override int Priority => 5;

        public override void Render(Graphics gr, GraphicsPath path)
        {
            using (var pen = new Pen(Color, Distance + 1))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                gr.DrawPath(pen, path);
            }
        }
    }
}
