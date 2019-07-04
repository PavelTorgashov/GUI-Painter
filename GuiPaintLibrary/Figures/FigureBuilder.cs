using GuiPaintLibrary.Common;
using GuiPaintLibrary.Geometries;
using GuiPaintLibrary.Renderers;
using GuiPaintLibrary.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Figures
{
    /// <summary>
    /// Строит компоненты фигуры
    /// </summary>
    public static class FigureBuilder
    {
        private const int MARKER_SIZE = 8;

        /// <summary>
        /// Построение пути для квадрата
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        public static void BuildSquareGeometry(Figure figure)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddRectangle(new RectangleF(-0.5f, -0.5f, 1, 1));
            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^
                (AllowedOperations.Size | AllowedOperations.Vertex))
            { Name = "Square" };
        }

        /// <summary>
        /// Построение пути для прямоугольника
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        public static void BuildRectangleGeometry(Figure figure)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddRectangle(new RectangleF(-0.5f, -0.5f, 1, 1));
            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^ AllowedOperations.Vertex)
            { Name = "Rectangle" };
        }

        /// <summary>
        /// Построение пути для прямоугольника со скругленными углами
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="radius">Радиус скругления (от 0 до 0.5)</param>
        public static void BuildRoundedRectangleGeometry(Figure figure, float radius)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddPath(RoundedRectangle.Create(new RectangleF(-0.5f, -0.5f, 1, 1),
                                                      radius < 0 ? 0 : radius > 0.5f ? 0.5f : radius), false);
            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^ AllowedOperations.Vertex)
            { Name = "RoundedRectangle" };
        }

        /// <summary>
        /// Построение пути для круга
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        public static void BuildCircleGeometry(Figure figure)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddEllipse(new RectangleF(-0.5f, -0.5f, 1, 1));
            figure.Geometry = new PrimitiveGeometry(path,
                AllowedOperations.All ^ (AllowedOperations.Size | AllowedOperations.Rotate | AllowedOperations.Vertex))
            { Name = "Circle" };
        }

        /// <summary>
        /// Построение пути для эллипса
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        public static void BuildEllipseGeometry(Figure figure)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddEllipse(new RectangleF(-0.5f, -0.5f, 1, 1));
            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^ AllowedOperations.Vertex)
            { Name = "Ellipse" };
        }

        /// <summary>
        /// Построение пути для маркера
        /// </summary>
        /// <param name="marker"></param>
        public static void BuildMarkerGeometry(Figure marker)
        {
            var path = new SerializableGraphicsPath();
            marker.Style.BorderStyle.Width = 0;
            // здесь задаём размер макера в 5 единиц и смешение от центра маркера в -2 единицы
            path.Path.AddRectangle(new RectangleF(-MARKER_SIZE / 2f, -MARKER_SIZE / 2f, MARKER_SIZE, MARKER_SIZE));
            marker.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^
                (AllowedOperations.Size | AllowedOperations.Rotate | AllowedOperations.Select |
                 AllowedOperations.Skew | AllowedOperations.Vertex | AllowedOperations.Pathed | AllowedOperations.Warp))
            { Name = "Marker" };
        }

        /// <summary>
        /// Подключаем к фигуре геометрию текстовой строки
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="text">Текстовая строка</param>
        public static void BuildTextGeometry(Figure figure, string text)
        {
            figure.Geometry = new TextGeometry { Text = text, Name = "Text" };
            figure.Style.FillStyle = new DefaultFill(AllowedFillDecorators.All ^
                AllowedFillDecorators.RadialGradient)
            { Color = Color.Black };
            figure.Style.BorderStyle.IsVisible = false; // рамка по умолчанию выключена
        }

        /// <summary>
        /// Подключаем к фигуре геометрию текстового блока
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="text">Текстовая строка</param>
        public static void BuildTextRenderGeometry(Figure figure, string text)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddRectangle(new RectangleF(-0.5f, -0.5f, 1, 1));

            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^
                (AllowedOperations.Vertex | AllowedOperations.Pathed))
            { Name = "TextBlock" };

            figure.Style.BorderStyle = null; // отключение рамки для рендера
            figure.Style.FillStyle = new DefaultFill(AllowedFillDecorators.All ^
                AllowedFillDecorators.RadialGradient)
            { Color = Color.Black };

            figure.Renderer = new TextRenderer(text);
        }

        /// <summary>
        /// Подключаем к фигуре геометрию текста на кривой
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="text">Текстовая строка</param>
        public static void BuildBezierTextGeometry(Figure figure, string text)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddBezier(-0.5f, -0.5f, 0, 1, 0.5f, -0.5f, 1, 1);

            figure.Geometry = new PrimitiveBezier(path, AllowedOperations.All ^
                (AllowedOperations.Pathed | AllowedOperations.Skew))
            { Name = "BezierText" };

            figure.Style.BorderStyle = null; // отключение рамки для рендера
            figure.Style.FillStyle = new DefaultFill(AllowedFillDecorators.All ^
                AllowedFillDecorators.RadialGradient)
            { Color = Color.Black };

            figure.Renderer = new BezierTextRenderer(text) { FontSize = 14f };
        }

        /// <summary>
        /// Подключаем к фигуре геометрию и рендерер внешнего графического файла
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="image"></param>
        public static void BuildImageRenderGeometry(Figure figure, Bitmap image)
        {
            var path = new SerializableGraphicsPath();
            path.Path.AddRectangle(new RectangleF(-0.5f, -0.5f, 1, 1));

            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^
                (AllowedOperations.Vertex | AllowedOperations.Pathed))
            { Name = "Image" };
            figure.Style.BorderStyle = null; // отключение рамки для рендера
            figure.Style.FillStyle = null; // отключение заливки для рендера
            figure.Renderer = new ImageRenderer(image);
        }

        /// <summary>
        /// Подключаем к фигуре геометрию ломаной линии
        /// </summary>
        /// <param name="figure"></param>
        public static void BuildPolyGeometry(Figure figure, bool isClosed = true, PointF[] points = null)
        {
            figure.Style.FillStyle.IsVisible = isClosed;
            figure.Geometry = new PolygoneGeometry(isClosed, points) { Name = isClosed ? "Polygon" : "Polyline" };
            NormalizeVertex(figure);
        }

        /// <summary>
        /// Подключаем к фигуре геометрию кривой линии
        /// </summary>
        /// <param name="figure"></param>
        public static void BuildBezierGeometry(Figure figure, PointF[] points, byte[] types)
        {
            figure.Style.FillStyle.IsVisible = false;
            figure.Geometry = new BezierGeometry(points, types) { Name = "Bezier" };
            if (types.Length > 3)
                NormalizeVertex(figure);
        }

        private static void NormalizeVertex(Figure figure)
        {
            var transformed = figure.Geometry as ITransformedGeometry;
            if (transformed != null)
            {
                var bounds = figure.GetTransformedPath().Path.GetBounds();
                var points = transformed.GetTransformedPoints(figure);
                figure.Transform.Matrix.TransformPoints(points);
                transformed.SetTransformedPoints(figure, points);
                figure.Transform.Matrix = new Matrix();
                var eps = Helper.EPSILON;
                var kfx = (bounds.Width < eps) ? eps : 1 / bounds.Width;
                var kfy = (bounds.Height < eps) ? eps : 1 / bounds.Height;
                var pts = transformed.GetTransformedPoints(figure);
                for (var i = 0; i < pts.Length; i++)
                {
                    pts[i].X = (bounds.Width < eps) ? 0 : (pts[i].X - bounds.X) / bounds.Width - 0.5f;
                    pts[i].Y = (bounds.Height < eps) ? 0 : (pts[i].Y - bounds.Y) / bounds.Height - 0.5f;
                }
                transformed.SetTransformedPoints(figure, pts);
                var m = new Matrix();
                m.Translate(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
                m.Scale(1 / kfx, 1 / kfy);
                figure.Transform.Matrix = m;
            }
        }

        /// <summary>
        /// Подключаем к фигуре геометрию линии дуги, хорды и, как её там...
        /// </summary>
        /// <param name="figure"></param>
        public static void BuildWedgeGeometry(Figure figure, WedgeKind kind)
        {
            figure.Style.FillStyle.IsVisible = kind != WedgeKind.Arc;
            figure.Geometry = new WedgeGeometry(kind, 0, 270);
        }

        /// <summary>
        /// Подключаем к фигуре геометрию полигона
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="vertexCount">Количество вершин</param>
        public static void BuildRegularGeometry(Figure figure, int vertexCount)
        {
            if (vertexCount < 3) throw new ArgumentOutOfRangeException("vertexCount",
                "Количество вершин должно быть три и более.");

            const float radius = 0.5f;
            var points = new List<PointF>();
            var stepAngle = Math.PI * 2 / vertexCount;
            var angle = -Math.PI * 2;
            for (var i = 0; i < vertexCount; i++)
            {
                points.Add(new PointF((float)(radius * Math.Cos(angle)), (float)(radius * Math.Sin(angle))));
                angle += stepAngle;
            }

            var path = new SerializableGraphicsPath();
            path.Path.AddPolygon(points.ToArray());

            var names = new[] { "Nothing", "Circle", "Line", "Triangle", "Romb", "Pentagon", "Gexagon" };

            figure.Geometry = new PrimitiveGeometry(path, AllowedOperations.All ^
                (AllowedOperations.Vertex | AllowedOperations.Size | AllowedOperations.Skew))
            { Name = vertexCount <= 6 ? names[vertexCount] : "Regular" + vertexCount };
        }

        /// <summary>
        /// Определение вида рамки выбора
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="startPoint"></param>
        public static void BuildFrameGeometry(Figure figure, Point startPoint)
        {
            figure.Style.BorderStyle.Width = 0;
            figure.Geometry = new FrameGeometry(startPoint) { Name = "Frame" };
        }

        public static void BuildAddLineGeometry(Figure figure, Point startPoint, bool isClosed, bool isSmoothed)
        {
            figure.Style.BorderStyle.Width = 0;
            figure.Geometry = new AddLineGeometry(startPoint)
            {
                Name = "AddLine",
                IsClosed = isClosed,
                IsSmoothed = isSmoothed
            };
        }
    }
}
