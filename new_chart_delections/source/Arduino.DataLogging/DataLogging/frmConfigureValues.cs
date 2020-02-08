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
    public partial class frmConfigureValues : Form
    {
        private int columnCount = 0;
        public frmConfigureValues()
        {
            InitializeComponent();
            btnAdd.Click += BtnAdd_Click;
            btnColor.Click += BtnColor_Click;
            btnReset.Click += BtnReset_Click;
            btnDone.Click += BtnDone_Click;
            txbColumnCount.Text = string.Format("{0}", columnCount);
            //txbName.Focus();
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ChartData.ChartColor.Clear();
            ChartData.Names.Clear();
            ChartData.Values.Clear();
            ChartData.Unit.Clear();

            lsvValues.Items.Clear();

            columnCount = 0;
            txbColumnCount.Text = string.Format("{0}", columnCount);
        }
        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.ShowDialog();
                btnColor.BackColor = colorDialog.Color;
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txbColumnCount.Text == "" || txbName.Text == "")
                return;
            foreach(string name in ChartData.Names)
            {
                if(name ==  txbName.Text)
                {
                    txbName.SelectionStart = 0;
                    txbName.SelectionLength = txbName.Text.Length;
                    txbName.Focus();
                    return;
                }
            }
            
            ChartData.ChartColor.Add(btnColor.BackColor);
            ChartData.Names.Add(txbName.Text);
            ChartData.Unit.Add(txbName.Text);
            ChartData.Values.Add(0.0f);

            ListViewItem item = new ListViewItem(columnCount.ToString());
            item.SubItems.Add(txbName.Text);
            item.SubItems.Add("  ");
            item.SubItems.Add(txbUnit.Text);
            lsvValues.Items.Add(item);
            int index = lsvValues.Items.Count - 1;
            lsvValues.Items[index].BackColor = btnColor.BackColor;

            columnCount++;
            txbColumnCount.Text = string.Format("{0}", columnCount);
        }
    }
}
