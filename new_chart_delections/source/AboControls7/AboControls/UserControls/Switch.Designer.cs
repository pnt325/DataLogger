namespace AboControls.UserControls
{
    partial class Switch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.btnOn = new System.Windows.Forms.CheckBox();
            this.btnOff = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.btnOn);
            // 
            // splitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.btnOff);
            this.SplitContainer.Size = new System.Drawing.Size(288, 37);
            this.SplitContainer.SplitterDistance = 141;
            this.SplitContainer.SplitterWidth = 1;
            this.SplitContainer.TabIndex = 0;
            // 
            // btnOn
            // 
            this.btnOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnOn.AutoSize = true;
            this.btnOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOn.Location = new System.Drawing.Point(0, 0);
            this.btnOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(141, 37);
            this.btnOn.TabIndex = 0;
            this.btnOn.Text = "On";
            this.btnOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnOff
            // 
            this.btnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnOff.AutoSize = true;
            this.btnOff.Checked = true;
            this.btnOff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOff.Location = new System.Drawing.Point(0, 0);
            this.btnOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(146, 37);
            this.btnOff.TabIndex = 0;
            this.btnOff.Text = "Off";
            this.btnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // Switch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Switch";
            this.Size = new System.Drawing.Size(288, 37);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel1.PerformLayout();
            this.SplitContainer.Panel2.ResumeLayout(false);
            this.SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox btnOn;
        private System.Windows.Forms.CheckBox btnOff;
    }
}
