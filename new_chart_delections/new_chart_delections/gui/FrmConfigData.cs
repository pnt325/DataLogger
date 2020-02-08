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
    public partial class FrmConfigData : Form
    {
        public FrmConfigData()
        {
            InitializeComponent();


            // Button
            btnColor.Click += BtnColor_Click;
            btnAdd.Click += BtnAdd_Click;
            btnAdd.Enabled = false;

            // ListView
            lsvMain.MouseClick += LsvMain_MouseClick;

            // TextBox
            txbName.TextChanged += TxbName_TextChanged;
            txbUnit.TextChanged += TxbName_TextChanged;

            this.Shown += FrmConfigData_Shown;
        }

        ListViewItem Object2Item(data.Objects obj)
        {
            ListViewItem item = new ListViewItem(obj.Name);
            item.SubItems.Add(obj.Command.ToString());
            item.SubItems.Add(obj.Unit);
            item.BackColor = obj.Color;

            item.ForeColor = Color.FromArgb(btnColor.BackColor.A,
                                             255 - btnColor.BackColor.R,
                                             255 - btnColor.BackColor.G,
                                             255 - btnColor.BackColor.B);

            return item;
        }

        private void FrmConfigData_Shown(object sender, EventArgs e)
        {
            //Load objet to listview
            foreach(data.Objects obj in Program.DataObjs)
            {
                lsvMain.Items.Add(Object2Item(obj));
            }
        }

        private void TxbName_TextChanged(object sender, EventArgs e)
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if(txbName.Text == "" || txbUnit.Text == "")
            {
                btnAdd.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            data.Objects obj = new data.Objects();
            obj.Name = txbName.Text;

            try
            {
                obj.Command = Convert.ToInt32(txbCommand.Text);
            }
            catch
            {
                obj.Command = 0;
                txbCommand.Text = "0";
            }

            obj.Unit = txbUnit.Text;
            obj.Color = btnColor.BackColor;

            // check that data name already exit
            int index = -1;
            foreach(data.Objects o in Program.DataObjs)
            {
                if(obj.Name == o.Name)
                {
                    index = Program.DataObjs.IndexOf(o);
                    break;
                }
            }

            if(index >= 0)
            {
                Program.DataObjs[index] = obj;
                lsvMain.Items[index] = Object2Item(obj);
            }
            else
            {
                Program.DataObjs.Add(obj);
                lsvMain.Items.Add(Object2Item(obj));
            }
        }

        private void LsvMain_MouseClick(object sender, MouseEventArgs e)
        {
            if(lsvMain.FocusedItem == null)
            {
                return;
            }

            if(e.Button == MouseButtons.Left)
            {
                txbName.Text = lsvMain.FocusedItem.SubItems[0].Text;
                txbCommand.Text = lsvMain.FocusedItem.SubItems[1].Text;
                txbUnit.Text = lsvMain.FocusedItem.SubItems[2].Text;
                btnColor.BackColor = lsvMain.FocusedItem.BackColor;
            }

            if(e.Button == MouseButtons.Right)
            {
                cmsMain.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog color = new ColorDialog())
            {
                if(color.ShowDialog() == DialogResult.OK)
                {
                    btnColor.BackColor = color.Color;
                }
            }
        }
    }
}
