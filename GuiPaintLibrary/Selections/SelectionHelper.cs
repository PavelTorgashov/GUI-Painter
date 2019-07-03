using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using GuiPaintLibrary.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiPaintLibrary.Selections
{
    public enum FigureAlignment
    {
        Near,
        Center,
        Far
    }

    public enum FigureResize
    {
        Width,
        Heigth,
        Both
    }

    public enum FigureArrange
    {
        Horizontal,
        Vertical
    }

    public static class SelectionHelper
    {
        /// <summary>
        /// Переводит точку из локальных нормализированных координат (0,0)-(1,1) в мировые координаты
        /// </summary>
        public static PointF ToWorldCoordinates(this Selection selection, PointF point)
        {
            var bounds = selection.GetTransformedPath().Path.GetBounds();
            return new PointF(bounds.Left + point.X * bounds.Width, bounds.Top + point.Y * bounds.Height);
        }

        /// <summary>
        /// Отражение по горизонтали
        /// </summary>
        public static void FlipX(this Selection selection)
        {
            //можем вращать?
            var allowRotate = selection.Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            var m = new SerializableGraphicsMatrix();
            var anchor = selection.ToWorldCoordinates(new PointF(0.5f, 0.5f));
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            //отражаем по горизонтали относительно якоря
            m.Matrix.Multiply(new Matrix(-1, 0, 0, 1, 0, 0));
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат
            //
            selection.Transform = m;
        }

        /// <summary>
        /// Отражение по вертикали
        /// </summary>
        public static void FlipY(this Selection selection)
        {
            //можем вращать?
            var allowRotate = selection.Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            var m = new SerializableGraphicsMatrix();
            var anchor = selection.ToWorldCoordinates(new PointF(0.5f, 0.5f));
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            //отражаем по вертикали относительно якоря
            m.Matrix.Multiply(new Matrix(1, 0, 0, -1, 0, 0));
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат
            //
            selection.Transform = m;
        }

        /// <summary>
        /// Поворот на четверть по часовой стрелке
        /// </summary>
        public static void Rotate90Cw(this Selection selection)
        {
            //можем вращать?
            var allowRotate = selection.Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            var m = new SerializableGraphicsMatrix();
            var anchor = selection.ToWorldCoordinates(new PointF(0.5f, 0.5f));
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            //отражаем по вертикали относительно якоря
            m.Matrix.Multiply(new Matrix(0, 1, -1, 0, 0, 0));
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат
            //
            selection.Transform = m;
        }

        /// <summary>
        /// Поворот на четверть против часовой стрелки
        /// </summary>
        public static void Rotate90Ccw(this Selection selection)
        {
            //можем вращать?
            var allowRotate = selection.Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            var m = new SerializableGraphicsMatrix();
            var anchor = selection.ToWorldCoordinates(new PointF(0.5f, 0.5f));
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            //отражаем по вертикали относительно якоря
            m.Matrix.Multiply(new Matrix(0, -1, 1, 0, 0, 0));
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат
            //
            selection.Transform = m;
        }

        /// <summary>
        /// Поворот на 180 градусов
        /// </summary>
        public static void Rotate180(this Selection selection)
        {
            //можем вращать?
            var allowRotate = selection.Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            var m = new SerializableGraphicsMatrix();
            var anchor = selection.ToWorldCoordinates(new PointF(0.5f, 0.5f));
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            //отражаем по вертикали относительно якоря
            m.Matrix.Multiply(new Matrix(-1, 0, 0, -1, 0, 0));
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат
            //
            selection.Transform = m;
        }

        /// <summary>
        /// Выравнивание фигур по горизонтали, относительно первой фигуры в списке
        /// </summary>
        public static void AlignHorizontal(this Selection selection, FigureAlignment alignment)
        {
            var exists = selection.Count > 1;
            if (!exists) return;
            var first = selection.First();
            var x = first.Transform.Matrix.Elements[4];
            var w = first.GetTransformedPath().Path.GetBounds().Width;
            foreach (var figure in selection)
            {
                var width = figure.GetTransformedPath().Path.GetBounds().Width;
                var el = figure.Transform.Matrix.Elements;
                switch (alignment)
                {
                    case FigureAlignment.Near:
                        el[4] = x - (w - width) / 2;
                        break;
                    case FigureAlignment.Center:
                        el[4] = x;
                        break;
                    case FigureAlignment.Far:
                        el[4] = x + (w - width) / 2;
                        break;
                }
                figure.Transform.Matrix = new Matrix(el[0], el[1], el[2], el[3], el[4], el[5]);
            }
        }

        /// <summary>
        /// Выравнивание фигур по по вертикали, относительно первой фигуры в списке
        /// </summary>
        public static void AlignVertical(this Selection selection, FigureAlignment alignment)
        {
            var exists = selection.Count > 1;
            if (!exists) return;
            var first = selection.First();
            var y = first.Transform.Matrix.Elements[5];
            var h = first.GetTransformedPath().Path.GetBounds().Height;
            foreach (var figure in selection)
            {
                var height = figure.GetTransformedPath().Path.GetBounds().Height;
                var el = figure.Transform.Matrix.Elements;
                switch (alignment)
                {
                    case FigureAlignment.Near:
                        el[5] = y - (h - height) / 2;
                        break;
                    case FigureAlignment.Center:
                        el[5] = y;
                        break;
                    case FigureAlignment.Far:
                        el[5] = y + (h - height) / 2;
                        break;
                }
                figure.Transform.Matrix = new Matrix(el[0], el[1], el[2], el[3], el[4], el[5]);
            }
        }

        /// <summary>
        /// Выравнивание по величине промежутков между фигурами
        /// </summary>
        /// <param name="selection">Текущее выделение фигур</param>
        /// <param name="arrange">Направление выравнивания</param>
        public static void EvenSpaces(this Selection selection, FigureArrange arrange)
        {
            var exists = selection.Count > 2;
            if (!exists) return;
            var width = 0f;
            var heigth = 0f;
            foreach (var bounds in selection.Select(figure =>
                figure.GetTransformedPath().Path.GetBounds()))
            {
                width += bounds.Width;
                heigth += bounds.Height;
            }
            var selectionBounds = selection.GetTransformedPath().Path.GetBounds();
            var figures = new List<Figure>();
            switch (arrange)
            {
                case FigureArrange.Horizontal:
                    var sW = (selectionBounds.Width - width) / (selection.Count - 1);
                    figures.AddRange(selection.OrderBy(item => item.GetTransformedPath().Path.GetBounds().Left));
                    var w = selectionBounds.Left + figures.First().GetTransformedPath().Path.GetBounds().Width + sW;
                    for (var i = 1; i < figures.Count - 1; i++)
                    {
                        var half = figures[i].GetTransformedPath().Path.GetBounds().Width / 2;
                        w += half;
                        var el = figures[i].Transform.Matrix.Elements;
                        el[4] = w;
                        figures[i].Transform.Matrix = new Matrix(el[0], el[1], el[2], el[3], el[4], el[5]);
                        w += half + sW;
                    }
                    break;
                case FigureArrange.Vertical:
                    var sH = (selectionBounds.Height - heigth) / (selection.Count - 1);
                    figures.AddRange(selection.OrderBy(item => item.GetTransformedPath().Path.GetBounds().Top));
                    var h = selectionBounds.Top + figures.First().GetTransformedPath().Path.GetBounds().Height + sH;
                    for (var i = 1; i < figures.Count - 1; i++)
                    {
                        var half = figures[i].GetTransformedPath().Path.GetBounds().Height / 2;
                        h += half;
                        var el = figures[i].Transform.Matrix.Elements;
                        el[5] = h;
                        figures[i].Transform.Matrix = new Matrix(el[0], el[1], el[2], el[3], el[4], el[5]);
                        h += half + sH;
                    }
                    break;
            }
        }

        /// <summary>
        /// Признак того, что фигура не была повёрнута или искажена
        /// </summary>
        /// <param name="figure">Фигура</param>
        /// <returns></returns>
        public static bool IsNotSkewAndRotated(Figure figure)
        {
            return Math.Abs(Helper.GetSkewAngle(figure.Transform.Matrix) - 90) < float.Epsilon &&
                Math.Abs(Helper.GetAngle(figure.Transform.Matrix)) < float.Epsilon;
        }

        /// <summary>
        /// Выравнивание размеров фигур
        /// </summary>
        /// <param name="selection">Текущее выделение фигур</param>
        /// <param name="resize">Направление выравнивания</param>
        public static void SameResize(this Selection selection, FigureResize resize)
        {
            var exists = selection.Count(IsNotSkewAndRotated) > 1;
            if (!exists) return;
            var first = selection.FirstOrDefault(IsNotSkewAndRotated);
            if (first == null) return;
            var size = Helper.GetSize(first.Transform);
            foreach (var figure in selection.Where(IsNotSkewAndRotated))
            {
                var el = figure.Transform.Matrix.Elements;
                switch (resize)
                {
                    case FigureResize.Width:
                        el[0] = size.Width;
                        break;
                    case FigureResize.Heigth:
                        el[3] = size.Height;
                        break;
                    case FigureResize.Both:
                        el[0] = size.Width;
                        el[3] = size.Height;
                        break;
                }
                figure.Transform.Matrix = new Matrix(el[0], el[1], el[2], el[3], el[4], el[5]);
            }
        }

        /// <summary>
        /// Преобразование фигуры к замкнутому полигону или к кривой
        /// </summary>
        public static void ConvertToPath(this IEnumerable<Figure> selection)
        {
            foreach (var fig in selection.Where(fig =>
                                    fig.Geometry.AllowedOperations.HasFlag(AllowedOperations.Pathed)))
            {
                var pathPoints = fig.Geometry.Path.Path.PathPoints;
                var pathTypes = fig.Geometry.Path.Path.PathTypes;
                var hasCurves = pathTypes.Any(item => (item & 0x03) == 0x03);
                if (hasCurves)
                {
                    fig.Geometry = new BezierGeometry(pathPoints, pathTypes) { Name = "Bezier" };
                    if (pathTypes.Length > 0 && (pathTypes.Last() & 0x80) > 0)
                        (fig.Geometry as BezierGeometry).IsClosed = true;
                }
                else
                {
                    fig.Geometry = new PolygoneGeometry() { Name = "Polygon" };
                    (fig.Geometry as PolygoneGeometry).Points = pathPoints;
                }
            }
        }

    }
}
