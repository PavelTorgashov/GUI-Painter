using GridTableBuilder.GridModel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridTableBuilder
{
    public partial class MainForm : Form
    {
        ISelectable Selected => pnDrawGrid.Selected;
        Edge SelectedEdge => Selected as Edge;

        public MainForm()
        {
            InitializeComponent();
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
    }
}
