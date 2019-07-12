using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    public abstract class BaseEdgeBuilder
    {
        public readonly Edge Edge;
        public Node Node1 => Edge.Node1;
        public Node Node2 => Edge.Node2;

        public BaseEdgeBuilder(Edge edge)
        {
            Edge = edge;
        }

        public abstract GraphicsPath GetPath();

        /// <summary> Get direction vector along some edge </summary>
        protected PointF GetDir(Node node, EdgeDirection dir)
        {
            var e = node.GetEdge(dir);
            if (e == null || e == Edge)
                return PointF.Empty;
            var node2 = e.GetOtherNode(node);
            var res = node2.LocationF.Sub(node.LocationF).Normalized();
            return res;
        }

        protected IEnumerable<EdgeDirection> GetPossibleDirections(Node node)
        {
            for (int d = 1; d <= 4; d++)
            {
                var e = node.GetEdge((EdgeDirection)d);
                if (e != null && e != Edge)
                    yield return (EdgeDirection)d;
            }
        }

        protected IEnumerable<EdgeDirection> GetPossibleDirections(Node node, EdgeDirection after)
        {
            for (int add = 1; add <= 4; add++)
            {
                var d = (EdgeDirection)(((int)after + add) % 5);
                if (d != after)
                {
                    var e = node.GetEdge(d);
                    if (e != null && e != Edge)
                        yield return d;
                }
            }
        }

        protected EdgeDirection GetNextDirection(Node node, EdgeDirection dir)
        {
            for (int add = 1; add <= 4; add++)
            {
                var d = (EdgeDirection)(((int)dir + add) % 5);
                if (d != dir)
                {
                    var e = node.GetEdge(d);
                    if (e != null && e != Edge)
                        return d;
                }
            }

            return dir;
        }

        protected void GetNextDirection(ref EdgeDirection dir1, ref EdgeDirection dir2)
        {
            Tuple<EdgeDirection, EdgeDirection> res = null;
            bool nextIsRes = false;

            foreach (var d1 in GetPossibleDirections(Node1))
            foreach (var d2 in GetPossibleDirections(Node2))
            {
                if (res == null)
                    res = new Tuple<EdgeDirection, EdgeDirection>(d1, d2);
                if (nextIsRes)
                {
                    res = new Tuple<EdgeDirection, EdgeDirection>(d1, d2);
                    goto next;
                }
                if (d1 == dir1 && d2 == dir2)
                    nextIsRes = true;
            }

            next:

            if (res == null)
                res = new Tuple<EdgeDirection, EdgeDirection>(EdgeDirection.None, EdgeDirection.None);

            dir1 = res.Item1;
            dir2 = res.Item2;
        }
    }

    //   1
    // 4 0 2
    //   3
    public enum EdgeDirection
    {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }
}
