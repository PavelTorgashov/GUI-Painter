using System;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    [Serializable]
    class CircleEdgeBuilder : BaseEdgeBuilder
    {
        public EdgeDirection EdgeDirection;

        public CircleEdgeBuilder(Edge edge) : base(edge)
        {
            GoNextDirection();
        }

        public void GoNextDirection()
        {
            //find parallel connected edges
            foreach(var d in GetPossibleDirections(Node1, EdgeDirection))
            {
                var e1 = Node1.GetEdge(d);
                var e2 = Node2.GetEdge(d);
                if (e1 != null && e1 != Edge && e2 != null && e2 != Edge)
                {
                    EdgeDirection = d;
                    break;
                }
            }
        }
        
        public override GraphicsPath GetPath()
        {
            const float CIRCLE_BEZIER_POWER = 0.666666f;
            var len = Node1.LocationF.DistanceTo(Node2.LocationF);
            var dir1 = GetDir(Node1, EdgeDirection);
            var dir2 = GetDir(Node2, EdgeDirection);
            var point1 = Node1.LocationF.Add(dir1.Mul(-CIRCLE_BEZIER_POWER * len));
            var point2 = Node2.LocationF.Add(dir2.Mul(-CIRCLE_BEZIER_POWER * len));

            var path = new GraphicsPath();
            path.AddBezier(Node1.Location, point1, point2, Node2.Location);
            return path;
        }
    }
}
