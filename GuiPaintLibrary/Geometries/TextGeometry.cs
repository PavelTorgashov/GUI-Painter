using GuiPaintLibrary.Common;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию текстовой строки
    /// </summary>
    [Serializable]
    public sealed class TextGeometry : Geometry, IDisposable
    {
        /// <summary>
        /// Текст для построения пути
        /// </summary>
        public string Text { get; set; }

        public StringAlignment Alignment { get; set; }

        /// <summary>
        /// Имя файла шрифта
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Тип шрифта
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Локальное поле для хранения пути
        /// </summary>
        private readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();

        /// <summary>
        /// Локальное поле для хранения ограничений для операций
        /// </summary>
        private readonly AllowedOperations _allowedOperations;

        /// <summary>
        /// Свойство возвращает путь, построенный по данным строки и свойств шрифта
        /// </summary>
        public override SerializableGraphicsPath Path
        {
            get
            {
                // сброс пути.
                _path.Path.Reset();
                // добавляем в путь текстовую строку
                var text = string.IsNullOrWhiteSpace(Text) ? "(empty)" : Text;
                using (var sf = new StringFormat(StringFormat.GenericTypographic))
                {
                    sf.Alignment = Alignment;
                    _path.Path.AddString(text, new FontFamily(FontName), (int)FontStyle, 14f, PointF.Empty, sf);
                }
                var textBounds = _path.Path.GetBounds();
                var pts = _path.Path.PathPoints;
                var eps = 0.0001f;
                var kfx = (textBounds.Width < eps) ? eps : 1 / textBounds.Width;
                var kfy = (textBounds.Height < eps) ? eps : 1 / textBounds.Height;
                for (var i = 0; i < pts.Length; i++)
                {
                    pts[i].X = kfx * (pts[i].X - textBounds.Left) - 0.5f;
                    pts[i].Y = kfy * (pts[i].Y - textBounds.Top) - 0.5f;
                }
                var ptt = _path.Path.PathTypes;
                _path.Path = new GraphicsPath(pts, ptt);
                // возвращаем настроенный путь
                return _path;
            }
        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedOperations AllowedOperations { get { return _allowedOperations; } }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal TextGeometry()
        {
            _allowedOperations = AllowedOperations.All ^ AllowedOperations.Pathed;
            Text = String.Empty;
            FontName = "Arial";
        }

        public void Dispose()
        {
            _path?.Dispose();
        }

        public override string ToString()
        {
            return "Text Line";
        }
    }
}
