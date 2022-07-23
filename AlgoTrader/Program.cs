using System;
using System.Windows.Forms;

namespace AlgoTrader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                //Eat any last Exceptions
            }
        }
    }
}
