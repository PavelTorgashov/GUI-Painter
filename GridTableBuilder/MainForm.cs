using GridTableBuilder.GridModel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridTableBuilder
{
    public partial class MainForm : Form
    {
        ISelectable Selected => pnDrawGrid.Selected;

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
        }

        private void btLine_Click(object sender, EventArgs e)
        {
            (Selected as Edge).BuildStraight();
            pnDrawGrid.Invalidate();
        }

        private void btCircleEdge_Click(object sender, EventArgs e)
        {
            (Selected as Edge).BuildCircle();
            pnDrawGrid.Invalidate();
        }
    }
}
