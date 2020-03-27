using System.Collections.Generic;
using System.Drawing;

namespace new_chart_delections.gui_new
{
    public class GraphInfo
    {
        /// <summary>
        /// LineChart Title
        /// </summary>
        public string Title { get; set; }
        public string UUID { get; set; }

        /// <summary>
        /// Update period in Hz
        /// </summary>
        public int Period { get; set; } = 100; // hz default value
        public List<LineInfo> LineInfos { get; set; } = new List<LineInfo>();

        /// <summary>
        /// Number of sample data collect
        /// </summary>
        public int Sample { get; set; }
    }

    public class LineInfo
    {
        /// <summary>
        /// Line data title name show on graph. it maybe hide by graph setting.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Line color show on graph
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Variable name, receive from serialport.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data address store on MemoryManage.
        /// </summary>
        public int Address { get; set; }
    }
}
