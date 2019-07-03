using GuiPaintLibrary.Figures;
using System.Drawing;

namespace GuiPaintLibrary.Styles
{
    public interface IGradientFill
    {
        PointF[] GetGradientPoints(Figure figure);
        void SetGradientPoints(Figure figure, PointF[] points);
        Color GradientColor { get; set; }
    }
}
