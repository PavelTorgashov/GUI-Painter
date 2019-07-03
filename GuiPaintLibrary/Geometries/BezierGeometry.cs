using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию кривой
    /// </summary>
    [Serializable]
    public sealed class BezierGeometry : Geometry, IDisposable, ITransformedGeometry
    {
        private PointF[] _points;
        private byte[] _types;

        private readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();
        private readonly AllowedOperations _allowedOperations;

        internal BezierGeometry(PointF[] points, byte[] types)
        {
            _allowedOperations = AllowedOperations.All ^ AllowedOperations.Pathed;
            Points = points;
            Types = types;
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
        /// Типы точек контура фигуры
        /// </summary>
        public byte[] Types
        {
            get { return _types; }
            set { _types = value; }
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

        public byte[] GetTransformedPointTypes(Figure owner)
        {
            return (byte[])_types.Clone();
        }

        public bool IsFlatten { get; set; }
        public float Flatness { get; set; } = 0.25f;
        public bool IsClosed { get; set; }

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

        /// <summary>
        /// Свойство возвращает путь, построенный по точкам
        /// </summary>
        public override SerializableGraphicsPath Path
        {
            get
            {
                var types = (byte[])_types.Clone();
                if (IsClosed)
                    types[types.Length - 1] |= 0x80;
                else
                    types[types.Length - 1] &= 0x7f;
                _path.Path = new GraphicsPath(_points, types);
                if (IsFlatten)
                    _path.Path.Flatten(new Matrix(), Flatness);
                return _path;
            }
        }

        public override AllowedOperations AllowedOperations { get { return _allowedOperations; } }

        public override string ToString()
        {
            return "Bezier";
        }

        public void Dispose()
        {
            _path?.Dispose();
        }
    }
}
