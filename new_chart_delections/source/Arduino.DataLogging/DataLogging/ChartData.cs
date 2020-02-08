using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogging
{
    public static class ChartData
    {
        public static List<string> Names = new List<string>();
        public static List<Color> ChartColor = new List<Color>();
        public static List<string> Unit = new List<string>();
        public static List<float> Values = new List<float>();
        public static int ScanRate = 10; // in Hz

    }
    public enum ChartType
    {
        Line,
        Column,
        Table
    }
}
