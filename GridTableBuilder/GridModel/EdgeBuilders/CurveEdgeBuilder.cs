using System;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    [Serializable]
    class CurveEdgeBuilder : BaseEdgeBuilder
    {
        public EdgeDirection EdgeDirection1;
        public EdgeDirection EdgeDirection2;
        public float Radius = 10;

        public CurveEdgeBuilder(Edge edge) : base(edge)
        {
            GoNextDirection();
        }

        public void GoNextDirection()
        {
            //get possible combinations
            GetNextDirection(ref EdgeDirection1, ref EdgeDirection2);
        }

        public override GraphicsPath GetPath()
        {
            const float CIRCLE_BEZIER_POWER = 0.666666f;
            var len = Node1.LocationF.DistanceTo(Node2.LocationF);
            var r = Radius;
            if (r > len / 2) r = len / 2f;

            var dir1 = GetDir(Node1, EdgeDirection1);
            var dir2 = GetDir(Node2, EdgeDirection2);
            var point1 = Node1.LocationF.Add(dir1.Mul(-r));
            var point2 = Node2.LocationF.Add(dir2.Mul(-r));

            var path = new GraphicsPath();
            path.AddBezier(Node1.Location, point1, point2, Node2.Location);
            return path;
        }
    }
}
