using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataLogger.Layout
{
    public partial class TestGui : Form
    {
        public TestGui()
        {
            InitializeComponent();

            ChartInit();
        }

        // chart init
        private void ChartInit()
        {
            ChartArea chartArea = new ChartArea();

            chartArea.AxisX.LabelStyle.Format = "dd/MM/yy";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 6);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 6);
            chart1.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = "TEMP_STACK_BOILER_1";
            series.ChartType = SeriesChartType.FastLine;
            series.XValueType = ChartValueType.Int32;
            series.YValueType = ChartValueType.Double;
            chart1.Series.Add(series);
        }

        bool isStartPlot = false;
        int dataCount = 0;

        Random random = new Random();
        private void btnStart_Click(object sender, EventArgs e)
        {
            // chart1.Series["TEMP_STACK_BOILER_1"].Points.AddXY(1,2);
            /* Draw 100 point */

            for (int i = 0; i < 1000; i++)
            {
                //chart1.Series["TEMP_STACK_BOILER_1"].Points.Add(i, random.Next(10, 100));
                chart1.Series["TEMP_STACK_BOILER_1"].Points.AddXY(i, random.Next(10, 100));
            }
            chart1.Series.Invalidate();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isStartPlot = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            chart1.Series["TEMP_STACK_BOILER_1"].Points.Clear();
        }
    }
}
