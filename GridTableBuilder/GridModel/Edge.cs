using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder.GridModel
{
    /// <summary> Edge of grid </summary>
    public class Edge : IDrawable, ISelectable
    {
        public readonly Node Node1;
        public readonly Node Node2;
        public Grid Grid => Node1.Grid;
        public bool IsHorisontal => Node1.OriginalLocation.Y.Around(Node2.OriginalLocation.Y);

        public readonly EdgeConnector Connector1 = new EdgeConnector();
        public readonly EdgeConnector Connector2 = new EdgeConnector();

        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;

            node1.Edges.Add(this);
            node2.Edges.Add(this);
        }

        public Node GetOtherNode(Node caller)
        {
            return caller == Node1 ? Node2 : Node1;
        }

        /// <summary> Get direction vector along some edge </summary>
        PointF GetDir(Node node, EdgeDirection dir)
        {
            var e = node.GetEdge(dir);
            if (e == null || e == this) return PointF.Empty;
            var node2 = e.GetOtherNode(node);
            var res = node2.Location.ToPointF().Sub(node.Location).Normalized();
            return res;
        }

        GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                if (Connector1.ConnectedEdge == EdgeDirection.None && Connector2.ConnectedEdge == EdgeDirection.None)
                {
                    path.AddLine(Node1.Location, Node2.Location);
                } else
                {
                    var len = Node1.Location.ToPointF().DistanceTo(Node2.Location);
                    var dir1 = GetDir(Node1, Connector1.ConnectedEdge);
                    var dir2 = GetDir(Node2, Connector2.ConnectedEdge);
                    var point1 = Node1.Location.ToPointF().Add(dir1.Mul(-Connector1.BezierPower * len));
                    var point2 = Node2.Location.ToPointF().Add(dir2.Mul(-Connector2.BezierPower * len));
                    path.AddBezier(Node1.Location, point1, point2, Node2.Location);
                }
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
                return p.Y.Around(Node1.OriginalLocation.Y)
                    && p.X >= Math.Min(Node1.OriginalLocation.X, Node2.OriginalLocation.X)
                    && p.X <= Math.Max(Node1.OriginalLocation.X, Node2.OriginalLocation.X);
            else
                return p.X.Around(Node1.OriginalLocation.X)
                    && p.Y >= Math.Min(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y)
                    && p.Y <= Math.Max(Node1.OriginalLocation.Y, Node2.OriginalLocation.Y);
        }

        public void BuildStraight()
        {
            Connector1.ConnectedEdge = EdgeDirection.None;
            Connector2.ConnectedEdge = EdgeDirection.None;
        }

        public void BuildCircle()
        {
            var startDir = (int)Connector1.ConnectedEdge;
            //find parallel connected edges
            for (int dir = 1; dir <= 4; dir++)
            {
                var d = (EdgeDirection) ((startDir + dir) % 5);
                var e1 = Node1.GetEdge(d);
                var e2 = Node2.GetEdge(d);
                if (e1 != null && e1 != this && e2 != null && e2 != this )
                {
                    BuildCircle(d);
                    return;
                }
            }
        }

        private void BuildCircle(EdgeDirection dir)
        {
            Connector1.BezierPower = EdgeConnector.CIRCLE_BEZIER_POWER;
            Connector1.ConnectedEdge = dir;
            Connector2.BezierPower = EdgeConnector.CIRCLE_BEZIER_POWER;
            Connector2.ConnectedEdge = dir;
        }

        #region IDrawable

        int IDrawable.Priority => 1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(Color.Black, 1))
            {
                if (ps.IsSelected)
                    pen.Color = Color.Blue;

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

    /// <summary> Encapsulates curvature type of connection to node </summary>
    public class EdgeConnector
    {
        public const float CIRCLE_BEZIER_POWER = 0.666666f;

        public EdgeDirection ConnectedEdge;
        public float BezierPower = 1;
    }

    //   1
    // 4 0 2
    //   3
    public enum EdgeDirection
    {
        None    = 0,
        North   = 1,
        East    = 2,
        South   = 3,
        West    = 4
    }
}
