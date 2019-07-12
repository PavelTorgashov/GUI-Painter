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
        PointF topLeft, bottomRight;
        SizeF size;

        class NodeInfo
        {
            public PointF OriginalLocation { get; set; }
            public PointF Offset { get; set; }
            public Node Node { get; set; }
        }
        List<NodeInfo> infos;

        public SizeMarkerDragger(SizeMarker marker)
        {
            this.marker = marker;
        }

        public void Start(MouseController mc)
        {
            this.mc = mc;

            //get my node list
            infos = new List<NodeInfo>();
            foreach (var node in marker.Grid.Nodes)
                infos.Add(new NodeInfo() { Node = node, OriginalLocation = node.OriginalLocation, Offset = node.Offset });

            topLeft = new PointF(infos.Min(n => n.OriginalLocation.X), infos.Min(n => n.OriginalLocation.Y));
            bottomRight = new PointF(infos.Max(n => n.OriginalLocation.X), infos.Max(n => n.OriginalLocation.Y));
            size = new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        private void Mc_MouseUp(MouseEventArgs obj)
        {
            Dispose();
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            float padding = marker.Offset * 2;
            var p = new PointF(e.Location.X, e.Location.Y);
            var dx = p.X - marker.Location.X;
            var dy = p.Y - marker.Location.Y;

            var width = e.Location.X - topLeft.X - padding;
            var height = e.Location.Y - topLeft.Y - padding;
            if (width < padding && dx < 0 || height < padding && dy < 0)
                return;

            var kx = (size.Width + dx) / size.Width;
            var ky = (size.Height + dy) / size.Height;
            foreach (var info in infos)
            {
                var pt = new PointF(info.OriginalLocation.X - topLeft.X, info.OriginalLocation.Y - topLeft.Y);
                if (pt.X < 0) pt.X = 0;
                if (pt.Y < 0) pt.Y = 0;
                info.Node.OriginalLocation = new PointF(topLeft.X + pt.X * kx, topLeft.Y + pt.Y * ky);
                info.Node.Offset = new PointF(info.Offset.X * kx, info.Offset.Y * ky);
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