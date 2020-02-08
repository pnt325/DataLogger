using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace DataLogging
{
    public class Chart:IDisposable
    {
        int xStart, yStart;
        int xEnd, yEnd;
        int xStep, yStep;
        //private bool IsDispose = false;
        private int sampleCount = 100;
        private List<int> indexList = new List<int>();
        private string graphName = "";
        System.Windows.Forms.Timer time;// = new System.Windows.Forms.Timer();
        private int valueCount = 0;
        private int chartType = 0;//Linechart
        private ListView table;

        Panel panel;
        ZedGraphControl graph;

        public void Dispose()
        {
            time.Stop();
            time.Dispose();
            if (table != null)
                table.Dispose();
            if (graph != null)
                graph.Dispose();
            indexList.Clear();
        }

        public Chart(Control control,int chartType, List<int> list, string name, int sample)
        {
            panel = control as Panel;
            panel.SizeChanged += Form_SizeChanged;

            xStart = DrawData.xStart / DrawData.xStep;
            yStart = DrawData.yStart / DrawData.yStep;
            xEnd = DrawData.xEnd / DrawData.xStep;
            yEnd = DrawData.yEnd / DrawData.yStep;

            indexList = list;
            graphName = name;
            sampleCount = sample;
            this.chartType = chartType;
        }

        private void Table_DoubleClick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Graph_DoubleClick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            xStep = panel.Width / DrawData.xGrid;
            yStep = panel.Height / DrawData.yGrid;

            if (chartType == (int)ChartType.Line || chartType == (int)ChartType.Column)
            {
                if (graph == null)
                    return;
                graph.Location = new Point(xStart * xStep, yStart * yStep);
                graph.Size = new Size((xEnd - xStart) * xStep, (yEnd - yStart) * yStep);
            }
            else if(chartType == (int)ChartType.Table)
            {
                if (table.IsDisposed)
                    return;
                table.Location = new Point(xStart * xStep, yStart * yStep);
                table.Size = new Size((xEnd - xStart) * xStep, (yEnd - yStart) * yStep);
                table.Columns[0].Width = table.Width / 3;
                table.Columns[1].Width = table.Width / 3;
                table.Columns[2].Width = table.Width / 3 - 10;
            }
        }
        public void Create()
        {
            if (chartType == (int)ChartType.Line || chartType == (int)ChartType.Column)
            {
                graph = new ZedGraphControl();

                graph.Location = new Point(xStart * DrawData.xStep, yStart * DrawData.yStep);
                graph.Size = new Size((xEnd - xStart) * DrawData.xStep, (yEnd - yStart) * DrawData.yStep);

                GraphPane graphPane = graph.GraphPane;
                PaneBase paneBase = graph.GraphPane;

                graphPane.Title.Text = graphName;
                graphPane.XAxis.Title.IsVisible = false;
                graphPane.YAxis.Title.IsVisible = false;
                graphPane.XAxis.MajorGrid.IsVisible = true;
                graphPane.YAxis.MajorGrid.IsVisible = true;

                Scale xScale = graph.GraphPane.XAxis.Scale;
                xScale.Min = 0;
                xScale.Max = sampleCount;

                foreach (int index in indexList)
                {
                    if (chartType == (int)ChartType.Line)
                        AddLine(graphPane, index);
                    else if(chartType ==(int)ChartType.Column)
                    {
                        AddColumn(graphPane, index);
                    }
                    panel.Controls.Add(graph);
                    graph.AxisChange();
                }
                graph.DoubleClick += Graph_DoubleClick;
            }
            if (chartType == (int)ChartType.Table)
            {
                table = new ListView();
                table.Location = new Point(xStart * DrawData.xStep, yStart * DrawData.yStep);
                table.Size = new Size((xEnd - xStart) * DrawData.xStep, (yEnd - yStart) * DrawData.yStep);
                table.GridLines = true;
                table.FullRowSelect = true;
                table.View = View.Details;

                table.Columns.Add("Name");
                table.Columns.Add("Value");
                table.Columns.Add("Unit");

                table.Columns[0].Width = table.Width / 3;
                table.Columns[1].Width = table.Width / 3;
                table.Columns[2].Width = table.Width / 3 - 10;

                foreach (int index in indexList)
                {
                    ListViewItem item = new ListViewItem(ChartData.Names[index]);
                    item.SubItems.Add(ChartData.Values[index].ToString());
                    item.SubItems.Add(ChartData.Unit[index].ToString());
                    table.Items.Add(item);
                }
                panel.Controls.Add(table);
                table.DoubleClick += Table_DoubleClick;
            }
            time = new System.Windows.Forms.Timer();
            time.Interval = 1000 / ChartData.ScanRate; // in millis
            time.Tick += Time_Tick;
            time.Start();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Update();
        }
        private void Update()
        {
            if (chartType == (int)ChartType.Line || chartType == (int)ChartType.Column)
            {
                valueCount++;
                GraphPane graphPane = graph.GraphPane;

                Scale xScale = graphPane.XAxis.Scale;
                xScale.MajorStepAuto = true;
                if (valueCount > xScale.Max)
                {
                    xScale.Max = valueCount;// + xScale.MajorStep;
                    xScale.Min = xScale.Max - sampleCount;
                }

                for (int i = 0; i < indexList.Count; i++)
                {
                    if(chartType == (int)ChartType.Line)
                        LineUpate(i, valueCount, ChartData.Values[indexList[i]]);
                    else if(chartType == (int)ChartType.Column)
                    {
                        ColumnUpdate(i, valueCount, ChartData.Values[indexList[i]]);
                    }
                }

                graph.AxisChange();
                graph.Invalidate();
            }
            else if(chartType == (int)ChartType.Table)
            {
                for(int i = 0; i < indexList.Count;i++)
                {
                    TableUpdate(i, ChartData.Values[indexList[i]]);
                }
            }
        }
        private void LineUpate(int index, int count, float value)
        {
            LineItem lineItem = graph.GraphPane.CurveList[index] as LineItem;
            IPointListEdit list = lineItem.Points as IPointListEdit;
            list.Add(count, value);
        }
        private void ColumnUpdate(int index, int count, float value)
        {
            BarItem lineItem = graph.GraphPane.CurveList[index] as BarItem;
            IPointListEdit list = lineItem.Points as IPointListEdit;
            list.Add(count, value);
        }
        private void TableUpdate(int index, float value)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                table.Items[i].SubItems[1].Text = ChartData.Values[indexList[i]].ToString();
            }
        }
        private void AddLine(GraphPane myPane, int index)
        {
            RollingPointPairList pairList;
            LineItem lineItem;

            pairList = new RollingPointPairList(sampleCount);
            lineItem = myPane.AddCurve(ChartData.Names[index], pairList, ChartData.ChartColor[index], SymbolType.None);
        }
        private void AddColumn(GraphPane myPane, int index)
        {
            RollingPointPairList pairList;
            BarItem barItem;
            pairList = new RollingPointPairList(sampleCount);
            barItem = myPane.AddBar(ChartData.Names[index], pairList, ChartData.ChartColor[index]);
        }
    }
}
