using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjectListViewEx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.objectListView1.SetObjects(Song.GetSongs());
            this.objectListView1.ShowGroups = false;
            //this.objectListView1.SmallImageList = 
            this.olvColumn1.ImageGetter = delegate (object obj)
            {
                Song s = (Song)obj;
                if(s.Rating >= 70)
                {
                    return "star";
                }
                else
                {
                    return "song";
                }
            };

            this.objectListView1.FormatRow += ObjectListView1_FormatRow;



            this.olvColumn4.AspectToStringConverter = delegate (object obj)
            {
                Button s = (Button)obj;
                return s.Text;
            };

            this.objectListView1.SelectedBackColor = Color.AliceBlue;
            this.objectListView1.SelectedForeColor = Color.Black;

            this.objectListView1.ButtonClick += ObjectListView1_ButtonClick;

            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;

            objectListView1.BuildList(true);
        }

        private void ObjectListView1_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            Song s = (Song)e.Model;
            if (e.RowIndex % 2 == 0)
            {
                e.Item.BackColor = Color.White;
            }
            else
            {
                e.Item.BackColor = Color.AliceBlue;
            }
        }

        private void ObjectListView1_ButtonClick(object sender, BrightIdeasSoftware.CellClickEventArgs e)
        {
            this.Text = string.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model);
        }
    }
}
