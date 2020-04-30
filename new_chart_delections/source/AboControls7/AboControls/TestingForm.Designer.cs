namespace AboControls
{
    partial class TestingForm
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
            this.digitalClock1 = new AboControls.ExtendedControls.DigitalClock();
            this.colorPickerControl1 = new AboControls.ExtendedControls.ColorPickerControl();
            this.SuspendLayout();
            // 
            // digitalClock1
            // 
            this.digitalClock1.AlarmSoundLocation = "";
            this.digitalClock1.Location = new System.Drawing.Point(23, 182);
            this.digitalClock1.MinimumSize = new System.Drawing.Size(20, 10);
            this.digitalClock1.Name = "digitalClock1";
            this.digitalClock1.OutlineThickness = 0F;
            this.digitalClock1.Size = new System.Drawing.Size(126, 70);
            this.digitalClock1.TabIndex = 1;
            this.digitalClock1.Text = "18:16";
            // 
            // colorPickerControl1
            // 
            this.colorPickerControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.colorPickerControl1.Location = new System.Drawing.Point(12, 12);
            this.colorPickerControl1.Name = "colorPickerControl1";
            this.colorPickerControl1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.colorPickerControl1.Size = new System.Drawing.Size(374, 150);
            this.colorPickerControl1.TabIndex = 0;
            this.colorPickerControl1.Text = "colorPickerControl1";
            this.colorPickerControl1.ColorPicked += new System.EventHandler(this.colorPickerControl1_ColorPicked);
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(725, 626);
            this.Controls.Add(this.digitalClock1);
            this.Controls.Add(this.colorPickerControl1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.KeyPreview = true;
            this.Name = "TestingForm";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ResumeLayout(false);

        }
















































































































        #endregion

        private ExtendedControls.ColorPickerControl colorPickerControl1;
        private ExtendedControls.DigitalClock digitalClock1;
    }
}