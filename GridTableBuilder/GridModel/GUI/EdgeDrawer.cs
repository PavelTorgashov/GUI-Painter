using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GridTableBuilder.Controls;

namespace GridTableBuilder.GridModel.GUI
{
    class EdgeDrawer : IDisposable
    {
        public Cursor Cursor => Cursors.Hand;

        MouseController mc;
        PointF From;
        PointF To;
        PointF currentPoint;
        public Grid Grid;

        public void Start(MouseController mc, Grid grid)
        {
            this.mc = mc;
            this.Grid = grid;
            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        private void Mc_MouseUp(MouseEventArgs obj)
        {
            if (From != Point.Empty && To != Point.Empty)
                Grid.AddEdges(From, To);
            Dispose();
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            //get orientation
            var loc = e.Location;
            var isHorizontal = Math.Abs(loc.X - mc.MouseDownPoint.X) > Math.Abs(loc.Y - mc.MouseDownPoint.Y);

            //get track points
            var points = Grid.Nodes.Select(n => isHorizontal ? n.OriginalLocation.X : n.OriginalLocation.Y).OrderBy(x => x).ToArray();

            //calc From and To points
            var fromLoc = isHorizontal ? Math.Min(loc.X, mc.MouseDownPoint.X) : Math.Min(loc.Y, mc.MouseDownPoint.Y);
            var toLoc = isHorizontal ? Math.Max(loc.X, mc.MouseDownPoint.X) : Math.Max(loc.Y, mc.MouseDownPoint.Y);
            var from = points.Where(x => x <= fromLoc).LastOrDefault();
            var to = points.Where(x => x >= toLoc).FirstOrDefault();
            if (Math.Abs(from) < 0.0001 || Math.Abs(to) < 0.0001)
            {
                From = To = Point.Empty;
                return;
            } else
            {
                if (isHorizontal)
                {
                    From = new PointF(from, mc.MouseDownPoint.Y);
                    To = new PointF(to, mc.MouseDownPoint.Y);
                } else
                {
                    From = new PointF(mc.MouseDownPoint.X, from);
                    To = new PointF(mc.MouseDownPoint.X, to);
                }
            }
        }

        public void Dispose()
        {
            if (mc != null)
            {
                mc.MouseUp -= Mc_MouseUp;
                mc.MouseMove -= Mc_MouseMove;
                mc = null;
            }
        }

        static float[] pattern = new float[] { 5, 3 };

        public void Draw(Graphics gr, DrawParams ps)
        {
            if (mc == null)
                return;

            if (From != Point.Empty && To != Point.Empty)
            {
                using (var pen = new Pen(Color.Black, 1))
                {
                    pen.DashPattern = pattern;
                    gr.DrawLine(pen, From, To);
                }
            }

            //gr.FillEllipse(Brushes.Blue, mc.MouseDownPoint.X - 2, mc.MouseDownPoint.Y - 2, 4, 4);
        }
    }
}
