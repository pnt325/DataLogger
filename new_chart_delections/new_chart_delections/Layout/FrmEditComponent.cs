using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmEditComponent : Form
    {
        public FrmEditComponent()
        {
            InitializeComponent();

            objectListView1.MouseClick += ObjectListView1_MouseClick;

            this.objectListView1.SelectedBackColor = Color.PaleGreen;
            this.objectListView1.SelectedForeColor = Color.Black;

            this.olvTypeCol.ImageGetter =  delegate (object obj)
            {
                string ret = "";
                ComponentHelp componentHelp = (ComponentHelp)obj;
                if (componentHelp.Type == Components.Type.ToString(Components.ComponentTypes.Chart))
                {
                    ret =  "com_chart";
                }
                else if (componentHelp.Type == Components.Type.ToString(Components.ComponentTypes.Label))
                {
                    ret = "com_label";
                }
                else if (componentHelp.Type == Components.Type.ToString(Components.ComponentTypes.Table))
                {
                    ret = "com_table";
                }

                return ret;
            };

            foreach (Core.ComponentItem item in Core.Component.Items)
            {
                ComponentHelp help = new ComponentHelp();

                help.Type = Components.Type.ToString(item.Type);
                help.StartPoint = item.StartPoint;
                help.EndPoint = item.EndPoint;
                help.Title = item.Title;
                help.UUID = item.Uuid;

                objectListView1.AddObject(help);
                
            }
        }
        
        ComponentHelp help;
        private void ObjectListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && objectListView1.SelectedObject != null)
            {
                help = (ComponentHelp)objectListView1.SelectedObject;

                if (Core.Component.GetStatus(help.UUID) == Core.ComponentStaus.Running)
                {
                    startToolStripMenuItem.Enabled = false;
                    stopToolStripMenuItem.Enabled = true;
                }
                else
                {
                    startToolStripMenuItem.Enabled = true;
                    stopToolStripMenuItem.Enabled = false;
                }
                
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.Component.SetStart(help.UUID);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.Component.SetStop(help.UUID);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.Component.Remove(help.UUID);
            objectListView1.RemoveObject(help);
        }
    }


    class ComponentHelp
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string UUID { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
    }
}
