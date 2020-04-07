//using new_chart_delections.gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            Program.Grid = new Grid(this);
            Program.ComponentManage = new ComponentManage();
            Program.MemoryManage = new MemoryManage();
            Program.Uart = new Uart();
            InitializeComponent();


            // graphics config
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);

            // Init static variable from Program.

            Program.MemoryManage.Load();
            Program.Grid.GridChanged += Grid_GridChanged;
            Program.Grid.AreaSelected = AreaSelected;

            //this.Shown += FrmMain_Shown;
        }

        //private void FrmMain_Shown(object sender, EventArgs e)
        //{
        //    XLabel lbl = new XLabel(new Point(0, 0), new Point(1, 1), new LabelInfo() { Name = "test" });
        //    this.Controls.Add(lbl);
        //}

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FrmRegister frm = new FrmRegister())
            {
                //show dialog on center.
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;

                frm.ShowDialog();
            }
        }

        private void AreaSelected(Point start, Point end)
        {
            ComponentType.Type type = ComponentType.Type.None;
            object selectObject = null;
            using(FrmSelectComponent frm =new FrmSelectComponent())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    type = frm.SelectType;
                    selectObject = frm.SelectObject;
                }
            }

            string uuid = Guid.NewGuid().ToString();

            //ComponentManage.   
            ComponentArea component = new ComponentArea()
            {
                StartPoint = start,
                EndPoint = end,
                UUID = uuid
            };

            if (type == ComponentType.Type.LineChart)
            {
                LineChart lineChart = new LineChart(start, end, (GraphInfo)selectObject);
                lineChart.UUID = uuid;

                this.Controls.Add(lineChart);

                Program.ComponentManage.AddAreaItem(component);
            }
            else if(type == ComponentType.Type.Label)
            {
                XLabel lbl = new XLabel(start, end, (LabelInfo)selectObject);
                lbl.UUID = uuid;
                this.Controls.Add(lbl);

                Program.ComponentManage.AddAreaItem(component);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ComponentManage.StartAll();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ComponentManage.StopAll();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(Configure frm = new Configure())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;
                frm.ShowDialog();
            }
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Connection.
            using(FrmConnection frm = new FrmConnection())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;
                frm.ShowDialog();
            }

        }
    }
}
