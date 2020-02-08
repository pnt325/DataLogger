using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace new_chart_delections.components
{
    public class CharCallBack
    {
        public event EventHandler Callback;

        /// <summary>
        /// Call when end of form resize
        /// </summary>
        public void Call()
        {
            Callback?.Invoke(null, null);
        }
    }

    public class Chart : IDisposable
    {
        // public define
        public const int LINE_TYPE = 0;
        public const int COLUMN_TYPE = 1;
        public const int TABLE_TYPE = 2;

        // VAFIABLE DEFINE
        ZedGraphControl graph;
        gui.ListViewNF table;

        // Location
        int xStart, yStart;
        int xEnd, yEnd;

        List<int> indexList = new List<int>();
        string graphName = "Chart xxx";
        int chartType = 0;
        int sampleCount = 100; // hz
        CharCallBack chartCallback;

        // METHOD DEFINE
        public void Dispose()
        {
            if (graph != null)
            {
                graph.Dispose();
            }

            if (chartCallback != null)
            {
                chartCallback.Callback -= ChartCallback_Callback;
                chartCallback = null;
            }
        }

        public Chart(CharCallBack callback, List<int> iList, int sample, int type, GridLine.RecSelect recSelect)
        {
            indexList = iList;
            chartType = type;
            sampleCount = sample;
            //frm.ResizeEnd += Frm_ResizeEnd;

            // get possition
            xStart = recSelect.xStart;
            yStart = recSelect.yStart;
            xEnd = recSelect.xCount;
            yEnd = recSelect.yCount;

            chartCallback = callback;
            chartCallback.Callback += ChartCallback_Callback;
        }

        private void ChartCallback_Callback(object sender, EventArgs e)
        {
            if (chartType == TABLE_TYPE)
            {
                if (table == null)
                {
                    return;
                }

                graph.Location = new System.Drawing.Point((int)(xStart * Program.FGrid.XStep), (int)(yStart * Program.FGrid.YStep));
                graph.Size = new System.Drawing.Size((int)(xEnd * Program.FGrid.XStep), (int)(yEnd * Program.FGrid.YStep));
            }
            else
            {
                if (graph == null)
                {
                    return;
                }

                table.Location = new System.Drawing.Point((int)(xStart * Program.FGrid.XStep), (int)(yStart * Program.FGrid.YStep));
                table.Size = new System.Drawing.Size((int)(xEnd * Program.FGrid.XStep), (int)(yEnd * Program.FGrid.YStep));

                table.Columns[0].Width = table.Width / 3;
                table.Columns[1].Width = table.Width / 3;
                table.Columns[2].Width = table.Width / 3 - 10;
            }
        }

        public void SetName(string name)
        {
            graphName = name;
        }

        public void Create()
        {
            if (chartType == TABLE_TYPE)
            {
                table = new gui.ListViewNF();

                graph.Location = new System.Drawing.Point((int)(xStart * Program.FGrid.XStep), (int)(yStart * Program.FGrid.YStep));
                graph.Size = new System.Drawing.Size((int)(xEnd * Program.FGrid.XStep), (int)(yEnd * Program.FGrid.YStep));

                graph.GraphPane.Title.Text = graphName;
                graph.GraphPane.XAxis.Title.IsVisible = false;
                graph.GraphPane.YAxis.Title.IsVisible = false;
                graph.GraphPane.XAxis.MajorGrid.IsVisible = true;
                graph.GraphPane.YAxis.MajorGrid.IsVisible = true;

                graph.GraphPane.XAxis.Scale.Min = 0;
                graph.GraphPane.XAxis.Scale.Max = sampleCount;

                foreach (int index in indexList)
                {
                    if (chartType == COLUMN_TYPE)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else
            {
                graph = new ZedGraphControl();
            }
        }

        void AddLine(GraphPane pane, int index)
        {
            RollingPointPairList pairList;
            LineItem lineItem;

            pairList = new RollingPointPairList(sampleCount);
            lineItem = pane.AddCurve(Program.DataObjs[index].Name, pairList, Program.DataObjs[index].Color, SymbolType.None);
        }

        void AddColumn(GraphPane pane, int index)
        {
            RollingPointPairList pairList;
            BarItem barItem;

            pairList = new RollingPointPairList(sampleCount);
            barItem = pane.AddBar(Program.DataObjs[index].Name, pairList, Program.DataObjs[index].Color);
        }
    }
}
