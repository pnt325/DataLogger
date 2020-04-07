using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public partial class Configure : Form
    {
        int currentX;
        int currentY;


        public Configure()
        {
            InitializeComponent();

            nudX.ValueChanged += NudX_ValueChanged;
            nudY.ValueChanged += NudX_ValueChanged;

            nudX.Value = Program.Grid.X;
            nudY.Value = Program.Grid.Y;

            currentX = (int)nudX.Value;
            currentY = (int)nudY.Value;
        }

        private void UpdateGrid()
        {
            if (Program.Grid.SetGrid((int)nudX.Value, (int)nudY.Value))
            {
                currentX = Program.Grid.X;
                currentY = Program.Grid.Y;
            }
            else
            {
                nudX.Value = currentX;
                nudY.Value = currentY;
            }
        }
        //private void NudY_ValueChanged(object sender, EventArgs e)
        //{
        //    UpdateGrid();
        //}

        private void NudX_ValueChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }
    }
}
