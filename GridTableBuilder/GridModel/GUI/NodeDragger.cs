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
        int minX;
        int maxX;
        int minY;
        int maxY;
        MouseController mc;

        public NodeDragger(Node node)
        {
            this.node = node;
        }

        public void Start(MouseController mc)
        {
            this.mc = mc;

            //calc min and max
            var pointsX = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.X).OrderBy(x => x).ToArray();
            var pointsY = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.Y).OrderBy(y => y).ToArray();

            const int padding = 3;
            minX = pointsX.Where(x => x < node.OriginalLocation.X).LastOrDefault() + padding;
            maxX = pointsX.Where(x => x > node.OriginalLocation.X).FirstOrDefault() - padding;
            if (maxX == -padding)
                maxX = 10000;

            minY = pointsY.Where(y => y < node.OriginalLocation.Y).LastOrDefault() + padding;
            maxY = pointsY.Where(y => y > node.OriginalLocation.Y).FirstOrDefault() - padding;
            if (maxY == -padding)
                maxY = 10000;

            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        private void Mc_MouseUp(MouseEventArgs obj)
        {
            Dispose();
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            var p = e.Location;
            if (p.X < minX) p.X = minX;
            if (p.X > maxX) p.X = maxX;
            if (p.Y < minY) p.Y = minY;
            if (p.Y > maxY) p.Y = maxY;

            node.Offset = new Point(p.X - node.OriginalLocation.X, p.Y - node.OriginalLocation.Y);
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
