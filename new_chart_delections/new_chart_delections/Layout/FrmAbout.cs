using System;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();

            /* Button */
            btnOk.Click += BtnOk_Click;

            KeyPreview = true;
            this.KeyPress += FrmAbout_KeyPress;
        }

        private void FrmAbout_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                BtnOk_Click(null, null);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
