using GuiPaintLibrary.Common;
using System;
using System.Drawing;

namespace GuiPaintLibrary.Geometries
{
    public enum WedgeKind
    {
        Arc,
        Chord,
        Pie
    }

    /// <summary>
    /// Содержит геометрию Arc, Chord or Pie
    /// </summary>
    [Serializable]
    public sealed class WedgeGeometry : Geometry, IDisposable
    {

        public WedgeKind Kind { get; set; }
        private readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();

        public WedgeGeometry(WedgeKind kind, float startAngle, float sweepAngle)
        {
            Kind = kind;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }

        public float StartAngle { get; set; }
        public float SweepAngle { get; set; }

        public override SerializableGraphicsPath Path
        {
            get
            {
                _path.Path.Reset();
                var rect = new RectangleF(-0.5f, -0.5f, 1, 1);
                switch (Kind)
                {
                    case WedgeKind.Arc:
                        _path.Path.AddArc(rect, StartAngle, SweepAngle);
                        break;
                    case WedgeKind.Chord:
                        _path.Path.AddArc(rect, StartAngle, SweepAngle);
                        _path.Path.CloseFigure();
                        break;
                    case WedgeKind.Pie:
                        _path.Path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, StartAngle, SweepAngle);
                        break;
                }
                return _path;
            }
        }

        public void Dispose()
        {
            _path?.Dispose();
        }

        public override AllowedOperations AllowedOperations { get { return AllowedOperations.All; } }

        public override string ToString()
        {
            return Kind == WedgeKind.Arc ? "Arc" : Kind == WedgeKind.Chord ? "Chord" : "Pie";
        }

    }
}
