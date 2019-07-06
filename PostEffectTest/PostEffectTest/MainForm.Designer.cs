namespace PostEffectTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cbChess = new System.Windows.Forms.CheckBox();
            this.lbColorPicker = new System.Windows.Forms.Label();
            this.nudOpacity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudBlur = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.cbEffect = new System.Windows.Forms.ComboBox();
            this.cbOuter = new System.Windows.Forms.CheckBox();
            this.lbFormTitle = new System.Windows.Forms.MouseTransparentLabel();
            this.fastList1 = new FastTreeNS.FastList();
            this.btClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // cbChess
            // 
            this.cbChess.AutoSize = true;
            this.cbChess.Location = new System.Drawing.Point(12, 36);
            this.cbChess.Name = "cbChess";
            this.cbChess.Size = new System.Drawing.Size(69, 21);
            this.cbChess.TabIndex = 0;
            this.cbChess.Text = "Chess";
            this.cbChess.UseVisualStyleBackColor = true;
            this.cbChess.CheckedChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // lbColorPicker
            // 
            this.lbColorPicker.BackColor = System.Drawing.Color.Black;
            this.lbColorPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbColorPicker.Location = new System.Drawing.Point(103, 37);
            this.lbColorPicker.Name = "lbColorPicker";
            this.lbColorPicker.Size = new System.Drawing.Size(53, 20);
            this.lbColorPicker.TabIndex = 1;
            this.lbColorPicker.Click += new System.EventHandler(this.lbColorPicker_Click);
            // 
            // nudOpacity
            // 
            this.nudOpacity.Location = new System.Drawing.Point(243, 38);
            this.nudOpacity.Name = "nudOpacity";
            this.nudOpacity.Size = new System.Drawing.Size(61, 22);
            this.nudOpacity.TabIndex = 2;
            this.nudOpacity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudOpacity.ValueChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Opacity:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Blur:";
            // 
            // nudBlur
            // 
            this.nudBlur.Location = new System.Drawing.Point(363, 38);
            this.nudBlur.Name = "nudBlur";
            this.nudBlur.Size = new System.Drawing.Size(61, 22);
            this.nudBlur.TabIndex = 4;
            this.nudBlur.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudBlur.ValueChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(441, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Distance:";
            // 
            // nudDistance
            // 
            this.nudDistance.Location = new System.Drawing.Point(514, 38);
            this.nudDistance.Name = "nudDistance";
            this.nudDistance.Size = new System.Drawing.Size(61, 22);
            this.nudDistance.TabIndex = 6;
            this.nudDistance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDistance.ValueChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // cbEffect
            // 
            this.cbEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEffect.FormattingEnabled = true;
            this.cbEffect.Location = new System.Drawing.Point(12, 78);
            this.cbEffect.Name = "cbEffect";
            this.cbEffect.Size = new System.Drawing.Size(171, 24);
            this.cbEffect.TabIndex = 8;
            this.cbEffect.SelectedIndexChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // cbOuter
            // 
            this.cbOuter.AutoSize = true;
            this.cbOuter.Location = new System.Drawing.Point(592, 39);
            this.cbOuter.Name = "cbOuter";
            this.cbOuter.Size = new System.Drawing.Size(66, 21);
            this.cbOuter.TabIndex = 9;
            this.cbOuter.Text = "Outer";
            this.cbOuter.UseVisualStyleBackColor = true;
            this.cbOuter.CheckedChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // lbFormTitle
            // 
            this.lbFormTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.lbFormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbFormTitle.ForeColor = System.Drawing.Color.White;
            this.lbFormTitle.Location = new System.Drawing.Point(2, 2);
            this.lbFormTitle.Name = "lbFormTitle";
            this.lbFormTitle.Size = new System.Drawing.Size(794, 108);
            this.lbFormTitle.TabIndex = 10;
            this.lbFormTitle.Text = "GUI Painter";
            // 
            // fastList1
            // 
            this.fastList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastList1.AutoScroll = true;
            this.fastList1.AutoScrollMinSize = new System.Drawing.Size(0, 322);
            this.fastList1.BackColor = System.Drawing.Color.DodgerBlue;
            this.fastList1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fastList1.FullItemSelect = true;
            this.fastList1.ImageCheckBoxOff = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageCheckBoxOff")));
            this.fastList1.ImageCheckBoxOn = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageCheckBoxOn")));
            this.fastList1.ImageCollapse = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageCollapse")));
            this.fastList1.ImageDefaultIcon = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageDefaultIcon")));
            this.fastList1.ImageEmptyExpand = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageEmptyExpand")));
            this.fastList1.ImageExpand = ((System.Drawing.Image)(resources.GetObject("fastList1.ImageExpand")));
            this.fastList1.IsEditMode = false;
            this.fastList1.ItemCount = 10;
            this.fastList1.ItemHeightDefault = 30;
            this.fastList1.Location = new System.Drawing.Point(681, 109);
            this.fastList1.Name = "fastList1";
            this.fastList1.SelectionColor = System.Drawing.Color.PowderBlue;
            this.fastList1.ShowCheckBoxes = true;
            this.fastList1.Size = new System.Drawing.Size(115, 390);
            this.fastList1.TabIndex = 12;
            this.fastList1.Visible = false;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Image = global::PostEffectTest.Properties.Resources.icons8_close_window_50;
            this.btClose.Location = new System.Drawing.Point(768, 2);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(28, 25);
            this.btClose.TabIndex = 13;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(798, 498);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.fastList1);
            this.Controls.Add(this.cbOuter);
            this.Controls.Add(this.cbEffect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudDistance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudBlur);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudOpacity);
            this.Controls.Add(this.lbColorPicker);
            this.Controls.Add(this.cbChess);
            this.Controls.Add(this.lbFormTitle);
            this.MoveOnWholeForm = true;
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbChess;
        private System.Windows.Forms.Label lbColorPicker;
        private System.Windows.Forms.NumericUpDown nudOpacity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudBlur;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.ComboBox cbEffect;
        private System.Windows.Forms.CheckBox cbOuter;
        private System.Windows.Forms.MouseTransparentLabel lbFormTitle;
        private FastTreeNS.FastList fastList1;
        private System.Windows.Forms.Button btClose;
    }
}

