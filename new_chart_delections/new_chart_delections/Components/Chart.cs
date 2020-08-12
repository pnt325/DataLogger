using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using ZedGraph;

namespace DataLogger.Components
{
    public class Chart : ZedGraphControl
    {
        private string UUID { get; set; }

        private Point startPoint;
        private Point endPoint;
        private ChartInfo chartInfo;

        private int delayTime;              // delay time convert from hz to ms
        private bool isStart = false;       // component run status
        private int valueCount = 0;         // value index data logger

        public Chart(Core.ComponentItem info)
        {
            // Init object 
            startPoint = new Point();
            endPoint = new Point();
            chartInfo = new ChartInfo();

            chartInfo = (ChartInfo)info.Info;
            UUID = info.Uuid;

            this.startPoint = info.StartPoint;
            this.endPoint = info.EndPoint;

            delayTime = 1000 / info.UpdatePeriod;

            this.Location = Core.Grid.GetPoint(startPoint);
            this.Size = Core.Grid.GetSize(startPoint, endPoint);

            // Zedgraph init
            this.GraphPane.Title.Text = chartInfo.Title;            /*Chart title*/
            this.GraphPane.XAxis.Title.IsVisible = false;           /*Chart hide axis x title*/
            this.GraphPane.YAxis.Title.IsVisible = false;           /*Chart hide axis y title*/
            this.GraphPane.XAxis.MajorGrid.IsVisible = true;        /*Chart show axis x grid*/
            this.GraphPane.YAxis.MajorGrid.IsVisible = true;        /*Chart show axis y grid*/
            this.GraphPane.IsFontsScaled = false;                   /*Chart disable scale font*/
            this.GraphPane.XAxis.Title.FontSpec.Size = 8.5f;        /*Axis title X font size*/
            this.GraphPane.YAxis.Title.FontSpec.Size = 8.5f;        /*Axis title Y font size*/

            this.GraphPane.XAxis.Scale.Min = 0;
            this.GraphPane.YAxis.Scale.Max = 100;
            this.GraphPane.XAxis.Scale.MajorStepAuto = true;

            foreach (ChartLine item in chartInfo.Lines)
            {
                RollingPointPairList rollingPointPairList = new RollingPointPairList(chartInfo.Sample);
                ZedGraph.LineItem lineItem = this.GraphPane.AddCurve(item.Name, rollingPointPairList, item.Color, SymbolType.None);
            }

            this.AxisChange();
            this.Invalidate();

            InitEvent();
        }

        private void InitEvent()
        {
            Core.Component.Start += Component_StartComponent;
            Core.Component.Stop += Component_StopComponent;
            Core.Component.Removed += Component_RemoveComponent;
            Core.Grid.SizeChanged += Grid_SizeChanged;
            this.DoubleClick += ZedGraphControl1_DoubleClick;
        }

        private void DeInitEvent()
        {
            Core.Component.Start -= Component_StartComponent;
            Core.Component.Stop -= Component_StopComponent;
            Core.Component.Removed -= Component_RemoveComponent;
            this.DoubleClick -= ZedGraphControl1_DoubleClick;
            Core.Grid.SizeChanged -= Grid_SizeChanged;
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            this.Location = Core.Grid.GetPoint(startPoint);
            this.Size = Core.Grid.GetSize(startPoint, endPoint);
        }

        private void Component_RemoveComponent(string uuid)
        {
            if (uuid == UUID)
            {
                Stop();
                DeInitEvent();
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

        private void ZedGraphControl1_DoubleClick(object sender, EventArgs e)
        {
            Core.Component.Remove(UUID);
        }

        private void Start()
        {
            if (isStart)
                return;

            // remap address
            for (int i = 0; i < chartInfo.Lines.Count; i++)
            {
                uint value;
                if (Core.Memory.Address.TryGetValue(chartInfo.Lines[i].VarName, out value))
                {
                    chartInfo.Lines[i].VarAddress = value;
                    chartInfo.Lines[i].VarType = Core.Memory.Types[chartInfo.Lines[i].VarName];
                }
                else
                {
                    return;
                }
            }

            isStart = true;
            Core.Component.SetStatus(UUID, Core.ComponentStaus.Running);

            Thread th = new Thread(() =>
            {
                double value = 0;
                while (isStart)
                {
                    for (int i = 0; i < this.GraphPane.CurveList.Count; i++)
                    {
                        value = (float)Core.Memory.Read(chartInfo.Lines[i].VarAddress, chartInfo.Lines[i].VarType);

                        LineItem lineItem = this.GraphPane.CurveList[i] as LineItem;

                        IPointListEdit list = lineItem.Points as IPointListEdit;
                        list.Add(valueCount, value);
                        valueCount++;
                    }

                    // auto scale exist
                    if (valueCount > this.GraphPane.XAxis.Scale.Max)
                    {
                        this.GraphPane.XAxis.Scale.Max = valueCount;
                        this.GraphPane.XAxis.Scale.Min = valueCount - chartInfo.Sample;
                    }

                    // udpate new value to chart
                    this.AxisChange();
                    this.Invalidate();

                    Thread.Sleep(delayTime);
                }

                // chart stop update, set upate status to component management
                Core.Component.SetStatus(UUID, Core.ComponentStaus.Stoped);
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
        public List<ChartLine> Lines { get; set; }

        public ChartInfo()
        {
            Lines = new List<ChartLine>();
        }
    }

    public class ChartLine
    {
        public string Name { get; set; }
        public string VarName { get; set; }
        public uint VarAddress { get; set; }
        public Core.MemoryTypes VarType { get; set; }
        public Color Color { get; set; }

        public ChartLine()
        {
            Color = new Color();
            VarType = new Core.MemoryTypes();
        }
    }
}
