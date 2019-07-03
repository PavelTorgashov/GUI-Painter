using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Styles
{
    /// <summary>
    /// Класс хранения данных контура фигуры
    /// </summary>
    [Serializable]
    public class Border
    {
        public Border()
        {
            IsVisible = true;
            Opacity = 255;
            Color = Color.Black;
            Width = 1f;
            DashStyle = DashStyle.Solid;
        }

        public int Opacity { get; set; }
        public float Width { get; set; }
        public Color Color { get; set; }
        public bool IsVisible { get; set; }
        public DashStyle DashStyle { get; set; }

        public Pen GetPen(Figure figure)
        {
            return new Pen(Color.FromArgb(Opacity, Color), Width) { DashStyle = DashStyle };
        }
    }

}
