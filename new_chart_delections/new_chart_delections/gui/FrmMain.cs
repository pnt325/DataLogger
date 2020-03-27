using System.Drawing;
using System.Windows.Forms;

namespace new_chart_delections
{
    public partial class FrmMain : Form
    {
        //GridLine.Grid FGrid = new GridLine.Grid();
        GridLine.RecSelect recSelect = new GridLine.RecSelect();

        gui.FrmConfigure frmConfigure = new gui.FrmConfigure();

        public FrmMain()
        {
            InitializeComponent();

            // avoid form flickering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
                true);

            // Form event
            this.MouseMove += FrmMain_MouseMove;
            this.Paint += FrmMain_Paint;
            this.MouseDown += FrmMain_MouseDown;
            this.MouseUp += FrmMain_MouseUp;

            /* Menu strip*/
            gridToolStripMenuItem.Click += GridToolStripMenuItem_Click;
            configureToolStripMenuItem.Click += ConfigureToolStripMenuItem_Click;
            datasToolStripMenuItem.Click += DatasToolStripMenuItem_Click;
            runToolStripMenuItem.Click += RunToolStripMenuItem_Click;

            Program.FGrid.SetGrid(4, 4, this.ClientSize);
        }

        private void RunToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // TODO Run thread or timer to show data on chart...
        }

        private void DatasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using(gui.FrmConfigData frm = new gui.FrmConfigData())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;
                frm.ShowDialog();
            }
        }

        private void ConfigureToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //frmConfigure.StartPosition = FormStartPosition.Manual;
            frmConfigure.Top = this.Top + (this.Size.Height - frmConfigure.Size.Height) / 2;
            frmConfigure.Left = this.Left + (this.Size.Width - frmConfigure.Size.Width) / 2;
            frmConfigure.ShowDialog();
        }

        private void GridToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (gui.FrmGridEdit frm = new gui.FrmGridEdit(Program.FGrid.X, Program.FGrid.Y))
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;
                frm.Grid += Frm_Grid;
                frm.ShowDialog();
                frm.Grid -= Frm_Grid;
            }
        }

        private void Frm_Grid(int x, int y)
        {
            if (x != Program.FGrid.X || y != Program.FGrid.Y)
            {
                Program.FGrid.SetGrid(x, y, ClientSize);
                this.Invalidate();
            }
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (recSelect.IsMouseDown)
            {
                //this.Text = string.Format("(x,y) = {0},{1}; index({2}, {3})", recSelect.xStart, recSelect.yStart,
                //    recSelect.xCount, recSelect.yCount);
                using (gui.FrmSelectItem frm = new gui.FrmSelectItem())
                {
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Top = this.Top + (this.Size.Height - frm.Size.Height) / 2;
                    frm.Left = this.Left + (this.Size.Width - frm.Size.Width) / 2;
                    frm.ShowDialog();

                    if (frm.ItemSelected.Count > 0)
                    {
                        // TODO add method call chart here.
                        data.ControlObject obj = new data.ControlObject();
                        obj.X = recSelect.xStart;
                        obj.Y = recSelect.yStart;
                        obj.StepX = recSelect.xCount;
                        obj.StepY = recSelect.yCount;

                        Program.ControlObjs.Add(obj);
                    }
                }
                this.Invalidate();
            }
            recSelect.IsMouseDown = false;
        }

        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                recSelect.IsMouseDown = true;
                recSelect.GetStartPoint(Program.FGrid, e.Location);
                recSelect.GetEndPoint(Program.FGrid, e.Location, ClientSize);
                this.Invalidate();
            }
        }

        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            Program.FGrid.Draw(e.Graphics, ClientSize);
            if (recSelect.IsMouseDown)
            {
                recSelect.Draw(e.Graphics);
            }
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (recSelect.IsMouseDown)
            {
                recSelect.GetEndPoint(Program.FGrid, e.Location, ClientSize);
                if (recSelect.IsChanged())
                {
                    Invalidate();
                }
            }
        }
    }
}
