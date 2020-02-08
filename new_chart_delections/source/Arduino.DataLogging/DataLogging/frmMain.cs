using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogging
{
    public partial class frmMain : Form
    {
        bool isMouseDown = false;
        Graphics graphics = null;
        Pen linePen = new Pen(Color.Black, 1.0f);
        Brush recBrush = new SolidBrush(Color.DarkGray);
        SerialPort serialPort = new SerialPort();
        Thread thDataReceive;

        public frmMain()
        {
            InitializeComponent();
            panelDraw.Paint += PanelDraw_Paint;
            linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            txbGridX.Click += TxbGridX_Click;
            txbGridY.Click += TxbGridY_Click;
            txbScanRate.Click += TxbScanRate_Click;

            txbScanRate.KeyPress += TxbScanRate_KeyPress;
            GridKeypress(txbGridX);
            GridKeypress(txbGridY);
            btnConnect.Click += BtnConnect_Click;
            this.SizeChanged += FrmMain_SizeChanged;
            this.Shown += FrmMain_Shown;

            panelDraw.MouseDown += PanelDraw_MouseDown;
            panelDraw.MouseUp += PanelDraw_MouseUp;
            panelDraw.MouseMove += PanelDraw_MouseMove;

            cbbPort.Click += CbbPort_Click;

            cbbPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbBaud.Click += CbbBaud_Click;

            cbbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbBaud.SelectedIndex = 2;//Default 9600

            DrawData.xGrid = 4;
            DrawData.yGrid = 4;

            txbGridX.Text = DrawData.xGrid.ToString();
            txbGridY.Text = DrawData.yGrid.ToString();
        }

        private void TxbScanRate_Click(object sender, EventArgs e)
        {
            SelectAllText(txbScanRate);
        }
        private void TxbGridY_Click(object sender, EventArgs e)
        {
            SelectAllText(txbGridY);
        }
        private void TxbGridX_Click(object sender, EventArgs e)
        {
            SelectAllText(txbGridX);
        }
        void SelectAllText(TextBox txb)
        {
            txb.SelectionStart = 0;
            txb.SelectionLength = txb.Text.Length;
        }
        private void PanelDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            for (int i = 0; i < DrawData.xGrid; i++)
            {
                if (e.X > i * DrawData.xStep && e.X < (i + 1) * DrawData.xStep)
                    DrawData.xEnd = (i + 1) * DrawData.xStep;
            }
            for (int i = 0; i < DrawData.yGrid; i++)
            {
                if (e.Y > i * DrawData.yStep && e.Y < (i + 1) * DrawData.yStep)
                    DrawData.yEnd = (i + 1) * DrawData.yStep;
            }
            if (DrawData.xEnd == DrawData.xStart || DrawData.xEnd < DrawData.xStart)
                return;
            if (DrawData.yEnd == DrawData.yStart || DrawData.yEnd < DrawData.yStart)
                return;
            if (DrawData.xEnd != DrawData.xEndOld || DrawData.yEnd != DrawData.yEndOld)
            {
                DrawData.xEndOld = DrawData.xEnd;
                DrawData.yEndOld = DrawData.yEnd;
                if (DrawData.xEnd >= DrawData.xStep * (DrawData.xGrid))
                    DrawData.xEnd = panelDraw.Width - 1;
                if (DrawData.yEnd >= DrawData.yStep * (DrawData.yGrid))
                    DrawData.yEnd = panelDraw.Height - 1;

                DrawRectangle();
            }
        }
        private void PanelDraw_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                string msg = string.Format("({0};{1}),({2};{3})", DrawData.xStart / DrawData.xStep, DrawData.yStart / DrawData.yStep, DrawData.xEnd / DrawData.xStep, DrawData.yEnd / DrawData.yStep);
                using (frmSelectChart frm = new frmSelectChart())
                {
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(this.Location.X + this.Width / 2 - frm.Width / 2, this.Location.Y + this.Height / 2 - frm.Height / 2);
                    frm.Send += Frm_Send; ;
                    frm.ShowDialog();
                    DrawLine();
                }
            }
        }
        private void Frm_Send(string chartName, int chartType, int sampleCount, List<int> index)
        {
            Chart chart = new Chart(panelDraw, chartType, index, chartName, sampleCount);
            chart.Create();
        }
        private void PanelDraw_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            for (int i = 0; i < DrawData.xGrid; i++)
            {
                if (e.X > i * DrawData.xStep && e.X < (i + 1) * DrawData.xStep)
                {
                    DrawData.xStart = i * DrawData.xStep;
                    DrawData.xEnd = (i + 1) * DrawData.xStep;
                }
            }
            for (int i = 0; i < DrawData.yGrid; i++)
            {
                if (e.Y > i * DrawData.yStep && e.Y < (i + 1) * DrawData.yStep)
                {
                    DrawData.yStart = i * DrawData.yStep;
                    DrawData.yEnd = (i + 1) * DrawData.yStep;
                }
            }

            DrawRectangle();
        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            DrawLine();
        }
        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {

            DrawLine();
        }
        private void BtnSetGrid_Click(object sender, EventArgs e)
        {
            if (txbGridX.Text == "")
                txbGridX.Text = DrawData.xGrid.ToString();
            if (txbGridY.Text == "")
                txbGridY.Text = DrawData.yGrid.ToString();
            int x = int.Parse(txbGridX.Text);
            int y = int.Parse(txbGridY.Text);
            if(x < 2)
            {
                txbGridX.Text = DrawData.xGrid.ToString();
                return;
            }
            if(y < 2)
            {
                txbGridY.Text = DrawData.yGrid.ToString();
                return;
            }
            if (x != DrawData.xGrid || y != DrawData.yGrid)
            {
                DrawData.xGrid = x;
                DrawData.yGrid = y;
            }
            else
                return;
            DrawLine();
        }
        private void PanelDraw_Paint(object sender, PaintEventArgs e)
        {
            graphics = panelDraw.CreateGraphics();
        }
        private void CbbPort_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbbPort.Items.Clear();
            foreach(string port in ports)
            {
                cbbPort.Items.Add(port);
            }
            cbbPort.DroppedDown = true;
        }
        private void CbbBaud_Click(object sender, EventArgs e)
        {
            cbbBaud.DroppedDown = true;
        }
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (cbbPort.Text == "" || cbbBaud.Text == "")
            {
                MessageBox.Show("Invalid Serial Port Configure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (btnConnect.Text == "Connect")
            {
                serialPort.PortName = cbbPort.Text;
                serialPort.BaudRate = int.Parse(cbbBaud.Text);
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;

                btnConnect.Enabled = false;
                //btnConnect.Enabled = true;
                btnConnect.Text = "Disconnect";
            }
            else if(btnConnect.Text == "Disconnect")
            {
                if (!serialPort.IsOpen)
                {
                    btnConnect.Text = "Connect";
                    return;
                }
                btnConnect.Enabled = false;
                serialPort.Close();
                btnConnect.Enabled = true;
                btnConnect.Text = "Connect";
            }

            if (btnConnect.Text == "Connect")
                return;
            using (frmConfigureValues frm = new frmConfigureValues())
            {
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(this.Location.X + this.Width / 2 - frm.Width / 2, this.Location.Y + this.Height / 2 - frm.Height / 2);
                //frm.Send += Frm_Send;
                frm.ShowDialog();

                if (serialPort.IsOpen)
                {
                    btnConnect.Text = "Disconnect";
                    return;
                }
                serialPort.Open();
                string s = serialPort.ReadExisting();// clear buffer receive
                btnConnect.Enabled = true;
                btnConnect.Text = "Disconnect";

                thDataReceive = new Thread(DataReceive);
                thDataReceive.IsBackground = true;
                thDataReceive.Start();
            }
        }

        void DataReceive()
        {
            while(true)
            {
                if (!serialPort.IsOpen)
                    break;
                string recv = "";
                try
                {
                    recv = serialPort.ReadLine();
                }
                catch (Exception)
                {
                }
                if (recv == "") // disconnect if receive null data
                    break;
                // pare data from serial
                string[] chain = recv.Split(',');
                if(chain.Length == ChartData.Names.Count)
                {
                    for (int i = 0; i < ChartData.Values.Count; i++)
                    {
                        ChartData.Values[i] = float.Parse(chain[i]);
                    }
                }

                Thread.Sleep(1);
            }
        }
        private void TxbScanRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                //Set scan rate
                int scanRate = int.Parse(txbScanRate.Text);
                if (scanRate > 999)
                    txbScanRate.Text = ChartData.ScanRate.ToString();
                ChartData.ScanRate = scanRate;
            }
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        void DrawLine()
        {
            panelDraw.Refresh();//Lam moi panel
            //Truc x:

            DrawData.xStep = panelDraw.Width / DrawData.xGrid;
            DrawData.yStep = panelDraw.Height / DrawData.yGrid;

            for (int i = 1; i < DrawData.xGrid; i++)
            {
                Point[] points =
                {
                        new Point(DrawData.xStep*i, 0),
                        new Point(DrawData.xStep*i, panelDraw.Height)
                };
                graphics.DrawLines(linePen, points);
            }
            //Truc y:
            for (int i = 1; i < DrawData.yGrid; i++)
            {
                Point[] points =
                {
                        new Point(0, DrawData.yStep*i),
                        new Point(panelDraw.Width, DrawData.yStep*i)
                    };
                graphics.DrawLines(linePen, points);
            }
        }
        void DrawRectangle()
        {
            int x = DrawData.xStart;
            int y = DrawData.yStart;
            int width = DrawData.xEnd - DrawData.xStart;
            int height = DrawData.yEnd - DrawData.yStart;
            if (width < 0 || height < 0) return;
            DrawLine();
            graphics.FillRectangle(recBrush, x, y, width, height);
        }
        void GridKeypress(TextBox txb)
        {
            txb.KeyPress += Txb_KeyPress;
        }
        private void Txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                BtnSetGrid_Click(this, null);
                this.Width += 1;
            }
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}
