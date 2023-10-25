using OfficeOpenXml;
using System;
using System.Windows.Forms;

namespace ReportPrint
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
            Application.SetCompatibleTextRenderingDefault(false);

            //EPPPlusライセンス設定
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Application.Run(new MainForm());
        }
    }
}
