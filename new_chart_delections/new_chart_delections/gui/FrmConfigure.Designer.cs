namespace new_chart_delections.gui
{
    partial class FrmConfigure
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUartConnect = new System.Windows.Forms.Button();
            this.grbUart = new System.Windows.Forms.GroupBox();
            this.cbbBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbcDataRule = new System.Windows.Forms.TabControl();
            this.tbpUart = new System.Windows.Forms.TabPage();
            this.tbpTcp = new System.Windows.Forms.TabPage();
            this.tbpMqtt = new System.Windows.Forms.TabPage();
            this.grbTcpIp = new System.Windows.Forms.GroupBox();
            this.t = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.txbIp = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grbMqtt = new System.Windows.Forms.GroupBox();
            this.btnMqttConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txbMqttPass = new System.Windows.Forms.TextBox();
            this.txbMqttUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txbMqttPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txbMqttServer = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.chbUart = new System.Windows.Forms.CheckBox();
            this.chbTcp = new System.Windows.Forms.CheckBox();
            this.chbMqtt = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.grbUart.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbcDataRule.SuspendLayout();
            this.grbTcpIp.SuspendLayout();
            this.grbMqtt.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbMqtt);
            this.groupBox1.Controls.Add(this.chbTcp);
            this.groupBox1.Controls.Add(this.chbUart);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // btnUartConnect
            // 
            this.btnUartConnect.Location = new System.Drawing.Point(44, 136);
            this.btnUartConnect.Name = "btnUartConnect";
            this.btnUartConnect.Size = new System.Drawing.Size(85, 23);
            this.btnUartConnect.TabIndex = 1;
            this.btnUartConnect.Text = "Connect";
            this.btnUartConnect.UseVisualStyleBackColor = true;
            // 
            // grbUart
            // 
            this.grbUart.Controls.Add(this.btnUartConnect);
            this.grbUart.Controls.Add(this.cbbBaud);
            this.grbUart.Controls.Add(this.label2);
            this.grbUart.Controls.Add(this.cbbPort);
            this.grbUart.Controls.Add(this.label1);
            this.grbUart.Location = new System.Drawing.Point(106, 12);
            this.grbUart.Name = "grbUart";
            this.grbUart.Size = new System.Drawing.Size(135, 166);
            this.grbUart.TabIndex = 2;
            this.grbUart.TabStop = false;
            this.grbUart.Text = "Uart";
            // 
            // cbbBaud
            // 
            this.cbbBaud.FormattingEnabled = true;
            this.cbbBaud.Location = new System.Drawing.Point(44, 46);
            this.cbbBaud.Name = "cbbBaud";
            this.cbbBaud.Size = new System.Drawing.Size(85, 21);
            this.cbbBaud.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Baud";
            // 
            // cbbPort
            // 
            this.cbbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPort.FormattingEnabled = true;
            this.cbbPort.Location = new System.Drawing.Point(44, 19);
            this.cbbPort.Name = "cbbPort";
            this.cbbPort.Size = new System.Drawing.Size(85, 21);
            this.cbbPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbcDataRule);
            this.groupBox3.Location = new System.Drawing.Point(12, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(511, 235);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data rule";
            // 
            // tbcDataRule
            // 
            this.tbcDataRule.Controls.Add(this.tbpUart);
            this.tbcDataRule.Controls.Add(this.tbpTcp);
            this.tbcDataRule.Controls.Add(this.tbpMqtt);
            this.tbcDataRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcDataRule.Location = new System.Drawing.Point(3, 16);
            this.tbcDataRule.Name = "tbcDataRule";
            this.tbcDataRule.SelectedIndex = 0;
            this.tbcDataRule.Size = new System.Drawing.Size(505, 216);
            this.tbcDataRule.TabIndex = 0;
            // 
            // tbpUart
            // 
            this.tbpUart.Location = new System.Drawing.Point(4, 22);
            this.tbpUart.Name = "tbpUart";
            this.tbpUart.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUart.Size = new System.Drawing.Size(497, 190);
            this.tbpUart.TabIndex = 0;
            this.tbpUart.Text = "Uart";
            this.tbpUart.UseVisualStyleBackColor = true;
            // 
            // tbpTcp
            // 
            this.tbpTcp.Location = new System.Drawing.Point(4, 22);
            this.tbpTcp.Name = "tbpTcp";
            this.tbpTcp.Padding = new System.Windows.Forms.Padding(3);
            this.tbpTcp.Size = new System.Drawing.Size(497, 190);
            this.tbpTcp.TabIndex = 1;
            this.tbpTcp.Text = "Tcp/ip";
            this.tbpTcp.UseVisualStyleBackColor = true;
            // 
            // tbpMqtt
            // 
            this.tbpMqtt.Location = new System.Drawing.Point(4, 22);
            this.tbpMqtt.Name = "tbpMqtt";
            this.tbpMqtt.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMqtt.Size = new System.Drawing.Size(497, 190);
            this.tbpMqtt.TabIndex = 2;
            this.tbpMqtt.Text = "Mqtt";
            this.tbpMqtt.UseVisualStyleBackColor = true;
            // 
            // grbTcpIp
            // 
            this.grbTcpIp.Controls.Add(this.t);
            this.grbTcpIp.Controls.Add(this.txbPort);
            this.grbTcpIp.Controls.Add(this.txbIp);
            this.grbTcpIp.Controls.Add(this.lblPort);
            this.grbTcpIp.Controls.Add(this.label3);
            this.grbTcpIp.Location = new System.Drawing.Point(247, 12);
            this.grbTcpIp.Name = "grbTcpIp";
            this.grbTcpIp.Size = new System.Drawing.Size(135, 166);
            this.grbTcpIp.TabIndex = 2;
            this.grbTcpIp.TabStop = false;
            this.grbTcpIp.Text = "Tcp/Ip";
            // 
            // t
            // 
            this.t.Location = new System.Drawing.Point(44, 136);
            this.t.Name = "t";
            this.t.Size = new System.Drawing.Size(85, 23);
            this.t.TabIndex = 1;
            this.t.Text = "Connect";
            this.t.UseVisualStyleBackColor = true;
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(9, 71);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(120, 20);
            this.txbPort.TabIndex = 1;
            // 
            // txbIp
            // 
            this.txbIp.Location = new System.Drawing.Point(9, 32);
            this.txbIp.Name = "txbIp";
            this.txbIp.Size = new System.Drawing.Size(120, 20);
            this.txbIp.TabIndex = 1;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 55);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Server";
            // 
            // grbMqtt
            // 
            this.grbMqtt.Controls.Add(this.btnMqttConnect);
            this.grbMqtt.Controls.Add(this.label4);
            this.grbMqtt.Controls.Add(this.txbMqttPass);
            this.grbMqtt.Controls.Add(this.txbMqttUser);
            this.grbMqtt.Controls.Add(this.label7);
            this.grbMqtt.Controls.Add(this.txbMqttPort);
            this.grbMqtt.Controls.Add(this.label6);
            this.grbMqtt.Controls.Add(this.label5);
            this.grbMqtt.Controls.Add(this.txbMqttServer);
            this.grbMqtt.Location = new System.Drawing.Point(388, 12);
            this.grbMqtt.Name = "grbMqtt";
            this.grbMqtt.Size = new System.Drawing.Size(135, 166);
            this.grbMqtt.TabIndex = 2;
            this.grbMqtt.TabStop = false;
            this.grbMqtt.Text = "Mqtt";
            // 
            // btnMqttConnect
            // 
            this.btnMqttConnect.Location = new System.Drawing.Point(44, 136);
            this.btnMqttConnect.Name = "btnMqttConnect";
            this.btnMqttConnect.Size = new System.Drawing.Size(85, 23);
            this.btnMqttConnect.TabIndex = 1;
            this.btnMqttConnect.Text = "Connect";
            this.btnMqttConnect.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Server";
            // 
            // txbMqttPass
            // 
            this.txbMqttPass.Location = new System.Drawing.Point(41, 110);
            this.txbMqttPass.Name = "txbMqttPass";
            this.txbMqttPass.PasswordChar = '●';
            this.txbMqttPass.Size = new System.Drawing.Size(88, 20);
            this.txbMqttPass.TabIndex = 1;
            // 
            // txbMqttUser
            // 
            this.txbMqttUser.Location = new System.Drawing.Point(41, 84);
            this.txbMqttUser.Name = "txbMqttUser";
            this.txbMqttUser.Size = new System.Drawing.Size(88, 20);
            this.txbMqttUser.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Pass";
            // 
            // txbMqttPort
            // 
            this.txbMqttPort.Location = new System.Drawing.Point(41, 58);
            this.txbMqttPort.Name = "txbMqttPort";
            this.txbMqttPort.Size = new System.Drawing.Size(88, 20);
            this.txbMqttPort.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "User";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port";
            // 
            // txbMqttServer
            // 
            this.txbMqttServer.Location = new System.Drawing.Point(9, 32);
            this.txbMqttServer.Name = "txbMqttServer";
            this.txbMqttServer.Size = new System.Drawing.Size(120, 20);
            this.txbMqttServer.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(458, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(387, 422);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(65, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // chbUart
            // 
            this.chbUart.AutoSize = true;
            this.chbUart.Location = new System.Drawing.Point(6, 19);
            this.chbUart.Name = "chbUart";
            this.chbUart.Size = new System.Drawing.Size(46, 17);
            this.chbUart.TabIndex = 0;
            this.chbUart.Text = "Uart";
            this.chbUart.UseVisualStyleBackColor = true;
            // 
            // chbTcp
            // 
            this.chbTcp.AutoSize = true;
            this.chbTcp.Location = new System.Drawing.Point(7, 43);
            this.chbTcp.Name = "chbTcp";
            this.chbTcp.Size = new System.Drawing.Size(58, 17);
            this.chbTcp.TabIndex = 1;
            this.chbTcp.Text = "Tcp/ip";
            this.chbTcp.UseVisualStyleBackColor = true;
            // 
            // chbMqtt
            // 
            this.chbMqtt.AutoSize = true;
            this.chbMqtt.Location = new System.Drawing.Point(7, 66);
            this.chbMqtt.Name = "chbMqtt";
            this.chbMqtt.Size = new System.Drawing.Size(47, 17);
            this.chbMqtt.TabIndex = 1;
            this.chbMqtt.Text = "Mqtt";
            this.chbMqtt.UseVisualStyleBackColor = true;
            // 
            // FrmConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 455);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grbMqtt);
            this.Controls.Add(this.grbTcpIp);
            this.Controls.Add(this.grbUart);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Connection configure";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbUart.ResumeLayout(false);
            this.grbUart.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tbcDataRule.ResumeLayout(false);
            this.grbTcpIp.ResumeLayout(false);
            this.grbTcpIp.PerformLayout();
            this.grbMqtt.ResumeLayout(false);
            this.grbMqtt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grbUart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grbTcpIp;
        private System.Windows.Forms.GroupBox grbMqtt;
        private System.Windows.Forms.Button btnUartConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbBaud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbIp;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button t;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbMqttPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbMqttServer;
        private System.Windows.Forms.TextBox txbMqttUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbMqttPass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMqttConnect;
        private System.Windows.Forms.TabControl tbcDataRule;
        private System.Windows.Forms.TabPage tbpUart;
        private System.Windows.Forms.TabPage tbpTcp;
        private System.Windows.Forms.TabPage tbpMqtt;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox chbMqtt;
        private System.Windows.Forms.CheckBox chbTcp;
        private System.Windows.Forms.CheckBox chbUart;
    }
}