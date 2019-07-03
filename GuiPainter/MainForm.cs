using GuiPainter.Controllers;
using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using System;
using System.Windows.Forms;

namespace GuiPainter
{
    public partial class MainForm : Form
    {
        readonly string _caption;
        readonly VersionInfo _versionInfo;

        Layer _layer;
        SelectionController _selectionController;
        UndoRedoController _undoRedoController;

        public MainForm()
        {
            InitializeComponent();
            _versionInfo = new VersionInfo();
            _caption = string.Format("GUI Painter (Ver {0:0.0})",
                (decimal)_versionInfo.Version / 10);
            _layer = new Layer();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
