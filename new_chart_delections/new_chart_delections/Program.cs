using System;
using System.Windows.Forms;

namespace new_chart_delections
{
    static class Program
    {
        public static Network.Uart Uart = new Network.Uart();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Layout.FrmMain());
        }
    }
}


