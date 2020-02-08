using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.gui
{
    public partial class FrmConfigure : Form
    {
        public FrmConfigure()
        {
            InitializeComponent();

            this.KeyPreview = true;

            this.KeyPress += FrmConfigure_KeyPress;

            // ComboBox
            cbbPort.DropDown += CbbPort_DropDown;
            cbbPort.TextChanged += Cbb_TextChanged;
            cbbBaud.TextChanged += Cbb_TextChanged;

            // Button
            btnUartConnect.Enabled = false;

            // checkbox
            chbUart.CheckedChanged += Chb_CheckedChanged;
            chbTcp.CheckedChanged += Chb_CheckedChanged;
            chbMqtt.CheckedChanged += Chb_CheckedChanged;

            // Groupbox
            grbTcpIp.Enabled = false;
            grbUart.Enabled = false;
            grbMqtt.Enabled = false;


        }

        private void Chb_CheckedChanged(object sender, EventArgs e)
        {
            grbUart.Enabled = chbUart.Checked;
            grbTcpIp.Enabled = chbTcp.Checked;
            grbMqtt.Enabled = chbMqtt.Checked;
        }

        private void CbbPort_DropDown(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbbPort.Items.Clear();
            foreach(string port in ports)
            {
                cbbPort.Items.Add(port);
            }
        }

        private void Cbb_TextChanged(object sender, EventArgs e)
        {
            if(cbbPort.Text == "" || cbbBaud.Text == "")
            {
                btnUartConnect.Enabled = false;
            }
            else
            {
                btnUartConnect.Enabled = true;
            }
        }

        private void FrmConfigure_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
