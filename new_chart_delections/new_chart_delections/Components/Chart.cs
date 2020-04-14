using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Threading;
using new_chart_delections.Core;
using System.Diagnostics.Eventing.Reader;

namespace new_chart_delections.Components
{
    public class Chart: ZedGraphControl
    {
        public string UUID { get; set; }

        private Point startPoint;
        private Point endPoint;
        private ChartInfo chartInfo;

        private int delayTime;
        private bool isStart = false;
        private int valueCount = 0;

        public Chart(Point start, Point end, ComponentItem info)
        {
            // Init object 
            startPoint = new Point();
            endPoint = new Point();
            chartInfo = new ChartInfo();

            chartInfo = (ChartInfo)info.Info;
            UUID = info.Uuid;

            this.startPoint = start;
            this.endPoint = end;

            delayTime = 1000 / info.UpdatePeriod;

            this.Location = Core.Grid.GetPoint(start);
            this.Size = Core.Grid.GetSize(start, end);

            // Zedgraph init
            this.GraphPane.Title.Text = chartInfo.Title;
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

            foreach(Line item in chartInfo.Lines)
            {
                RollingPointPairList rollingPointPairList = new RollingPointPairList(chartInfo.Sample);
                ZedGraph.LineItem lineItem = this.GraphPane.AddCurve(item.Name, rollingPointPairList, item.Color, SymbolType.None);
            }

            this.AxisChange();
            this.Invalidate();

            EventInit();
        }

        private void EventInit()
        {
            Core.Component.Start += Component_StartComponent;
            Core.Component.Stop += Component_StopComponent;
            Core.Component.Removed += Component_RemoveComponent;
            Core.Grid.SizeChanged += Grid_SizeChanged;
            this.DoubleClick += ZedGraphControl1_DoubleClick;
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            this.Location = Core.Grid.GetPoint(startPoint);
            this.Size = Core.Grid.GetSize(startPoint, endPoint);
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

        private void Component_StopComponent(string uuid)
        {
            if (UUID == uuid)
            {
                Stop();
            }
        }

        private void Component_StartComponent(string uuid)
        {
            if (uuid == UUID)
            {
                Start();
            }
        }

        private void EventDeInit()
        {
            Core.Component.Start -= Component_StartComponent;
            Core.Component.Stop -= Component_StopComponent;
            Core.Component.Removed -= Component_RemoveComponent;
            this.DoubleClick -= ZedGraphControl1_DoubleClick;
            Core.Grid.SizeChanged -= Grid_SizeChanged;
        }

        private void ZedGraphControl1_DoubleClick(object sender, EventArgs e)
        {
            // Request remove component
            Core.Component.Remove(UUID);
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Core.Grid.GetPoint(startPoint);
            this.Size = Core.Grid.GetSize(startPoint, endPoint);
        }

        private void Start()
        {
            if (isStart)
                return;

            // remap address
            for (int i = 0; i < chartInfo.Lines.Count; i++)
            {
                uint value;
                if(Core.Memory.VarAddress.TryGetValue(chartInfo.Lines[i].VarName, out value))
                {
                    chartInfo.Lines[i].VarAddress = value;
                    chartInfo.Lines[i].VarType = Core.Memory.VarTypes[chartInfo.Lines[i].VarName];
                }
            }

            isStart = true;
            Thread th = new Thread(() =>
            {
                while (isStart)
                {
                    for (int i = 0; i < this.GraphPane.CurveList.Count; i++)
                    {
                        double value = (float)Core.Memory.Read(chartInfo.Lines[i].VarAddress, chartInfo.Lines[i].VarType);

                        LineItem lineItem = this.GraphPane.CurveList[i] as LineItem;
                        IPointListEdit list = lineItem.Points as IPointListEdit;

                        list.Add(valueCount, value);
                        valueCount++;
                    }

                    // scale exist
                    if (valueCount > this.GraphPane.XAxis.Scale.Max)
                    {
                        this.GraphPane.XAxis.Scale.Max = valueCount;
                        this.GraphPane.XAxis.Scale.Min = valueCount - chartInfo.Sample;
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
    }

    public class ChartInfo
    {
        public string Title { get; set; }
        public int Sample { get; set; }
        public List<Line> Lines { get; set; }

        public ChartInfo()
        {
            Lines = new List<Line>();
        }
    }

    public class Line
    {
        public string Name { get; set; }
        public string VarName { get; set; }
        public uint VarAddress { get; set; }
        public Core.MemoryTypes VarType { get; set; }
        public Color Color { get; set; }

        public Line()
        {
            Color = new Color();
            VarType = new MemoryTypes();
        }
    }
}
