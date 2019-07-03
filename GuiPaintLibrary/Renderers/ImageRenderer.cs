using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Класс рисовальщика картинки
    /// </summary>
    [Serializable]
    public sealed class ImageRenderer : Renderer
    {
        public SerializableGraphicsImage Image { get; set; }

        public ImageRenderer(Bitmap image)
        {
            Image = image;
        }

        public override void Render(Graphics graphics, Figure figure)
        {
            using (var path = figure.GetTransformedPath().Path)
            {
                var bounds = path.GetBounds();
                var image = Image.Bitmap;
                if (image == null)
                {
                    using (var pen = new Pen(Color.Black, 0f))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        graphics.DrawRectangles(pen, new[] { bounds });
                        using (var sf = new StringFormat(StringFormat.GenericTypographic))
                        {
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;
                            graphics.DrawString("Image Place Holder", SystemFonts.DefaultFont, Brushes.Black, bounds, sf);
                        }
                    }
                }
                else
                {
                    PointF[] destinationPoints = { new PointF(-0.5f, -0.5f),    // destination for upper-left point of original
                                                   new PointF(0.5f, -0.5f),     // destination for upper-right point of original
                                                   new PointF(-0.5f, 0.5f)};    // destination for lower-left point of original
                    figure.Transform.Matrix.TransformPoints(destinationPoints);
                    var unit = GraphicsUnit.Pixel;
                    var rect = Image.Bitmap.GetBounds(ref unit);
                    graphics.DrawImage(image, destinationPoints, rect, GraphicsUnit.Pixel);
                }
            }
        }

        public override AllowedRendererDecorators AllowedDecorators
        {
            get
            {
                return AllowedRendererDecorators.All ^
                  (AllowedRendererDecorators.Shadow |
                   AllowedRendererDecorators.Glow |
                   AllowedRendererDecorators.TextBlock);
            }
        }
    }
}
