//using new_chart_delections.gui;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace new_chart_delections.Layout
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            // Core settings
            Core.Memory.Init();
            Core.Grid.Init(this);

            // graphics Settings
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);

            // Event
            InitEvent();
        }

        private void InitEvent()
        {
            Core.Grid.SizeChanged += Grid_SizeChanged;
            Core.Component.AddControl += Component_AddControl;
            this.Shown += FrmMain_Shown;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            // check to open file setting
            if(Program.FileName != "")
            {
                if(File.Exists(Program.FileName))
                {
                    Configure.Load.FromFile(Program.FileName);
                }
            }
        }

        private void Component_AddControl(Control control)
        {
            this.Controls.Add(control);
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FrmRegister frm = new FrmRegister())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;

                frm.ShowDialog();
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FrmConfigure frm = new FrmConfigure())
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
            using (FrmConnection frm = new FrmConnection())
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
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Grid layout (*.cfg)|*.cfg";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filename = sfd.FileName;
                }
            }

            if (filename == "")
            {
                return;
            }

            Configure.Save.Dump(filename);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Grid layout (*.cfg)|*.cfg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filename = ofd.FileName;
                }
            }

            if (filename == "")
            {
                return;
            }

            Configure.Load.FromFile(filename);

        }

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.SetStart(item.Uuid);
            }
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.SetStop(item.Uuid);
            }
        }

        private void componentToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if(Program.Uart.Status)
            {
                startToolStripMenuItem1.Enabled = true;
                stopToolStripMenuItem1.Enabled = true;
            }
            else
            {
                startToolStripMenuItem1.Enabled = false;
                stopToolStripMenuItem1.Enabled = false;
            }
        }
    }
}
