using GridTableBuilder.GridModel.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder.GridModel
{
    /// <summary> Node of Grid </summary>
    public class Node : IDraggable, IDrawable, ISelectable
    {
        /// <summary> Actual location of node </summary>
        public Point Location => LocationF.ToPoint();
        /// <summary> Actual location of node </summary>
        public PointF LocationF => OriginalLocation.Add(Offset);
        /// <summary> Original (design) location </summary>
        public PointF OriginalLocation;
        /// <summary> Offset of location relative to OriginalLocation</summary>
        public PointF Offset;
        /// <summary> List of edges </summary>
        public readonly List<Edge> Edges = new List<Edge>(4);
        /// <summary> Parent Grid </summary>
        public readonly Grid Grid;

        public Node(Grid grid, PointF originalLocation)
        {
            Grid = grid;
            OriginalLocation = originalLocation;
            grid.Nodes.AddLast(this);
        }

        public Edge GetEdge(EdgeDirection dir)
        {
            var p1 = Point.Round(OriginalLocation);
            foreach (var e in Edges)
            {
                var p2 = Point.Round(e.GetOtherNode(this).OriginalLocation);
                switch(dir)
                {
                    case EdgeDirection.North: if(p1.X == p2.X && p1.Y > p2.Y) return e; break;
                    case EdgeDirection.East:  if(p1.Y == p2.Y && p1.X < p2.X) return e; break;
                    case EdgeDirection.South: if(p1.X == p2.X && p1.Y < p2.Y) return e; break;
                    case EdgeDirection.West:  if(p1.Y == p2.Y && p1.X > p2.X) return e; break;
                }
            }

            return null;
        }

        GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                var rect = new RectangleF(new PointF(Location.X - 3, Location.Y - 3), new SizeF(6, 6));
                path.AddEllipse(rect);
                return path;
            }
        }

        #region IDraggable

        int IDraggable.Priority => 5;

        IDragger IDraggable.GetDragger(PointF mousePos)
        {
            const int padding = 3;
            var location = OriginalLocation;
            location = PointF.Add(location, new SizeF(Offset.X, Offset.Y));
            if (Math.Abs(mousePos.X - location.X) <= padding && Math.Abs(mousePos.Y - location.Y) <= padding)
                return new NodeDragger(this);

            return null;
        }

        #endregion

        #region IDrawable

        int IDrawable.Priority => 1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            if (ps.IsSelected)
                gr.FillPath(Brushes.Blue, Path);
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
