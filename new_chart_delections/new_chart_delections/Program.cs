using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections
{
    static class Program
    {
        // System variable
        public static List<data.Objects> DataObjs = new List<data.Objects>();
        public static List<data.ControlObject> ControlObjs = new List<data.ControlObject>();
        public static GridLine.Grid FGrid = new GridLine.Grid();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
            //Application.Run(new gui.FrmTest());
        }
    }
}
