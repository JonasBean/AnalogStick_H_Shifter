using System;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ShowError("Application Error", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex) ShowError("Critical Error", ex);
            };

            try
            {
                Application.Run(new H_Shifter());
            }
            catch (Exception ex)
            {
                ShowError("Application Startup Error", ex);
            }
        }

        static void ShowError(string title, Exception ex)
        {
            MessageBox.Show($"{ex.Message}\n\nDetails:\n{ex}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
