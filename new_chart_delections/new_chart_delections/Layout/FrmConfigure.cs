﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.Layout
{
    public partial class FrmConfigure : Form
    {
        int currentX;
        int currentY;


        public FrmConfigure()
        {
            InitializeComponent();

            nudX.ValueChanged += NudX_ValueChanged;
            nudY.ValueChanged += NudX_ValueChanged;

            // Get value from grid...
            nudX.Value = Core.Grid.X;
            nudY.Value = Core.Grid.Y;

            currentX = (int)nudX.Value;
            currentY = (int)nudY.Value;
        }

        private void UpdateGrid()
        {
            if (Core.Grid.SetSize((int)nudX.Value, (int)nudY.Value))
            {
                currentX = Core.Grid.X;
                currentY = Core.Grid.Y;
            } 
            else
            {
                nudX.Value = currentX;
                nudY.Value = currentY;
            }
        }

        private void NudX_ValueChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }
    }
}