using System.Drawing;

namespace GridTableBuilder.GridModel
{
    interface IDraggable
    {
        int Priority { get; }
        IDragger GetDragger(PointF mousePos);
    }
}
