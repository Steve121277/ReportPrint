using ReportPrint.Model.Statistics;
using ReportPrinting;
using System.Drawing;

namespace ReportPrint.Report
{
    internal class PrintReportIItem : IReportMaker
    {
        StatisticItem s_item;

        public PrintReportIItem(StatisticItem s_item)
        {
            this.s_item = s_item;
        }

        public void MakeDocument(ReportDocument reportDocument)
        {
            reportDocument.DocumentUnit = GraphicsUnit.Document;

            ReportBuilder builder = new ReportBuilder(reportDocument);

            builder.StartLinearLayout(Direction.Vertical);

            builder.AddPrintReportSection(s_item);

            builder.FinishLinearLayout();
        }
    }
}
