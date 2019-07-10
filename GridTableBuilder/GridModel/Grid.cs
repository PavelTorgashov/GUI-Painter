using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridTableBuilder.GridModel
{
    /// <summary> Grid </summary>
    public class Grid
    {
        /// <summary> Nodes </summary>
        public LinkedList<Node> Nodes = new LinkedList<Node>();

        /// <summary> All nodes and edges </summary>
        public IEnumerable<object> AllElements
        {
            get
            {
                foreach (var node in Nodes)
                    yield return node;

                foreach (var edge in Nodes.SelectMany(n => n.Edges).Distinct())
                    yield return edge;

                foreach (var track in Nodes.Select(n => n.OriginalLocation.X).Distinct().Select(x => new TrackLine(this, x, false)).OrderBy(t => t.Location))
                    yield return track;

                foreach (var track in Nodes.Select(n => n.OriginalLocation.Y).Distinct().Select(y => new TrackLine(this, y, true)).OrderBy(t => t.Location))
                    yield return track;
            }
        }

        public void RemoveTransitNodes()
        {
            bool removed = false;

            do
            {
                removed = false;

                //remove "transit nodes"
                foreach (var n in Nodes.ToArray())
                {
                    if (n.Edges.Count == 2)
                    if (!(n.Edges[0].IsHorisontal ^ n.Edges[1].IsHorisontal))
                    {
                        var n1 = n.Edges[0].Nodes.First(nn => nn != n);
                        var n2 = n.Edges[1].Nodes.First(nn => nn != n);
                        n1.Edges.Remove(n.Edges[0]);
                        n2.Edges.Remove(n.Edges[1]);
                        new Edge(n1, n2);
                        Nodes.Remove(n);
                        removed = true;
                    }
                }
            } while (removed);
        }

        public void RemoveEmptyNodes()
        {
            foreach (var n in Nodes.ToArray())
            if (n.Edges.Count == 0)
                Nodes.Remove(n);
        }

        public void RemoveFreeEdges()
        {
            bool removed = false;

            do
            {
                removed = false;

                foreach (var e in AllElements.OfType<Edge>().ToArray())
                    if (e.Node1.Edges.Count == 1 || e.Node2.Edges.Count == 1)
                    {
                        //remove edge
                        e.Node1.Edges.Remove(e);
                        e.Node2.Edges.Remove(e);
                        removed = true;
                    }
            } while (removed);
        }

        public Grid()
        {
            //var n1 = new Node(this, new Point(100, 100));
            //var n2 = new Node(this, new Point(300, 100));
            //var n3 = new Node(this, new Point(300, 200));
            //var n4 = new Node(this, new Point(100, 200));

            //new Edge(n1, n2);
            //new Edge(n2, n3);
            //new Edge(n3, n4);
            //new Edge(n4, n1);

            var n1 = new Node(this, new Point(100, 100));
            var n2 = new Node(this, new Point(300, 100));
            var n3 = new Node(this, new Point(300, 200));
            var n4 = new Node(this, new Point(100, 200));

            var n5 = new Node(this, new Point(150, 100));
            var n6 = new Node(this, new Point(150, 200));

            new Edge(n1, n5);
            new Edge(n5, n2);
            new Edge(n2, n3);
            new Edge(n3, n6);
            new Edge(n6, n4);
            new Edge(n4, n1);

            new Edge(n5, n6);
        }
    }
}
