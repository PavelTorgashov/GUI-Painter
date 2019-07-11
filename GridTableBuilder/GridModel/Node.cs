using GridTableBuilder.GridModel.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GridTableBuilder.GridModel
{
    public class Node : IDraggable
    {
        /// <summary> Actual location of node </summary>
        public Point Location => new Point(OriginalLocation.X + Offset.X, OriginalLocation.Y + Offset.Y);
        /// <summary> Original (design) location </summary>
        public Point OriginalLocation;
        /// <summary> Offset of location relative to OriginalLocation</summary>
        public Point Offset;
        /// <summary> List of edges </summary>
        public readonly List<Edge> Edges = new List<Edge>(4);
        /// <summary> Parent Grid </summary>
        public readonly Grid Grid;

        public Node(Grid grid, Point originalLocation)
        {
            Grid = grid;
            OriginalLocation = originalLocation;
            grid.Nodes.AddLast(this);
        }

        #region IDraggable

        int IDraggable.Priority => 5;

        IDragger IDraggable.GetDragger(Point mousePos)
        {
            const int padding = 3;
            var location = OriginalLocation;
            location.Offset(Offset);
            if (Math.Abs(mousePos.X - location.X) <= padding && Math.Abs(mousePos.Y - location.Y) <= padding)
                return new NodeDragger(this);

            return null;
        }

        #endregion
    }
}
