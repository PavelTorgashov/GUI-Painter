using System;
using System.Drawing;

namespace GridTableBuilder
{
    public struct Line
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }

        public bool IsEmpty { get { return Point1.IsEmpty && Point2.IsEmpty; } }

        public static Line Empty { get { return new Line(); } }

        public Line Offset(int dx, int dy)
        {
            return new Line
            {
                Point1 = new Point(Point1.X + dx, Point1.Y + dy),
                Point2 = new Point(Point2.X + dx, Point2.Y + dy)
            };
        }

        public bool Contains(Point point)
        {
            if (IsEmpty) return false;
            return Math.Abs(Point1.X - Point2.X) < Helper.Epsilon && point.Y >= Point1.Y && point.Y <= Point2.Y ||
                   Math.Abs(Point1.Y - Point2.Y) < Helper.Epsilon && point.X >= Point1.X && point.X <= Point2.X;
        }

        public override string ToString()
        {
            return $"{Point1} {Point2}";
        }
    }
}
