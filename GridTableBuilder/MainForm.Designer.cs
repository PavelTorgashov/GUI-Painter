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
            this.components = new System.ComponentModel.Container();
            this.pnEdgeProperties = new System.Windows.Forms.Panel();
            this.lbRadius = new System.Windows.Forms.Label();
            this.nudRadius = new System.Windows.Forms.NumericUpDown();
            this.btCurve = new System.Windows.Forms.Button();
            this.btLine = new System.Windows.Forms.Button();
            this.btCircleEdge = new System.Windows.Forms.Button();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btBackground = new System.Windows.Forms.ToolStripButton();
            this.btPreview = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btNew = new System.Windows.Forms.ToolStripMenuItem();
            this.btOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.btSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.btLoadTranslucentImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.btQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.fileManager = new System.Windows.Forms.FileManager(this.components);
            this.pnDrawGrid = new GridTableBuilder.Controls.DrawGridPanel();
            this.pnEdgeProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
            this.tsMain.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileManager)).BeginInit();
            this.SuspendLayout();
            // 
            // pnEdgeProperties
            // 
            this.pnEdgeProperties.Controls.Add(this.lbRadius);
            this.pnEdgeProperties.Controls.Add(this.nudRadius);
            this.pnEdgeProperties.Controls.Add(this.btCurve);
            this.pnEdgeProperties.Controls.Add(this.btLine);
            this.pnEdgeProperties.Controls.Add(this.btCircleEdge);
            this.pnEdgeProperties.Location = new System.Drawing.Point(142, 22);
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
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btBackground,
            this.btPreview});
            this.tsMain.Location = new System.Drawing.Point(0, 24);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(800, 27);
            this.tsMain.TabIndex = 7;
            this.tsMain.Text = "toolStrip1";
            // 
            // btBackground
            // 
            this.btBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btBackground.Image = global::GridTableBuilder.Properties.Resources.chess;
            this.btBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btBackground.Name = "btBackground";
            this.btBackground.Size = new System.Drawing.Size(24, 24);
            this.btBackground.Text = "Background";
            this.btBackground.Click += new System.EventHandler(this.btBackground_Click);
            // 
            // btPreview
            // 
            this.btPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPreview.Image = global::GridTableBuilder.Properties.Resources.preview;
            this.btPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(24, 24);
            this.btPreview.Text = "Preview";
            this.btPreview.Click += new System.EventHandler(this.btPreview_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btNew,
            this.btOpen,
            this.btSave,
            this.btSaveAs,
            this.toolStripMenuItem2,
            this.btLoadTranslucentImage,
            this.toolStripMenuItem1,
            this.btQuit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // btNew
            // 
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(207, 22);
            this.btNew.Text = "New";
            // 
            // btOpen
            // 
            this.btOpen.Name = "btOpen";
            this.btOpen.Size = new System.Drawing.Size(207, 22);
            this.btOpen.Text = "Open";
            // 
            // btSave
            // 
            this.btSave.Enabled = false;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(207, 22);
            this.btSave.Text = "Save";
            // 
            // btSaveAs
            // 
            this.btSaveAs.Name = "btSaveAs";
            this.btSaveAs.Size = new System.Drawing.Size(207, 22);
            this.btSaveAs.Text = "Save as ...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
            // 
            // btLoadTranslucentImage
            // 
            this.btLoadTranslucentImage.Name = "btLoadTranslucentImage";
            this.btLoadTranslucentImage.Size = new System.Drawing.Size(207, 22);
            this.btLoadTranslucentImage.Text = "Load translucent image...";
            this.btLoadTranslucentImage.Click += new System.EventHandler(this.btLoadTranslucentImage_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(204, 6);
            // 
            // btQuit
            // 
            this.btQuit.Name = "btQuit";
            this.btQuit.Size = new System.Drawing.Size(207, 22);
            this.btQuit.Text = "Quit";
            // 
            // fileManager
            // 
            this.fileManager.CurrentFileName = null;
            this.fileManager.DefaultFolder = null;
            this.fileManager.DefaultSaveExtension = null;
            this.fileManager.Document = null;
            this.fileManager.DocumentType = "image";
            this.fileManager.IsDocumentChanged = false;
            this.fileManager.MainForm = this;
            this.fileManager.MainFormTitle = "GUI Painter";
            this.fileManager.NewButton = this.btNew;
            this.fileManager.OpenButton = this.btOpen;
            this.fileManager.OpenFilter = "GUI Painter file|*.guip";
            this.fileManager.SaveAsButton = this.btSaveAs;
            this.fileManager.SaveButton = this.btSave;
            this.fileManager.SaveFilter = "GUI Painter file|*.guip";
            this.fileManager.Text = null;
            this.fileManager.DocOpenedOrCreated += new System.EventHandler(this.fileManager_DocOpenedOrCreated);
            this.fileManager.NewDocNeeded += new System.EventHandler<System.Windows.Forms.DocEventArgs>(this.fileManager_NewDocNeeded);
            // 
            // pnDrawGrid
            // 
            this.pnDrawGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnDrawGrid.BackColor = System.Drawing.Color.White;
            this.pnDrawGrid.BackgroundType = GridTableBuilder.Controls.BackgroundType.White;
            this.pnDrawGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDrawGrid.ChessPattern = global::GridTableBuilder.Properties.Resources.chess;
            this.pnDrawGrid.Location = new System.Drawing.Point(9, 53);
            this.pnDrawGrid.Margin = new System.Windows.Forms.Padding(2);
            this.pnDrawGrid.Name = "pnDrawGrid";
            this.pnDrawGrid.PreviewMode = false;
            this.pnDrawGrid.Selected = null;
            this.pnDrawGrid.Size = new System.Drawing.Size(783, 387);
            this.pnDrawGrid.TabIndex = 5;
            this.pnDrawGrid.SelectedChanged += new System.Action<GridTableBuilder.GridModel.ISelectable>(this.pnDrawGrid_SelectedChanged);
            this.pnDrawGrid.GridChanged += new System.Action(this.pnDrawGrid_GridChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnEdgeProperties);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnDrawGrid);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GUI Painter - New image";
            this.pnEdgeProperties.ResumeLayout(false);
            this.pnEdgeProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.DrawGridPanel pnDrawGrid;
        private System.Windows.Forms.Panel pnEdgeProperties;
        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.Button btCircleEdge;
        private System.Windows.Forms.Button btCurve;
        private System.Windows.Forms.Label lbRadius;
        private System.Windows.Forms.NumericUpDown nudRadius;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton btBackground;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btNew;
        private System.Windows.Forms.ToolStripMenuItem btOpen;
        private System.Windows.Forms.ToolStripMenuItem btSave;
        private System.Windows.Forms.ToolStripMenuItem btSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btQuit;
        private System.Windows.Forms.FileManager fileManager;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem btLoadTranslucentImage;
        private System.Windows.Forms.ToolStripButton btPreview;
    }
}

