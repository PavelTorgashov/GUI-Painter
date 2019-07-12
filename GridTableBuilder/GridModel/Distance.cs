using System.Drawing;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    public class Distance : IDrawable
    {
        public readonly float Location;
        public readonly bool IsHorizontal;
        public float Size;
        /// <summary> Parent Grid </summary>
        public readonly Grid Grid;

        public Distance(Grid grid, float location, bool isHorizontal, float size)
        {
            Grid = grid;
            IsHorizontal = isHorizontal;
            Location = location;
            Size = size;
        }

        #region IDrawable

        int IDrawable.Priority => -1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            var loc = IsHorizontal ? new PointF(Location - Size / 2, 10) : new PointF(10, Location - Size / 2);
            using (var font = new Font("Lucida Console", 10))
            using (var brush = new SolidBrush(Color.Silver))
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (IsHorizontal)
                    gr.DrawString($"{(int)Size}px", font, brush, loc, sf);
                else
                {
                    GraphicsState state = gr.Save();
                    gr.TranslateTransform(-loc.X, -loc.Y, MatrixOrder.Append);
                    gr.RotateTransform(-90, MatrixOrder.Append);
                    gr.TranslateTransform(loc.X, loc.Y, MatrixOrder.Append);
                    gr.DrawString($"{Size}px", font, brush, loc, sf);
                    gr.Restore(state);
                }
            }
        }

        #endregion
    }
}
