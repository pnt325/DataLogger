namespace GettingStarted1
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.olvSongs = new BrightIdeasSoftware.ObjectListView();
            this.titleColumn = new BrightIdeasSoftware.OLVColumn();
            this.sizeColumn = new BrightIdeasSoftware.OLVColumn();
            this.lastPlayedColumn = new BrightIdeasSoftware.OLVColumn();
            this.ratingColumn = new BrightIdeasSoftware.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)(this.olvSongs)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "star");
            this.imageList1.Images.SetKeyName(1, "song");
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 403);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Show Groups";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // olvSongs
            // 
            this.olvSongs.AllColumns.Add(this.titleColumn);
            this.olvSongs.AllColumns.Add(this.sizeColumn);
            this.olvSongs.AllColumns.Add(this.lastPlayedColumn);
            this.olvSongs.AllColumns.Add(this.ratingColumn);
            this.olvSongs.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumn,
            this.sizeColumn,
            this.lastPlayedColumn,
            this.ratingColumn});
            this.olvSongs.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvSongs.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvSongs.Location = new System.Drawing.Point(12, 26);
            this.olvSongs.Name = "olvSongs";
            this.olvSongs.ShowGroups = false;
            this.olvSongs.Size = new System.Drawing.Size(565, 370);
            this.olvSongs.SmallImageList = this.imageList1;
            this.olvSongs.TabIndex = 0;
            this.olvSongs.UseAlternatingBackColors = true;
            this.olvSongs.UseCompatibleStateImageBehavior = false;
            this.olvSongs.View = System.Windows.Forms.View.Details;
            // 
            // titleColumn
            // 
            this.titleColumn.AspectName = "Title";
            this.titleColumn.Text = "Title";
            this.titleColumn.Width = 160;
            // 
            // sizeColumn
            // 
            this.sizeColumn.AspectName = "SizeInBytes";
            this.sizeColumn.AspectToStringFormat = "";
            this.sizeColumn.Text = "Size";
            this.sizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeColumn.Width = 94;
            // 
            // lastPlayedColumn
            // 
            this.lastPlayedColumn.AspectName = "LastPlayed";
            this.lastPlayedColumn.AspectToStringFormat = "{0:d}";
            this.lastPlayedColumn.Text = "Last Played";
            this.lastPlayedColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lastPlayedColumn.Width = 150;
            // 
            // ratingColumn
            // 
            this.ratingColumn.AspectName = "Rating";
            this.ratingColumn.Text = "Rating";
            this.ratingColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ratingColumn.Width = 85;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 430);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.olvSongs);
            this.Name = "Form1";
            this.Text = "ObjectListView Getting Started 2";
            ((System.ComponentModel.ISupportInitialize)(this.olvSongs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvSongs;
        private BrightIdeasSoftware.OLVColumn titleColumn;
        private BrightIdeasSoftware.OLVColumn sizeColumn;
        private BrightIdeasSoftware.OLVColumn lastPlayedColumn;
        private BrightIdeasSoftware.OLVColumn ratingColumn;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

