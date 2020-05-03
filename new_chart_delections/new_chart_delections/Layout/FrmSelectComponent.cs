using BrightIdeasSoftware;
using DataLogger.Components;
using DataLogger.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataLogger.Layout
{
    public partial class FrmSelectComponent : Form
    {
        public FrmSelectComponent()
        {
            InitializeComponent();

            // Combobox
            cbbComponent.Items.AddRange(Components.Type.Names());
            //btnChange.Enabled = false;
            btnOk.Enabled = false;

            // listview 

            InitEvent();
            NormalSize();
        }

        private void ObjectListView1_FormatRow(object sender, FormatRowEventArgs e)
        {
            e.Item.SubItems[3].BackColor = Color.Blue;
        }

        private void InitEvent()
        {
            cbbComponent.TextChanged += CbbComponent_TextChanged;
            //btnChange.Click += BtnChange_Click;
            lsvSelect.MouseClick += LsvSelect_MouseClick;
            btnCancel.Click += BtnCancel_Click;
            btnOk.Click += BtnOk_Click;
            this.FormClosing += FrmSelectComponent_FormClosing;
            lsvData.ItemCheck += LsvData_ItemCheck;
            lsvData.ItemChecked += LsvData_ItemChecked;
            txbTitle.TextChanged += TxbTitle_TextChanged;
            txbName.KeyPress += TxbName_KeyPress;
        }

        private void TxbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                BtnChange_Click(null, null);
            }
        }

        private void TxbTitle_TextChanged(object sender, EventArgs e)
        {
            InputVerify();
        }

        private void LsvData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            InputVerify();
        }

        private void DeInitEvent()
        {
            cbbComponent.TextChanged -= CbbComponent_TextChanged;
            //btnChange.Click -= BtnChange_Click;
            lsvSelect.MouseClick -= LsvSelect_MouseClick;
            //lblColor.Click -= LblColor_Click;
            btnCancel.Click -= BtnCancel_Click;
            btnOk.Click -= BtnOk_Click;
            this.FormClosing -= FrmSelectComponent_FormClosing;
            lsvData.ItemCheck -= LsvData_ItemCheck;
            lsvData.ItemChecked -= LsvData_ItemChecked;
            txbTitle.TextChanged -= TxbTitle_TextChanged;
        }

        private void LsvData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (cbbComponent.Text != Components.Type.ToString(Components.ComponentTypes.Label))
            {
                return;
            }

            for (int i = 0; i < lsvData.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    lsvData.Items[i].Checked = false;
                }
            }
        }

        private void FrmSelectComponent_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeInitEvent();
        }

        public Core.ComponentItem SelectComponent = new Core.ComponentItem();
        private void BtnOk_Click(object sender, EventArgs e)
        {
            SelectComponent.Type = Components.Type.ToType(cbbComponent.Text);
            SelectComponent.UpdatePeriod = (int)nudPeriod.Value;
            SelectComponent.Uuid = Guid.NewGuid().ToString();
            SelectComponent.Title = txbTitle.Text;

            switch (SelectComponent.Type)
            {
                case Components.ComponentTypes.None:
                    this.DialogResult = DialogResult.None;
                    break;
                case Components.ComponentTypes.Chart:
                    Components.ChartInfo chartInfo = new Components.ChartInfo();
                    chartInfo.Sample = (int)nudSample.Value;
                    chartInfo.Title = txbTitle.Text;

                    foreach (ListViewItem item in lsvSelect.Items)
                    {
                        Components.ChartLine line = new ChartLine();
                        line.Name = item.SubItems[0].Text;
                        line.VarName = item.SubItems[1].Text;
                        line.VarType = Core.MemoryType.ToType(item.SubItems[2].Text);
                        line.VarAddress = Core.Memory.Address[line.VarName];
                        line.Color = item.SubItems[3].BackColor;

                        chartInfo.Lines.Add(line);
                    }

                    SelectComponent.Info = chartInfo;
                    this.DialogResult = DialogResult.OK;
                    break;
                case Components.ComponentTypes.Label:
                    Components.LabelInfo labelInfo = new LabelInfo();
                    labelInfo.Title = txbTitle.Text;
                    labelInfo.VarName = lsvData.CheckedItems[0].SubItems[0].Text;
                    labelInfo.VarType = Core.MemoryType.ToType(lsvData.CheckedItems[0].SubItems[1].Text);
                    labelInfo.VarAddress = Core.Memory.Address[labelInfo.VarName];

                    SelectComponent.Info = labelInfo;
                    this.DialogResult = DialogResult.OK;
                    break;
                case Components.ComponentTypes.Table:
                    List<Components.TableInfo> tableInfos = new List<TableInfo>();

                    foreach (ListViewItem item in lsvSelect.Items)
                    {
                        Components.TableInfo tableInfo = new TableInfo();
                        tableInfo.Name = item.SubItems[0].Text;
                        tableInfo.VarName = item.SubItems[1].Text;
                        tableInfo.VarType = Core.MemoryType.ToType(item.SubItems[2].Text);
                        tableInfo.VarAddress = Core.Memory.Address[tableInfo.VarName];

                        tableInfos.Add(tableInfo);
                    }

                    SelectComponent.Info = tableInfos;
                    this.DialogResult = DialogResult.OK;
                    break;
                default:
                    break;
            }

            if(SelectComponent.Type == ComponentTypes.None)
            {
                this.DialogResult = DialogResult.None;
            }

            this.Close();
        }

        private void InputVerify()
        {
            if(cbbComponent.Text == Components.Type.ToString(Components.ComponentTypes.Label) && txbTitle.Text != "")
            {
                if(lsvData.CheckedItems.Count == 1)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    btnOk.Enabled = false;
                }
            }
            else
            {
                if(txbTitle.Text != "" && lsvSelect.Items.Count > 0)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    btnOk.Enabled = false;
                }
            }
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
                    //lblColor.BackColor = colorDialog.Color;
                }
            }
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            if (lsvSelect.FocusedItem != null)
            {
                lsvSelect.Items[selectedIndex].SubItems[0].Text = txbName.Text;
            }
            selectedIndex = -1; // reset selected
            txbName.Text = "";
            //btnChange.Enabled = false;
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
            //lblColor.BackColor = lsvSelect.FocusedItem.SubItems[3].BackColor;

            //grbEdit.Text = "Edit [" + txbName.Text + "]";
            //btnChange.Enabled = true;
        }

        private void CbbComponent_TextChanged(object sender, EventArgs e)
        {
            if (cbbComponent.Text != Components.Type.ToString(Components.ComponentTypes.Label))
            {
                ExternSize();
            }
            else
            {
                NormalSize();
                lsvSelect.Items.Clear();
            }

            UpdateVarList();
        }

        private void NormalSize()
        {
            this.Size = new Size(246, 421);
            btnAddSingle.Hide();
            btnRemoveSingle.Hide();
        }

        private void ExternSize()
        {
            this.Size = new Size(886, 425);
            btnAddSingle.Show();
            btnRemoveSingle.Show();
        }

        private void UpdateVarList()
        {
            lsvData.Items.Clear();
            foreach (KeyValuePair<string, Core.MemoryTypes> keyPair in Core.Memory.Types)
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
                listViewItem.SubItems.Add(Core.MemoryType.ToString(keyPair.Value));
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

            InputVerify();
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

            UpdateVarList();
            InputVerify();
            btnRemoveSingle.Enabled = true;
            selectedIndex = -1;
        }

        private void colorHexagon1_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            if(selectedIndex >= 0)
            {
                lsvSelect.Items[selectedIndex].SubItems[3].BackColor = args.SelectedColor;
            }
        }
    }
}
