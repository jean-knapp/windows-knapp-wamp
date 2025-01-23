using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace aetherion_server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set DPI awareness for monitor scaling
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            WindowsFormsSettings.AllowDpiScale = true;
            WindowsFormsSettings.AllowRoundedWindowCorners = DevExpress.Utils.DefaultBoolean.True; 

            Application.Run(new MainForm());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
