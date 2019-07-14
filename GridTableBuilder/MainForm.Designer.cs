namespace GridTableBuilder
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnEdgeProperties = new System.Windows.Forms.Panel();
            this.lbRadius = new System.Windows.Forms.Label();
            this.nudRadius = new System.Windows.Forms.NumericUpDown();
            this.btCurve = new System.Windows.Forms.Button();
            this.btLine = new System.Windows.Forms.Button();
            this.btCircleEdge = new System.Windows.Forms.Button();
            this.pnDrawGrid = new GridTableBuilder.Controls.DrawGridPanel();
            this.pnEdgeProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // pnEdgeProperties
            // 
            this.pnEdgeProperties.Controls.Add(this.lbRadius);
            this.pnEdgeProperties.Controls.Add(this.nudRadius);
            this.pnEdgeProperties.Controls.Add(this.btCurve);
            this.pnEdgeProperties.Controls.Add(this.btLine);
            this.pnEdgeProperties.Controls.Add(this.btCircleEdge);
            this.pnEdgeProperties.Location = new System.Drawing.Point(9, 1);
            this.pnEdgeProperties.Margin = new System.Windows.Forms.Padding(2);
            this.pnEdgeProperties.Name = "pnEdgeProperties";
            this.pnEdgeProperties.Size = new System.Drawing.Size(580, 24);
            this.pnEdgeProperties.TabIndex = 6;
            // 
            // lbRadius
            // 
            this.lbRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRadius.AutoSize = true;
            this.lbRadius.Location = new System.Drawing.Point(440, 6);
            this.lbRadius.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbRadius.Name = "lbRadius";
            this.lbRadius.Size = new System.Drawing.Size(40, 13);
            this.lbRadius.TabIndex = 4;
            this.lbRadius.Text = "Radius";
            // 
            // nudRadius
            // 
            this.nudRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudRadius.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudRadius.Location = new System.Drawing.Point(483, 4);
            this.nudRadius.Margin = new System.Windows.Forms.Padding(2);
            this.nudRadius.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudRadius.Name = "nudRadius";
            this.nudRadius.Size = new System.Drawing.Size(85, 20);
            this.nudRadius.TabIndex = 3;
            this.nudRadius.ValueChanged += new System.EventHandler(this.nudRadius_ValueChanged);
            // 
            // btCurve
            // 
            this.btCurve.Location = new System.Drawing.Point(124, 2);
            this.btCurve.Margin = new System.Windows.Forms.Padding(2);
            this.btCurve.Name = "btCurve";
            this.btCurve.Size = new System.Drawing.Size(56, 20);
            this.btCurve.TabIndex = 2;
            this.btCurve.Text = "Curve";
            this.btCurve.UseVisualStyleBackColor = true;
            this.btCurve.Click += new System.EventHandler(this.btCurve_Click);
            // 
            // btLine
            // 
            this.btLine.Location = new System.Drawing.Point(2, 2);
            this.btLine.Margin = new System.Windows.Forms.Padding(2);
            this.btLine.Name = "btLine";
            this.btLine.Size = new System.Drawing.Size(56, 20);
            this.btLine.TabIndex = 1;
            this.btLine.Text = "Line";
            this.btLine.UseVisualStyleBackColor = true;
            this.btLine.Click += new System.EventHandler(this.btLine_Click);
            // 
            // btCircleEdge
            // 
            this.btCircleEdge.Location = new System.Drawing.Point(63, 2);
            this.btCircleEdge.Margin = new System.Windows.Forms.Padding(2);
            this.btCircleEdge.Name = "btCircleEdge";
            this.btCircleEdge.Size = new System.Drawing.Size(56, 20);
            this.btCircleEdge.TabIndex = 0;
            this.btCircleEdge.Text = "Circle";
            this.btCircleEdge.UseVisualStyleBackColor = true;
            this.btCircleEdge.Click += new System.EventHandler(this.btCircleEdge_Click);
            // 
            // pnDrawGrid
            // 
            this.pnDrawGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnDrawGrid.BackColor = System.Drawing.Color.White;
            this.pnDrawGrid.BackgroundType = GridTableBuilder.Controls.BackgroundType.Chess;
            this.pnDrawGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDrawGrid.Location = new System.Drawing.Point(9, 28);
            this.pnDrawGrid.Margin = new System.Windows.Forms.Padding(2);
            this.pnDrawGrid.Name = "pnDrawGrid";
            this.pnDrawGrid.Selected = null;
            this.pnDrawGrid.Size = new System.Drawing.Size(783, 412);
            this.pnDrawGrid.TabIndex = 5;
            this.pnDrawGrid.SelectedChanged += new System.Action<GridTableBuilder.GridModel.ISelectable>(this.pnDrawGrid_SelectedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnEdgeProperties);
            this.Controls.Add(this.pnDrawGrid);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GridTableBuilder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnEdgeProperties.ResumeLayout(false);
            this.pnEdgeProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.DrawGridPanel pnDrawGrid;
        private System.Windows.Forms.Panel pnEdgeProperties;
        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.Button btCircleEdge;
        private System.Windows.Forms.Button btCurve;
        private System.Windows.Forms.Label lbRadius;
        private System.Windows.Forms.NumericUpDown nudRadius;
    }
}

