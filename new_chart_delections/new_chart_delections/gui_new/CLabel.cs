using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public class CLabel: UserControl
    {
        private Label lblName;
        private Label lblValue;

        private int delayTime;
        private Point startPoint;
        private Point endPoint;
        private LabelInfo labelInfo;

        private bool isStart = false;

        


        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(248, 40);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue
            // 
            this.lblValue.BackColor = System.Drawing.Color.White;
            this.lblValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblValue.Location = new System.Drawing.Point(0, 38);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(248, 80);
            this.lblValue.TabIndex = 0;
            this.lblValue.Text = "Value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CLabel
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblName);
            this.Name = "CLabel";
            this.Size = new System.Drawing.Size(248, 118);
            this.ResumeLayout(false);

        }

        public CLabel()
        {
            InitializeComponent();
            this.EventInit();
        }


        public CLabel(Point start, Point end, LabelInfo labelInfo)
        {
            this.startPoint = start;
            this.endPoint = end;
            this.labelInfo = labelInfo;

            lblName.Text = this.labelInfo.Name;

            delayTime = 1000 / this.labelInfo.Period;

            this.Location = Program.Grid.GetLocation(this.startPoint);
            this.Size = Program.Grid.GetSize(this.startPoint, this.endPoint);

            this.EventInit();

            InitializeComponent();
        }


        private void EventInit()
        {
            Program.Grid.GridChanged += Grid_GridChanged;
            lblName.DoubleClick += LblName_DoubleClick;
            lblValue.DoubleClick += LblName_DoubleClick;
            Program.ComponentManage.StartMonitor += ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor += ComponentManage_StopMonitor;

            this.SizeChanged += CLabel_SizeChanged;
        }

        private void CLabel_SizeChanged(object sender, EventArgs e)
        {
            // update label size
            int nameHight = this.Height / 3;
            lblName.Width = nameHight;
            lblValue.Height = this.Height - nameHight;

            // update font size
            LabelScaleFontSize(lblValue);
            LabelScaleFontSize(lblName);
        }

        private void LabelScaleFontSize(Label lbl)
        {
            // Only bother if there's text.
            //string txt = lbl.Text;
            //if (txt.Length > 0)
            //{
            //    int best_size = 100;

            //    // See how much room we have, allowing a bit
            //    // for the Label's internal margin.
            //    int wid = lbl.DisplayRectangle.Width - 3;
            //    int hgt = lbl.DisplayRectangle.Height - 3;

            //    // Make a Graphics object to measure the text.
            //    using (Graphics gr = lbl.CreateGraphics())
            //    {
            //        for (int i = 1; i <= 100; i++)
            //        {
            //            using (Font test_font =
            //                new Font(lbl.Font.FontFamily, i))
            //            {
            //                // See how much space the text would
            //                // need, specifying a maximum width.
            //                SizeF text_size =
            //                    gr.MeasureString(txt, test_font);
            //                if ((text_size.Width > wid) ||
            //                    (text_size.Height > hgt))
            //                {
            //                    best_size = i - 1;
            //                    break;
            //                }
            //            }
            //        }
            //    }

            //    // Use that font size.
            //    lbl.Font = new Font(lbl.Font.FontFamily, best_size);
            //}


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
            EventDeInit();
            this.Dispose();
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Program.Grid.GetLocation(this.startPoint);
            this.Size = Program.Grid.GetSize(this.startPoint, this.endPoint);
        }

        private void Start()
        {
            if (isStart)
                return;

            // remap value name and address
            int value;
            if(Program.MemoryManage.Address.TryGetValue(this.labelInfo.VarName, out value))
            {
                this.labelInfo.Address = value;
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
                        lblValue.Text = v.ToString();
                        lblValue.Text = string.Format("{0:00}", v);
                        lblValue.Text = v.ToString();
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
