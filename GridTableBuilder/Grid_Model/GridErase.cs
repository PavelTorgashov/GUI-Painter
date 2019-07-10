using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GridTableBuilder
{
    /// <summary>
    /// Часть класса, отвечающая за удаление рёбер и узлов
    /// </summary>
    public partial class Grid
    {
        Rectangle ribberRect;
        List<Edge> edgesToDelete = new List<Edge>();

        private void LeftMouseDownInEraseMode(Point location)
        {
            down = true;
            firstPoint = lastPoint = location;
            ribberRect = new Rectangle(Point.Subtract(location, new Size(1, 1)), new Size(3, 3));
            edgesToDelete = GetEdgesSecantRect(ribberRect).ToList();
        }

        private void MouseMoveInEraseMode(Point location)
        {
            if (down)
            {
                lastPoint = location;
                var loc = Point.Subtract(new Point(Math.Min(firstPoint.X, lastPoint.X), Math.Min(firstPoint.Y, lastPoint.Y)), new Size(1, 1));
                var size = new Size(Math.Abs(lastPoint.X - firstPoint.X), Math.Abs(lastPoint.Y - firstPoint.Y));
                ribberRect = new Rectangle(loc, size);

                edgesToDelete = GetEdgesSecantRect(ribberRect).ToList();
            }
        }

        private void LeftMouseUpInEraseMode(Point location)
        {
            if (down)
            {
                down = false;
                if (edgesToDelete.Count > 0)
                {
                    foreach (var edge in edgesToDelete)
                    {
                        Edges.Remove(edge);
                        foreach (var pn in Nodes)
                            if (pn.Edges.Contains(edge))
                                pn.Edges.Remove(edge);
                        foreach (var pn in Nodes.ToList())
                            RemoveIsAnadromousNode(pn);
                    }
                    edgesToDelete.Clear();
                    RemovePendentEdges();
                }
                firstPoint = lastPoint = location;
                ribberRect = Rectangle.Empty;
            }

        }

        private void PaintInEraseMode(Graphics graphics)
        {
            if (!ribberRect.IsEmpty)
                using (var pen = new Pen(Color.Red, 1))
                    graphics.DrawRectangle(pen, ribberRect);
            // рисуем рёбра для удаления
            foreach (var ed in edgesToDelete)
                using (var pen = new Pen(Color.FromArgb(100, Color.Red), 3))
                    graphics.DrawLine(pen, ed.Node1.Offset, ed.Node2.Offset);

        }

        /// <summary>
        /// Список рёбер, которые пересекает область действия ластика
        /// </summary>
        /// <param name="ribberRect">Область действия ластика</param>
        /// <returns></returns>
        private IEnumerable<Edge> GetEdgesSecantRect(Rectangle ribberRect)
        {
            var list = new List<Edge>();
            foreach (var edge in Edges)
            {
                if (edge.IsVertical) // если ребро вертикальное
                {
                    var x = edge.Node1.Offset.X;
                    if (x >= ribberRect.X && x < ribberRect.X + ribberRect.Width &&
                        edge.Node1.Offset.Y <= ribberRect.Y + ribberRect.Height && edge.Node2.Offset.Y >= ribberRect.Y)
                        list.Add(edge);
                }
                else if (edge.IsHorizontal) // если ребро горизонтальное
                {
                    var y = edge.Node1.Offset.Y;
                    if (y >= ribberRect.Y && y < ribberRect.Y + ribberRect.Height &&
                        edge.Node1.Offset.X <= ribberRect.X + ribberRect.Width && edge.Node2.Offset.X >= ribberRect.X)
                        list.Add(edge);
                }
            }
            return list;
        }

        /// <summary>
        /// Удаление проходных узловых точек
        /// </summary>
        private void RemoveIsAnadromousNode(PointNode pn)
        {
            if (!pn.IsAnadromous) return;
            if (pn.Edges.Count != 2) return;
            var pn1 = pn.Edges[0].Node1;
            var pn2 = pn.Edges[1].Node2;
            if (pn1 == pn2)
            {
                pn1 = pn.Edges[0].Node2;
                pn2 = pn.Edges[1].Node1;
                if (pn1 == pn2) return;
            }
            var edge4delete = GetJointEdge(pn, pn2);
            var edge2modify = GetJointEdge(pn1, pn);
            if (edge4delete != null && edge2modify != null)
            {
                pn2.Edges.Remove(edge4delete);
                pn2.Edges.Add(edge2modify);
                if (edge2modify.Node1 == pn)
                    edge2modify.Node1 = pn2;
                else if (edge2modify.Node2 == pn)
                    edge2modify.Node2 = pn2;
                Edges.Remove(edge4delete);
                Nodes.Remove(pn);
            }
        }

        /// <summary>
        /// Удаление "висячих" точек и рёбер
        /// </summary>
        private void RemovePendentEdges()
        {
            while (Nodes.Any(pn => pn.Edges.Count == 1))
                foreach (var pn in Nodes.ToList().Where(pn => pn.Edges.Count == 1))
                {
                    if (pn.Edges.Count == 0) continue;
                    var edge = pn.Edges[0];
                    // "висящее" ребро на главном периметре не должно удаляться
                    var masterEdge  = (edge.IsHorizontal && (edge.Node1.Offset.Y == Area.Top && edge.Node2.Offset.Y == Area.Top ||
                                                             edge.Node1.Offset.Y == Area.Bottom && edge.Node2.Offset.Y == Area.Bottom)) ||
                                      (edge.IsVertical && (edge.Node1.Offset.X == Area.Left && edge.Node2.Offset.X == Area.Left || 
                                                           edge.Node1.Offset.X == Area.Right && edge.Node2.Offset.X == Area.Right));
                    if (masterEdge) goto exit;
                    var pnOther = edge.Node1 == pn ? edge.Node2 : edge.Node1;
                    pnOther.Edges.Remove(edge);
                    Edges.Remove(edge);
                    Nodes.Remove(pn);
                    RemoveIsAnadromousNode(pnOther);
                }
            exit:
            // удаление одиноких узловых точек
            Nodes.RemoveAll(pn => pn.IsEmpty);
        }

        /// <summary>
        /// Получить общее для двух точек ребро
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        private Edge GetJointEdge(PointNode pt1, PointNode pt2)
        {
            foreach (var e1 in pt1.Edges)
                foreach (var e2 in pt2.Edges)
                    if (e1 == e2)
                        return e1;
            return null;
        }

    }
}
