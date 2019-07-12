using GridTableBuilder.GridModel.GUI;
using System;
using System.Drawing;

namespace GridTableBuilder.GridModel
{
    class SizeMarker : IDraggable, IDrawable
    {
        /// <summary> Parent Grid </summary>
        public Grid Grid;

        public PointF Location;

        public float Offset => 4;

        public SizeMarker(Grid grid, PointF location)
        {
            Grid = grid;
            Location = location;
        }

        #region IDraggable

        int IDraggable.Priority => 10;

        IDragger IDraggable.GetDragger(PointF mousePos)
        {
            const int padding = 3;
            var loc = mousePos;
            if (Math.Abs(loc.X - Location.X) <= padding && Math.Abs(loc.Y - Location.Y) <= padding)
                return new SizeMarkerDragger(this);

            return null;
        }

        #endregion

        #region IDrawable

        int IDrawable.Priority => 2;

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(Color.Gray, 1))
            {
                var loc = Location;
                loc.X -= Offset;
                loc.Y -= Offset;
                gr.DrawRectangles(pen, new RectangleF[] { new RectangleF(loc, new SizeF(Offset * 2, Offset * 2)) });
            }
        }

        #endregion

    }

}
