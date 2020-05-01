namespace FoobarLookalike
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
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle1 = new BrightIdeasSoftware.HeaderStateStyle();
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle2 = new BrightIdeasSoftware.HeaderStateStyle();
            BrightIdeasSoftware.HeaderStateStyle headerStateStyle3 = new BrightIdeasSoftware.HeaderStateStyle();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnImage = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnTrackNumber = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnTrack = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnDuration = new BrightIdeasSoftware.OLVColumn();
            this.hotItemStyle1 = new BrightIdeasSoftware.HotItemStyle();
            this.headerFormatStyle1 = new BrightIdeasSoftware.HeaderFormatStyle();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnImage);
            this.objectListView1.AllColumns.Add(this.olvColumnTrackNumber);
            this.objectListView1.AllColumns.Add(this.olvColumnTrack);
            this.objectListView1.AllColumns.Add(this.olvColumnDuration);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnImage,
            this.olvColumnTrackNumber,
            this.olvColumnTrack,
            this.olvColumnDuration});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(244)))));
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.HeaderFormatStyle = this.headerFormatStyle1;
            this.objectListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.objectListView1.HeaderUsesThemes = false;
            this.objectListView1.HighlightBackgroundColor = System.Drawing.Color.Black;
            this.objectListView1.HighlightForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.objectListView1.HotItemStyle = this.hotItemStyle1;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.OwnerDraw = true;
            this.objectListView1.RowHeight = 16;
            this.objectListView1.SelectColumnsOnRightClick = false;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(589, 417);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCellFormatEvents = true;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseCustomSelectionColors = true;
            this.objectListView1.UseHotItem = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.objectListView1_FormatCell);
            // 
            // olvColumnImage
            // 
            this.olvColumnImage.Text = "";
            this.olvColumnImage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnImage.Width = 200;
            // 
            // olvColumnTrackNumber
            // 
            this.olvColumnTrackNumber.AspectName = "TrackNumber";
            this.olvColumnTrackNumber.AspectToStringFormat = "{0:00}";
            this.olvColumnTrackNumber.MaximumWidth = 32;
            this.olvColumnTrackNumber.MinimumWidth = 32;
            this.olvColumnTrackNumber.Text = "";
            this.olvColumnTrackNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnTrackNumber.Width = 32;
            // 
            // olvColumnTrack
            // 
            this.olvColumnTrack.AspectName = "Title";
            this.olvColumnTrack.FillsFreeSpace = true;
            this.olvColumnTrack.Text = "Track";
            // 
            // olvColumnDuration
            // 
            this.olvColumnDuration.AspectName = "Duration";
            this.olvColumnDuration.AspectToStringFormat = "{0:hh:mm:ss}";
            this.olvColumnDuration.Text = "Time";
            this.olvColumnDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // hotItemStyle1
            // 
            this.hotItemStyle1.Font = null;
            this.hotItemStyle1.ForeColor = System.Drawing.Color.White;
            // 
            // headerFormatStyle1
            // 
            headerStateStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            headerStateStyle1.ForeColor = System.Drawing.Color.White;
            this.headerFormatStyle1.Hot = headerStateStyle1;
            headerStateStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            headerStateStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.headerFormatStyle1.Normal = headerStateStyle2;
            headerStateStyle3.BackColor = System.Drawing.Color.Gray;
            headerStateStyle3.ForeColor = System.Drawing.Color.White;
            this.headerFormatStyle1.Pressed = headerStateStyle3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(613, 441);
            this.Controls.Add(this.objectListView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.Text = "Foobar Look-Alike";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnTrack;
        private BrightIdeasSoftware.OLVColumn olvColumnTrackNumber;
        private BrightIdeasSoftware.OLVColumn olvColumnDuration;
        private BrightIdeasSoftware.OLVColumn olvColumnImage;
        private BrightIdeasSoftware.HotItemStyle hotItemStyle1;
        private BrightIdeasSoftware.HeaderFormatStyle headerFormatStyle1;
    }
}

