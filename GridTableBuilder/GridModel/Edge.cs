using System.Drawing;

namespace GridTableBuilder.GridModel
{
    /// <summary> Edge of grid </summary>
    public class Edge : IDrawable, ISelectable
    {
        public Node Node1;
        public Node Node2;
        public Grid Grid => Node1.Grid;

        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;

            node1.Edges.Add(this);
            node2.Edges.Add(this);
        }

        #region IDrawable

        int IDrawable.Priority => 1;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(Color.Black, 1))
            {
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                gr.DrawLine(pen, Node1.Location, Node2.Location);
            }
        }

        #endregion

        #region ISelectable

        int ISelectable.Priority => 1;

        #endregion
    }
}
