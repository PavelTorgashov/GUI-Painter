using GridTableBuilder.GridModel.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridTableBuilder.GridModel
{
    public class Node : IDraggable
    {
        /// <summary> Actual location of node </summary>
        public PointF Location => new PointF(OriginalLocation.X + Offset.X, OriginalLocation.Y + Offset.Y);
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
    }
}
