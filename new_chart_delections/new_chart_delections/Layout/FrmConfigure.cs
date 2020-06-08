using System;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmConfigure : Form
    {
        int currentX;
        int currentY;

        public FrmConfigure()
        {
            InitializeComponent();

            // Get value from grid...
            nudX.Value = Core.Grid.X;
            nudY.Value = Core.Grid.Y;

            nudX.ValueChanged += NudX_ValueChanged;
            nudY.ValueChanged += NudX_ValueChanged;

            currentX = (int)nudX.Value;
            currentY = (int)nudY.Value;
        }

        private void UpdateGrid()
        {
            if (Core.Grid.SetSize((int)nudX.Value, (int)nudY.Value))
            {
                currentX = Core.Grid.X;
                currentY = Core.Grid.Y;
            }
            else
            {
                nudX.Value = currentX;
                nudY.Value = currentY;
            }
        }

        private void NudX_ValueChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }
    }
}
