using AboControls.ExtendedControls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AboControls
{
    public partial class TestingForm : Form
    {
        public TestingForm()
        {
            InitializeComponent();
        }

        private void colorPickerControl1_ColorPicked(object sender, EventArgs e)
        {
            Color pc = (Color)sender;
            this.BackColor = pc;
        }
    }
}
