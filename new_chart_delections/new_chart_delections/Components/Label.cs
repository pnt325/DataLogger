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

namespace new_chart_delections.Components
{
    public partial class Label : UserControl
    {
        public string UUID { get; set; } = "";

        private int delayTime;
        private Point startPoint;
        private Point endPoint;
        private LabelInfo labelInfo;
        private bool isStart = false;

        public Label(Point start, Point end, Core.ComponentItem info)
        {
            startPoint = new Point();
            endPoint = new Point();
            labelInfo = new LabelInfo();

            InitializeComponent();

            this.UUID = info.Uuid;
            this.startPoint = start;
            this.endPoint = end;
            labelInfo = (LabelInfo)info.Info;

            lblName.Text = labelInfo.Title;

            delayTime = 1000 / info.UpdatePeriod;

            this.Location = Core.Grid.GetPoint(this.startPoint);
            this.Size = Core.Grid.GetSize(this.startPoint, this.endPoint);

            EventInit();
            Grid_GridChanged(null, null);
        }

        private void EventInit()
        {
            lblName.DoubleClick += LblName_DoubleClick;
            lblValue.DoubleClick += LblName_DoubleClick;
            Core.Component.Start += Component_StartComponent;
            Core.Component.Stop += Component_StopComponent;
            Core.Component.Removed += Component_RemoveComponent;
            Core.Grid.SizeChanged += Grid_SizeChanged;
        }

        private void Component_RemoveComponent(string uuid)
        {
            if(uuid == UUID)
            {
                Stop();
                EventDeInit();
                this.Dispose();
            }
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            Grid_GridChanged(null, null);
        }

        private void Component_StopComponent(string uuid)
        {
            if(uuid == UUID)
            {
                Stop();
            }
        }

        private void Component_StartComponent(string uuid)
        {
            if(uuid == UUID)
            {
                Start();
            }
        }

        private void EventDeInit()
        {
            lblName.DoubleClick -= LblName_DoubleClick;
            lblValue.DoubleClick -= LblName_DoubleClick;
            Core.Component.Start -= Component_StartComponent;
            Core.Component.Stop -= Component_StopComponent;
            Core.Component.Removed -= Component_RemoveComponent;
            Core.Grid.SizeChanged -= Grid_SizeChanged;
        }

        private void LblName_DoubleClick(object sender, EventArgs e)
        {
            Core.Component.Remove(UUID);
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Core.Grid.GetPoint(this.startPoint);
            this.Size = Core.Grid.GetSize(this.startPoint, this.endPoint);

            lblName.Height = this.Height / 3;
            lblName.Width = this.Width;
            lblName.Location = new Point(0, 0);

            lblValue.Height = this.Height - lblName.Height;
            lblValue.Width = this.Width;
            lblValue.Location = new Point(0, lblName.Height);

            LabelFontScale(lblValue);
            LabelFontScale(lblName);
        }

        private void LabelFontScale(System.Windows.Forms.Label lbl)
        {
            float fac = (lbl.Height / 23.0f)* 9.0f;

            lbl.Font = new Font(lbl.Font.FontFamily, fac, lbl.Font.Style);
        }

        private void Start()
        {
            if (isStart)
                return;

            // remap
            uint address = 0;
            if (Core.Memory.VarAddress.TryGetValue(labelInfo.VarName, out address))
            {
                labelInfo.VarAddress = address;
                labelInfo.VarType = Core.Memory.VarTypes[labelInfo.VarName];
            }

            isStart = true;

            Thread th = new Thread(() =>
            {
                while(isStart)
                {
                    float v = (float)Core.Memory.Read(labelInfo.VarAddress, labelInfo.VarType);
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

    public class LabelInfo
    {
        public string Title { get; set; }
        public string VarName { get; set; }
        public uint VarAddress { get; set; }
        public Core.MemoryTypes VarType { get; set; }

        public LabelInfo()
        {
            VarType = new Core.MemoryTypes();
        }
    }
}
