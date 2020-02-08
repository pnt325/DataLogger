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
    public partial class FrmGridEdit : Form
    {
        int xGrid = 0;
        int yGrid = 0;

        public delegate void GridEvent(int x, int y);
        public event GridEvent Grid;

        public FrmGridEdit(int x, int y)
        {
            InitializeComponent();
            this.KeyPreview = true;

            this.KeyPress += FrmGridEdit_KeyPress;
            this.btnOk.Click += BtnOk_Click;


            txbX.Text = x.ToString();
            txbY.Text = y.ToString();

            txbX.Click += TxbX_Click;
            txbY.Click += TxbY_Click;
        }

        private void TxbY_Click(object sender, EventArgs e)
        {
            txbY.SelectionStart = 0;
            txbY.SelectionLength = txbX.Text.Length;
        }

        private void TxbX_Click(object sender, EventArgs e)
        {
            txbX.SelectionStart = 0;
            txbX.SelectionLength = txbX.Text.Length;
        }

        private bool GetGridValue()
        {
            try
            {
                xGrid = Convert.ToInt32(txbX.Text);
                //y = Convert.ToInt32(txbY.Text);
            }
            catch
            {
                txbX.Focus();
                return false;
            }
            try
            {
                yGrid = Convert.ToInt32(txbY.Text);
            }
            catch
            {
                txbY.Focus();
                return false;
            }

            if (xGrid == 0)
            {
                txbY.Focus();
                return false;
            }
            if (yGrid == 0)
            {
                txbY.Focus();
                return false; 
            }


            foreach (data.ControlObject obj in Program.ControlObjs)
            {
                if (xGrid < (obj.X + obj.StepX) || xGrid < 2)
                {
                    xGrid = Program.FGrid.X;
                    txbX.Text = xGrid.ToString();
                }

                if(yGrid < (obj.Y + obj.StepY) || yGrid < 2)
                {
                    yGrid = Program.FGrid.Y;
                    txbY.Text = yGrid.ToString();
                }
            }

            Grid?.Invoke(xGrid, yGrid);
            return true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (GetGridValue())
            {
                this.Close();
            }
        }

        private void FrmGridEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else if(e.KeyChar == (char)Keys.Enter)
            {
                //BtnOk_Click(null, null);
                GetGridValue();
            }
        }
    }
}
