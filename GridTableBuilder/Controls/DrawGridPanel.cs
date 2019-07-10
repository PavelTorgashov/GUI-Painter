using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridTableBuilder.Controls
{
    public class DrawGridPanel : UserControl
    {
        MouseController mouse;

        public DrawGridPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            mouse = new MouseController(this, Mouse_MouseDown, Mouse_MouseMove, Mouse_MouseUp);
        }

        private void Mouse_MouseUp(MouseEventArgs e)
        {
        }

        private void Mouse_MouseMove(MouseEventArgs e)
        {
        }

        private void Mouse_MouseDown(MouseEventArgs e)
        {
        }
    }
}
