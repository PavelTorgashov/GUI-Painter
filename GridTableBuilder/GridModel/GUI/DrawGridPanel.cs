using GridTableBuilder.GridModel;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridTableBuilder.Controls
{
    public class DrawGridPanel : UserControl
    {
        public ISelectable Selected;
        public event Action<ISelectable> SelectedChanged = delegate { };

        MouseController mouse;
        Grid grid = new Grid();
        IDragger dragger;

        public DrawGridPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            mouse = new MouseController(this, Mouse_MouseDown, Mouse_MouseMove, Mouse_MouseUp);
        }

        public void Build (Grid grid)
        {
            this.grid = grid;
            Selected = null;
            SelectedChanged(null);
            Invalidate(false);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            var ps = new DrawParams();
            grid.AllElements.OfType<IDrawable>().OrderBy(e => e.Priority).ToList().ForEach(e => e.Draw(pe.Graphics, ps));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragger == null)
            {
                //get draggers
                var dr = grid.AllElements.OfType<IDraggable>().OrderBy(n => n.Priority).Select(n => n.GetDragger(e.Location)).FirstOrDefault(d => d != null);
                //set cursor
                if (dr != null)
                    Cursor = dr.Cursor;
                else
                    Cursor = Cursors.Default;
            }
        }

        private void Mouse_MouseUp(MouseEventArgs e)
        {
            dragger = null;
        }

        private void Mouse_MouseMove(MouseEventArgs e)
        {
            Invalidate(false);
        }

        private void Mouse_MouseDown(MouseEventArgs e)
        {
            //get selectable
            var newSelectable = grid.AllElements.OfType<ISelectable>().OrderBy(n => n.Priority).FirstOrDefault();

            //select
            if (newSelectable != Selected)
            {
                Selected = newSelectable;
                SelectedChanged(newSelectable);
            }

            //get draggers
            dragger = grid.AllElements.OfType<IDraggable>().OrderBy(n => n.Priority).Select(n => n.GetDragger(e.Location)).FirstOrDefault(d => d != null);

            //start drag
            if (dragger != null)
                dragger.Start(mouse);
        }
    }
}
