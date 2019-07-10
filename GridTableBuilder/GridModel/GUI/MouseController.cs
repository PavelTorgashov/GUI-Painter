using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridTableBuilder.Controls
{
    class MouseController
    {
        Control parentControl;
        bool isDown;
        public Point MouseDownPoint;

        public event Action<MouseEventArgs> MouseDown = delegate { };
        public event Action<MouseEventArgs> MouseMove = delegate { };
        public event Action<MouseEventArgs> MouseUp = delegate { };

        public MouseController(Control parentControl, Action<MouseEventArgs> mouseDown, Action<MouseEventArgs> mouseMove, Action<MouseEventArgs> mouseUp)
        {
            this.parentControl = parentControl;
            parentControl.MouseDown += ParentControl_MouseDown;
            parentControl.MouseMove += ParentControl_MouseMove;
            parentControl.MouseUp += ParentControl_MouseUp;

            this.MouseUp = mouseUp;
            this.MouseDown = mouseDown;
            this.MouseMove = mouseMove;
        }

        private void ParentControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDown && e.Button == MouseButtons.Left)
            {
                MouseUp(e);
                isDown = false;
            }
        }

        private void ParentControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
                MouseMove(e);
        }

        private void ParentControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isDown)
                    MouseUp(e);
                isDown = true;
                MouseDownPoint = e.Location;
                MouseDown(e);
            }
        }
    }
}
