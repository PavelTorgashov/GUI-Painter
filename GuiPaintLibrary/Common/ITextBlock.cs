using System.Drawing;
using System.Windows.Forms;

namespace GuiPaintLibrary.Common
{
    public interface ITextBlock
    {
        /// <summary>
        /// Текст для построения пути
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Имя файла шрифта
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Размер шрифта
        /// </summary>
        float FontSize { get; set; }

        /// <summary>
        /// Тип шрифта
        /// </summary>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Выравнивание текста
        /// </summary>
        ContentAlignment Alignment { get; set; }

        Padding Padding { get; set; }

        bool WordWrap { get; set; }

    }
}
