using GridTableBuilder.GridModel;
using GridTableBuilder.GridModel.GUI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GridTableBuilder.Controls
{
    public class DrawGridPanel : UserControl
    {
        public ISelectable Selected;
        public event Action<ISelectable> SelectedChanged = delegate { };

        MouseController mouse;
        Grid grid;
        GuiBuilder guiBuilder;
        IDragger dragger;
        EdgeDrawer edgeDrawer;

        public DrawGridPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            mouse = new MouseController(this, Mouse_MouseDown, Mouse_MouseMove, Mouse_MouseUp);

            Build(new Grid());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Build(grid);
        }

        public void Build (Grid grid)
        {
            this.grid = grid;
            guiBuilder = new GuiBuilder(grid);
            Selected = null;
            SelectedChanged(null);

            Invalidate(false);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            var ps = new DrawParams();
            foreach (var elem in guiBuilder.AllElements.OfType<IDrawable>().OrderBy(e => e.Priority))
            {
                ps.IsSelected = Selected == elem;
                elem.Draw(pe.Graphics, ps);
            }

            if (edgeDrawer != null)
                edgeDrawer.Draw(pe.Graphics, ps);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (edgeDrawer != null)
                return;

            if (dragger == null)
            {
                //get draggers
                var dr = guiBuilder.AllElements.OfType<IDraggable>().OrderBy(n => n.Priority).Select(n => n.GetDragger(e.Location)).FirstOrDefault(d => d != null);
                //set cursor
                if (dr != null)
                    Cursor = dr.Cursor;
                else
                    Cursor = Cursors.Default;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Selected is Edge)
                {
                    (Selected as Edge).TryRemove();
                    Invalidate(false);
                }
            }
            base.OnKeyDown(e);
        }

        private void Mouse_MouseUp(MouseEventArgs e)
        {
            dragger = null;
            edgeDrawer = null;
            Invalidate(false);
        }

        private void Mouse_MouseMove(MouseEventArgs e)
        {
            Invalidate(false);
        }

        private void Mouse_MouseDown(MouseEventArgs e)
        {
            //get selectable
            var newSelectable = guiBuilder.AllElements.OfType<ISelectable>().OrderBy(n => n.Priority).Where(n=>n.IsHit(e.Location)).FirstOrDefault();

            //select
            if (newSelectable != Selected)
            {
                Selected = newSelectable;
                SelectedChanged(newSelectable);
            }

            //get draggers
            dragger = guiBuilder.AllElements.OfType<IDraggable>().OrderBy(n => n.Priority).Select(n => n.GetDragger(e.Location)).FirstOrDefault(d => d != null);

            //start drag
            if (dragger != null)
                dragger.Start(mouse);

            //start draw edge
            if (Selected == null && dragger == null)
            {
                edgeDrawer = new EdgeDrawer();
                edgeDrawer.Start(mouse, grid);
            }else
            {
                edgeDrawer?.Dispose();
                edgeDrawer = null;
            }

            //
            Invalidate();
        }
    }
}
