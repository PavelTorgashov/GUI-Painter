using GuiPaintLibrary.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GuiPaintLibrary.Geometries
{
    [Serializable]
    public sealed class AddLineGeometry : Geometry, IDisposable, IGeometry
    {
        public readonly List<PointF> Points = new List<PointF>();

        public PointF EndPoint { get; set; }

        /// <summary>
        /// Признак замкнутого контура фигуры
        /// </summary>
        public bool IsClosed { get; set; } = true;

        public bool IsSmoothed { get; set; }

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
                if (Points.Count > 0)
                {
                    if (IsSmoothed)
                    {
                        var list = new List<PointF>(Points);
                        list.AddRange(new[] { EndPoint });
                        if (list.Count > 2)
                            _path.Path.AddCurve(list.ToArray());
                        else
                            _path.Path.AddLines(list.ToArray());
                    }
                    else
                    {
                        _path.Path.AddLines(Points.ToArray());
                        _path.Path.AddLines(new[] { EndPoint });
                        if (IsClosed)
                            _path.Path.CloseFigure();
                    }
                }
                return _path;
            }
        }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal AddLineGeometry(PointF startPoint)
        {
            Points.Add(startPoint);
            EndPoint = startPoint;
        }

        public void Dispose()
        {
            _path?.Dispose();
        }

        public void AddPoint(Point point)
        {
            if (!Points.Contains(point))
                Points.Add(point);
            EndPoint = point;
        }
    }
}
