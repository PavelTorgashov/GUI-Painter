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
            this.btLine = new System.Windows.Forms.Button();
            this.btCircleEdge = new System.Windows.Forms.Button();
            this.pnEdgeProperties.SuspendLayout();
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
            this.pnEdgeProperties.Controls.Add(this.btLine);
            this.pnEdgeProperties.Controls.Add(this.btCircleEdge);
            this.pnEdgeProperties.Location = new System.Drawing.Point(12, 1);
            this.pnEdgeProperties.Name = "pnEdgeProperties";
            this.pnEdgeProperties.Size = new System.Drawing.Size(343, 30);
            this.pnEdgeProperties.TabIndex = 6;
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
            this.btCircleEdge.Location = new System.Drawing.Point(79, 3);
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
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.DrawGridPanel pnDrawGrid;
        private System.Windows.Forms.Panel pnEdgeProperties;
        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.Button btCircleEdge;
    }
}

