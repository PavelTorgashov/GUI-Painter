using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию полигона
    /// </summary>
    [Serializable]
    public sealed class PolygoneGeometry : Geometry, IDisposable, ITransformedGeometry
    {
        private PointF[] _points;

        private readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();
        private readonly AllowedOperations _allowedOperations;
        public bool IsClosed { get; set; } = true;
        public bool IsSmoothed { get; set; }

        public byte[] GetTransformedPointTypes(Figure owner)
        {
            return (byte[])_path.Path.PathTypes.Clone();
        }

        /// <summary>
        /// Точки контура фигуры
        /// </summary>
        public PointF[] Points
        {
            get { return _points; }
            set { _points = value; }
        }

        /// <summary>
        /// Get Transformed Points
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public PointF[] GetTransformedPoints(Figure owner)
        {
            var points = (PointF[])Points.Clone();
            owner.Transform.Matrix.TransformPoints(points);
            return points;
        }

        /// <summary>
        /// Set Transformed Points
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="points"></param>
        public void SetTransformedPoints(Figure owner, PointF[] points)
        {
            points = (PointF[])points.Clone();
            var m = owner.Transform.Matrix.Clone();
            m.Invert();
            m.TransformPoints(points);
            Points = points;
        }

        public void Dispose()
        {
            _path?.Dispose();
        }

        /// <summary>
        /// Конструктор с настройками по умолчанию
        /// </summary>
        internal PolygoneGeometry(bool isClosed = true, PointF[] points = null)
        {
            IsClosed = isClosed;
            _allowedOperations = AllowedOperations.All ^ AllowedOperations.Pathed;
            var rect = new RectangleF(-0.5f, -0.5f, 1, 1);
            if (points != null)
            {
                _points = (PointF[])points.Clone();
            }
            else if (isClosed)
                _points = new[]
                    {
                        new PointF(rect.Left, rect.Top),
                        new PointF(rect.Left + rect.Width, rect.Top),
                        new PointF(rect.Left + rect.Width, rect.Top + rect.Height),
                        new PointF(rect.Left, rect.Top + rect.Height)
                    };
            else
                _points = new[]
                    {
                        new PointF(rect.Left, rect.Top),
                        new PointF(rect.Left + rect.Width, rect.Top + rect.Height)
                    };
        }

        public override SerializableGraphicsPath Path
        {
            get
            {
                // сброс пути
                _path.Path.Reset();
                // добавляем в путь построенный по точкам единичного прямоугольника полигон
                if (IsClosed)
                {
                    if (IsSmoothed)
                        _path.Path.AddClosedCurve(_points);
                    else
                        _path.Path.AddPolygon(_points);
                }
                else
                {
                    if (IsSmoothed)
                        _path.Path.AddCurve(_points);
                    else
                        _path.Path.AddLines(_points);
                }
                return _path;
            }
        }

        public override AllowedOperations AllowedOperations { get { return _allowedOperations; } }

        public override string ToString()
        {
            return IsClosed ? "Polygon" : "Polyline";
        }

    }
}
