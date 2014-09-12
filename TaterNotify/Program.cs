using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaterNotify
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                MessageBox.Show("TaterNotify is already running.", "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ApplicationContext appContext = new CustomApplicationContext();
            Application.Run(appContext);
        }
    }
}
