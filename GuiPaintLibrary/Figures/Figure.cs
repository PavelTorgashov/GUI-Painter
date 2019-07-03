using GuiPaintLibrary.Common;
using GuiPaintLibrary.Geometries;
using System;
using System.Drawing.Drawing2D;
using GuiPaintLibrary.Styles;
using GuiPaintLibrary.Renderers;

namespace GuiPaintLibrary.Figures
{
    [Serializable]
    public class Figure
    {
        public SerializableGraphicsMatrix Transform { get; set; }
        public Geometry Geometry { get; set; }
        public Style Style { get; private set; }
        public Renderer Renderer { get; set; }

        public Figure()
        {
            Transform = new SerializableGraphicsMatrix();
            Style = new Style();
            Renderer = new DefaultRenderer();
        }

        public virtual SerializableGraphicsPath GetTransformedPath()
        {
            var path = (GraphicsPath)Geometry.Path.Path.Clone();
            path.Transform(Transform);
            return path;
        }

        public virtual void PushTransform(Matrix matrix)
        {
            Transform.Matrix.Multiply(matrix, MatrixOrder.Append);
        }

    }
}
