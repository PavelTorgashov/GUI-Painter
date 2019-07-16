﻿using GridTableBuilder.GridModel;
using GridTableBuilder.GridModel.GUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GridTableBuilder.Controls
{
    public class DrawGridPanel : UserControl
    {
        public ISelectable Selected { get => guiBuilder.Selected; set => guiBuilder.Selected = value; }

        [Category("Appearance"), Description("The current value of the background type drawgridpanel control."), DefaultValue(typeof(BackgroundType), "BackColor")]
        public BackgroundType BackgroundType
        {
            get => backgroundType;
            set
            {
                backgroundType = value;
                Invalidate();
            }
        }

        public event Action<ISelectable> SelectedChanged = delegate { };
        public event Action GridChanged = delegate { };

        MouseController mouse;
        Grid grid;
        GuiBuilder guiBuilder;
        IDragger dragger;
        EdgeDrawer edgeDrawer;

        BackgroundType backgroundType;

        TranslucentDrawer translucentDrawer;

        public void LoadTranslucent(string fileName)
        {
            translucentDrawer.Load(fileName);
        }

        public DrawGridPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            mouse = new MouseController(this, Mouse_MouseDown, Mouse_MouseMove, Mouse_MouseUp);

            translucentDrawer = new TranslucentDrawer(mouse);

            Build(new Grid());
        }

        ~DrawGridPanel()
        {
            translucentDrawer.Dispose();
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

        public Image ChessPattern { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            //adjust draw params
            var ps = new DrawParams() { ServiceLineColor = Color.Silver, DrawLineColor = Color.Black, SelectedLineColor = Color.Blue };

            switch (backgroundType)
            {
                case BackgroundType.Black:
                    ps.ServiceLineColor = Color.Silver;
                    ps.DrawLineColor = Color.White;
                    ps.SelectedLineColor = Color.Aquamarine;
                    break;
                case BackgroundType.Chess:
                    ps.ServiceLineColor = Color.Gray;
                    break;
                case BackgroundType.Gray:
                    ps.ServiceLineColor = Color.Silver.Light(-50);
                    break;
            }

            #region Draw background

            var backgroundRect = new Rectangle(0, 0, Width, Height);
            var colors = new Color[] { this.BackColor, Color.Transparent, Color.White, Color.Black, Color.Silver };
            if (backgroundType == BackgroundType.Chess && ChessPattern != null)
            {
                backgroundRect.Offset(15, 15);
                using (var brush = new TextureBrush(ChessPattern))
                    pe.Graphics.FillRectangle(brush, backgroundRect);
            } else
            {
                using (var brush = new SolidBrush(colors[(int)backgroundType]))
                    pe.Graphics.FillRectangle(brush, backgroundRect);
            }

            #endregion

            #region Draw translucent image

            translucentDrawer.Draw(pe.Graphics, ps);

            #endregion

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
                    Cursor = translucentDrawer.GetCursor(e.Location);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Selected is Edge)
                {
                    (Selected as Edge).TryRemove();
                    Selected = null;
                    SelectedChanged(Selected);
                    GridChanged();
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
            GridChanged();
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

    public enum BackgroundType
    {
        BackColor,
        Chess,
        White,
        Black,
        Gray
    }

}
