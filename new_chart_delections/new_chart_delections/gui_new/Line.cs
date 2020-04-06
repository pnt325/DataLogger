using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ZedGraph;
using System.Threading;

namespace new_chart_delections.gui_new
{
    public partial class LineChart : UserControl
    {
        public string UUID { get; set; }
        Point startPoint;
        Point endPoint;
        GraphInfo graphInfo;
        int delayTime;
        bool isStart = false;
        int valueCount = 0;

        public LineChart(Point start, Point end, GraphInfo graph)
        {
            InitializeComponent();

            startPoint = start;
            endPoint = end;
            graphInfo = graph;

            delayTime = 1000 / graph.Period;

            this.Location = Program.Grid.GetLocation(startPoint);
            this.Size = Program.Grid.GetSize(startPoint, endPoint);

            // Zedgraph init
            zedGraphControl1.GraphPane.Title.Text = graph.Title;
            zedGraphControl1.GraphPane.XAxis.Title.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.Title.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.IsFontsScaled = false;
            zedGraphControl1.GraphPane.XAxis.Title.FontSpec.Size = 8.5f;
            zedGraphControl1.GraphPane.YAxis.Title.FontSpec.Size = 8.5f;

            zedGraphControl1.GraphPane.XAxis.Scale.Min = 0;
            zedGraphControl1.GraphPane.YAxis.Scale.Max = 100;
            zedGraphControl1.GraphPane.XAxis.Scale.MajorStepAuto = true;

            foreach(LineInfo lineInfo in graph.LineInfos)
            {
                RollingPointPairList rollingPointPairList = new RollingPointPairList(graph.Sample);
                ZedGraph.LineItem lineItem = zedGraphControl1.GraphPane.AddCurve(lineInfo.Name, rollingPointPairList, lineInfo.Color, SymbolType.None);
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            EventInit();
        }


        #region PRIVATE METHOD
        private void EventInit()
        {
            Program.Grid.GridChanged += Grid_GridChanged;
            zedGraphControl1.DoubleClick += ZedGraphControl1_DoubleClick;
            Program.ComponentManage.StartMonitor += ComponentManage_StartMonitor;
            Program.ComponentManage.StopMonitor += ComponentManage_StopMonitor;
        }

        private void EventDeInit()
        {
            Program.Grid.GridChanged -= Grid_GridChanged;
            zedGraphControl1.DoubleClick -= ZedGraphControl1_DoubleClick;
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
            zedGraphControl1.Dispose();
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

            for (int i = 0; i < graphInfo.LineInfos.Count; i++)
            {
                int value = 0;
                if(Program.MemoryManage.Address.TryGetValue(graphInfo.LineInfos[i].Name, out value))
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
                while(isStart)
                {
                    for (int i = 0; i < zedGraphControl1.GraphPane.CurveList.Count; i++)
                    {
                        double value = (float)Program.MemoryManage.Read(graphInfo.LineInfos[i].Address);

                        LineItem lineItem = zedGraphControl1.GraphPane.CurveList[i] as LineItem;
                        IPointListEdit list = lineItem.Points as IPointListEdit;

                        list.Add(valueCount, value);
                        //zedGraphControl1.GraphPane.CurveList[i].AddPoint(valueCount, value);
                        valueCount++;
                    }

                    // scale exist
                    if(valueCount > zedGraphControl1.GraphPane.XAxis.Scale.Max)
                    {
                        zedGraphControl1.GraphPane.XAxis.Scale.Max = valueCount;
                        zedGraphControl1.GraphPane.XAxis.Scale.Min = valueCount - graphInfo.Sample;
                    }

                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Invalidate();

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

        #endregion
    }
}
