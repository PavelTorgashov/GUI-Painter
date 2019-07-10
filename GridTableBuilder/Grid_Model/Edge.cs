namespace GridTableBuilder
{
    public class Edge
    {
        public PointNode Node1 { get; set; }
        public PointNode Node2 { get; set; }

        public int Index { get; set; }

        public Edge(PointNode pn1, PointNode pn2)
        {
            if (pn1.Offset.X == pn2.Offset.X)
            {
                if (pn1.Offset.Y < pn2.Offset.Y)
                {
                    Node1 = pn1;
                    Node2 = pn2;
                }
                else
                {
                    Node1 = pn2;
                    Node2 = pn1;
                }
            }
            else
            {
                if (pn1.Offset.X < pn2.Offset.X)
                {
                    Node1 = pn1;
                    Node2 = pn2;
                }
                else
                {
                    Node1 = pn2;
                    Node2 = pn1;
                }
            }
        }

        public bool IsVertical
        {
            get { return Node1.Offset.X == Node2.Offset.X; }
        }

        public bool IsHorizontal
        {
            get { return Node1.Offset.Y == Node2.Offset.Y; }
        }

        public bool IsSameOrientation(Edge edge)
        {
            return this.IsHorizontal && edge.IsHorizontal || this.IsVertical && edge.IsVertical;
        }

        public bool IsPerpendicularOrientation(Edge edge)
        {
            return this.IsHorizontal && edge.IsVertical || this.IsVertical && edge.IsHorizontal;
        }

        public override string ToString()
        {
            return $"e{Index}";
        }
    }
}
