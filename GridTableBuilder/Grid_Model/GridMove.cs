using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder
{
    /// <summary>
    /// Часть класса, отвечающая за перемещение рёбер и узлов
    /// </summary>
    public partial class Grid
    {
        Point startMove;
        List<Edge> edgesToMove = new List<Edge>();
        Rectangle permissible = new Rectangle();

        private void LeftMouseDownInMoveMode(Point location)
        {
            down = true;
            drarPoint = location;
            SplitKind = MouseInHSplit(location) 
                ? SplitKind.Vertical 
                : MouseInVSplit(location) 
                      ? SplitKind.Horizontal 
                      : SplitKind.None;
            splitLine = Line.Empty;
            startMove = Point.Empty;
            edgesToMove.Clear();
            if (SplitKind != SplitKind.None)
            {
                edgesToMove = GetAjacentEdges(location, SplitKind);
                splitLine = GetAjacentLine(edgesToMove);
                startMove = splitLine.Point1;
                permissible = GetPermissibleArea(edgesToMove, SplitKind, startMove);
            }
        }

        private void MouseMoveInMoveMode(Point location)
        {
            if (down)
            {
                if (!splitLine.IsEmpty)
                {
                    var dx = SplitKind == SplitKind.Vertical ? location.X - drarPoint.X : 0;
                    var dy = SplitKind == SplitKind.Horizontal ? location.Y - drarPoint.Y : 0;
                    var line = splitLine.Offset(dx, dy);
                    if (permissible.Contains(line.Point1) && permissible.Contains(line.Point2))
                        splitLine = line;
                }
                drarPoint = location;
            }
        }

        private void LeftMouseUpInMoveMode(Point location)
        {
            if (down)
            {
                down = false;
                if (edgesToMove.Count > 0)
                {
                    var dx = splitLine.Point1.X - startMove.X;
                    var dy = splitLine.Point1.Y - startMove.Y;
                    var points = new List<PointNode>();
                    foreach (var edge in edgesToMove)
                    {
                        var pn1 = edge.Node1;
                        var pn2 = edge.Node2;
                        if (!points.Contains(pn1)) points.Add(pn1);
                        if (!points.Contains(pn2)) points.Add(pn2);
                    }
                    foreach (var pn in points)
                    {
                        pn.Offset = new Point(pn.Offset.X + dx, pn.Offset.Y + dy);
                    }

                    edgesToMove.Clear();
                }
                splitLine = Line.Empty;
            }
        }

        private void PaintInMoveMode(Graphics graphics)
        {
            // рисуем перетаскиваемое ребро
            if (!splitLine.IsEmpty)
                using (var pen = new Pen(Color.Magenta, 2))
                    graphics.DrawLine(pen, splitLine.Point1, splitLine.Point2);

        }

        /// <summary>
        /// Вычисление разрешённой области для перетаскивания рёбер
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="splitKind"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        private Rectangle GetPermissibleArea(List<Edge> edges, SplitKind splitKind, Point origin)
        {
            var rect = Area; // первоначальное ограничение
            var points = new List<PointNode>();
            var perpendiculars = new List<Edge>();
            // получим все точки из списка рёбер, содержащих в своих списках рёбер перпендикулярные рёбра
            foreach (var edge in edges)
            {
                var pn1 = edge.Node1;
                var pn2 = edge.Node2;
                if (!points.Contains(pn1) && pn1.Edges.Any(e => e.IsPerpendicularOrientation(edge)))
                {
                    points.Add(pn1);
                    perpendiculars.AddRange(pn1.Edges.Where(e => e.IsPerpendicularOrientation(edge)));
                }
                if (!points.Contains(pn2) && pn2.Edges.Any(e => e.IsPerpendicularOrientation(edge)))
                {
                    points.Add(pn2);
                    perpendiculars.AddRange(pn2.Edges.Where(e => e.IsPerpendicularOrientation(edge)));
                }
            }
            var farPoints = new List<PointNode>();
            // получим все точки на противополжных концах рёбер
            foreach (var edge in perpendiculars)
            {
                var pn1 = edge.Node1;
                var pn2 = edge.Node2;
                if (!farPoints.Contains(pn1) && !points.Contains(pn1)) farPoints.Add(pn1);
                if (!farPoints.Contains(pn2) && !points.Contains(pn2)) farPoints.Add(pn2);
            }
            if (splitKind == SplitKind.Vertical)
            {
                var leftPoint = farPoints.Where(pn => pn.Offset.X < origin.X).OrderBy(pn => Math.Abs(pn.Offset.X - origin.X)).FirstOrDefault();
                var left = leftPoint != null ? leftPoint.Offset.X : Area.Left;
                var rightPoint = farPoints.Where(pn => pn.Offset.X > origin.X).OrderBy(pn => Math.Abs(pn.Offset.X - origin.X)).FirstOrDefault();
                var right = rightPoint != null ? rightPoint.Offset.X : Area.Right;
                rect = new Rectangle(left, Area.Top, right - left, Area.Height);
            }
            else if (splitKind == SplitKind.Horizontal)
            {
                var topPoint = farPoints.Where(pn => pn.Offset.Y < origin.Y).OrderBy(pn => Math.Abs(pn.Offset.Y - origin.Y)).FirstOrDefault();
                var top = topPoint != null ? topPoint.Offset.Y : Area.Top;
                var bottomPoint = farPoints.Where(pn => pn.Offset.Y > origin.Y).OrderBy(pn => Math.Abs(pn.Offset.Y - origin.Y)).FirstOrDefault();
                var bottom = bottomPoint != null ? bottomPoint.Offset.Y : Area.Bottom;
                rect = new Rectangle(Area.Left, top, Area.Width, bottom - top);
            }
            // дополнительные ограничения
            if (SplitKind == SplitKind.Horizontal)
                rect.Inflate(1, -(int)Helper.Epsilon * 2);
            else if (SplitKind == SplitKind.Vertical)
                rect.Inflate(-(int)Helper.Epsilon * 2, 1);
            return rect;
        }

        /// <summary>
        /// Получить линию для смежных рёбер
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        private Line GetAjacentLine(List<Edge> edges)
        {
            using (var grp = new GraphicsPath())
            {
                foreach (var edge in edges)
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                var rect = Rectangle.Ceiling(grp.GetBounds());
                return new Line() { Point1 = new Point(rect.Left, rect.Top), Point2 = new Point(rect.Right, rect.Bottom) };
            }
        }

        /// <summary>
        /// Получить список смежных одинаково ориентированных рёбер с ребром, на которую указывает курсор
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private List<Edge> GetAjacentEdges(Point location, SplitKind splitKind, float width = Helper.Epsilon)
        {
            if (splitKind == SplitKind.None) return new List<Edge>();
            using (var grp = new GraphicsPath())
            using (var pen = new Pen(Color.Black, width))
            {
                foreach (var item in Edges.Where(edge => splitKind == SplitKind.Vertical && edge.IsVertical || 
                                                         splitKind == SplitKind.Horizontal && edge.IsHorizontal))
                {
                    var edge = item;
                    grp.Reset();
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                    if (grp.IsOutlineVisible(location, pen))
                    {
                        var result = new List<Edge>();
                        var queue = new Queue<Edge>();
                        queue.Enqueue(edge);
                        while (true)
                        {
                            if (queue.Count == 0) break;
                            edge = queue.Dequeue();
                            var pn1 = edge.Node1;
                            var pn2 = edge.Node2;
                            // найти все рёбра, разделяющие общие узловые точки
                            var list = Edges.Where(e => !result.Contains(e) && e.IsSameOrientation(edge) &&
                                                        (e.Node1 == pn1 || e.Node1 == pn2 || e.Node2 == pn1 || e.Node2 == pn2)).ToList();
                            if (list.Count > 0)
                            {
                                foreach (var e in list) queue.Enqueue(e);
                                result.AddRange(list);
                            }
                        }
                        return result;
                    }
                }
            }
            return new List<Edge>();
        }

        /// <summary>
        /// Определение позиции курсора над внутренним горизонтальным ребром
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool MouseInVSplit(Point location, float width = Helper.Epsilon)
        {
            if (Area.IsEmpty) return false;
            var horizontals = Edges.Where(edge => edge.IsHorizontal &&
                                          edge.Node1.Offset.Y > Area.Y + Helper.Epsilon && edge.Node1.Offset.Y < Area.Y + Area.Height - Helper.Epsilon);
            using (var grp = new GraphicsPath())
            using (var pen = new Pen(Color.Black, width))
            {
                var lp = Area.Location;
                foreach (var edge in horizontals)
                {
                    grp.Reset();
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                    if (grp.IsOutlineVisible(location, pen))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Определение позиции курсора над внутренним вертикальным ребром
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool MouseInHSplit(Point location, float width = Helper.Epsilon)
        {
            if (Area.IsEmpty) return false;
            var verticals = Edges.Where(edge => edge.IsVertical &&
                                          edge.Node1.Offset.X > Area.X + Helper.Epsilon && edge.Node1.Offset.X < Area.X + Area.Width - Helper.Epsilon);
            using (var grp = new GraphicsPath())
            using (var pen = new Pen(Color.Black, width))
            {
                foreach (var edge in verticals)
                {
                    grp.Reset();
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                    if (grp.IsOutlineVisible(location, pen))
                        return true;
                }
            }
            return false;
        }

    }
}
