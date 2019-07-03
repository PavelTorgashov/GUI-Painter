using GuiPaintLibrary.Common;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию рамки, изменяющей свои размеры
    /// </summary>
    [Serializable]
    public sealed class FrameGeometry : Geometry, IDisposable
    {
        /// <summary>
        /// Локальное поле для хранения пути
        /// </summary>
        private readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedOperations AllowedOperations { get { return AllowedOperations.None; } }

        public override SerializableGraphicsPath Path
        {
            get
            {
                _path.Path.Reset();
                // коррекция координат углов рамки выбора, вне зависимости от направления выделения
                var minX = Math.Min(StartPoint.X, EndPoint.X);
                var maxX = Math.Max(StartPoint.X, EndPoint.X);
                var minY = Math.Min(StartPoint.Y, EndPoint.Y);
                var maxY = Math.Max(StartPoint.Y, EndPoint.Y);
                var rect = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
                _path.Path.AddRectangle(rect);
                return _path;
            }
        }

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal FrameGeometry(Point startPoint)
        {
            EndPoint = StartPoint = startPoint;
        }

        public void Dispose()
        {
            _path?.Dispose();
        }
    }
}
