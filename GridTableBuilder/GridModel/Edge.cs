using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder.GridModel
{
    /// <summary> Edge of grid </summary>
    public class Edge : IDrawable, ISelectable
    {
        public Node Node1;
        public Node Node2;
        public Grid Grid => Node1.Grid;
        public bool IsHorisontal => Node1.OriginalLocation.Y == Node2.OriginalLocation.Y;

        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;

            node1.Edges.Add(this);
            node2.Edges.Add(this);
        }

        GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                path.AddLine(Node1.Location, Node2.Location);
                return path;
            }
        }

        public Node[] Nodes => new Node[] { Node1, Node2 };

        public bool TryRemove()
        {
            if (Grid.Nodes.All(n => n.Edges.Count <= 2))//single cyclomatic graph
                return false;

            //remove edge
            Node1.Edges.Remove(this);
            Node2.Edges.Remove(this);

            //normalize
            Grid.NormalizeAfterEdgeRemoving();

            return true;
        }

        public bool Contains(PointF p)
        {
            if (IsHorisontal)
                return p.Y == Node1.OriginalLocation.Y
                    && p.X >= Math.Min(Node1.OriginalLocation.X, Node2.OriginalLocation.X)
                    && p.X <= Math.Max(Node1.OriginalLocation.X, Node2.OriginalLocation.X);
            else
                return p.X == Node1.OriginalLocation.X
                    && p.Y >= Math.Min(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y)
                    && p.Y <= Math.Max(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y);
        }

        #region IDrawable

        int IDrawable.Priority => 1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(Color.Black, 1))
            {
                if (ps.IsSelected)
                    pen.Color = Color.Blue;

                pen.StartCap = pen.EndCap = LineCap.RoundAnchor;
                gr.DrawPath(pen, Path);
            }
        }

        #endregion

        #region ISelectable

        int ISelectable.Priority => 1;

        bool ISelectable.IsHit(Point mousePos)
        {
            const int padding = 3;
            using (var pen = new Pen(Color.Black, padding * 2))
            {
                return Path.IsOutlineVisible(mousePos, pen);
            }
        }

        #endregion
    }
}
