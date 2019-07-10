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
        int minLocation;
        int maxLocation;
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
            foreach (var node in nodes)
                if (line.IsHorizontal)
                    node.OriginalLocation = new Point(node.OriginalLocation.X, e.Location.Y);
                else
                    node.OriginalLocation = new Point(e.Location.X, node.OriginalLocation.Y);
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
