using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder.GridModel
{
    /// <summary> Edge of grid </summary>
    [Serializable]
    public class Edge : IDrawable, ISelectable
    {
        public readonly Node Node1;
        public readonly Node Node2;
        public Grid Grid => Node1.Grid;
        public bool IsHorisontal => Node1.OriginalLocation.Y.Around(Node2.OriginalLocation.Y);

        public BaseEdgeBuilder Builder;

        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;

            node1.Edges.Add(this);
            node2.Edges.Add(this);

            Builder = new LineEdgeBuilder(this);
        }

        public Node GetOtherNode(Node caller)
        {
            return caller == Node1 ? Node2 : Node1;
        }

        GraphicsPath Path => Builder.GetPath();

        public Node[] Nodes => new Node[] { Node1, Node2 };

        public bool TryRemove()
        {
            if (Grid.Nodes.All(n => n.Edges.Count <= 2) && Grid.IsFullyConnected)//single cyclomatic graph
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
                return p.Y.Around(Node1.OriginalLocation.Y)
                    && p.X >= Math.Min(Node1.OriginalLocation.X, Node2.OriginalLocation.X)
                    && p.X <= Math.Max(Node1.OriginalLocation.X, Node2.OriginalLocation.X);
            else
                return p.X.Around(Node1.OriginalLocation.X)
                    && p.Y >= Math.Min(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y)
                    && p.Y <= Math.Max(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y);
        }

        #region Builder stuffs

        //public void BuildStraight()
        //{
        //    Connector1.ConnectedEdge = EdgeDirection.None;
        //    Connector2.ConnectedEdge = EdgeDirection.None;
        //}

        //public void BuildCircle()
        //{
        //    //find parallel connected edges
        //    foreach (var d in GetNextDirection(Node1, Connector1.ConnectedEdge))
        //    {
        //        var e1 = Node1.GetEdge(d);
        //        var e2 = Node2.GetEdge(d);
        //        if (e1 != null && e1 != this && e2 != null && e2 != this )
        //        {
        //            BuildCircle(d);
        //            return;
        //        }
        //    }
        //}

        //private void BuildCircle(EdgeDirection dir)
        //{
        //    Connector1.BezierPower = EdgeConnector.CIRCLE_BEZIER_POWER;
        //    Connector1.ConnectedEdge = dir;
        //    Connector2.BezierPower = EdgeConnector.CIRCLE_BEZIER_POWER;
        //    Connector2.ConnectedEdge = dir;
        //}

        #endregion

        #region IDrawable

        int IDrawable.Priority => 1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(ps.DrawLineColor, 1))
            {
                if (ps.IsSelected)
                    pen.Color = ps.SelectedLineColor;

                if (ps.IsEditMode)
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
