using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using System.Runtime.CompilerServices;

namespace new_chart_delections.gui_new
{
    public partial class XLabel : UserControl
    {
        public string UUID { get; set; } = "";

        private int delayTime;
        private Point startPoint;
        private Point endPoint;
        private LabelInfo labelInfo;
        private bool isStart = false;

        public XLabel(Point start, Point end, LabelInfo labelInfo)
        {
            InitializeComponent();

            this.startPoint = start;
            this.endPoint = end;
            this.labelInfo = labelInfo;

            lblName.Text = this.labelInfo.Name;

            delayTime = 1000 / this.labelInfo.Period;

            this.Location = Program.Grid.GetLocation(this.startPoint);
            this.Size = Program.Grid.GetSize(this.startPoint, this.endPoint);

            EventInit();
            Grid_GridChanged(null, null);
        }

        private void EventInit()
        {
            Program.Grid.GridChanged += Grid_GridChanged;
            lblName.DoubleClick += LblName_DoubleClick;
            lblValue.DoubleClick += LblName_DoubleClick;
            Program.ComponentManage.StartMonitor += ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor += ComponentManage_StopMonitor;
        }

        private void EventDeInit()
        {
            Program.Grid.GridChanged -= Grid_GridChanged;
            lblName.DoubleClick -= LblName_DoubleClick;
            lblValue.DoubleClick -= LblName_DoubleClick;
            Program.ComponentManage.StartMonitor -= ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor -= ComponentManage_StopMonitor;
        }

        private void ComponentManage_StopMonitor(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void ComponentManage_StartMonitor(object sender, EventArgs e)
        {
            this.Start();
        }

        private void LblName_DoubleClick(object sender, EventArgs e)
        {
            this.Stop();
            EventDeInit();
            Program.ComponentManage.RemoveAreaItem(this.UUID);
            this.Dispose();
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Program.Grid.GetLocation(this.startPoint);
            this.Size = Program.Grid.GetSize(this.startPoint, this.endPoint);

            lblName.Height = this.Height / 3;
            lblName.Width = this.Width;
            lblName.Location = new Point(0, 0);

            lblValue.Height = this.Height - lblName.Height;
            lblValue.Width = this.Width;
            lblValue.Location = new Point(0, lblName.Height);

            LabelFontScale(lblValue);
            LabelFontScale(lblName);
        }

        private void LabelFontScale(Label lbl)
        {
            float fac = (lbl.Height / 23.0f)* 9.0f;

            lbl.Font = new Font(lbl.Font.FontFamily, fac, lbl.Font.Style);
        }

        private void Start()
        {
            if (isStart)
                return;

            int address = 0;
            if(Program.MemoryManage.Address.TryGetValue(this.labelInfo.VarName, out address))
            {
                this.labelInfo.Address = address;
            }
            else
            {
                return;
            }

            isStart = true;

            Thread th = new Thread(() =>
            {
                while(isStart)
                {
                    float v = (float)Program.MemoryManage.Read(this.labelInfo.Address);
                    lblValue.Invoke((MethodInvoker)delegate
                    {
                        lblValue.Text = string.Format("{0:00}", v);
                    });

                    Thread.Sleep(delayTime);
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void Stop()
        {
            isStart = false;
        }
    }
}
