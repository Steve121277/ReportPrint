using ReportPrint.Model.Statistics;
using ReportPrinting;

namespace ReportPrint.Report
{
    internal static class ReportManager
    {
        internal static void AddPrintReportSection(this ReportBuilder reportBuilder, StatisticItem s_item)
        {
            PrintReportSection section = new PrintReportSection(s_item);

            section.HorizontalAlignment = HorizontalAlignment.Center;

            reportBuilder.AddSection(section);
        }
    }
}
