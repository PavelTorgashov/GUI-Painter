using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Класс рисовальщика фигуры с искажением
    /// </summary>
    [Serializable]
    public class WarpRendererDecorator : RendererDecorator
    {
        private readonly Renderer _renderer;
        private PointF[] _points;

        public WarpRendererDecorator(Renderer renderer)
            : base(renderer)
        {
            _renderer = renderer;
            _points = new[] { new PointF(-0.5f, -0.5f), new PointF(0.5f, -0.5f),
                              new PointF(-0.5f, 0.5f), new PointF(0.5f, 0.5f) };
        }

        /// <summary>
        /// Точки границ градиента
        /// </summary>
        public PointF[] Points
        {
            get { return _points; }
            set { _points = value; }
        }

        /// <summary>
        /// Get Gradient Points
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public PointF[] GetWarpPoints(Figure owner)
        {
            var points = (PointF[])Points.Clone();
            owner.Transform.Matrix.TransformPoints(points);
            return points;
        }

        /// <summary>
        /// Set Gradient Points
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="points"></param>
        public void SetWarpPoints(Figure owner, PointF[] points)
        {
            points = (PointF[])points.Clone();
            var m = owner.Transform.Matrix.Clone();
            m.Invert();
            m.TransformPoints(points);
            Points = points;
        }

        public override void Render(Graphics graphics, Figure figure)
        {
            var imageRenderer = GetBaseRenderer(figure.Renderer) as ImageRenderer;
            if (imageRenderer != null && imageRenderer.Image.Bitmap != null)
            {
                using (var path = figure.GetTransformedPath().Path)
                {
                    // todo: Это, конечно, полурешение. Четвёртая точка не задействуется,
                    // но стандартного пути решения для картинки нет.
                    PointF[] pts = { Points[0],    // destination for upper-left point of original
                                     Points[1],    // destination for upper-right point of original
                                     Points[2]};   // destination for lower-left point of original

                    figure.Transform.Matrix.TransformPoints(pts);
                    var unit = GraphicsUnit.Pixel;
                    var rect = imageRenderer.Image.Bitmap.GetBounds(ref unit);
                    graphics.DrawImage(imageRenderer.Image.Bitmap, pts, rect, GraphicsUnit.Pixel);
                }
                return;
            }
            var baseRenderer = GetBaseRenderer(figure.Renderer) as IRendererTransformedPath;
            // получаем путь для рисования, трансформированный методом фигуры
            using (var path = baseRenderer != null
                                ? baseRenderer.GetTransformedPath(graphics, figure)
                                : figure.GetTransformedPath().Path)
            {
                // поместить код искажения здесь
                path.Warp(Points, path.GetBounds(), figure.Transform.Matrix, WarpMode.Perspective);
                // если разрешено использование заливки
                if (figure.Style.FillStyle != null && figure.Style.FillStyle.IsVisible)
                    // то получаем кисть из стиля рисования фигуры
                    using (var brush = figure.Style.FillStyle.GetBrush(figure))
                        graphics.FillPath(brush, path);
                // если разрешено рисование контура
                if (figure.Style.BorderStyle != null && figure.Style.BorderStyle.IsVisible)
                    // то получаем карандаш из стиля рисования фигуры
                    using (var pen = figure.Style.BorderStyle.GetPen(figure))
                        graphics.DrawPath(pen, path);
            }
        }

        public override AllowedRendererDecorators AllowedDecorators
        {
            get { return AllowedRendererDecorators.All ^ AllowedRendererDecorators.Warp; }
        }

        public override string ToString()
        {
            return "Warp";
        }
    }
}
