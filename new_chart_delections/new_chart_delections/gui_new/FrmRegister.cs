using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();

            btnAdd.Enabled = false;

            lsvData.MouseClick += LsvData_MouseClick;
            cbbType.TextChanged += CbbType_TextChanged;
            txbName.TextChanged += TxbName_TextChanged;

            this.FormClosing += FrmRegister_FormClosing;

            btnAdd.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;

            btnSave.Enabled = false;

            cbbType.Items.AddRange(Type.List());
            LoadRegister();
        }

        private void FrmRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            lsvData.MouseClick -= LsvData_MouseClick;
            cbbType.TextChanged -= CbbType_TextChanged;
            txbName.TextChanged -= TxbName_TextChanged;

            this.FormClosing -= FrmRegister_FormClosing;

            btnAdd.Click -= BtnAdd_Click;
            btnSave.Click -= BtnSave_Click;
        }

        private void LoadRegister()
        {
            foreach (KeyValuePair<string, string> type in Program.MemoryManage.ValueType)
            {
                ListViewItem item = new ListViewItem(type.Key);
                item.SubItems.Add(type.Value); // get type

                lsvData.Items.Add(item);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;


            Program.MemoryManage.Clear();

            // Query listview to add new register./.
            foreach (ListViewItem item in lsvData.Items)
            {
                string key = item.SubItems[0].Text;
                string type = item.SubItems[1].Text;    // string type.

                Program.MemoryManage.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }

            Program.MemoryManage.Save();

            this.Close();
        }

        private void LsvData_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // create new listview item
            ListViewItem item = new ListViewItem(txbName.Text);
            item.SubItems.Add(cbbType.Text);

            lsvData.Items.Add(item);    // Add Item to listview: lsvData

            // enable Button Save to save
            btnSave.Enabled = true;
        }

        private void CheckInput()
        {
            if (txbName.Text != "" && cbbType.Text != "")
            {
                btnAdd.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
            }
        }

        private void TxbName_TextChanged(object sender, EventArgs e)
        {
            CheckInput();
        }

        private void CbbType_TextChanged(object sender, EventArgs e)
        {
            CheckInput();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvData.FocusedItem == null)
            {
                return;
            }

            lsvData.Items.RemoveAt(lsvData.FocusedItem.Index);

            btnSave.Enabled = true;
        }
    }
}
