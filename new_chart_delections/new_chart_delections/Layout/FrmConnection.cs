using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmConnection : Form
    {
        public FrmConnection()
        {
            InitializeComponent();
            btnConnect.Enabled = false;

            InitEvent();

            if (Program.Uart.Status)
            {
                cbbPort.Items.Add(Program.Uart.PortName);
                cbbPort.SelectedIndex = 0;

                cbbPort.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown1.Value = Program.Uart.BaudRate;

                btnConnect.Text = "Disconnect";
                btnConnect.Enabled = true;
            }
        }

        private void InitEvent()
        {
            cbbPort.SelectedIndexChanged += CbbPort_SelectedIndexChanged;
            cbbPort.Click += CbbPort_Click;
            Program.Uart.Disconnected += Uart_Disconnected;
            btnConnect.Click += BtnConnect_Click;
            this.FormClosing += FrmConnection_FormClosing;
        }

        private void DeInitEvent()
        {
            cbbPort.SelectedIndexChanged -= CbbPort_SelectedIndexChanged;
            cbbPort.Click -= CbbPort_Click;
            Program.Uart.Disconnected -= Uart_Disconnected;
            btnConnect.Click -= BtnConnect_Click;
            this.FormClosing -= FrmConnection_FormClosing;
        }

        private void FrmConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeInitEvent();
        }

        private void CbbPort_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbbPort.Items.Clear();
            cbbPort.Items.AddRange(ports);
        }

        private void Uart_Disconnected(object sender, EventArgs e)
        {
            cbbPort.Enabled = true;

            numericUpDown1.Invoke((MethodInvoker)delegate
            {
                numericUpDown1.Enabled = true;
            });
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Program.Uart.PortName = cbbPort.Text;
            Program.Uart.BaudRate = (int)numericUpDown1.Value;

            if (btnConnect.Text == "Connect")
            {
                if (Program.Uart.Connect())
                {
                    btnConnect.Text = "Disconnect";
                    cbbPort.Enabled = false;
                    numericUpDown1.Enabled = false;

                    this.Close();
                }
            }
            else if (btnConnect.Text == "Disconnect")
            {
                Program.Uart.Disconnect();
                cbbPort.Enabled = true;
                numericUpDown1.Enabled = false;
                btnConnect.Text = "Connect";
            }
        }

        private void CbbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbPort.Text != "")
            {
                btnConnect.Enabled = true;
            }
            else
            {
                btnConnect.Enabled = false;
            }
        }
    }
}
