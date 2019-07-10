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

        public Grid()
        {
            var n1 = new Node(this, new Point(100, 100));
            var n2 = new Node(this, new Point(300, 100));
            var n3 = new Node(this, new Point(300, 200));
            var n4 = new Node(this, new Point(100, 200));
            new Edge(n1, n2);
            new Edge(n2, n3);
            new Edge(n3, n4);
            new Edge(n4, n1);
        }
    }
}
