using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace new_chart_delections
{
    static class Program
    {
        public static gui_new.Grid Grid;
        public static gui_new.ComponentManage ComponentManage;
        public static gui_new.MemoryManage MemoryManage;
        public static gui_new.Uart Uart;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmMain());
            Application.Run(new gui_new.FrmMain());
        }
    }
}
