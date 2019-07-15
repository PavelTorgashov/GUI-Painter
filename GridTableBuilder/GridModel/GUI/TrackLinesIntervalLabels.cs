using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder.GridModel
{
    /// <summary> Draws intervals between track lines </summary>
    public class TrackLinesIntervalLabels : IDrawable
    {
        /// <summary> Parent Grid </summary>
        public readonly Grid Grid;

        public TrackLinesIntervalLabels(Grid grid)
        {
            Grid = grid;
        }

        #region IDrawable

        int IDrawable.Priority => -1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var font = new Font("Lucida Console", 8))
            using (var brush = new SolidBrush(ps.ServiceLineColor))
            {
                var sf = (StringFormat)StringFormat.GenericTypographic.Clone();
                sf.Alignment = StringAlignment.Center;
                sf.FormatFlags |= StringFormatFlags.NoWrap;

                //horiz
                var points = Grid.Nodes.Select(n => n.OriginalLocation.X).Distinct().OrderBy(x => x).ToArray();
                for (int i = 0; i < points.Length - 1; i++)
                {
                    var size = points[i + 1] - points[i];
                    var text = $"{Math.Round(size):0.}";
                    if (size >= 40) text += "px";
                    var rect = new RectangleF(points[i], 0, size, 20);
                    var ts = gr.MeasureString(text, font);
                    if (ts.Width > rect.Width)
                        using (var f = new Font("Lucida Console", 6))
                            gr.DrawString(text, f, brush, rect, sf);
                    else
                        gr.DrawString(text, font, brush, rect, sf);
                }

                //vert
                points = Grid.Nodes.Select(n => n.OriginalLocation.Y).Distinct().OrderBy(y => y).ToArray();
                for (int i = 0; i < points.Length - 1; i++)
                {
                    var size = points[i + 1] - points[i];
                    var text = $"{Math.Round(size):0.}";
                    if (size >= 40)
                        text += "px";
                    var loc = new PointF(0, points[i]);
                    var rect = new RectangleF(-size, 0, size, 20);

                    GraphicsState state = gr.Save();
                    gr.RotateTransform(-90, MatrixOrder.Append);
                    gr.TranslateTransform(loc.X, loc.Y, MatrixOrder.Append);
                    var ts = gr.MeasureString(text, font);
                    if (ts.Width > rect.Width)
                        using (var f = new Font("Lucida Console", 6))
                            gr.DrawString(text, f, brush, rect, sf);
                    else
                        gr.DrawString(text, font, brush, rect, sf);
                    gr.Restore(state);
                }
            }
        }

        #endregion
    }
}
