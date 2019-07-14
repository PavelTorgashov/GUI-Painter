using System;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    [Serializable]
    class LineEdgeBuilder : BaseEdgeBuilder
    {
        public LineEdgeBuilder(Edge edge) : base(edge)
        {
        }

        public override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddLine(Node1.Location, Node2.Location);
            return path;
        }
    }
}
