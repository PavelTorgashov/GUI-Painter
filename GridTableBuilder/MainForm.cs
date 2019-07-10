using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridTableBuilder
{
    public partial class MainForm : Form
    {
        Grid grid;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            grid = new Grid() { Area = new Rectangle(200, 50, 400, 300) };
            grid.Init();
            Text = $"Nodes: {grid.Nodes.Count}, Edges: {grid.Edges.Count}";
            FillTreeView();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                grid.OnLeftMouseDown(e.Location);
                Invalidate();
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            grid.OnMouseMove(e.Location);
            Invalidate();
            if (grid.WorkMode == GridWorkMode.Move)
            {
                if (e.Button == MouseButtons.Left)
                    Cursor = grid.SplitKind == SplitKind.Horizontal
                        ? Cursors.HSplit : grid.SplitKind == SplitKind.Vertical ? Cursors.VSplit : Cursors.Default;
                else
                    Cursor = grid.MouseInVSplit(e.Location) ? Cursors.HSplit : grid.MouseInHSplit(e.Location) ? Cursors.VSplit : Cursors.Default;
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            grid.OnLeftMouseUp(e.Location);
            Invalidate();
            Text = $"Nodes: {grid.Nodes.Count}, Edges: {grid.Edges.Count}";
            FillTreeView();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            grid.OnPaint(e.Graphics);
        }

        private void FillTreeView()
        {
            try
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                foreach (var pn in grid.Nodes)
                {
                    var nd = new TreeNode($"p{pn.Index}");
                    treeView1.Nodes.Add(nd);
                    foreach (var ed in pn.Edges)
                    {
                        var name1 = ed.Node1 != null ? $"p{ed.Node1.Index}" : "?";
                        var name2 = ed.Node2 != null ? $"p{ed.Node2.Index}" : "?";
                        nd.Nodes.Add($"e{ed.Index} ({name1},{name2})");
                    }
                }
                treeView1.ExpandAll();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void rbCreate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCreate.Checked)
                grid.WorkMode = GridWorkMode.Draw;
            else if (rbMove.Checked)
                grid.WorkMode = GridWorkMode.Move;
            else if (rbDelete.Checked)
                grid.WorkMode = GridWorkMode.Erase;
            Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            grid.ShowNodeNames = ((CheckBox)sender).Checked;
            Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            grid.ShowEdgeNames = ((CheckBox)sender).Checked;
            Invalidate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = ((CheckBox)sender).Checked;
        }
    }
}
