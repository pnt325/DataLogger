using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace new_chart_delections
{
    static class Program
    {
        public static Network.Uart Uart = new Network.Uart();
        public static string FileName = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Get input file.
            if(args.Length > 0)
            {
                FileName = args[0];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Layout.FrmMain());
        }
    }
}


