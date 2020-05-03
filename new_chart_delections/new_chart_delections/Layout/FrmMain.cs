//using new_chart_delections.gui;
using DataLogger.Core;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataLogger.Layout
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
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);

            this.DoubleBuffered = true;

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

            toolStripStatusLblGrid.Text = string.Format("Grid: {0}x{1}", Grid.X, Grid.Y);
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
            Program.Uart.Disconnect();

            // Stop component update
            foreach(Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.SetStop(item.Uuid);
            }

            // Save current layout
            if (Core.Component.Items.Count > 0)
            {
                if (MessageBox.Show("Do you want to save current layout?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    saveAsToolStripMenuItem_Click(null, null);
                }
            }

            // Close form
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configure.Save.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configure.Load.Show();

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Configure.Save.Show(Configure.Save.FileName) == false)
            {
                if(MessageBox.Show("Do you want to save to other location?", "File name not exist", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Configure.Save.Show();
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(FrmAbout frm =new FrmAbout())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;

                frm.ShowDialog();
            }
        }

        private void compoentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(FrmEditComponent frm = new FrmEditComponent())
            {
                frm.ShowDialog();
            }
        }
    }
}
