using GridTableBuilder.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GridTableBuilder.GridModel.GUI
{
    class SizeMarkerDragger : IDragger, IDisposable
    {
        public Cursor Cursor => Cursors.SizeNWSE;

        SizeMarker marker;
        List<Node> nodes;
        MouseController mc;
        PointF topLeft;

        public SizeMarkerDragger(SizeMarker marker)
        {
            this.marker = marker;
        }

        public void Start(MouseController mc)
        {
            this.mc = mc;

            //get my node list
            nodes = marker.Grid.Nodes.ToList();
            topLeft = new PointF(nodes.Min(n => n.OriginalLocation.X), nodes.Min(n => n.OriginalLocation.Y));

            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        private void Mc_MouseUp(MouseEventArgs obj)
        {
            Dispose();
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            const float padding = 12;
            var p = new PointF(e.Location.X, e.Location.Y);

            var dx = p.X - marker.Location.X;
            var dy = p.Y - marker.Location.Y;
            marker.Location.X = p.X;
            marker.Location.Y = p.Y;

            var bottomRight = new PointF(nodes.Max(n => n.OriginalLocation.X), nodes.Max(n => n.OriginalLocation.Y));
            var size = new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            if (size.Width < padding && dx < 0 || size.Height < padding && dy < 0)
                return;
            var kx = (size.Width + dx) / size.Width;
            var ky = (size.Height + dy) / size.Height;
            foreach (var node in nodes)
            {
                var pt = new PointF(node.OriginalLocation.X - topLeft.X, node.OriginalLocation.Y - topLeft.Y);
                if (pt.X < 0) pt.X = 0;
                if (pt.Y < 0) pt.Y = 0;
                node.OriginalLocation = new PointF(topLeft.X + pt.X * kx, topLeft.Y + pt.Y * ky);
                node.Offset = new PointF(node.Offset.X * kx, node.Offset.Y * ky);
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