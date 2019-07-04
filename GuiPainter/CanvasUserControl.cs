using System.Windows.Forms;

namespace GuiPainter
{
    public partial class CanvasUserControl : UserControl
    {
        public CanvasUserControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
    }
}
