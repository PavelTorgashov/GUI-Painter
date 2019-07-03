using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Styles
{
    /// <summary>
    /// Допустимые операции над заливкой
    /// </summary>
    [Serializable]
    [Flags]
    public enum AllowedFillDecorators : uint
    {
        None = 0x0,             // ничего нельзя
        LinearGradient = 0x1,   // может задавать линейный градиент
        RadialGradient = 0x2,   // может задавать радиальный градиент
        Hatch = 0x4,            // может задавать штриховку
        Texture = 0x8,          // может задавать текстуру
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }

    [Serializable]
    public abstract class Fill
    {
        public virtual int Opacity { get; set; }
        public virtual Color Color { get; set; }
        public virtual bool IsVisible { get; set; }
        public abstract Brush GetBrush(Figure figure);
        public abstract AllowedFillDecorators AllowedDecorators { get; }
    }
}
