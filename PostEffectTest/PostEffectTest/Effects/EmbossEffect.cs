using SuperfastBlur;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostEffectTest.Effects
{
    public class EmbossEffect : BaseEffect
    {
        public Color Color { get; set; } = Color.White;
        public Color ColorShadow { get; set; } = Color.Black;
        public int Distance { get; set; } = 2;
        public PointF Direction { get; set; } = new PointF(0.7f, 1);

        public override int Priority => 15;

        public override void Render(Graphics gr, GraphicsPath path)
        {
            var color1 = Color;
            var color2 = ColorShadow;
            var light = Direction.Normalized();

            var flatten = (GraphicsPath)path.Clone();
            flatten.Flatten();

            //get points from the path
            var points = flatten.PathPoints;
            var types = flatten.PathTypes;
            var pen = new Pen(Color.Black, Distance + 1);
            pen.StartCap = pen.EndCap = LineCap.Flat;
            //

            void Render(int from, int to)
            {
                for (int i = from; i <= to; i++)
                {
                    var p1 = points[i];
                    var p2 = i == to ? points[from] : points[i + 1];
                    var dir = p2.Sub(p1).Normalized();
                    var n = new PointF(-dir.Y, dir.X);
                    var dot = n.DotScalar(light);
                    var c = GraphicsHelper.Lerp(ColorShadow, Color, (dot + 1) / 2f);
                    pen.Color = c;
                    gr.DrawLine(pen, p1, p2);
                }
            }

            var start = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if ((types[i] == 0xa1 || types[i] == 0xa3 || types[i] == 129))//end of figure
                {
                    if (start >= 0)
                        Render(start, i);
                    start = i + 1;
                }
            }

            if (start < points.Length)
                Render(start, points.Length - 1);

            pen.Dispose();
        }
    }
}
