﻿using PostEffectTest.Effects;
using SuperfastBlur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostEffectTest
{
    public partial class Form1 : Form
    {
        GraphicsPath path = new GraphicsPath();

        public Form1()
        {
            InitializeComponent();

            cbEffect.DataSource = new string[] { "DropShadowEffect", "GlowEffect", "BevelEffect", "EmbossEffect" };

            var pos = new Point(200, 200);
            var size = new Size(300, 200);
            path.AddLines(new Point[] 
            { new Point(pos.X, pos.Y),
              new Point(pos.X + size.Width, pos.Y),
              new Point(pos.X + size.Width, pos.Y + size.Height),
              new Point(pos.X, pos.Y + size.Height)});
            path.CloseFigure();
            //path.AddLines(new Point[]
            //{ new Point(pos.X + size.Width, pos.Y),
            //  new Point(pos.X + size.Width + size.Width, pos.Y),
            //  new Point(pos.X + size.Width + size.Width, pos.Y + size.Height),
            //  new Point(pos.X + size.Width, pos.Y + size.Height)});
            //path.CloseFigure();

            path.AddEllipse(500, 500, 100, 100);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var effectName = cbEffect.Text;

            using (var brush = cbChess.Checked ? (Brush)new TextureBrush(Properties.Resources.chess) : (Brush)new SolidBrush(Color.White))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (effectName == "DropShadowEffect")
                RenderShadowEffect(e.Graphics, path);

            new FillEffect() { Color = Color.Gray }.Render(e.Graphics, path);

            if (effectName == "BevelEffect")
                RenderBevelEffect(e.Graphics, path);
            if (effectName == "EmbossEffect")
                RenderEmbossEffect(e.Graphics, path);
            if (effectName == "GlowEffect")
                RenderGlowEffect(e.Graphics, path);
        }

        private void RenderShadowEffect(Graphics gr, GraphicsPath path)
        {
            var e = new DropShadowEffect();
            var opactity = (byte)(255f * (float)nudOpacity.Value / 100f);
            e.Color = Color.FromArgb(opactity, lbColorPicker.BackColor);
            e.Blur = (int)nudBlur.Value;
            e.Distance = (int)nudDistance.Value;

            e.Render(gr, path);
        }

        private void RenderGlowEffect(Graphics gr, GraphicsPath path)
        {
            var e = new GlowEffect() { OuterGlow = cbOuter.Checked };
            var opactity = (byte)(255f * (float)nudOpacity.Value / 100f);
            e.Color = Color.FromArgb(opactity, lbColorPicker.BackColor);
            e.Blur = (int)nudBlur.Value;
            //e.Distance = (int)nudDistance.Value;

            e.Render(gr, path);
        }

        private void RenderBevelEffect(Graphics gr, GraphicsPath path)
        {
            var e = new BevelEffect() { };
            var opactity = (byte)(255f * (float)nudOpacity.Value / 100f);
            e.Color = Color.FromArgb(opactity, lbColorPicker.BackColor);
            e.ColorShadow = Color.FromArgb(GraphicsHelper.Saturate(opactity) , Color.Black);
            e.Blur = (int)nudBlur.Value;
            e.Distance = (int)nudDistance.Value;

            e.Render(gr, path);
        }

        private void RenderEmbossEffect(Graphics gr, GraphicsPath path)
        {
            var e = new EmbossEffect() { };
            var opactity = (byte)(255f * (float)nudOpacity.Value / 100f);
            e.Color = lbColorPicker.BackColor;
            e.ColorShadow = Color.Black;
            e.Distance = (int)nudDistance.Value;

            e.Render(gr, path);
        }

        private void cbChess_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate(false);
        }

        private void lbColorPicker_Click(object sender, EventArgs e)
        {
            var label = sender as Label;
            var dlg = new ColorDialog();
            dlg.Color = label.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
                label.BackColor = dlg.Color;

            Invalidate(false);
        }
    }
}
