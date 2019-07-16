using GridTableBuilder.Controls;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GridTableBuilder.GridModel.GUI
{
    class TranslucentDrawer : IDisposable
    {
        MouseController mc;
        Image image;
        Rectangle rect;
        Point marker;
        bool down;
        bool sized;

        private float ratio = 1;
        public float MarkersSize => 4;


        public Point Location
        {
            get { return rect.Location; }
            set { rect.Location = value; }
        }

        public Size Size
        {
            get { return rect.Size; }
            set { rect.Size = value; }
        }

        public TranslucentDrawer(MouseController mc)
        {
            this.mc = mc;
            mc.MouseDown += Mc_MouseDown;
            mc.MouseMove += Mc_MouseMove;
            mc.MouseUp += Mc_MouseUp;
        }

        public void Load(Image image)
        {
            if (this.image != null) this.image.Dispose();
            this.image = Translucent(image);
            rect.Location = new Point(100, 100);
            rect.Size = new Size(image.Width, image.Height);
            ratio = (float)rect.Size.Width / rect.Size.Height;
        }

        public void Load(string fileName)
        {
            Load((Bitmap)Image.FromFile(fileName));
        }

        public void Draw(Graphics gr, DrawParams ps)
        {
            if (mc == null) return;

            if (image != null && !rect.IsEmpty)
            {
                gr.DrawImage(image, rect);

                var sz = (int)MarkersSize;

                var mlr = GetLocationRect();
                gr.DrawRectangle(ps.ServiceLineColor.Pen(), mlr);
                gr.DrawLine(ps.ServiceLineColor.Pen(), new Point(mlr.X, mlr.Y + sz), new Point(mlr.X + sz * 2, mlr.Y + sz));
                gr.DrawLine(ps.ServiceLineColor.Pen(), new Point(mlr.X + sz, mlr.Y), new Point(mlr.X + sz, mlr.Y + sz * 2));

                var mls = GetLocationSizeRect();
                gr.DrawRectangle(ps.ServiceLineColor.Pen(), mls);
                gr.DrawLine(ps.ServiceLineColor.Pen(), new Point(mls.X, mls.Y), new Point(mls.X + sz * 2, mls.Y + sz * 2));
            }
        }

        private Rectangle GetLocationRect()
        {
            var sz = (int)MarkersSize;
            var loc = Location;
            loc.Offset(-sz * 3, -sz * 3);
            return new Rectangle(loc, new Size(sz * 2, sz * 2)); 
        }

        private Rectangle GetLocationSizeRect()
        {
            var sz = (int)MarkersSize;
            var loc = Location;
            loc.Offset(rect.Width + sz, rect.Height + sz);
            return new Rectangle(loc, new Size(sz * 2, sz * 2));
        }

        public Cursor GetCursor(Point location)
        {
            return GetLocationRect().Contains(location) ? Cursors.SizeAll
                : GetLocationSizeRect().Contains(location) ? Cursors.SizeNWSE : Cursors.Default;
        }

        private void Mc_MouseDown(MouseEventArgs e)
        {
            var loc = e.Location;
            if (GetLocationRect().Contains(loc))
            {
                down = true;
                marker = loc;
                sized = false;
            }
            else if (GetLocationSizeRect().Contains(loc))
            {
                down = true;
                marker = loc;
                sized = true;
            }
        }

        private void Mc_MouseUp(MouseEventArgs e)
        {
            down = false;
        }

        private void Mc_MouseMove(MouseEventArgs e)
        {
            if (!down) return;

            float padding = MarkersSize * 2;
            var p = new Point(e.Location.X, e.Location.Y);
            var dx = p.X - marker.X;
            var dy = p.Y - marker.Y;
            marker = p;
            if (sized)
            {
                var size = new Size(Size.Width + dx, Size.Height + dy);
                if (size.Width - padding < 0 || size.Height - padding < 0) return;
                size.Width = (int)(size.Height * ratio);
                Size = size;
            }
            else
            {
                var location = new Point(Location.X + dx, Location.Y + dy);
                if (location.X - padding < 0 || location.Y - padding < 0) return;
                Location = location;
            }
        }

        public void Dispose()
        {
            if (mc != null)
            {
                mc.MouseDown -= Mc_MouseDown;
                mc.MouseMove -= Mc_MouseMove;
                mc.MouseUp -= Mc_MouseUp;
                mc = null;
            }
        }

        /// <source>
        /// http://www.cyberforum.ru/csharp-net/thread522535.html#post3956206
        /// </source>
        private static Image Translucent(Image original, byte alpha = 128)
        {
            Bitmap myImage = new Bitmap(original);

            BitmapData imageData = myImage.LockBits(
                                        new Rectangle(0, 0, myImage.Width, myImage.Height),
                                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = imageData.Stride;
            IntPtr Scan0 = imageData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - myImage.Width * 4;
                int nWidth = myImage.Width;
                for (int y = 0; y < myImage.Height; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        //p[0] =... // задаём синий
                        //p[1] =... // задаём зелёный
                        //p[2] =... // задаём красный
                        p[3] = alpha; // задаём альфа канал 0 - полностью прозрачный
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            myImage.UnlockBits(imageData);
            return (Image)myImage;
        }

    }
}
