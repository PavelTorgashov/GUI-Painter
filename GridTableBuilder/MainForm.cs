using GridTableBuilder.Controls;
using GridTableBuilder.GridModel;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GridTableBuilder
{
    public partial class MainForm : Form
    {
        ISelectable Selected => pnDrawGrid.Selected;
        Edge SelectedEdge => Selected as Edge;
        Grid grid => (Grid)fileManager.Document;
        string TempFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileName(Application.ExecutablePath) + ".tmp");

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            //autosave
            SaverLoader.SaveToFile(TempFilePath, grid);
        }

        private void pnDrawGrid_SelectedChanged(GridModel.ISelectable obj)
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            pnEdgeProperties.Visible = Selected is Edge;
            lbRadius.Visible = nudRadius.Visible = SelectedEdge != null && (SelectedEdge.Builder is CurveEdgeBuilder);

            pnDrawGrid.Invalidate();
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            SelectedEdge.Builder = new LineEdgeBuilder(SelectedEdge);
            BuildInterface();
        }

        private void btCircleEdge_Click(object sender, EventArgs e)
        {
            if (SelectedEdge.Builder is CircleEdgeBuilder)
                (SelectedEdge.Builder as CircleEdgeBuilder).GoNextDirection();
            else
                SelectedEdge.Builder = new CircleEdgeBuilder(SelectedEdge);
            BuildInterface();
        }

        private void btCurve_Click(object sender, EventArgs e)
        {
            if (SelectedEdge.Builder is CurveEdgeBuilder)
                (SelectedEdge.Builder as CurveEdgeBuilder).GoNextDirection();
            else
                SelectedEdge.Builder = new CurveEdgeBuilder(SelectedEdge);
            nudRadius.Value = (decimal)(SelectedEdge.Builder as CurveEdgeBuilder).Radius;
            BuildInterface();
        }

        private void nudRadius_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedEdge.Builder is CurveEdgeBuilder)
                (SelectedEdge.Builder as CurveEdgeBuilder).Radius = (float)nudRadius.Value;
            pnDrawGrid.Invalidate();
        }

        private void btBackground_Click(object sender, EventArgs e)
        {
            var res = ((int)pnDrawGrid.BackgroundType + 1) % Enum.GetValues(typeof(BackgroundType)).Length;
            pnDrawGrid.BackgroundType = (BackgroundType)res;
            pnDrawGrid.Invalidate();
        }

        private void fileManager_DocOpenedOrCreated(object sender, EventArgs e)
        {
            pnDrawGrid.Build(grid);
            BuildInterface();
        }

        private void fileManager_NewDocNeeded(object sender, DocEventArgs e)
        {
            if (e.FirstDocument)
            if (File.Exists(TempFilePath))
                try
                {
                    e.Document = SaverLoader.LoadFromFile(TempFilePath);
                    return;
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            e.Document = new Grid();
        }

        private void pnDrawGrid_GridChanged()
        {
            fileManager.IsDocumentChanged = true;
        }

        private void btLoadTranslucentImage_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = @"Graphics files (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            pnDrawGrid.LoadTranslucent(dlg.FileName);
            pnDrawGrid.Invalidate();
        }
    }
}
