using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public class BorderLessForm : Form
    {
        /// <summary>
        /// Border color
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// Header back color
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent")]
        public Color HeaderBackColor { get; set; }

        /// <summary>
        /// Width of area where user can handle window border
        /// </summary>
        [DefaultValue(4)]
        public int ResizeHandleSize { get; set; }

        /// <summary>
        /// Width of area where user can change window size (right bottom corner)
        /// </summary>
        [DefaultValue(10)]
        public int ResizeHandleSizeBottomRight { get; set; }

        /// <summary>
        /// Window can be resized
        /// </summary>
        [DefaultValue(true)]
        public bool Resizeable { get; set; }

        /// <summary>
        /// Window can be moved
        /// </summary>
        [DefaultValue(true)]
        public bool Moveable { get; set; }

        /// <summary>
        /// Draws grip at right bottom corner
        /// </summary>
        [DefaultValue(true)]
        public bool GripVisible { get; set; }

        /// <summary>
        /// Height of top window header
        /// </summary>
        [DefaultValue(20)]
        public int HeaderHeight { get; set; }

        /// <summary>
        /// Allows to drag form by any part of the window
        /// </summary>
        [DefaultValue(false)]
        public bool MoveOnWholeForm { get; set; }

        public BorderLessForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(2, 2, 2, 2);
            ResizeHandleSize = 4;
            ResizeHandleSizeBottomRight = 10;
            Resizeable = true;
            Moveable = true;
            HeaderHeight = 20;
            HeaderBackColor = Color.Transparent;
            BorderColor = Color.Transparent;
            MoveOnWholeForm = false;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (Resizeable)
                if (e.Y < HeaderHeight && ControlBox)
                {
                    if (WindowState == FormWindowState.Maximized)
                        WindowState = FormWindowState.Normal;
                    else
                        WindowState = FormWindowState.Maximized;
                }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ControlBox)
            {
                if (HeaderBackColor != Color.Transparent)
                {
                    using (var brush = new SolidBrush(HeaderBackColor))
                        e.Graphics.FillRectangle(brush, 0, 0, Width, HeaderHeight);
                }

                if (ShowIcon)
                    e.Graphics.DrawIcon(Icon, new Rectangle(ResizeHandleSize + 1, 1, HeaderHeight, HeaderHeight));
            }

            if (BorderColor != Color.Transparent)
            {
                using (var pen = new Pen(BorderColor))
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

                if (SizeGripStyle != System.Windows.Forms.SizeGripStyle.Hide)
                    if (Resizeable)
                        using (var pen = new Pen(BorderColor, 5))
                        {
                            e.Graphics.DrawLine(pen, Width - 4, Height, Width, Height - 4);
                        }
            }
        }

        #region Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && WindowState != FormWindowState.Maximized)
                if (Moveable && (e.Y < HeaderHeight || MoveOnWholeForm))
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    return;
                }

            base.OnMouseMove(e);
        }
        #endregion

        #region Resize
        protected override void WndProc(ref Message m)
        {
            if (!Resizeable || WindowState == FormWindowState.Maximized)
            {
                base.WndProc(ref m);
                return;
            }

            const UInt32 WM_NCHITTEST = 0x0084;
            const UInt32 WM_MOUSEMOVE = 0x0200;

            const UInt32 HTLEFT = 10;
            const UInt32 HTRIGHT = 11;
            const UInt32 HTBOTTOMRIGHT = 17;
            const UInt32 HTBOTTOM = 15;
            const UInt32 HTBOTTOMLEFT = 16;
            const UInt32 HTTOP = 12;
            const UInt32 HTTOPLEFT = 13;
            const UInt32 HTTOPRIGHT = 14;

            bool handled = false;
            if (m.Msg == WM_NCHITTEST || m.Msg == WM_MOUSEMOVE)
            {
                Size formSize = this.Size;
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);

                var boxes = new Dictionary<UInt32, Rectangle>() {
                    {HTBOTTOMRIGHT, new Rectangle(formSize.Width - ResizeHandleSizeBottomRight, formSize.Height - ResizeHandleSizeBottomRight, ResizeHandleSizeBottomRight, ResizeHandleSizeBottomRight)},
                    {HTBOTTOMLEFT, new Rectangle(0, formSize.Height - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize)},
                    {HTBOTTOM, new Rectangle(ResizeHandleSize, formSize.Height - ResizeHandleSize, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize)},
                    {HTRIGHT, new Rectangle(formSize.Width - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize)},
                    {HTTOPRIGHT, new Rectangle(formSize.Width - ResizeHandleSize, 0, ResizeHandleSize, ResizeHandleSize) },
                    {HTTOP, new Rectangle(ResizeHandleSize, 0, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize) },
                    {HTTOPLEFT, new Rectangle(0, 0, ResizeHandleSize, ResizeHandleSize) },
                    {HTLEFT, new Rectangle(0, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize) }
                };

                foreach (KeyValuePair<UInt32, Rectangle> hitBox in boxes)
                {
                    if (hitBox.Value.Contains(clientPoint))
                    {
                        m.Result = (IntPtr)hitBox.Key;
                        handled = true;
                        break;
                    }
                }
            }

            if (!handled)
                base.WndProc(ref m);
        }
        #endregion
    }
}
