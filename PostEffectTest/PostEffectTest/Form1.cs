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

            path.AddLines(new Point[] 
            { new Point(200, 200),
              new Point(500, 200),
              new Point(500, 400),
              new Point(200, 400)});
            path.CloseFigure();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = cbChess.Checked ? (Brush)new TextureBrush(Properties.Resources.chess) : (Brush)new SolidBrush(Color.White))
                e.Graphics.FillRectangle(brush, ClientRectangle);
            
            DrawEffect(e.Graphics, path);
            using (var brush = new SolidBrush(Color.FromArgb(255, Color.Red)))
                e.Graphics.FillPath(brush, path);
        }

        private void DrawEffect(Graphics gr, GraphicsPath path)
        {
            var opactity = (byte)(255f * (float)nudOpacity.Value / 100f);
            var color = Color.FromArgb(opactity, lbColorPicker.BackColor);
            var size = (int)nudSize.Value;
            var distance = (int)nudDistance.Value;
            var offset = new Point(distance, distance);

            var rect = path.GetBounds();
            var radius = size;
            size *= 3;
            var w = (int)(rect.Width + size * 2 + 4);
            var h = (int)(rect.Height + size * 2 + 4);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.InterpolationMode = InterpolationMode.HighQualityBilinear;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var bmp = new Bitmap(w, h))
            using (var bmpGr = Graphics.FromImage(bmp))
            {
                bmpGr.TranslateTransform(-rect.Location.X + size + 1, -rect.Location.Y + size + 1);
                if (radius > 0)
                {
                    bmpGr.Clear(Color.Black);
                    using (var brush = new SolidBrush(Color.White))
                        bmpGr.FillPath(brush, path);

                    var blur = new GaussianBlurOneChannel(bmp);
                    using (var bmp2 = blur.Process(radius, color))
                    {
                        gr.DrawImage(bmp2, rect.Location.X - size - 1 + offset.X, rect.Location.Y - size - 1 + offset.Y);
                    }
                } else
                {
                    using (var brush = new SolidBrush(color))
                        bmpGr.FillPath(brush, path);
                    gr.DrawImage(bmp, rect.Location.X - size - 1 + offset.X, rect.Location.Y - size - 1 + offset.Y);
                }
            }
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
