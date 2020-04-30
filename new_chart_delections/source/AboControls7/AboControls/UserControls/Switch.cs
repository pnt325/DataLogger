using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AboControls.UserControls
{
    public partial class Switch : UserControl
    {
        public Switch()
        {
            InitializeComponent();
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            btnOff.Checked = false;
            btnOn.Checked = true;
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            btnOn.Checked = false;
            btnOff.Checked = true;
        }

        [Category("Behavior")]
        [Description("The current state of the switch")]
        [DefaultValue(false)]
        public bool IsOn
        {
            get { return btnOn.Checked; }
            set
            {
                if (btnOn.Checked)
                {
                    btnOff.Checked = true;
                    btnOn.Checked = false;
                }
                else
                {
                    btnOff.Checked = false;
                    btnOn.Checked = true;
                }
            }
        }

        [DisplayName("On Button")]
        [Category("State Controls")]
        [Description("The on button of the control")]
        public CheckBox OnButton
        {
            get { return btnOn; }
        }

        [DisplayName("Off Button")]
        [Category("State Controls")]
        [Description("The off button of the control")]
        public CheckBox OffButton
        {
            get { return btnOff; }
        }

        [DisplayName("Split Container"), Category("State Controls"), Description("The spliting container of the control")]
        public SplitContainer SplitContainer { get; set; }
    }
}
