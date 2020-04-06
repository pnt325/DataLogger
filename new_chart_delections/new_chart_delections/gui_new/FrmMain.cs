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
            InitializeComponent();

            // graphics config
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);

            // Init static variable from Program.
            Program.Grid = new Grid(this);
            Program.ComponentManage = new ComponentManage();
            Program.MemoryManage = new MemoryManage();
            Program.Uart = new Uart();
            Program.MemoryManage.Load();

            Program.Grid.GridChanged += Grid_GridChanged;

            Program.Grid.AreaSelected = AreaSelected;
        }

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
            GraphInfo graphInfo = new GraphInfo();
            ComponentType.Type type = ComponentType.Type.None;
            using(FrmSelectComponent frm =new FrmSelectComponent())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    type = frm.SelectType;
                    graphInfo = (GraphInfo)frm.SelectObject;
                }
            }

            string uuid = Guid.NewGuid().ToString();
            if (type == ComponentType.Type.LineChart)
            {
                LineChart lineChart = new LineChart(start, end, graphInfo);
                lineChart.UUID = uuid;

                this.Controls.Add(lineChart);

                // Add ComponentManage.   
                ComponentArea component = new ComponentArea()
                {
                    StartPoint = start,
                    EndPoint = end,
                    UUID = uuid
                };

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
