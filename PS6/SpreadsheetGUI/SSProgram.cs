using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    static class SSProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            SSApplicationContext appContext = SSApplicationContext.getAppContext();
            appContext.RunForm(new SSGUI());
            Application.Run(appContext);
        }
    }
}
