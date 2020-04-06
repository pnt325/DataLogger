using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public partial class FrmSelectComponent : Form
    {
        public FrmSelectComponent()
        {
            InitializeComponent();

            btnChange.Enabled = false;
            // Load component
            cbbComponent.TextChanged += CbbComponent_TextChanged;
            btnChange.Click += BtnChange_Click;

            cbbComponent.Items.AddRange(ComponentType.GetNames());
            lsvSelect.MouseClick += LsvSelect_MouseClick;

            lblColor.Click += LblColor_Click;
            btnCancel.Click += BtnCancel_Click;
            btnOk.Enabled = false;
            btnOk.Click += BtnOk_Click;
            this.FormClosing += FrmSelectComponent_FormClosing;
        }

        private void FrmSelectComponent_FormClosing(object sender, FormClosingEventArgs e)
        {
            cbbComponent.TextChanged -= CbbComponent_TextChanged;
            btnChange.Click -= BtnChange_Click;

            lsvSelect.MouseClick -= LsvSelect_MouseClick;

            lblColor.Click -= LblColor_Click;
            btnCancel.Click -= BtnCancel_Click;
            btnOk.Click -= BtnOk_Click;
        }

        public object SelectObject = new object();
        public ComponentType.Type SelectType = ComponentType.Type.None;
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (cbbComponent.Text == ComponentType.ToString(ComponentType.Type.LineChart))
            {
                GraphInfo graphInfo = new GraphInfo();
                graphInfo.Title = txbTitle.Text;
                graphInfo.Period = (int)nudPeriod.Value;
                graphInfo.Sample = (int)nudSample.Value;
                graphInfo.UUID = Guid.NewGuid().ToString();

                List<LineInfo> lineInfos = new List<LineInfo>();

                // add linechart info to graphinfo
                foreach (ListViewItem item in lsvSelect.Items)
                {
                    graphInfo.LineInfos.Add(new LineInfo()
                    {
                        Address = Program.MemoryManage.Address[item.SubItems[1].Text],
                        Color = item.SubItems[3].BackColor,
                        Name = item.SubItems[1].Text,
                        Title = item.SubItems[0].Text
                    });
                }
                SelectType = ComponentType.Type.LineChart;
                SelectObject = graphInfo;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LblColor_Click(object sender, EventArgs e)
        {
            if (selectedIndex < 0)
            {
                return;
            }

            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    if(selectedIndex >= 0)
                    {
                        lsvSelect.Items[selectedIndex].SubItems[3].BackColor = colorDialog.Color;
                    }
                }
            }
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            if (lsvSelect.FocusedItem != null)
            {
                lsvSelect.Items[selectedIndex].SubItems[0].Text = txbName.Text;
                lsvSelect.Items[selectedIndex].SubItems[3].BackColor = lblColor.BackColor;
            }
            selectedIndex = -1; // reset selected
            grbEdit.Text = "Edit";
            txbName.Text = "";
            lblColor.BackColor = Color.White;
            btnChange.Enabled = false;
        }

        int selectedIndex = -1;
        private void LsvSelect_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || lsvSelect.FocusedItem == null)
            {
                return;
            }

            txbName.Text = lsvSelect.FocusedItem.SubItems[0].Text;
            selectedIndex = lsvSelect.FocusedItem.Index;
            lblColor.BackColor = lsvSelect.FocusedItem.SubItems[3].BackColor;

            grbEdit.Text = "Edit [" + txbName.Text + "]";
            btnChange.Enabled = true;
        }

        private void CbbComponent_TextChanged(object sender, EventArgs e)
        {
            if (cbbComponent.Text == ComponentType.ToString(ComponentType.Type.LineChart))
            {
                RegisterToList();
            }
        }

        private void RegisterToList()
        {
            lsvData.Items.Clear();
            foreach (KeyValuePair<string, string> keyPair in Program.MemoryManage.ValueType)
            {
                bool nameExist = false;
                foreach (ListViewItem item in lsvSelect.Items)
                {
                    if (item.SubItems[1].Text == keyPair.Key)
                    {
                        nameExist = true;
                        break;
                    }
                }

                if (nameExist)
                    continue;

                ListViewItem listViewItem = new ListViewItem(keyPair.Key);
                listViewItem.SubItems.Add(keyPair.Value);
                lsvData.Items.Add(listViewItem);
            }
        }

        private void btnAddSingle_Click(object sender, EventArgs e)
        {
            if (lsvData.CheckedItems.Count == 0)
                return;

            btnAddSingle.Enabled = false;
            foreach (ListViewItem item in lsvData.CheckedItems)
            {
                string varName = item.SubItems[0].Text;
                ListViewItem listViewItem = new ListViewItem(varName);
                listViewItem.SubItems.Add(varName);
                listViewItem.SubItems.Add(item.SubItems[1].Text);
                listViewItem.SubItems.Add("");
                listViewItem.UseItemStyleForSubItems = false;

                lsvSelect.Items.Add(listViewItem);

                lsvData.Items.Remove(item);
            }
            btnAddSingle.Enabled = true;
            btnOk.Enabled = true;
        }

        private void btnRemoveSingle_Click(object sender, EventArgs e)
        {
            if (lsvSelect.CheckedItems.Count == 0)
                return;

            btnRemoveSingle.Enabled = false;
            foreach (ListViewItem lsvItem in lsvSelect.CheckedItems)
            {
                lsvSelect.Items.Remove(lsvItem);
            }

            RegisterToList();

            btnRemoveSingle.Enabled = true;

            if (lsvSelect.Items.Count == 0)
            {
                btnOk.Enabled = false;
            }
        }
    }
}
