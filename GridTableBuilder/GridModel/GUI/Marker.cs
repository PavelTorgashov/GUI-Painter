using GuiPaintLibrary.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GridTableBuilder.GridModel.GUI
{
    public class Marker : Figure
    {
        public Cursor Cursor;
        public PointF Position { get; set; }
        public PointF AnchorPosition { get; set; }
        public MarkerType MarkerType { get; set; }

        public void PushPositionToTransform()
        {
            Transform = new Matrix();
            Transform.Matrix.Translate(Position.X, Position.Y);
        }
    }

    public enum MarkerType
    {
        Scale
    }
}
