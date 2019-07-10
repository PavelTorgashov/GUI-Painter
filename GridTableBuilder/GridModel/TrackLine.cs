using GridTableBuilder.GridModel.GUI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GridTableBuilder.GridModel
{
    public class TrackLine : IDraggable, IDrawable
    {
        public readonly int Location;
        public readonly bool IsHorizontal;
        /// <summary> Parent Grid </summary>
        public readonly Grid Grid;

        public TrackLine(Grid grid, int location, bool isHorizontal)
        {
            Grid = grid;
            IsHorizontal = isHorizontal;
            Location = location;
        }

        #region IDraggable

        int IDraggable.Priority => 10;

        IDragger IDraggable.GetDragger(Point mousePos)
        {
            const int padding = 3;
            var loc = IsHorizontal ? mousePos.Y : mousePos.X;
            if (Math.Abs(loc - Location) <= padding)
                return new TrackLineDragger(this);

            return null;
        }

        #endregion

        #region IDrawable

        int IDrawable.Priority => -1;
        static float[] pattern = new float[] { 2, 3 };

        void IDrawable.Draw(Graphics gr, DrawParams ps)
        {
            using (var pen = new Pen(Color.Silver, 1))
            {
                const int max = 10000;
                pen.DashPattern = pattern;
                if (IsHorizontal)
                    gr.DrawLine(pen, -max, Location, max, Location);
                else
                    gr.DrawLine(pen, Location, max, Location, -max);
            }
        }

        #endregion
    }
}
