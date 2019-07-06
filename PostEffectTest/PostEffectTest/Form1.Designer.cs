namespace PostEffectTest
{
    partial class Form1
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
            this.cbChess = new System.Windows.Forms.CheckBox();
            this.lbColorPicker = new System.Windows.Forms.Label();
            this.nudOpacity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.cbEffect = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // cbChess
            // 
            this.cbChess.AutoSize = true;
            this.cbChess.Location = new System.Drawing.Point(12, 12);
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
            this.lbColorPicker.Location = new System.Drawing.Point(103, 13);
            this.lbColorPicker.Name = "lbColorPicker";
            this.lbColorPicker.Size = new System.Drawing.Size(53, 20);
            this.lbColorPicker.TabIndex = 1;
            this.lbColorPicker.Click += new System.EventHandler(this.lbColorPicker_Click);
            // 
            // nudOpacity
            // 
            this.nudOpacity.Location = new System.Drawing.Point(243, 14);
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
            this.label1.Location = new System.Drawing.Point(177, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Opacity:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Size:";
            // 
            // nudSize
            // 
            this.nudSize.Location = new System.Drawing.Point(363, 14);
            this.nudSize.Name = "nudSize";
            this.nudSize.Size = new System.Drawing.Size(61, 22);
            this.nudSize.TabIndex = 4;
            this.nudSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudSize.ValueChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(441, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Distance:";
            // 
            // nudDistance
            // 
            this.nudDistance.Location = new System.Drawing.Point(514, 14);
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
            this.cbEffect.Location = new System.Drawing.Point(12, 54);
            this.cbEffect.Name = "cbEffect";
            this.cbEffect.Size = new System.Drawing.Size(250, 24);
            this.cbEffect.TabIndex = 8;
            this.cbEffect.SelectedIndexChanged += new System.EventHandler(this.cbChess_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(709, 498);
            this.Controls.Add(this.cbEffect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudDistance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudOpacity);
            this.Controls.Add(this.lbColorPicker);
            this.Controls.Add(this.cbChess);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nudSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.ComboBox cbEffect;
    }
}

