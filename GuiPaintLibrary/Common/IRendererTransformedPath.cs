using GuiPaintLibrary.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Common
{
    public interface IRendererTransformedPath
    {
        GraphicsPath GetTransformedPath(Graphics graphics, Figure figure);
    }
}
