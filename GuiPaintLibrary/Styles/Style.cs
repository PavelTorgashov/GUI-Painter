using System;

namespace GuiPaintLibrary.Styles
{
    [Serializable]
    public class Style
    {
        public Border BorderStyle { get; set; }
        public Fill FillStyle { get; set; }

        public Style()
        {
            BorderStyle = new Border();
            FillStyle = new DefaultFill();
        }
    }
}
