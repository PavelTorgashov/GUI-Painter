﻿using GridTableBuilder.Controls;
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
            var pointsX = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.X).OrderBy(x => x).ToArray();
            var pointsY = node.Grid.Nodes.Where(n => n != node).Select(n => n.OriginalLocation.Y).OrderBy(y => y).ToArray();

            const float padding = 3;
            minX = pointsX.Where(x => x < node.OriginalLocation.X).LastOrDefault() + padding;
            maxX = pointsX.Where(x => x > node.OriginalLocation.X).FirstOrDefault() - padding;
            if (Math.Abs(-padding - maxX) < 0.0001)
                maxX = 10000;

            minY = pointsY.Where(y => y < node.OriginalLocation.Y).LastOrDefault() + padding;
            maxY = pointsY.Where(y => y > node.OriginalLocation.Y).FirstOrDefault() - padding;
            if (Math.Abs(-padding - maxY) < 0.0001)
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
            var p = new PointF(e.Location.X, e.Location.Y);
            if (p.X < minX) p.X = minX;
            if (p.X > maxX) p.X = maxX;
            if (p.Y < minY) p.Y = minY;
            if (p.Y > maxY) p.Y = maxY;
            var offset = new PointF(p.X - node.OriginalLocation.X, p.Y - node.OriginalLocation.Y);
            // прилипание
            const float padding = 5;
            if (Math.Abs(offset.X) < padding) offset.X = 0;
            if (Math.Abs(offset.Y) < padding) offset.Y = 0;
            node.Offset = offset;
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
