using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Класс рисовальщика текстового блока
    /// </summary>
    [Serializable]
    public class TextRenderer : Renderer, ITextBlock, IRendererTransformedPath
    {
        /// <summary>
        /// Текст для построения пути
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Имя файла шрифта
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Тип шрифта
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Выравнивание текста
        /// </summary>
        public ContentAlignment Alignment { get; set; }

        public Padding Padding { get; set; }

        public bool WordWrap { get; set; }

        public TextRenderer(string text)
        {
            Text = text;
            FontName = "Arial";
            FontSize = 36f;
            Alignment = ContentAlignment.MiddleCenter;
            Padding = new Padding();
        }

        /// <summary>
        /// Метод отрисовки фигуры на канве
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="figure">Фигура со свойствами для рисования</param>
        public override void Render(Graphics graphics, Figure figure)
        {
            if (figure.Style.FillStyle != null && figure.Style.FillStyle.IsVisible)
            {
                using (var gp = GetTransformedPath(graphics, figure))
                using (var brush = figure.Style.FillStyle.GetBrush(figure))
                    graphics.FillPath(brush, gp);
            }
        }

        /// <summary>
        /// Реализация интерфейса IRendererTransformedPath
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="figure"></param>
        /// <returns></returns>
        public virtual GraphicsPath GetTransformedPath(Graphics graphics, Figure figure)
        {
            return GetTextBlockTransformedPath(graphics, figure, this, Padding, WordWrap);
        }

        /// <summary>
        /// Графический путь для рисования текстового блока
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="figure"></param>
        /// <param name="textBlock"></param>
        /// <param name="padding"></param>
        /// <param name="wordWrap"></param>
        /// <returns></returns>
        public static GraphicsPath GetTextBlockTransformedPath(Graphics graphics, Figure figure,
            ITextBlock textBlock, Padding padding, bool wordWrap = false)
        {
            var outRectSize = Helper.GetSize(figure.Transform);
            var text = textBlock.Text;
            var graphicsPath = new GraphicsPath();
            using (var sf = new StringFormat(StringFormat.GenericTypographic))
            {
                Helper.UpdateStringFormat(sf, textBlock.Alignment);
                if (!string.IsNullOrWhiteSpace(textBlock.Text))
                {
                    if (wordWrap)
                    {
                        var size = outRectSize;
                        size.Width -= padding.Left + padding.Right;
                        var rect = new RectangleF(PointF.Empty, size);
                        graphicsPath.AddString(text, new FontFamily(textBlock.FontName),
                            (int)textBlock.FontStyle, textBlock.FontSize, rect, sf);
                    }
                    else
                        graphicsPath.AddString(text, new FontFamily(textBlock.FontName),
                            (int)textBlock.FontStyle, textBlock.FontSize, PointF.Empty, sf);
                }
                else
                    graphicsPath.AddLine(PointF.Empty, PointF.Empty);
            }
            var textBounds = graphicsPath.GetBounds();
            if (textBounds.IsEmpty) return new GraphicsPath();
            var dx = outRectSize.Width - textBounds.Width - padding.Right;
            var dy = outRectSize.Height - textBounds.Height - padding.Bottom;
            switch (textBlock.Alignment)
            {
                case ContentAlignment.TopLeft:
                    dx = padding.Left;
                    dy = padding.Top;
                    break;
                case ContentAlignment.TopCenter:
                    dx = dx / 2f + padding.Left / 2;
                    dy = padding.Top;
                    break;
                case ContentAlignment.TopRight:
                    dy = padding.Top;
                    break;
                case ContentAlignment.MiddleLeft:
                    dx = padding.Left;
                    dy = dy / 2f + padding.Top / 2;
                    break;
                case ContentAlignment.MiddleCenter:
                    dx = dx / 2f + padding.Left / 2;
                    dy = dy / 2f + padding.Top / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    dy = dy / 2f + padding.Top / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    dx = padding.Left;
                    break;
                case ContentAlignment.BottomCenter:
                    dx = dx / 2f + padding.Left / 2;
                    break;
            }
            var eps = Helper.EPSILON;
            var kfx = (outRectSize.Width < eps) ? eps : 1 / outRectSize.Width;
            var kfy = (outRectSize.Height < eps) ? eps : 1 / outRectSize.Height;
            var pts = graphicsPath.PathPoints;
            for (var i = 0; i < pts.Length; i++)
            {
                pts[i].X = kfx * (pts[i].X - textBounds.Left + dx) - 0.5f;
                pts[i].Y = kfy * (pts[i].Y - textBounds.Top + dy) - 0.5f;
            }
            var ptt = graphicsPath.PathTypes;
            figure.Transform.Matrix.TransformPoints(pts);
            return new GraphicsPath(pts, ptt);
        }

        public override AllowedRendererDecorators AllowedDecorators
        {
            get { return AllowedRendererDecorators.All ^ AllowedRendererDecorators.TextBlock; }
        }
    }
}
