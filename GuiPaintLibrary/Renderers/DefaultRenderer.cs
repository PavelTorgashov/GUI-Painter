using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Renderers
{
    [Serializable]
    public class DefaultRenderer : Renderer
    {
        public override void Render(Graphics graphics, Figure figure)
        {
            // получаем путь для рисования, трансформированный методом фигуры
            using (var path = figure.GetTransformedPath().Path)
            {
                // если разрешено использование заливки
                if (figure.Style.FillStyle != null && figure.Style.FillStyle.IsVisible)
                    using (var brush = figure.Style.FillStyle.GetBrush(figure))
                        graphics.FillPath(brush, path);
                // если разрешено рисование контура
                if (figure.Style.BorderStyle != null && figure.Style.BorderStyle.IsVisible)
                    using (var pen = figure.Style.BorderStyle.GetPen(figure))
                        graphics.DrawPath(pen, path);
            }
        }

        public override AllowedRendererDecorators AllowedDecorators
        {
            get { return AllowedRendererDecorators.All; }
        }
    }
}
