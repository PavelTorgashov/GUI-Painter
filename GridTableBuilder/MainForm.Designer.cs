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
            this.pnDrawGrid = new GridTableBuilder.Controls.DrawGridPanel();
            this.pnEdgeProperties = new System.Windows.Forms.Panel();
            this.lbRadius = new System.Windows.Forms.Label();
            this.nudRadius = new System.Windows.Forms.NumericUpDown();
            this.btCurve = new System.Windows.Forms.Button();
            this.btLine = new System.Windows.Forms.Button();
            this.btCircleEdge = new System.Windows.Forms.Button();
            this.pnEdgeProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDrawGrid
            // 
            this.pnDrawGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnDrawGrid.BackColor = System.Drawing.Color.White;
            this.pnDrawGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDrawGrid.Location = new System.Drawing.Point(12, 35);
            this.pnDrawGrid.Name = "pnDrawGrid";
            this.pnDrawGrid.Selected = null;
            this.pnDrawGrid.Size = new System.Drawing.Size(1043, 507);
            this.pnDrawGrid.TabIndex = 5;
            this.pnDrawGrid.SelectedChanged += new System.Action<GridTableBuilder.GridModel.ISelectable>(this.pnDrawGrid_SelectedChanged);
            // 
            // pnEdgeProperties
            // 
            this.pnEdgeProperties.Controls.Add(this.lbRadius);
            this.pnEdgeProperties.Controls.Add(this.nudRadius);
            this.pnEdgeProperties.Controls.Add(this.btCurve);
            this.pnEdgeProperties.Controls.Add(this.btLine);
            this.pnEdgeProperties.Controls.Add(this.btCircleEdge);
            this.pnEdgeProperties.Location = new System.Drawing.Point(12, 1);
            this.pnEdgeProperties.Name = "pnEdgeProperties";
            this.pnEdgeProperties.Size = new System.Drawing.Size(774, 30);
            this.pnEdgeProperties.TabIndex = 6;
            // 
            // lbRadius
            // 
            this.lbRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRadius.AutoSize = true;
            this.lbRadius.Location = new System.Drawing.Point(586, 7);
            this.lbRadius.Name = "lbRadius";
            this.lbRadius.Size = new System.Drawing.Size(52, 17);
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
            this.nudRadius.Location = new System.Drawing.Point(644, 5);
            this.nudRadius.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudRadius.Name = "nudRadius";
            this.nudRadius.Size = new System.Drawing.Size(113, 22);
            this.nudRadius.TabIndex = 3;
            this.nudRadius.ValueChanged += new System.EventHandler(this.nudRadius_ValueChanged);
            // 
            // btCurve
            // 
            this.btCurve.Location = new System.Drawing.Point(165, 3);
            this.btCurve.Name = "btCurve";
            this.btCurve.Size = new System.Drawing.Size(75, 25);
            this.btCurve.TabIndex = 2;
            this.btCurve.Text = "Curve";
            this.btCurve.UseVisualStyleBackColor = true;
            this.btCurve.Click += new System.EventHandler(this.btCurve_Click);
            // 
            // btLine
            // 
            this.btLine.Location = new System.Drawing.Point(3, 3);
            this.btLine.Name = "btLine";
            this.btLine.Size = new System.Drawing.Size(75, 25);
            this.btLine.TabIndex = 1;
            this.btLine.Text = "Line";
            this.btLine.UseVisualStyleBackColor = true;
            this.btLine.Click += new System.EventHandler(this.btLine_Click);
            // 
            // btCircleEdge
            // 
            this.btCircleEdge.Location = new System.Drawing.Point(84, 3);
            this.btCircleEdge.Name = "btCircleEdge";
            this.btCircleEdge.Size = new System.Drawing.Size(75, 25);
            this.btCircleEdge.TabIndex = 0;
            this.btCircleEdge.Text = "Circle";
            this.btCircleEdge.UseVisualStyleBackColor = true;
            this.btCircleEdge.Click += new System.EventHandler(this.btCircleEdge_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.pnEdgeProperties);
            this.Controls.Add(this.pnDrawGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GridTableBuilder";
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

