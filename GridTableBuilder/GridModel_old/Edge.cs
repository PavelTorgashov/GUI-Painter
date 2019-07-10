namespace Grid_Model_old
{
    public class Edge
    {
        public PointNode Node1 { get; set; }
        public PointNode Node2 { get; set; }

        public int Index { get; set; }

        public Edge(PointNode pn1, PointNode pn2)
        {
            if (pn1.Location.X == pn2.Location.X)
            {
                if (pn1.Location.Y < pn2.Location.Y)
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
                if (pn1.Location.X < pn2.Location.X)
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
            get { return Node1.Location.X == Node2.Location.X; }
        }

        public bool IsHorizontal
        {
            get { return Node1.Location.Y == Node2.Location.Y; }
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
