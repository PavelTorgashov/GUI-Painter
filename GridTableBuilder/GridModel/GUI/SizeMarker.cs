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

        public float Size => 4;

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
            gr.DrawRectangle(Pens.Silver, Location.X - Size, Location.Y - Size, Size * 2, Size * 2);
        }

        #endregion

    }

}
