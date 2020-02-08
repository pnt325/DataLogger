using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogging
{
    public partial class frmSelectChart : Form
    {
        public delegate void SendMessage(string chartName,int chartType, int sampleCount, List<int> index);
        public event SendMessage Send;
        public frmSelectChart()
        {
            InitializeComponent();

            txbSample.KeyPress += TxbSample_KeyPress;
            cbbChartType.Click += CbbChartType_Click;
            cbbChartType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbChartType.Items.Add("Line Chart");
            cbbChartType.Items.Add("Column Chart");
            cbbChartType.Items.Add("Variable Table");
            cbbChartType.SelectedIndex = 0;//default chart.
            cbbChartType.SelectedIndexChanged += CbbChartType_SelectedIndexChanged;

            btnAddChart.Click += BtnAddChart_Click;

            lsvValues.CheckBoxes = true;
            DataToListView();
        }
        private void CbbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbChartType.SelectedIndex == (int)ChartType.Line || cbbChartType.SelectedIndex == (int)ChartType.Column)
            {
                txbName.Enabled = true;
                txbSample.Enabled = true;
            }
            else if(cbbChartType.SelectedIndex == (int)ChartType.Table)
            {
                txbName.Enabled = false;
                txbSample.Enabled = false;
            }
        }
        private void DataToListView()
        {
            if (ChartData.Names == null)
                return;
            foreach(string name in ChartData.Names)
            {
                lsvValues.Items.Add(name);
            }
        }
        private void BtnAddChart_Click(object sender, EventArgs e)
        {
            if (cbbChartType.Text == "")
                return;
            if (cbbChartType.SelectedIndex == (int)ChartType.Line || cbbChartType.SelectedIndex == (int)ChartType.Column)
            {
                if (txbSample.Text == "" || lsvValues.CheckedItems.Count == 0 || txbName.Text == "")
                {
                    MessageBox.Show("Invalid Configure Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if(cbbChartType.SelectedIndex == (int)ChartType.Table)
            {
                if(lsvValues.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Invalid Configure Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            List<int> indexList = new List<int>();
            for (int i = 0; i < lsvValues.Items.Count; i++)
            {
                if (lsvValues.Items[i].Checked)
                    indexList.Add(i);
            }
            if (cbbChartType.SelectedIndex == (int)ChartType.Table)
                if (Send != null)
                {
                    Send(txbName.Text, cbbChartType.SelectedIndex, 0, indexList);
                }
                //Send?.Invoke(txbName.Text, cbbChartType.SelectedIndex, 0, indexList); //  available on framework 4.5.1
                else
                {
                    if (Send != null)
                    {
                        Send(txbName.Text, cbbChartType.SelectedIndex, int.Parse(txbSample.Text), indexList);
                    }
                    //Send?.Invoke(txbName.Text, cbbChartType.SelectedIndex, int.Parse(txbSample.Text), indexList); //  available on framework 4.5.1
                }
            this.Close();
        }
        private void CbbChartType_Click(object sender, EventArgs e)
        {
            cbbChartType.DroppedDown = true;
        }
        private void TxbSample_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
    }
}
