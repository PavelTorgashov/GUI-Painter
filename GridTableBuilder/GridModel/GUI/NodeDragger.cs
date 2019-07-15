using GridTableBuilder.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GridTableBuilder.GridModel.GUI
{
    class NodeDragger : IDragger, IDisposable
    {
        public Cursor Cursor => Cursors.Hand;

        Node node;
        float minX;
        float maxX;
        float minY;
        float maxY;
        MouseController mc;

        public NodeDragger(Node node)
        {
            this.node = node;
        }

        public void Start(MouseController mc)
        {
            this.mc = mc;

            //calc min and max
            var pointsX = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.X).ToArray();
            var pointsY = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.Y).ToArray();

            minX = pointsX.Min();
            maxX = pointsX.Max();

            minY = pointsY.Min();
            maxY = pointsY.Max();

            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        private void Mc_MouseUp(MouseEventArgs obj)
        {
            Dispose();
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            var p = new PointF(e.Location.X, e.Location.Y);
            if (p.X < minX && node.OriginalLocation.X > minX) p.X = minX;
            if (p.X > maxX && node.OriginalLocation.X < maxX) p.X = maxX;
            if (p.Y < minY && node.OriginalLocation.Y > minY) p.Y = minY;
            if (p.Y > maxY && node.OriginalLocation.Y < maxY) p.Y = maxY;

            // прилипание
            const float padding = 5;
            var dx = node.Grid.GetDistanceToNearestStopPointX(p.X);
            var dy = node.Grid.GetDistanceToNearestStopPointY(p.Y);
            if (Math.Abs(dx) < padding) p.X += dx;
            if (Math.Abs(dy) < padding) p.Y += dy;

            //
            node.Offset = new PointF(p.X - node.OriginalLocation.X, p.Y - node.OriginalLocation.Y);
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

    }
}
