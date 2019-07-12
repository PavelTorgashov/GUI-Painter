using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            }
        }


        #region Add edges

        public void AddEdges(PointF from, PointF to)
        {
            if (from.X.Around(to.X))
                AddVertEdges(from, to);
            else
                AddHorizEdges(from, to);
        }

        private void AddVertEdges(PointF from, PointF to)
        {
            var fromY = Math.Min(from.Y, to.Y);
            var toY = Math.Max(from.Y, to.Y);

            //find intersected edges
            var edges = AllElements.OfType<Edge>().Where(
                    e => e.IsHorisontal
                    && e.Node1.OriginalLocation.Y >= fromY
                    && e.Node1.OriginalLocation.Y <= toY
                    && e.Contains(new PointF(from.X, e.Node1.OriginalLocation.Y))
                ).OrderBy(e => e.Node1.OriginalLocation.Y).ToArray();
            if (edges.Length < 2)
                return;

            Node prevNode = new Node(this, new PointF(from.X, edges[0].Node1.OriginalLocation.Y));
            DivideEdge(edges[0], prevNode);
            for (int i = 1; i < edges.Length; i++)
            {
                Node node = new Node(this, new PointF(from.X, edges[i].Node1.OriginalLocation.Y));
                DivideEdge(edges[i], node);
                new Edge(prevNode, node);
                prevNode = node;
            }
        }

        private void AddHorizEdges(PointF from, PointF to)
        {
            var fromX = Math.Min(from.X, to.X);
            var toX = Math.Max(from.X, to.X);

            //find intersected edges
            var edges = AllElements.OfType<Edge>().Where(
                    e => !e.IsHorisontal 
                    && e.Node1.OriginalLocation.X >= fromX 
                    && e.Node1.OriginalLocation.X <= toX 
                    && e.Contains(new PointF(e.Node1.OriginalLocation.X, from.Y))
                ).OrderBy(e => e.Node1.OriginalLocation.X).ToArray();
            if (edges.Length < 2)
                return;

            Node prevNode = new Node(this, new PointF(edges[0].Node1.OriginalLocation.X, from.Y));
            DivideEdge(edges[0], prevNode);
            for (int i = 1; i < edges.Length; i++)
            {
                Node node = new Node(this, new PointF(edges[i].Node1.OriginalLocation.X, from.Y));
                DivideEdge(edges[i], node);
                new Edge(prevNode, node);
                prevNode = node;
            }
        }

        void DivideEdge(Edge edge, Node node)
        {
            edge.Node1.Edges.Remove(edge);
            edge.Node2.Edges.Remove(edge);
            new Edge(edge.Node1, node);
            new Edge(edge.Node2, node);
        }

        #endregion

        #region Normalize grid after edge removing

        public void NormalizeAfterEdgeRemoving()
        {
            var removed = false;

            do
            {
                removed = false;

                //remove transit nodes
                if (RemoveTransitNodes())
                    removed = true;

                //remove "free" edges
                if (RemoveFreeEdges())
                    removed = true;
            } while (removed);

            //remove nodes w/o edges
            RemoveEmptyNodes();
        }

        bool RemoveTransitNodes()
        {
            bool removed = false;
            bool result = false;

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
                        result = true;
                    }
                }
            } while (removed);

            return result;
        }

        void RemoveEmptyNodes()
        {
            foreach (var n in Nodes.ToArray())
            if (n.Edges.Count == 0)
                Nodes.Remove(n);
        }

        bool RemoveFreeEdges()
        {
            bool removed = false;
            bool result = false;

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
                        result = true;
                    }
            } while (removed);

            return result;
        }

        #endregion

        public Grid()
        {
            var n1 = new Node(this, new PointF(100, 100));
            var n2 = new Node(this, new PointF(300, 100));
            var n3 = new Node(this, new PointF(300, 200));
            var n4 = new Node(this, new PointF(100, 200));

            new Edge(n1, n2);
            new Edge(n2, n3);
            new Edge(n3, n4);
            new Edge(n4, n1);
        }
    }
}
