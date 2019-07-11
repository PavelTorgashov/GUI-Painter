using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridTableBuilder.Controls;

namespace GridTableBuilder.GridModel.GUI
{
    class TrackLineDragger : IDragger, IDisposable
    {
        public Cursor Cursor => line.IsHorizontal ? Cursors.HSplit : Cursors.VSplit;

        TrackLine line;
        float minLocation;
        float maxLocation;
        List<Node> nodes;
        MouseController mc;

        public TrackLineDragger(TrackLine line)
        {
            this.line = line;
        }

        public void Start(MouseController mc)
        {
            this.mc = mc;

            //calc min and max
            var points = line.Grid.Nodes.Select(n => line.IsHorizontal ? n.OriginalLocation.Y : n.OriginalLocation.X).OrderBy(x => x).ToArray();
            const float padding = 3;
            minLocation = points.Where(x => x < line.Location).LastOrDefault() + padding;
            maxLocation = points.Where(x => x > line.Location).FirstOrDefault() - padding;
            if (maxLocation == -padding)
                maxLocation = 10000;

            //get my node list
            nodes = new LinkedList<Node>(line.Grid.Nodes.Where(n => line.IsHorizontal ? n.OriginalLocation.Y == line.Location : n.OriginalLocation.X == line.Location)).ToList();
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

            foreach (var node in nodes)
            {
                var loc = node.OriginalLocation;
                if (line.IsHorizontal)
                {
                    if (p.Y < minLocation) p.Y = minLocation;
                    if (p.Y > maxLocation) p.Y = maxLocation;
                    loc.Y = p.Y;
                } else
                {
                    if (p.X < minLocation) p.X = minLocation;
                    if (p.X > maxLocation) p.X = maxLocation;
                    loc.X = p.X;
                }
                node.OriginalLocation = loc;
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
    }
}
