using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Допустимые операции над рисованием
    /// </summary>
    [Serializable]
    [Flags]
    public enum AllowedRendererDecorators : uint
    {
        None = 0x0,         // ничего нельзя
        Shadow = 0x1,       // может задавать тень
        Glow = 0x2,         // может задавать "свечение"
        TextBlock = 0x4,    // может задавать внутренний текстовый блок
        Warp = 0x8,         // может задавать искажение
        Arrow = 0x10,       // может задавать стрелки на концах линий
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }

    [Serializable]
    public abstract class Renderer
    {
        public abstract void Render(Graphics graphics, Figure figure);
        public abstract AllowedRendererDecorators AllowedDecorators { get; }
    }
}
