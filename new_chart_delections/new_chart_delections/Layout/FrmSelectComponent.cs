﻿using new_chart_delections.Components;
using new_chart_delections.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace new_chart_delections.Layout
{
    public partial class FrmSelectComponent : Form
    {
        public FrmSelectComponent()
        {
            InitializeComponent();

            // Combobox
            cbbComponent.Items.AddRange(Components.Type.GetNames());
            btnChange.Enabled = false;
            btnOk.Enabled = false;

            InitEvent();
        }

        private void InitEvent()
        {
            cbbComponent.TextChanged += CbbComponent_TextChanged;
            btnChange.Click += BtnChange_Click;
            lsvSelect.MouseClick += LsvSelect_MouseClick;
            lblColor.Click += LblColor_Click;
            btnCancel.Click += BtnCancel_Click;
            btnOk.Click += BtnOk_Click;
            this.FormClosing += FrmSelectComponent_FormClosing;
            lsvData.ItemCheck += LsvData_ItemCheck;
            lsvData.ItemChecked += LsvData_ItemChecked;
            txbTitle.TextChanged += TxbTitle_TextChanged;
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
            btnChange.Click -= BtnChange_Click;
            lsvSelect.MouseClick -= LsvSelect_MouseClick;
            lblColor.Click -= LblColor_Click;
            btnCancel.Click -= BtnCancel_Click;
            btnOk.Click -= BtnOk_Click;
            this.FormClosing -= FrmSelectComponent_FormClosing;
            lsvData.ItemCheck -= LsvData_ItemCheck;
            lsvData.ItemChecked -= LsvData_ItemChecked;
            txbTitle.TextChanged -= TxbTitle_TextChanged;
        }

        private void LsvData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (cbbComponent.Text != Components.Type.ToString(Components.Types.Label))
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

            switch (SelectComponent.Type)
            {
                case Components.Types.None:
                    this.DialogResult = DialogResult.None;
                    break;
                case Components.Types.Chart:
                    Components.ChartInfo chartInfo = new Components.ChartInfo();
                    chartInfo.Sample = (int)nudSample.Value;
                    chartInfo.Title = txbTitle.Text;
                    
                    foreach(ListViewItem item in lsvSelect.Items)
                    {
                        Components.Line line = new Line();
                        line.Name = item.SubItems[0].Text;
                        line.VarName = item.SubItems[1].Text;
                        line.VarType = Core.MemoryType.ToType(item.SubItems[2].Text);
                        line.VarAddress = Core.Memory.VarAddress[line.VarName];
                        line.Color = item.SubItems[3].BackColor;

                        chartInfo.Lines.Add(line);
                    }

                    SelectComponent.Info = chartInfo;
                    this.DialogResult = DialogResult.OK;
                    break;
                case Components.Types.Label:
                    Components.LabelInfo labelInfo = new LabelInfo();
                    labelInfo.Title = txbTitle.Text;
                    labelInfo.VarName = lsvData.CheckedItems[0].SubItems[0].Text;
                    labelInfo.VarType = Core.MemoryType.ToType(lsvData.CheckedItems[0].SubItems[1].Text);
                    labelInfo.VarAddress = Core.Memory.VarAddress[labelInfo.VarName];

                    SelectComponent.Info = labelInfo;
                    this.DialogResult = DialogResult.OK;
                    break;
                case Components.Types.Table:
                    break;
                default:
                    break;
            }
            this.Close();
        }

        private void InputVerify()
        {
            if(cbbComponent.Text == Components.Type.ToString(Components.Types.Label) && txbTitle.Text != "")
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
                    lblColor.BackColor = colorDialog.Color;
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
            if (cbbComponent.Text != Components.Type.ToString(Components.Types.Label))
            {
                this.Size = new Size(706, 416);
            }
            else
            {
                this.Size = new Size(304, 416);
                lsvSelect.Items.Clear();
            }

            UpdateVarList();
        }

        private void UpdateVarList()
        {
            lsvData.Items.Clear();
            foreach (KeyValuePair<string, Core.MemoryTypes> keyPair in Core.Memory.VarTypes)
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

            btnRemoveSingle.Enabled = true;

            InputVerify();
        }
    }
}