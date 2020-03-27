using System;
using System.Drawing;
using System.Threading;
using ZedGraph;

namespace new_chart_delections.gui_new
{
    public class LineChart : ZedGraphControl
    {
        Point startPoint = new Point();
        Point endPoint = new Point();
        GraphInfo lineChartInfo = new GraphInfo();

        int timeDelay = 0;
        bool isRun = false;

        /// <summary>
        /// Init linechart, create new zedgraph, init size, location
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="info"></param>
        public LineChart(Point start, Point end, GraphInfo info)
        {
            startPoint = start;
            endPoint = end;
            lineChartInfo = info;

            timeDelay = 1000 / info.Period;   // convert hz to ms

            this.Location = Program.Grid.GetLocation(startPoint);
            this.Size = Program.Grid.GetSize(startPoint, endPoint);

            Program.Grid.GridChanged += Grid_GridChanged;

            // init graph info
            this.GraphPane.Title.Text = info.Title;
            this.GraphPane.XAxis.Title.IsVisible = false;
            this.GraphPane.YAxis.Title.IsVisible = false;
            this.GraphPane.XAxis.MajorGrid.IsVisible = true;
            this.GraphPane.YAxis.MajorGrid.IsVisible = true;

            this.GraphPane.IsFontsScaled = false;
            this.GraphPane.XAxis.Title.FontSpec.Size = 8.5f;
            this.GraphPane.YAxis.Title.FontSpec.Size = 8.5f;

            this.GraphPane.XAxis.Scale.Min = 0;
            this.GraphPane.XAxis.Scale.Max = info.Sample;

            Program.ComponentManage.StartMonitor += Start;
            Program.ComponentManage.StopMonitor += Stop;

            this.DoubleClick += GraphControl_DoubleClick;

            // Init linechart   
            foreach (LineInfo lineData in info.LineInfos)
            {
                RollingPointPairList rollingPointPairList = new RollingPointPairList(info.Sample);  // create list store point.
                ZedGraph.LineItem lineItem = this.GraphPane.AddCurve(lineData.Name, rollingPointPairList, lineData.Color, SymbolType.None);
            }

            this.AxisChange();
            this.Invalidate();
        }

        private void Grid_GridChanged(object sender, EventArgs e)
        {
            this.Location = Program.Grid.GetLocation(startPoint);
            this.Size = Program.Grid.GetSize(startPoint, endPoint);
        }

        private void GraphControl_DoubleClick(object sender, EventArgs e)
        {
            this.Dispose();
            this.Stop(null, null);
        }

        Thread thPrepareData;
        Thread thUpdateGraph;
        EventWaitHandle eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        public void Start(object sender, EventArgs e)
        {
            if (isRun)
                return; // if thread alread start.

            // Re-map address of value. if value add to register form change.
            foreach (LineInfo lineInfo in lineChartInfo.LineInfos)
            {
                //Program.MemoryManage.Register.

            }

            for (int i = 0; i < lineChartInfo.LineInfos.Count; i++)
            {
                int value = 0;
                if (Program.MemoryManage.Address.TryGetValue(lineChartInfo.LineInfos[i].Name, out value))
                {
                    lineChartInfo.LineInfos[i].Address = value; // update new address for VarName key store on MemoryManage.
                }
                else
                {
                    // VarName do not exist or removed.
                    return; // do not start thread. save error to log files.
                }
            }


            isRun = true;

            // Thread update change to zedgraph.

            thUpdateGraph = new Thread(() =>
            {
                while (isRun)
                {
                    this.AxisChange();
                    this.Invalidate();
                    eventWait.Set();
                    Thread.Sleep(timeDelay);
                }
            });
            thUpdateGraph.IsBackground = true;
            thUpdateGraph.Start();

            // Thread process any data for graph and store to graph point list...
            thPrepareData = new Thread(() =>
            {
                int valueCount = 0;
                while (isRun)
                {
                    for (int i = 0; i < this.GraphPane.CurveList.Count; i++)
                    {
                        this.GraphPane.CurveList[i].AddPoint(valueCount++, (double)Program.MemoryManage.Read(lineChartInfo.LineInfos[i].Address));
                    }

                    eventWait.WaitOne();
                }
            });
            thPrepareData.IsBackground = true;
            thPrepareData.Start();
        }

        public void Stop(object sender, EventArgs e)
        {
            // stop thread update 
            isRun = false;

            try
            {
                thUpdateGraph.Join();
                thPrepareData.Join();
            }
            catch
            {

            }
        }
    }
}
