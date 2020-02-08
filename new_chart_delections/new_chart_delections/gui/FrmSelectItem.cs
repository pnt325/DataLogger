using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.gui
{
    public partial class FrmSelectItem : Form
    {
        public List<int> ItemSelected = new List<int>();
        public FrmSelectItem()
        {
            InitializeComponent();
            this.KeyPreview = true;

            //this.KeyPress += FrmSelectItem_KeyPress;
            // form event
            this.Shown += FrmSelectItem_Shown;  //load form and load info

            // ComboBox
            cbbItem.Items.Add(data.SelectItem.CHART);
            cbbItem.Items.Add(data.SelectItem.LABEL);
            cbbItem.Items.Add(data.SelectItem.TABLE);

            cbbItem.SelectedIndexChanged += CbbItem_SelectedIndexChanged;
            cbbChartItem.Visible = false;

            cbbItem.TextChanged += CbbItem_TextChanged;
            cbbChartItem.TextChanged += CbbItem_TextChanged;

            // Button
            btnOk.Enabled = false;
            btnOk.Click += BtnOk_Click;

            // Listview
            lsvMain.ItemChecked += LsvMain_ItemChecked;

            // TextBox
            txbPeriod.TextChanged += TxbPeriod_TextChanged;
            txbTitle.TextChanged += TxbPeriod_TextChanged;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // add select data and close form
            foreach(ListViewItem item in lsvMain.Items)
            {
                if (item.Checked)
                {
                    ItemSelected.Add(item.Index);
                }
            }

            Close();
        }

        private void FrmSelectItem_Shown(object sender, EventArgs e)
        {
            // load data to listview
            foreach(var data in Program.DataObjs)
            {
                ListViewItem item = new ListViewItem(data.Name);
                item.SubItems.Add(data.Unit);
                lsvMain.Items.Add(item);
            }
        }

        private void TxbPeriod_TextChanged(object sender, EventArgs e)
        {
            CheckInput();
        }

        private void CbbItem_TextChanged(object sender, EventArgs e)
        {
            CheckInput();
        }

        int itemCheckCount = 0;
        private void LsvMain_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked == true)
            {
                itemCheckCount += 1;
            }
            else
            {
                itemCheckCount -= 1;
                if (itemCheckCount < 0)
                {
                    itemCheckCount = 0;
                }
            }

            CheckInput();
        }

        private void CbbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbItem.Text == data.SelectItem.CHART)
            {
                cbbChartItem.Visible = true;
            }
            else
            {
                cbbChartItem.Visible = false;
            }
        }

        #region Private
        private void CheckInput()
        {
            if(cbbChartItem.Visible == true)
            {
                if (cbbChartItem.Text == "" || txbPeriod.Text == "" || txbTitle.Text == "" || itemCheckCount == 0)
                {
                    btnOk.Enabled = false;
                }
                else
                {
                    btnOk.Enabled = true;
                }
            }
            else
            {
                if(cbbItem.Text == "" || txbPeriod.Text == "" || txbTitle.Text == "" || itemCheckCount == 0)
                {
                    btnOk.Enabled = false;
                }
                else
                {
                    btnOk.Enabled = true;
                }
            }
        }
        #endregion
    }
}
