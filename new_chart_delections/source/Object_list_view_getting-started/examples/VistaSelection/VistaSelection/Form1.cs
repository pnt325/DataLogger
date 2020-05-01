using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VistaSelection
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
        }
        public void InitializeModel() {
            this.tasks = new List<Task>();
            this.tasks.Add(new Task("entire_network", "Repair Broken Shortcuts", "Searches for shortcuts that have lost their files and restores them automatically", "ball", "Not selected", ""));
            this.tasks.Add(new Task("scheduled_tasks", "Clean Up Disk", "Find and delete temporary and empty files, cookies, empty recycle bin and lots more good stuff", "ball", "Not selected", ""));
            this.tasks.Add(new Task("search", "Error checking", "Checks the disk file system for errors and corrects them", "tick", "Done", ""));
            this.tasks.Add(new Task("workgroup", "Clean Up Memory", "Compacts and extends memory through true VirtualMemory (tm) technology", "tick", "Done", ""));
            this.tasks.Add(new Task("write_document", "Manage Services", "Display unnecessary or malicious services that are installed", "ball", "Not selected", ""));
        }

        public void InitializeObjectListView() {
            this.olvColumnFeature.AspectToStringConverter = delegate(object x) {
                return "";
            };
            this.objectListView1.SetObjects(this.tasks);
        }

        private List<Task> tasks;

        private void Form1_Load(object sender, EventArgs e) {
            this.InitializeModel();
            this.InitializeObjectListView();
        }

        private void objectListView1_FormatCell(object sender, BrightIdeasSoftware.FormatCellEventArgs e) {
            if (e.ColumnIndex == 0) {
                Task task = (Task)e.Model;
                NamedDescriptionDecoration decoration = new NamedDescriptionDecoration();
                decoration.ImageList = this.imageListLarge;
                decoration.Title = task.Feature;
                decoration.ImageName = task.FeatureIcon;
                decoration.Description = task.Description;
                e.SubItem.Decoration = decoration;
            }
        }
    }

    public class Task 
    {
        public Task(string FeatureIcon, string Feature, string Description, string ProgressIcon, string Progress, string Report) {
            this.FeatureIcon = FeatureIcon;
            this.Feature = Feature;
            this.Description = Description;
            this.ProgressIcon = ProgressIcon;
            this.Progress = Progress;
            this.Report = Report;
        }

        public string FeatureIcon;
        public string Feature;
        public string Description;
        public string ProgressIcon;
        public string Progress;
        public string Report;
    }

    public class NamedDescriptionDecoration : BrightIdeasSoftware.AbstractDecoration
    {
        public ImageList ImageList;
        public string ImageName;
        public string Title;
        public string Description;

        public Font TitleFont = new Font("Tahoma", 9, FontStyle.Bold);
        public Color TitleColor = Color.FromArgb(255, 32, 32, 32);
        public Font DescripionFont = new Font("Tahoma", 9);
        public Color DescriptionColor = Color.FromArgb(255, 96, 96, 96);
        public Size CellPadding = new Size(1, 1);

        public override void Draw(BrightIdeasSoftware.ObjectListView olv, Graphics g, Rectangle r) {
            Rectangle cellBounds = this.CellBounds;
            cellBounds.Inflate(-this.CellPadding.Width, -this.CellPadding.Height);
            Rectangle textBounds = cellBounds;

            if (this.ImageList != null && !String.IsNullOrEmpty(this.ImageName)) {
                g.DrawImage(this.ImageList.Images[this.ImageName], cellBounds.Location);
                textBounds.X += this.ImageList.ImageSize.Width;
                textBounds.Width -= this.ImageList.ImageSize.Width;
            }

            //g.DrawRectangle(Pens.Red, textBounds);

            // Draw the title
            StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap);
            fmt.Trimming = StringTrimming.EllipsisCharacter;
            fmt.Alignment = StringAlignment.Near;
            fmt.LineAlignment = StringAlignment.Near;

            using (SolidBrush b = new SolidBrush(this.TitleColor)) {
                g.DrawString(this.Title, this.TitleFont, b, textBounds, fmt);
            }

            // Draw the description
            SizeF size = g.MeasureString(this.Title, this.TitleFont, (int)textBounds.Width, fmt);
            textBounds.Y += (int)size.Height;
            textBounds.Height -= (int)size.Height;
            StringFormat fmt2 = new StringFormat();
            fmt2.Trimming = StringTrimming.EllipsisCharacter;
            using (SolidBrush b = new SolidBrush(this.DescriptionColor)) {
                g.DrawString(this.Description, this.DescripionFont, b, textBounds, fmt2);
            }
        }
    }
}