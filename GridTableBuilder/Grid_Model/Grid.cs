using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GridTableBuilder
{
    [Serializable]
    public enum GridWorkMode
    {
        Draw,
        Move,
        Erase
    }

    public enum SplitKind
    {
        None,
        Vertical,
        Horizontal
    }

    [Serializable]
    public partial class Grid
    {
        int pointCount = 0;
        int edgeCount = 0;

        bool down;
        Point firstPoint;
        Point lastPoint;
        Point drarPoint;

        Line splitLine;

        public Rectangle Area { get; set; } = new Rectangle();
        public List<PointNode> Nodes { get; set; } = new List<PointNode>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public GridWorkMode WorkMode { get; set; }
        public SplitKind SplitKind { get; set; }

        public bool ShowNodeNames { get; set; }
        public bool ShowEdgeNames { get; set; }

        public void Init()
        {
            Nodes.Clear();
            // добавление основных узловых точек на границах фигуры
            Nodes.Add(new PointNode(new Point(Area.X, Area.Y)) { Index = pointCount++ });
            Nodes.Add(new PointNode(new Point(Area.X + Area.Width, Area.Y)) { Index = pointCount++ });
            Nodes.Add(new PointNode(new Point(Area.X + Area.Width, Area.Y + Area.Height)) { Index = pointCount++ });
            Nodes.Add(new PointNode(new Point(Area.X, Area.Y + Area.Height)) { Index = pointCount++ });
            Edges.Clear();
            Edges.Add(new Edge(Nodes[0], Nodes[1]) { Index = edgeCount++ });
            Edges.Add(new Edge(Nodes[1], Nodes[2]) { Index = edgeCount++ });
            Edges.Add(new Edge(Nodes[2], Nodes[3]) { Index = edgeCount++ });
            Edges.Add(new Edge(Nodes[3], Nodes[0]) { Index = edgeCount++ });
            Nodes[0].Edges.Add(Edges[0]);
            Nodes[0].Edges.Add(Edges[3]);
            Nodes[1].Edges.Add(Edges[0]);
            Nodes[1].Edges.Add(Edges[1]);
            Nodes[2].Edges.Add(Edges[1]);
            Nodes[2].Edges.Add(Edges[2]);
            Nodes[3].Edges.Add(Edges[2]);
            Nodes[3].Edges.Add(Edges[3]);

            WorkMode = GridWorkMode.Draw;
        }

        public void OnLeftMouseDown(Point location)
        {
            if (WorkMode == GridWorkMode.Draw)
                LeftMouseDownInDrawMode(location);
            else if (WorkMode == GridWorkMode.Move)
                LeftMouseDownInMoveMode(location);
            else if (WorkMode == GridWorkMode.Erase)
                LeftMouseDownInEraseMode(location);
        }

        public void OnMouseMove(Point location)
        {
            if (WorkMode == GridWorkMode.Draw)
                MouseMoveInDrawMode(location);
            else if (WorkMode == GridWorkMode.Move)
                MouseMoveInMoveMode(location);
            else if (WorkMode == GridWorkMode.Erase)
                MouseMoveInEraseMode(location);
        }

        public void OnLeftMouseUp(Point location)
        {
            if (WorkMode == GridWorkMode.Draw)
                LeftMouseUpInDrawMode(location);
            else if (WorkMode == GridWorkMode.Move)
                LeftMouseUpInMoveMode(location);
            else if (WorkMode == GridWorkMode.Erase)
                LeftMouseUpInEraseMode(location);
        }

        public void OnPaint(Graphics graphics)
        {
            var gr = graphics;
            using (var pen = new Pen(Color.Gray, 1))
            {
                pen.DashStyle = DashStyle.Dash;
                gr.DrawRectangle(pen, Area);
            }
            // рисуем узловые точки
            foreach (var np in Nodes)
            {
                var rect = new Rectangle(np.Offset, new Size(8, 8));
                rect.Offset(-4, -4);
                gr.FillEllipse(Brushes.Gray, rect);
                if (ShowNodeNames)
                {
                    rect.Offset(5, 5);
                    using (var font = new Font("Arial", 8))
                        gr.DrawString($"p{np.Index}", font, Brushes.Black, rect.Location);
                }
            }
            // рисуем рёбра
            foreach (var ed in Edges)
            {
                using (var pen = new Pen(Color.Black, 1))
                    gr.DrawLine(pen, ed.Node1.Offset, ed.Node2.Offset);
                if (ShowEdgeNames)
                {
                    var p = new Point(ed.Node1.Offset.X, ed.Node1.Offset.Y);
                    p.Offset((ed.Node2.Offset.X - ed.Node1.Offset.X) / 2 - 8, (ed.Node2.Offset.Y - ed.Node1.Offset.Y) / 2 - 12);
                    using (var font = new Font("Arial", 8))
                        gr.DrawString($"e{ed.Index}", font, Brushes.Black, p);
                }
            }
            //
            if (WorkMode == GridWorkMode.Draw)
                PaintInDrawMode(graphics);
            else if (WorkMode == GridWorkMode.Move)
                PaintInMoveMode(graphics);
            else if (WorkMode == GridWorkMode.Erase)
                PaintInEraseMode(graphics);
        }

    }
}
