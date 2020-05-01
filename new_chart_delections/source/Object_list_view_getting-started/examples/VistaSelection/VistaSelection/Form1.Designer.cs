namespace VistaSelection
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnFeature = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnProgress = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnReport = new BrightIdeasSoftware.OLVColumn();
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnFeature);
            this.objectListView1.AllColumns.Add(this.olvColumnProgress);
            this.objectListView1.AllColumns.Add(this.olvColumnReport);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFeature,
            this.olvColumnProgress,
            this.olvColumnReport});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.OverlayImage.Offset = new System.Drawing.Size(0, 0);
            this.objectListView1.OverlayText.Offset = new System.Drawing.Size(0, 0);
            this.objectListView1.OwnerDraw = true;
            this.objectListView1.RowHeight = 48;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.ShowImagesOnSubItems = true;
            this.objectListView1.Size = new System.Drawing.Size(559, 396);
            this.objectListView1.SmallImageList = this.imageListSmall;
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCellFormatEvents = true;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.objectListView1_FormatCell);
            // 
            // olvColumnFeature
            // 
            this.olvColumnFeature.AspectName = "Feature";
            this.olvColumnFeature.FillsFreeSpace = true;
            this.olvColumnFeature.HeaderFont = null;
            this.olvColumnFeature.ImageAspectName = "FeatureIcon";
            this.olvColumnFeature.Text = "Feature";
            // 
            // olvColumnProgress
            // 
            this.olvColumnProgress.AspectName = "Progress";
            this.olvColumnProgress.HeaderFont = null;
            this.olvColumnProgress.ImageAspectName = "ProgressIcon";
            this.olvColumnProgress.Text = "Progress";
            this.olvColumnProgress.Width = 120;
            // 
            // olvColumnReport
            // 
            this.olvColumnReport.AspectName = "Report";
            this.olvColumnReport.HeaderFont = null;
            this.olvColumnReport.Text = "Report";
            this.olvColumnReport.Width = 120;
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "ball");
            this.imageListSmall.Images.SetKeyName(1, "tick");
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "entire_network");
            this.imageListLarge.Images.SetKeyName(1, "scheduled_tasks");
            this.imageListLarge.Images.SetKeyName(2, "search");
            this.imageListLarge.Images.SetKeyName(3, "workgroup");
            this.imageListLarge.Images.SetKeyName(4, "write_document");
            this.imageListLarge.Images.SetKeyName(5, "tick");
            this.imageListLarge.Images.SetKeyName(6, "ball");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 420);
            this.Controls.Add(this.objectListView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnFeature;
        private BrightIdeasSoftware.OLVColumn olvColumnProgress;
        private BrightIdeasSoftware.OLVColumn olvColumnReport;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ImageList imageListSmall;
    }
}

