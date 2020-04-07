using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Threading;

namespace new_chart_delections.gui_new
{
    public class LineChart : ZedGraphControl
    {
        public string UUID { get; set; }

        Point startPoint;
        Point endPoint;
        GraphInfo graphInfo;

        int delayTime;
        bool isStart = false;
        int valueCount = 0;

        public LineChart(Point start, Point end, GraphInfo graphInfo)
        {
            this.startPoint = start;
            this.endPoint = end;
            this.graphInfo = graphInfo;

            delayTime = 1000 / this.graphInfo.Period;

            this.Location = Program.Grid.GetLocation(startPoint);
            this.Size = Program.Grid.GetSize(startPoint, endPoint);

            // Zedgraph init
            this.GraphPane.Title.Text = this.graphInfo.Title;
            this.GraphPane.XAxis.Title.IsVisible = false;
            this.GraphPane.YAxis.Title.IsVisible = false;
            this.GraphPane.XAxis.MajorGrid.IsVisible = true;
            this.GraphPane.YAxis.MajorGrid.IsVisible = true;
            this.GraphPane.IsFontsScaled = false;
            this.GraphPane.XAxis.Title.FontSpec.Size = 8.5f;
            this.GraphPane.YAxis.Title.FontSpec.Size = 8.5f;

            this.GraphPane.XAxis.Scale.Min = 0;
            this.GraphPane.YAxis.Scale.Max = 100;
            this.GraphPane.XAxis.Scale.MajorStepAuto = true;

            foreach (LineInfo lineInfo in graphInfo.LineInfos)
            {
                RollingPointPairList rollingPointPairList = new RollingPointPairList(graphInfo.Sample);
                ZedGraph.LineItem lineItem = this.GraphPane.AddCurve(lineInfo.Name, rollingPointPairList, lineInfo.Color, SymbolType.None);
            }

            this.AxisChange();
            this.Invalidate();

            EventInit();
        }

        private void EventInit()
        {
            Program.Grid.GridChanged += Grid_GridChanged;
            this.DoubleClick += ZedGraphControl1_DoubleClick;
            Program.ComponentManage.StartMonitor += ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor += ComponentManage_StopMonitor;
        }

        private void EventDeInit()
        {
            Program.Grid.GridChanged -= Grid_GridChanged;
            this.DoubleClick -= ZedGraphControl1_DoubleClick;
            Program.ComponentManage.StartMonitor -= ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor -= ComponentManage_StopMonitor;
        }
        private void ComponentManage_StopMonitor(object sender, EventArgs e)
        {
            Stop();
        }

        private void ComponentManage_StartMonitor(object sender, EventArgs e)
        {
            Start();
        }

        private void ZedGraphControl1_DoubleClick(object sender, EventArgs e)
        {
            Stop();
            Program.ComponentManage.RemoveAreaItem(UUID);
            EventDeInit();
            this.Dispose();
            this.Dispose();
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Program.Grid.GetLocation(startPoint);
            this.Size = Program.Grid.GetSize(startPoint, endPoint);
        }

        private void Start()
        {
            if (isStart)
                return;

            // remap address.
            for (int i = 0; i < graphInfo.LineInfos.Count; i++)
            {
                int value = 0;
                if (Program.MemoryManage.Address.TryGetValue(graphInfo.LineInfos[i].Name, out value))
                {
                    graphInfo.LineInfos[i].Address = value;
                }
                else
                {
                    // do not thing
                    return;
                }
            }

            isStart = true;

            Thread th = new Thread(() =>
            {
                while (isStart)
                {
                    for (int i = 0; i < this.GraphPane.CurveList.Count; i++)
                    {
                        double value = (float)Program.MemoryManage.Read(graphInfo.LineInfos[i].Address);

                        LineItem lineItem = this.GraphPane.CurveList[i] as LineItem;
                        IPointListEdit list = lineItem.Points as IPointListEdit;

                        list.Add(valueCount, value);
                        //this.GraphPane.CurveList[i].AddPoint(valueCount, value);
                        valueCount++;
                    }

                    // scale exist
                    if (valueCount > this.GraphPane.XAxis.Scale.Max)
                    {
                        this.GraphPane.XAxis.Scale.Max = valueCount;
                        this.GraphPane.XAxis.Scale.Min = valueCount - graphInfo.Sample;
                    }

                    this.AxisChange();
                    this.Invalidate();

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "LineChart";
            this.Size = new System.Drawing.Size(347, 173);
            this.ResumeLayout(false);

        }
    }
}
