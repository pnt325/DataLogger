namespace AboControls.UserControls
{
    partial class LockingMech
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
            this.btnLocker = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocker
            // 
            this.btnLocker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLocker.BackgroundImage = global::AboControls.Properties.Resources.GraySlideButton;
            this.btnLocker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLocker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLocker.FlatAppearance.BorderSize = 0;
            this.btnLocker.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLocker.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLocker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocker.Location = new System.Drawing.Point(3, 3);
            this.btnLocker.Name = "btnLocker";
            this.btnLocker.Size = new System.Drawing.Size(80, 54);
            this.btnLocker.TabIndex = 0;
            this.btnLocker.UseVisualStyleBackColor = false;
            this.btnLocker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLocker_MouseDown);
            this.btnLocker.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnLocker_MouseMove);
            this.btnLocker.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLocker_MouseUp);
            // 
            // LockingMech
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.btnLocker);
            this.Name = "LockingMech";
            this.Size = new System.Drawing.Size(300, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocker;
    }
}
