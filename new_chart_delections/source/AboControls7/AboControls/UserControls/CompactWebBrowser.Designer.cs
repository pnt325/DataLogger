using AboControls.ExtendedControls;
namespace AboControls.UserControls
{
    partial class CompactWebBrowser
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.txtAdress = new System.Windows.Forms.TextBox();
            this.btnRefresh = new RolloverButton();
            this.btnHome = new RolloverButton();
            this.btnStop = new RolloverButton();
            this.btnGoForward = new RolloverButton();
            this.btnGoBack = new RolloverButton();
            this.btnNavigate = new RolloverButton();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(5, 74);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(541, 315);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            // 
            // txtAdress
            // 
            this.txtAdress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdress.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdress.Location = new System.Drawing.Point(5, 5);
            this.txtAdress.Name = "txtAdress";
            this.txtAdress.Size = new System.Drawing.Size(504, 26);
            this.txtAdress.TabIndex = 1;
            this.txtAdress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAdress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAdress_KeyDown);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = global::AboControls.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(295, 33);
            this.btnRefresh.MouseDownImage = global::AboControls.Properties.Resources.Refresh;
            this.btnRefresh.MouseEnterImage = global::AboControls.Properties.Resources.Refresh_Over;
            this.btnRefresh.MouseLeaveImage = global::AboControls.Properties.Resources.Refresh;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(36, 36);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnHome
            // 
            this.btnHome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnHome.AutoSize = true;
            this.btnHome.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Image = global::AboControls.Properties.Resources.Home;
            this.btnHome.Location = new System.Drawing.Point(258, 33);
            this.btnHome.MouseDownImage = global::AboControls.Properties.Resources.Home;
            this.btnHome.MouseEnterImage = global::AboControls.Properties.Resources.Home_Over;
            this.btnHome.MouseLeaveImage = global::AboControls.Properties.Resources.Home;
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(36, 36);
            this.btnHome.TabIndex = 6;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnStop.AutoSize = true;
            this.btnStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::AboControls.Properties.Resources.Exit;
            this.btnStop.Location = new System.Drawing.Point(222, 33);
            this.btnStop.MouseDownImage = global::AboControls.Properties.Resources.Exit;
            this.btnStop.MouseEnterImage = global::AboControls.Properties.Resources.Exit_Over;
            this.btnStop.MouseLeaveImage = global::AboControls.Properties.Resources.Exit;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(36, 36);
            this.btnStop.TabIndex = 5;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnGoForward
            // 
            this.btnGoForward.AutoSize = true;
            this.btnGoForward.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGoForward.BackColor = System.Drawing.Color.Transparent;
            this.btnGoForward.FlatAppearance.BorderSize = 0;
            this.btnGoForward.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGoForward.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGoForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoForward.Image = global::AboControls.Properties.Resources.Forward;
            this.btnGoForward.Location = new System.Drawing.Point(41, 32);
            this.btnGoForward.MouseDownImage = global::AboControls.Properties.Resources.Forward;
            this.btnGoForward.MouseEnterImage = global::AboControls.Properties.Resources.Forward_Over;
            this.btnGoForward.MouseLeaveImage = global::AboControls.Properties.Resources.Forward;
            this.btnGoForward.Name = "btnGoForward";
            this.btnGoForward.Size = new System.Drawing.Size(36, 36);
            this.btnGoForward.TabIndex = 4;
            this.btnGoForward.UseVisualStyleBackColor = false;
            this.btnGoForward.Click += new System.EventHandler(this.btnGoForward_Click);
            // 
            // btnGoBack
            // 
            this.btnGoBack.AutoSize = true;
            this.btnGoBack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGoBack.BackColor = System.Drawing.Color.Transparent;
            this.btnGoBack.FlatAppearance.BorderSize = 0;
            this.btnGoBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGoBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGoBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoBack.Image = global::AboControls.Properties.Resources.Back;
            this.btnGoBack.Location = new System.Drawing.Point(5, 32);
            this.btnGoBack.MouseDownImage = global::AboControls.Properties.Resources.Back;
            this.btnGoBack.MouseEnterImage = global::AboControls.Properties.Resources.Back_Over;
            this.btnGoBack.MouseLeaveImage = global::AboControls.Properties.Resources.Back;
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(36, 36);
            this.btnGoBack.TabIndex = 3;
            this.btnGoBack.UseVisualStyleBackColor = false;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            // 
            // btnNavigate
            // 
            this.btnNavigate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNavigate.AutoSize = true;
            this.btnNavigate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNavigate.BackColor = System.Drawing.Color.Transparent;
            this.btnNavigate.FlatAppearance.BorderSize = 0;
            this.btnNavigate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNavigate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNavigate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavigate.Image = global::AboControls.Properties.Resources.Go;
            this.btnNavigate.Location = new System.Drawing.Point(512, 0);
            this.btnNavigate.MouseDownImage = global::AboControls.Properties.Resources.Go;
            this.btnNavigate.MouseEnterImage = global::AboControls.Properties.Resources.Go_Over;
            this.btnNavigate.MouseLeaveImage = global::AboControls.Properties.Resources.Go;
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(36, 36);
            this.btnNavigate.TabIndex = 2;
            this.btnNavigate.UseVisualStyleBackColor = false;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // CompactWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(157)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtAdress);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.btnGoForward);
            this.Controls.Add(this.btnGoBack);
            this.Controls.Add(this.btnNavigate);
            this.MinimumSize = new System.Drawing.Size(285, 188);
            this.Name = "CompactWebBrowser";
            this.Size = new System.Drawing.Size(552, 395);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompactWebBrowser_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TextBox txtAdress;
        private RolloverButton btnNavigate;
        private RolloverButton btnGoBack;
        private RolloverButton btnGoForward;
        private RolloverButton btnStop;
        private RolloverButton btnHome;
        private RolloverButton btnRefresh;
    }
}
