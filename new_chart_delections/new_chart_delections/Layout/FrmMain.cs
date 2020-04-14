//using new_chart_delections.gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.Layout
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            Core.Memory.Init(); // default memory init.
            InitializeComponent();
            Core.Grid.Init(this);

            // graphics config
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);

            InitEvent();
        }

        private void InitEvent()
        {
            Core.Grid.SizeChanged += Grid_SizeChanged;
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Grid_AreaSelected(Point start, Point end)
        {

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

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.SetStart(item.Uuid);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.SetStop(item.Uuid);
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(FrmConfigure frm = new FrmConfigure())
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Stop connection.
            //Program.Uart.Disconnect();

            // Stop component update
            //Program.ComponentManage.StopAll();

            // Save current layout
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Grid layout (*.cfg)|*.cfg";
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    filename = sfd.FileName;
                }
            }

            if(filename == "")
            {
                return;
            }

            Configure.Save.Dump(filename);
        }
    }
}
