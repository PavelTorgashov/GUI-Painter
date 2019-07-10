using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GridTableBuilder
{
    /// <summary>
    /// Часть класса, отвечающая за рисование таблицы
    /// </summary>
    public partial class Grid
    {
        private void LeftMouseDownInDrawMode(Point location)
        {
            down = true;
            firstPoint = lastPoint = location;
            splitLine = Line.Empty;
            CheckBorders(firstPoint, ref splitLine);
        }

        private void MouseMoveInDrawMode(Point location)
        {
            if (down)
            {
                // рисуем линию, только если внутри области
                if (Area.Contains(firstPoint))
                {
                    lastPoint = location;

                    var dx = Math.Abs(lastPoint.X - firstPoint.X);
                    var dy = Math.Abs(lastPoint.Y - firstPoint.Y);
                    // распознавание перпендикулярной линии
                    if (dy > 0 && dx / dy > 5)
                    {
                        // это горизонталь
                        lastPoint.Y = firstPoint.Y;
                        splitLine = FindAmongVerticalEdges(firstPoint.X, lastPoint.X, firstPoint.Y);
                    }
                    else if (dx > 0 && dy / dx > 5)
                    {
                        // это вертикаль
                        lastPoint.X = firstPoint.X;
                        splitLine = FindAmongHorizontalEdges(firstPoint.X, firstPoint.Y, lastPoint.Y);
                    }
                    else
                        splitLine = Line.Empty;
                }
            }
        }

        private void LeftMouseUpInDrawMode(Point location)
        {
            if (down)
            {
                down = false;
                if (!splitLine.IsEmpty)
                {
                    // добавляем новые узлы или присоединяемся к существующим
                    var pn1 = Nodes.FirstOrDefault(pn => pn.Offset == splitLine.Point1);
                    if (pn1 == null)
                    {
                        pn1 = new PointNode(splitLine.Point1) { Index = pointCount++ };
                        Nodes.Add(pn1);
                        // разбиваем ребро на два при добавлении узла
                        SplitEdge(pn1);
                    }
                    var pn2 = Nodes.FirstOrDefault(pn => pn.Offset == splitLine.Point2);
                    if (pn2 == null)
                    {
                        pn2 = new PointNode(splitLine.Point2) { Index = pointCount++ };
                        Nodes.Add(pn2);
                        // разбиваем ребро на два при добавлении узла
                        SplitEdge(pn2);
                    }
                    // добавляем новое ребро
                    var edge = new Edge(pn1, pn2) { Index = edgeCount++ };
                    pn1.Edges.Add(edge);
                    pn2.Edges.Add(edge);
                    // добавляем новые узлы в местах пересечения новым ребром
                    // новое ребро может делиться старыми на несколько частей
                    AddCrossNodesByEdge(edge);
                }

                firstPoint = lastPoint = location;
                splitLine = Line.Empty;
            }
        }

        private void PaintInDrawMode(Graphics graphics)
        {
            using (var pen = new Pen(Color.Magenta))
            {
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawLine(pen, firstPoint, lastPoint);
            }
            // рисуем возможное ребро
            if (!splitLine.IsEmpty)
                using (var pen = new Pen(Color.FromArgb(100, Color.Black), 3))
                    graphics.DrawLine(pen, splitLine.Point1, splitLine.Point2);
        }

        /// <summary>
        /// Проверка на существование пограничных рёбер 
        /// </summary>
        private void CheckBorders(Point point, ref Line splitLine)
        {
            // проверка левой границы
            if (Math.Abs(point.X - Area.Left) < Helper.Epsilon &&
                point.Y > Area.Top + Helper.Epsilon && point.Y < Area.Bottom + Helper.Epsilon &&
                GetNearEdge(point).IsEmpty)
            {
                var list = Edges.Where(edge => edge.IsVertical && edge.Node1.Offset.X == Area.Left).ToList();
                var line = new Line() { Point1 = new Point(Area.Left, Area.Top), Point2 = new Point(Area.Left, Area.Bottom) };
                var full = GetEdgeLine(list, line, point);
                if (full.IsEmpty) return;
                splitLine = new Line
                {
                    Point1 = new Point(Area.Left, full.Top),
                    Point2 = new Point(Area.Left, full.Bottom)
                };
            }
            else
            // проверка правой границы
            if (Math.Abs(point.X - (Area.Right)) < Helper.Epsilon &&
                point.Y > Area.Top + Helper.Epsilon && point.Y < Area.Bottom + Helper.Epsilon &&
                GetNearEdge(point).IsEmpty)
            {
                var list = Edges.Where(edge => edge.IsVertical && edge.Node1.Offset.X == Area.Right).ToList();
                var line = new Line() { Point1 = new Point(Area.Right, Area.Top), Point2 = new Point(Area.Right, Area.Bottom) };
                var full = GetEdgeLine(list, line, point);
                if (full.IsEmpty) return;
                splitLine = new Line
                {
                    Point1 = new Point(Area.Right, full.Top),
                    Point2 = new Point(Area.Right, full.Bottom)
                };
            }
            else
            // проверка верхней границы
            if (Math.Abs(point.Y - Area.Top) < Helper.Epsilon &&
                point.X > Area.Left + Helper.Epsilon && point.X < Area.Right + Helper.Epsilon &&
                GetNearEdge(point).IsEmpty)
            {
                var list = Edges.Where(edge => edge.IsHorizontal && edge.Node1.Offset.Y == Area.Top).ToList();
                var line = new Line() { Point1 = new Point(Area.Left, Area.Top), Point2 = new Point(Area.Right, Area.Top) };
                var full = GetEdgeLine(list, line, point);
                if (full.IsEmpty) return;
                splitLine = new Line
                {
                    Point1 = new Point(full.Left, Area.Top),
                    Point2 = new Point(full.Right, Area.Top)
                };
            }
            else
            // проверка нижней границы
            if (Math.Abs(point.Y - (Area.Bottom)) < Helper.Epsilon &&
                point.X > Area.Left + Helper.Epsilon && point.X < Area.Right + Helper.Epsilon &&
                GetNearEdge(point).IsEmpty)
            {
                var list = Edges.Where(edge => edge.IsHorizontal && edge.Node1.Offset.Y == Area.Bottom).ToList();
                var line = new Line() { Point1 = new Point(Area.X, Area.Bottom), Point2 = new Point(Area.Right, Area.Bottom) };
                var full = GetEdgeLine(list, line, point);
                if (full.IsEmpty) return;
                splitLine = new Line
                {
                    Point1 = new Point(full.Left, Area.Bottom),
                    Point2 = new Point(full.Right, Area.Bottom)
                };
            }
        }

        /// <summary>
        /// Построить линию в точке над существующим ребром
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private Line GetNearEdge(Point location, float width = Helper.Epsilon)
        {
            using (var grp = new GraphicsPath())
            using (var pen = new Pen(Color.Black, width))
            {
                foreach (var edge in Edges)
                {
                    grp.Reset();
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                    if (grp.IsOutlineVisible(location, pen))
                        return new Line() { Point1 = edge.Node1.Offset, Point2 = edge.Node2.Offset };
                }
            }
            return Line.Empty;
        }

        /// <summary>
        /// Расчёт размерного прямоугольника для отсутствующих рёбер внешнего контура
        /// </summary>
        /// <param name="list"></param>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private Rectangle GetEdgeLine(List<Edge> list, Line line, Point point)
        {
            using (var grp = new GraphicsPath())
            {
                grp.AddLine(line.Point1, line.Point2);
                var full = Rectangle.Ceiling(grp.GetBounds());
                if (full.Width == 0)
                {
                    full.Width = 4;
                    full.Offset(-2, 0);
                }
                else if (full.Height == 0)
                {
                    full.Height = 4;
                    full.Offset(0, -2);
                }
                using (var fr = new Region(full))
                {
                    foreach (var edge in list)
                    {
                        grp.Reset();
                        grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                        var rect = Rectangle.Ceiling(grp.GetBounds());
                        if (rect.Width == 0)
                        {
                            rect.Width = 4;
                            rect.Offset(-2, 0);
                        }
                        else if (rect.Height == 0)
                        {
                            rect.Height = 4;
                            rect.Offset(0, -2);
                        }
                        using (var rg = new Region(rect))
                            fr.Exclude(rg);
                    }
                    using (var image = new Bitmap(Area.Width, Area.Height))
                    using (var gr = Graphics.FromImage(image))
                    {
                        foreach (var rect in fr.GetRegionScans(new Matrix()))
                            if (rect.Contains(point))
                                return Rectangle.Ceiling(rect);
                    }
                }
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// Поиск вертикального отрезка среди горизонтальных рёбер
        /// </summary>
        /// <param name="x">X-координата вертикального отрезка</param>
        /// <param name="y1">Начальная Y-координата</param>
        /// <param name="y2">Конечная Y-координата</param>
        /// <returns></returns>
        private Line FindAmongHorizontalEdges(int x, int y1, int y2)
        {
            var firstY = Math.Min(y1, y2);
            var lastY = Math.Max(y1, y2);
            // смотрим горизонтальные рёбра
            var horizontalEdges = Edges.Where(edge => edge.IsHorizontal)
                                       .Where(edge => edge.Node1.Offset.X < x && edge.Node2.Offset.X > x).ToList();
            if (horizontalEdges.Count < 2)
                return Line.Empty;
            var edgeA = horizontalEdges.OrderBy(edge => Math.Abs(edge.Node1.Offset.Y - firstY)).First();
            var edgeB = horizontalEdges.OrderBy(edge => Math.Abs(edge.Node1.Offset.Y - lastY)).First();
            return edgeA == edgeB 
                ? Line.Empty
                : new Line() { Point1 = new Point(x, edgeA.Node1.Offset.Y), Point2 = new Point(x, edgeB.Node2.Offset.Y) };
        }

        /// <summary>
        /// Поиск горизонтального отрезка среди вертикальных рёбер
        /// </summary>
        /// <param name="x1">Начальная X-координата</param>
        /// <param name="x2">Конечная X-координата</param>
        /// <param name="y">Y-координата горизонтального отрезка</param>
        /// <returns></returns>
        private Line FindAmongVerticalEdges(int x1, int x2, int y)
        {
            var firstX = Math.Min(x1, x2);
            var lastX = Math.Max(x1, x2);
            // смотрим вертикальные рёбра
            var verticalEdges = Edges.Where(edge => edge.IsVertical)
                                     .Where(edge => edge.Node1.Offset.Y < y && edge.Node2.Offset.Y > y).ToList();
            if (verticalEdges.Count < 2)
                return Line.Empty;
            var edgeA = verticalEdges.OrderBy(edge => Math.Abs(edge.Node1.Offset.X - firstX)).First();
            var edgeB = verticalEdges.OrderBy(edge => Math.Abs(edge.Node1.Offset.X - lastX)).First();
            return edgeA == edgeB 
                ? Line.Empty
                : new Line() { Point1 = new Point(edgeA.Node1.Offset.X, y), Point2 = new Point(edgeB.Node2.Offset.X, y) };
        }

        /// <summary>
        /// Новое ребро пересекает старые и образует новые узлы, которые будут делить старые рёбра на две части
        /// Новое ребро может делиться старыми на несколько частей
        /// </summary>
        /// <param name="edge">Ссылка на новое ребро</param>
        private void AddCrossNodesByEdge(Edge edge)
        {
            var verticals = GetCrossEdges(edge, Edges.Where(e => edge.IsHorizontal && e.Node1.Offset.X == e.Node2.Offset.X &&
                                             e.Node1.Offset.X != edge.Node1.Offset.X && e.Node2.Offset.X != edge.Node2.Offset.X).ToList());
            var horizontals = GetCrossEdges(edge, Edges.Where(e => edge.IsVertical && e.Node1.Offset.Y == e.Node2.Offset.Y &&
                                               e.Node1.Offset.Y != edge.Node1.Offset.Y && e.Node2.Offset.Y != edge.Node2.Offset.Y).ToList());
            var points = new List<PointNode>();
            if (verticals.Count > 0)
            {
                foreach (var e in verticals)
                    points.Add(new PointNode(new Point(e.Node1.Offset.X, edge.Node1.Offset.Y)));
            }
            else if (horizontals.Count > 0)
            {
                foreach (var e in horizontals)
                    points.Add(new PointNode(new Point(edge.Node1.Offset.X, e.Node1.Offset.Y)));
            }
            // добавляем новое ребро
            Edges.Add(edge);
            // которое, возможно, будем делить
            foreach (var pn in points)
            {
                pn.Index = pointCount++;
                Nodes.Add(pn);
                SplitEdge(pn);
            }
        }

        /// <summary>
        /// Ищем список ребёр, имеющих точку пересечения с указанным ребром
        /// </summary>
        /// <param name="edge">Новое ребро</param>
        /// <param name="list">Список ребёр для тестирования</param>
        /// <returns></returns>
        private List<Edge> GetCrossEdges(Edge edge, IEnumerable<Edge> list)
        {
            var result = new List<Edge>();
            var r1 = RectangleF.Empty;
            using (var pen = new Pen(Color.Black, 1))
            using (var mx = new Matrix())
            {
                using (var grp = new GraphicsPath())
                {
                    grp.AddLine(edge.Node1.Offset, edge.Node2.Offset);
                    r1 = grp.GetBounds(mx, pen);
                }
                using (var grp = new GraphicsPath())
                {
                    foreach (var e in list)
                    {
                        grp.Reset();
                        grp.AddLine(e.Node1.Offset, e.Node2.Offset);
                        var rect = grp.GetBounds(mx, pen);
                        if (rect.IntersectsWith(r1))
                            result.Add(e);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Новый узел разбивает существующее ребро на два
        /// </summary>
        /// <param name="pn">Ссылка на новый узел</param>
        private void SplitEdge(PointNode pn)
        {
            var verticals = Edges.Where(edge => edge.IsVertical && pn.Offset.X == edge.Node1.Offset.X &&
                                        pn.Offset.Y >= edge.Node1.Offset.Y && pn.Offset.Y <= edge.Node2.Offset.Y).ToList();
            var horizontals = Edges.Where(edge => edge.IsHorizontal && pn.Offset.Y == edge.Node1.Offset.Y &&
                                          pn.Offset.X >= edge.Node1.Offset.X && pn.Offset.X <= edge.Node2.Offset.X).ToList();
            foreach (var edge in verticals.Union(horizontals))
            {
                // добавляем новые рёбра
                var edg1 = new Edge(edge.Node1, pn) { Index = edgeCount++ };
                Edges.Add(edg1);
                pn.Edges.Add(edg1);
                var edg2 = new Edge(pn, edge.Node2) { Index = edgeCount++ };
                Edges.Add(edg2);
                pn.Edges.Add(edg2);
                // удаляем старое ребро
                edge.Node1.Edges.Remove(edge);
                edge.Node1.Edges.Add(edg1);
                edge.Node2.Edges.Remove(edge);
                edge.Node2.Edges.Add(edg2);
                Edges.Remove(edge);
            }
        }

    }
}
