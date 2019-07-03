using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Styles
{
    [Serializable]
    public class DefaultFill : Fill
    {
        private readonly AllowedFillDecorators _allowedFillDecorators;

        public DefaultFill(AllowedFillDecorators allowedDecorators = AllowedFillDecorators.All)
        {
            IsVisible = true;
            Color = Color.White;
            Opacity = 255;
            _allowedFillDecorators = allowedDecorators;
        }

        public override Brush GetBrush(Figure figure)
        {
            return new SolidBrush(Color.FromArgb(Opacity, Color));
        }

        public override AllowedFillDecorators AllowedDecorators
        {
            get { return _allowedFillDecorators; }
        }
    }
}
