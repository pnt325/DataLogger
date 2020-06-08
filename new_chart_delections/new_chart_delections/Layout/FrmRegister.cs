using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();

            btnAdd.Enabled = false;
            btnSave.Enabled = false;

            cbbType.Items.AddRange(Core.MemoryType.Names());

            InitEvent();
            MemToList();
        }

        private void InitEvent()
        {
            // List view
            lsvData.MouseClick += LsvData_MouseClick;

            // Combobox
            cbbType.TextChanged += CbbType_TextChanged;

            // TextBox
            txbName.TextChanged += TxbName_TextChanged;

            // Button
            btnAdd.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Form
            this.FormClosing += FrmRegister_FormClosing;
        }

        private void DeInitEvent()
        {
            // List view
            lsvData.MouseClick -= LsvData_MouseClick;

            // Combobox
            cbbType.TextChanged -= CbbType_TextChanged;

            // Textbox
            txbName.TextChanged -= TxbName_TextChanged;

            // Button
            btnAdd.Click -= BtnAdd_Click;
            btnSave.Click -= BtnSave_Click;

            // Form
            this.FormClosing -= FrmRegister_FormClosing;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeInitEvent();
        }

        private void MemToList()
        {
            lsvData.Items.Clear();
            foreach(KeyValuePair<string, Core.MemoryTypes> varType in Core.Memory.Types)
            {
                ListViewItem item = new ListViewItem(varType.Key);
                item.Tag = varType.Value;
                item.SubItems.Add(Core.MemoryType.ToString(varType.Value));
                lsvData.Items.Add(item);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            Core.Memory.Clear();

            // Query listview to add new register./.
            uint adr = 0;
            foreach (ListViewItem item in lsvData.Items)
            {
                Core.Memory.Add(item.SubItems[0].Text, (Core.MemoryTypes)item.Tag, out adr);
            }

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
            bool varNameExist = false;
            foreach(ListViewItem i in lsvData.Items)
            {
                if(i.SubItems[0].Text == txbName.Text)
                {
                    varNameExist = true;
                    break;
                }
            }

            if(varNameExist)
            {
                MessageBox.Show("Variable name existed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // create new listview item
            ListViewItem item = new ListViewItem(txbName.Text);
            item.SubItems.Add(cbbType.Text);
            item.Tag = Core.MemoryType.ToType(cbbType.Text);
            lsvData.Items.Add(item);

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
            // Check for item selected
            if (lsvData.FocusedItem == null)
            {
                return;
            }

            // Check all component stop to delete variable
            bool isRun = false;
            foreach (var item in Core.Component.Items)
            {
                if (item.Status == Core.ComponentStaus.Running)
                {
                    isRun = true;
                    break;
                }
            }

            if (isRun)
            {
                MessageBox.Show("Has component use data running", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Remove data from listview.
            lsvData.Items.RemoveAt(lsvData.FocusedItem.Index);
            btnSave.Enabled = true;
        }
    }
}
