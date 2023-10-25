using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReportPrint.Model
{
    internal class ExcelReader
    {
        public static bool CheckExcelInstalled()
        {
            const string ExcelAppPath = "Excel.Application";

            using (RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ExcelAppPath))
            {
                if (rk != null && rk.GetValue("") != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<User> GetUsersFromExcel()
        {
            List<User> users = new List<User>();

            if (!File.Exists(Config.ExcelUserFilePath))
            {
                return users;
            }

            FileInfo file = new FileInfo(Config.ExcelUserFilePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Get the first worksheet from the Excel package
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Iterate over the rows in the worksheet
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    if (!int.TryParse(worksheet.Cells[row, 1].Value?.ToString(), out int id))
                        continue;

                    User user = new User()
                    {
                        ID = id,
                        Name = (string)worksheet.Cells[row, 2].Value,
                        Birth = (DateTime)worksheet.Cells[row, 3].Value,
                        Sex = (string)worksheet.Cells[row, 4].Value
                    };

                    users.Add(user);
                    //// Access individual cells using the Cells property
                    //string cellValue = worksheet.Cells[row, 1].Value?.ToString();
                    //Console.WriteLine("Cell Value: " + cellValue);
                }
            }

            //xlWorkBook = xlApp.Workbooks.Open(Config.ExcelUserFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //range = xlWorkSheet.UsedRange;

            //rows = range.Rows.Count;

            //for (rowIndex = 2; rowIndex <= rows; rowIndex++)
            //{
            //    if (!int.TryParse((range.Cells[rowIndex, 1] as Excel.Range).Value2.ToString(), out int id))
            //        continue;

            //    User user = new User()
            //    {
            //        ID = id,
            //        Name = (string)(range.Cells[rowIndex, 2] as Excel.Range).Value2,
            //        Birth = ((DateTime)(range.Cells[rowIndex, 3] as Excel.Range).Value),
            //        Sex = (string)(range.Cells[rowIndex, 4] as Excel.Range).Value2
            //    };

            //    users.Add(user);
            //}

            //xlWorkBook.Close(true, null, null);
            //xlApp.Quit();
            //Marshal.ReleaseComObject(xlWorkSheet);
            //Marshal.ReleaseComObject(xlWorkBook);
            //Marshal.ReleaseComObject(xlApp);

            return users;
        }

    }
}
