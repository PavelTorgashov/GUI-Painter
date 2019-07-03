using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию фиксированной кривой Безье
    /// </summary>
    [Serializable]
    public class PrimitiveBezier : PrimitiveGeometry, ITransformedGeometry
    {
        private PointF[] _points;
        private byte[] _types;

        internal PrimitiveBezier(SerializableGraphicsPath path, AllowedOperations allowed) : base(path, allowed)
        {
            Name = "Bezier";
            _points = path.Path.PathPoints;
            _types = path.Path.PathTypes;
        }

        public override SerializableGraphicsPath Path => new SerializableGraphicsPath()
        { Path = new System.Drawing.Drawing2D.GraphicsPath(_points, _types) };

        public bool IsClosed => false;

        public PointF[] GetTransformedPoints(Figure owner)
        {
            var points = (PointF[])_points.Clone();
            owner.Transform.Matrix.TransformPoints(points);
            return points;
        }

        public byte[] GetTransformedPointTypes(Figure owner)
        {
            return (byte[])_types.Clone();
        }

        public void SetTransformedPoints(Figure owner, PointF[] points)
        {
            if (points.Length != _points.Length) return;
            points = (PointF[])points.Clone();
            var m = owner.Transform.Matrix.Clone();
            m.Invert();
            m.TransformPoints(points);
            _points = points;
        }
    }
}
