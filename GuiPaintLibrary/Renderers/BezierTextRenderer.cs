using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Класс рисовальщика текста на кривой
    /// </summary>
    [Serializable]
    public class BezierTextRenderer : TextRenderer
    {
        public BezierTextRenderer(string text) : base(text) { }

        /// <summary>
        /// Метод отрисовки фигуры на канве
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="figure">Фигура со свойствами для рисования</param>
        public override void Render(Graphics graphics, Figure figure)
        {
            if (figure.Style.FillStyle != null && figure.Style.FillStyle.IsVisible)
            {
                using (var brush = figure.Style.FillStyle.GetBrush(figure))
                using (var font = new Font(FontName, FontSize, FontStyle))
                using (var gp = figure.GetTransformedPath())
                {
                    DrawTextOnPath(graphics, brush, font, Text, gp, Alignment);
                }
            }
        }


        /// <summary>
        /// Реализация интерфейса IRendererTransformedPath
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="figure"></param>
        /// <returns></returns>
        public override GraphicsPath GetTransformedPath(Graphics graphics, Figure figure)
        {
            var path = new GraphicsPath();
            if (figure.Style.FillStyle != null && figure.Style.FillStyle.IsVisible)
            {
                using (var brush = figure.Style.FillStyle.GetBrush(figure))
                using (var font = new Font(FontName, FontSize, FontStyle))
                using (var gp = figure.GetTransformedPath())
                {
                    DrawTextOnPath(graphics, brush, font, Text, gp, Alignment, path);
                }
            }
            return path;
        }

        #region using from http://csharphelper.com/blog/2016/01/draw-text-on-a-curve-in-c/, с небольшими доработками

        // Draw some text along a GraphicsPath.
        private void DrawTextOnPath(Graphics gr, Brush brush, Font font,
            string txt, GraphicsPath path, ContentAlignment alignment, GraphicsPath resultPath = null)
        {
            if (resultPath != null) resultPath.Reset();

            // Make a copy so we don't mess up the original.
            path = (GraphicsPath)path.Clone();

            // Flatten the path into segments.
            path.Flatten(new Matrix(), 0.005f);

            var points = path.PathPoints;
            gr.Transform.TransformPoints(points);

            // Draw characters.
            int start_ch = 0;
            PointF start_point = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                PointF end_point = points[i];
                DrawTextOnSegment(gr, brush, font, txt, ref start_ch, ref start_point, end_point, alignment, resultPath);
                if (start_ch >= txt.Length) break;
            }
        }

        #endregion

        #region using from http://csharphelper.com/blog/2016/01/draw-text-on-a-line-segment-in-c/, с небольшими доработками

        // Draw some text along a line segment.
        // Leave char_num pointing to the next character to be drawn.
        // Leave start_point holding the last point used.
        private void DrawTextOnSegment(Graphics gr, Brush brush,
            Font font, string txt, ref int first_ch,
            ref PointF start_point, PointF end_point,
            ContentAlignment alignment, GraphicsPath resultPath = null)
        {
            var kp = new PointF[] { new PointF(1f, 1f) };
            gr.Transform.TransformPoints(kp);

            var dx = end_point.X - start_point.X;
            var dy = end_point.Y - start_point.Y;
            var dist = (float)Math.Sqrt(dx * dx + dy * dy);
            dist /= kp[0].X; // компенсанция выравнивания текста по линии от масштабирования канвы
            dx /= dist;
            dy /= dist;

            // See how many characters will fit.
            int last_ch = first_ch;
            while (last_ch < txt.Length)
            {
                var test_string =
                    txt.Substring(first_ch, last_ch - first_ch + 1);
                if (gr.MeasureString(test_string, font).Width > dist)
                {
                    // This is one too many characters.
                    last_ch--;
                    break;
                }
                last_ch++;
            }
            if (last_ch < first_ch) return;
            if (last_ch >= txt.Length) last_ch = txt.Length - 1;
            var chars_that_fit =
                txt.Substring(first_ch, last_ch - first_ch + 1);

            var hy = 0f;
            if (alignment == ContentAlignment.MiddleLeft ||
                alignment == ContentAlignment.MiddleCenter ||
                alignment == ContentAlignment.MiddleRight)
                hy = -gr.MeasureString(chars_that_fit, font).Height / 2;
            else
            if (alignment == ContentAlignment.TopLeft ||
                alignment == ContentAlignment.TopCenter ||
                alignment == ContentAlignment.TopRight)
                hy = -gr.MeasureString(chars_that_fit, font).Height;
            hy *= kp[0].Y; // компенсанция выравнивания текста по линии от масштабирования канвы
            var angle = (float)(180 * Math.Atan2(dy, dx) / Math.PI);

            if (resultPath != null)
            {
                using (var path = new GraphicsPath())
                {
                    using (var m = new Matrix())
                    {
                        m.Translate(0, hy);
                        m.Rotate(angle, MatrixOrder.Append);
                        m.Translate(start_point.X, start_point.Y, MatrixOrder.Append);
                        path.AddString(chars_that_fit, font.FontFamily, (int)font.Style, font.Size, Point.Empty, null);
                        var pts = path.PathPoints;
                        m.TransformPoints(pts);
                        using (var transpath = new GraphicsPath(pts, path.PathTypes))
                            resultPath.AddPath(transpath, false);
                    }
                }
            }
            else
            {
                // Rotate and translate to position the characters.
                GraphicsState state = gr.Save();

                gr.TranslateTransform(0, hy, MatrixOrder.Append);
                gr.RotateTransform(angle, MatrixOrder.Append);
                gr.TranslateTransform(start_point.X, start_point.Y, MatrixOrder.Append);

                // Draw the characters that fit.
                gr.DrawString(chars_that_fit, font, brush, 0, 0);
                // Restore the saved state.
                gr.Restore(state);
            }

            // Update first_ch and start_point.
            first_ch = last_ch + 1;
            // временно подменим пробелы на подчёркивания, для корректного измерения длины
            var text_width = gr.MeasureString(chars_that_fit.Replace(' ', '_'), font).Width;
            start_point = new PointF(
                start_point.X + dx * text_width,
                start_point.Y + dy * text_width);
        }

        #endregion

        /// <summary>
        /// Свойство возвращает ограничения для подключения декораторов
        /// </summary>
        public override AllowedRendererDecorators AllowedDecorators
        {
            get
            {
                return AllowedRendererDecorators.All ^
                  (AllowedRendererDecorators.TextBlock | AllowedRendererDecorators.Shadow | AllowedRendererDecorators.Glow);
            }
        }
    }
}
