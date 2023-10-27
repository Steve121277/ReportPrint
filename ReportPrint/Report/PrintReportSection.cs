using ReportPrint.Model;
using ReportPrint.Model.Statistics;
using ReportPrint.Report.Charts;
using ReportPrinting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace ReportPrint.Report
{
    internal class PrintReportSection : ReportSection
    {
        StatisticItem s_item;

        const int xLeftMargin = 105;
        const int xWidth = 2252;

        LineChart chart1 = new LineChart() { Caption = Config.TtileOfAll_ashiage, YLabel = $"({Config.TtileOfAll_ashiage_unit})" };
        Legend legend1 = new Legend();
        DataCollection dc1 = new DataCollection();
        LineChart chart2 = new LineChart() { Caption = Config.TtileOfAll_ssfive, YLabel = $"({Config.TtileOfAll_ssfive_unit})" };
        DataCollection dc2 = new DataCollection();
        LineChart chart3 = new LineChart() { Caption = Config.TitleOfTUG, YLabel = $"({Config.TitleOfTUG_unit})" };
        DataCollection dc3 = new DataCollection();
        LineChart chart4 = new LineChart() { Caption = Config.TitleOfCarePitLog, YLabel = $"({Config.TitleOfCarePitLog_unit})" };
        DataCollection dc4 = new DataCollection();

        public PrintReportSection(StatisticItem s_item)
        {
            this.UseFullHeight = true;
            this.UseFullWidth = true;

            this.s_item = s_item;

            InitChat();
        }

        void InitChat()
        {
            chart1.ChartArea = new Rectangle(0, 0, 1120, 560);
            chart1.FirstMonth = s_item.FirstMonth;
            legend1.IsLegendVisible = true;
            chart1.YLimitMin = Config.ashiag_y_min;
            chart1.YLimitMax = Config.ashiag_y_max;
            chart1.YTick = Config.ashiag_y_intv;
            dc1.Add(
                new DataSeries()
                {
                    SeriesName = Config.TtileOfAll_ashiage_right,
                    PointList = GetPointsFromIndex((int)GameType.All_ashiage_right)
                }
            );
            dc1.Add(
                new DataSeries()
                {
                    SeriesName = Config.TtileOfAll_ashiage_left,
                    PointList = GetPointsFromIndex((int)GameType.All_ashiage_left)
                }
            );
            dc1.DataSeriesList[1].Symbol.SymbolType = SymbolTypeEnum.Circle;

            chart2.ChartArea = new Rectangle(xWidth - 1120, 0, 1120, 560);
            chart2.FirstMonth = s_item.FirstMonth;
            chart2.YLimitMin = Config.ssfive_y_min;
            chart2.YLimitMax = Config.ssfive_y_max;
            chart2.YTick = Config.ssfive_y_intv;
            dc2.Add(
                new DataSeries()
                {
                    PointList = GetPointsFromIndex((int)GameType.All_ssfive)
                });

            chart3.ChartArea = new Rectangle(0, 608, 1120, 560);
            chart3.FirstMonth = s_item.FirstMonth;
            chart3.YLimitMin = Config.tug_y_min;
            chart3.YLimitMax = Config.tug_y_max;
            chart3.YTick = Config.tug_y_intv;
            dc3.Add(
                new DataSeries()
                {
                    PointList = GetPointsFromIndex((int)GameType.TUG)
                });

            chart4.ChartArea = new Rectangle(xWidth - 1120, 608, 1120, 560);
            chart4.FirstMonth = s_item.FirstMonth;
            chart4.YLimitMin = Config.log_y_min;
            chart4.YLimitMax = Config.log_y_max;
            chart4.YTick = Config.log_y_intv;
            dc4.Add(
                new DataSeries()
                {
                    PointList = GetPointsFromIndex((int)GameType.CarePitLog)
                });
        }

        private List<PointF> GetPointsFromIndex(int index)
        {
            List<PointF> points = new List<PointF>();

            for (int i = 0; i < 12; i++)
            {
                if (Single.IsNaN(this.s_item.Values[index, i]))
                    continue;

                points.Add(new PointF(i + 1, this.s_item.Values[index, i]));
            }

            return points;
        }

        /// <summary>
        /// Setup for printing (do nothing)
        /// </summary>
        /// <param name="g">Graphics object</param>
        protected override void DoBeginPrint(Graphics g)
        {
        }

        protected override SectionSizeValues DoCalcSize(
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            SectionSizeValues retval = new SectionSizeValues();

            retval.Fits = true;

            return retval;
        }

        protected override void DoPrint(
            ReportDocument reportDocument,
            Graphics g,
            Bounds bounds
            )
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawSkelton(g);
        }

        private void DrawSkelton(Graphics g)
        {
            TransformGraphic(g, xLeftMargin, 74);
            DrawTitle(g);

            TransformGraphic(g, 0, 1 + 110);
            DrawInfo(g);

            TransformGraphic(g, 0, 40 + 285);
            DrawStatistics(g);

            TransformGraphic(g, 0, 48 + 642);
            DrawCharts(g);
            //charts region

            TransformGraphic(g, 0, 40 + 560 + 608);
            DrawNotes(g);
        }

        private void DrawTitle(Graphics g)
        {
            g.DrawHalfRoundedRectangle(0, 0, 700, 110, 23, new SolidBrush(Color.FromArgb(108, 108, 99)));

            Font fontTitle = new Font(Config.PrintFontName, 16, FontStyle.Regular);
            g.DrawString("身体機能測定結果", fontTitle, Brushes.White, new Point(70, 20));
        }

        private void DrawInfo(Graphics g)
        {
            Font font1 = new Font(Config.PrintFontName, 11, FontStyle.Bold);
            Font font2 = new Font(Config.PrintFontName, 11, FontStyle.Bold);
            Pen pen = new Pen(Color.Black, 3);
            CultureInfo japaneseCulture = new CultureInfo("ja-JP");
            japaneseCulture.DateTimeFormat.Calendar = new JapaneseCalendar();

            StringFormat stringFormatName = new StringFormat()
            {
                Alignment = StringAlignment.Far,
            };

            g.FillRoundRect(new SolidBrush(Color.FromArgb(197, 193, 183)), 0, 0, xWidth, 285, 25, 25, 23, 6);

            g.DrawString("ご利用者様氏名", font1, Brushes.Black, new Point(30, 30));
            g.DrawString($"(ID： {s_item.UserInfo.ID.ToString("D8")}  )", font2, Brushes.Black, new Point(430, 30));
            g.DrawString("様", font1, Brushes.Black, new Point(900, 190));
            g.DrawString(s_item.UserInfo.Name, font2, Brushes.Black, new Point(885, 190), stringFormatName);
            g.DrawLine(pen, 60, 250, 1010, 250);
            g.DrawString("生年月日", font1, Brushes.Black, new Point(1095, 30));
            g.DrawString(s_item.UserInfo.Birth.ToString("ggyy年M月d日", japaneseCulture), font2, Brushes.Black, new Point(1125, 85));
            g.DrawLine(pen, 1095, 150, 1800, 150);
            g.DrawString("年齢", font1, Brushes.Black, new Point(1850, 30));
            g.DrawString($"{s_item.UserInfo.Age}歳", font2, Brushes.Black, new Point(1860, 85));
            g.DrawLine(pen, 1850, 150, 2190, 150);

            g.DrawString("性別", font1, Brushes.Black, new Point(1095, 170));
            g.DrawString(s_item.UserInfo.Sex, font2, Brushes.Black, new Point(1260, 190));
            g.DrawLine(pen, 1095, 250, 1445, 250);
            g.DrawString("作成日", font1, Brushes.Black, new Point(1460, 170));
            g.DrawString(DateTime.Now.ToString("ggyy年M月d日", japaneseCulture), font2, Brushes.Black, new Point(1680, 190));
            g.DrawLine(pen, 1460, 250, 2190, 250);
        }

        private void DrawStatistics(Graphics g)
        {
            g.FillRoundRect(new SolidBrush(Color.FromArgb(152, 148, 140)), 0, 0, xWidth, 642, 10, 10, 32, 24);

            TransformGraphic(g, 136, 64);

            StaticsTable tbl = new StaticsTable(this.s_item);

            tbl.Draw(g);

            TransformGraphic(g, -136, -64);
        }

        private void DrawCharts(Graphics g)
        {
            chart1.AddChartStyle(g);
            dc1.AddLines(g, chart1);
            legend1.AddLegend(g, dc1, chart1);

            chart2.AddChartStyle(g);
            dc2.AddLines(g, chart2);

            chart3.AddChartStyle(g);
            dc3.AddLines(g, chart3);

            chart4.AddChartStyle(g);
            dc4.AddLines(g, chart4);
        }

        private void DrawNotes(Graphics g)
        {
            Font fontNotes = new Font(Config.PrintFontName, 12, FontStyle.Bold);

            g.FillRoundRect(new SolidBrush(Color.FromArgb(197, 193, 183)), 0, 0, xWidth, 826, 25, 25, 23, 6);

            g.DrawString("備考：", fontNotes, Brushes.Black, new Point(50, 60));
            g.DrawString(this.s_item.Notes, fontNotes, Brushes.Black, new Point(200, 60));
        }

        void TransformGraphic(Graphics g, int x, int y)
        {
            var transform = g.Transform;

            transform.Translate(x, y, MatrixOrder.Append);

            g.Transform = transform;
        }
    }
}
