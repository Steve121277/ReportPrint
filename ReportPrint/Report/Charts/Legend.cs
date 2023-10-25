using System;
using System.Drawing;

namespace ReportPrint.Report.Charts
{
    internal class Legend
    {
        public bool IsLegendVisible { get; set; } = false;
        public Color TextColor { get; set; } = Color.Black;
        public Font LegendFont { get; set; } = new Font("", 8, FontStyle.Regular);
        float LegendMarkWidth { get; set; } = 60f;
        float GapBetweenItems { get; set; } = 15f;
        float GapsBetweenLineAndText { get; set; } = 4f;

        public void AddLegend(Graphics g, DataCollection dc, LineChart chart)
        {
            if (dc.DataSeriesList.Count == 0)
            {
                return;
            }
            if (!IsLegendVisible)
            {
                return;
            }

            float totalWidth = 0;
            float legendHeight = 0;

            foreach (DataSeries ds in dc.DataSeriesList)
            {
                if (totalWidth > 0)
                    totalWidth += GapBetweenItems;

                totalWidth += LegendMarkWidth;

                SizeF size = g.MeasureString(ds.SeriesName, LegendFont);

                totalWidth += size.Width;
                legendHeight = Math.Max(legendHeight, size.Height);
            }

            DrawLegend(g,
                chart.ChartArea.Left + chart.ChartArea.Width / 2 - totalWidth / 2, chart.PlotArea.Bottom + 60,
                totalWidth, legendHeight, dc, chart);
        }

        void DrawLegend(Graphics g, float x, float y, float width, float height, DataCollection dc, LineChart chart)
        {
            Brush br = new SolidBrush(TextColor);

            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near
            };

            float yLine = y + height / 2;

            foreach (DataSeries ds in dc.DataSeriesList)
            {
                Pen aPen = new Pen(ds.LineStyle.LineColor, ds.LineStyle.LineThickness);

                aPen.DashStyle = ds.LineStyle.LinePattern;

                g.DrawLine(aPen, x, yLine, x + LegendMarkWidth, yLine);
                ds.Symbol.DrawSymbol(g, new PointF(x + LegendMarkWidth / 2, yLine));

                x += LegendMarkWidth + GapsBetweenLineAndText;

                SizeF size = g.MeasureString(ds.SeriesName, LegendFont);
                g.DrawString(ds.SeriesName, LegendFont, br, x, y);

                x += size.Width;
                x += GapBetweenItems;
            }
        }
    }
}
