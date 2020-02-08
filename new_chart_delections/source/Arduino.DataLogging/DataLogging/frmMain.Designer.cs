namespace DataLogging
{
    partial class frmMain
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
            this.panelDraw = new System.Windows.Forms.Panel();
            this.btnOpenLayout = new System.Windows.Forms.Button();
            this.btnSaveLayout = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.txbGridX = new System.Windows.Forms.TextBox();
            this.txbGridY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txbScanRate = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbPort = new System.Windows.Forms.ComboBox();
            this.cbbBaud = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // panelDraw
            // 
            this.panelDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDraw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDraw.Location = new System.Drawing.Point(0, 0);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(1025, 563);
            this.panelDraw.TabIndex = 0;
            this.panelDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelDraw_Paint);
            // 
            // btnOpenLayout
            // 
            this.btnOpenLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenLayout.Location = new System.Drawing.Point(8, 571);
            this.btnOpenLayout.Name = "btnOpenLayout";
            this.btnOpenLayout.Size = new System.Drawing.Size(90, 25);
            this.btnOpenLayout.TabIndex = 1;
            this.btnOpenLayout.Text = "Open Layout";
            this.btnOpenLayout.UseVisualStyleBackColor = true;
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveLayout.Location = new System.Drawing.Point(110, 571);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(90, 25);
            this.btnSaveLayout.TabIndex = 1;
            this.btnSaveLayout.Text = "Save Layout";
            this.btnSaveLayout.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(206, 571);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(90, 25);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // txbGridX
            // 
            this.txbGridX.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txbGridX.Location = new System.Drawing.Point(452, 573);
            this.txbGridX.Name = "txbGridX";
            this.txbGridX.Size = new System.Drawing.Size(37, 20);
            this.txbGridX.TabIndex = 2;
            this.txbGridX.Text = "5";
            this.txbGridX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txbGridY
            // 
            this.txbGridY.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txbGridY.Location = new System.Drawing.Point(513, 573);
            this.txbGridY.Name = "txbGridY";
            this.txbGridY.Size = new System.Drawing.Size(37, 20);
            this.txbGridY.TabIndex = 2;
            this.txbGridY.Text = "5";
            this.txbGridY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(495, 577);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 577);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Grid";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(632, 577);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Scan Rate";
            // 
            // txbScanRate
            // 
            this.txbScanRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txbScanRate.Location = new System.Drawing.Point(696, 573);
            this.txbScanRate.MaxLength = 4;
            this.txbScanRate.Name = "txbScanRate";
            this.txbScanRate.Size = new System.Drawing.Size(54, 20);
            this.txbScanRate.TabIndex = 4;
            this.txbScanRate.Text = "100";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(925, 571);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 25);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(752, 577);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Hz";
            // 
            // cbbPort
            // 
            this.cbbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbPort.FormattingEnabled = true;
            this.cbbPort.Location = new System.Drawing.Point(783, 573);
            this.cbbPort.Name = "cbbPort";
            this.cbbPort.Size = new System.Drawing.Size(65, 21);
            this.cbbPort.TabIndex = 5;
            this.cbbPort.Text = "COM9";
            // 
            // cbbBaud
            // 
            this.cbbBaud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbBaud.FormattingEnabled = true;
            this.cbbBaud.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400"});
            this.cbbBaud.Location = new System.Drawing.Point(854, 573);
            this.cbbBaud.Name = "cbbBaud";
            this.cbbBaud.Size = new System.Drawing.Size(65, 21);
            this.cbbBaud.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 604);
            this.Controls.Add(this.cbbPort);
            this.Controls.Add(this.cbbBaud);
            this.Controls.Add(this.txbScanRate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbGridY);
            this.Controls.Add(this.txbGridX);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSaveLayout);
            this.Controls.Add(this.btnOpenLayout);
            this.Controls.Add(this.panelDraw);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Data Logging";
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDraw;
        private System.Windows.Forms.Button btnOpenLayout;
        private System.Windows.Forms.Button btnSaveLayout;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txbGridX;
        private System.Windows.Forms.TextBox txbGridY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbScanRate;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbPort;
        private System.Windows.Forms.ComboBox cbbBaud;
    }
}

