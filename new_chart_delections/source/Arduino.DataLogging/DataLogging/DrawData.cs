using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogging
{
    public static class DrawData
    {
        public static int xStart { get; set; }
        public static int xEnd { get; set; }
        public static int yStart { get; set; }
        public static int yEnd { get; set; }
        public static int xGrid { get; set; }
        public static int yGrid { get; set; }

        public static int xStep { get; set; }
        public static int yStep { get; set; }
        public static int xEndOld { get; set; }
        public static int yEndOld { get; set; }
    }
}
