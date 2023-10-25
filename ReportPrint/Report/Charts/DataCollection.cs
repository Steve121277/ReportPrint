﻿using System.Collections.Generic;
using System.Drawing;

namespace ReportPrint.Report.Charts
{
    internal class DataCollection
    {
        public List<DataSeries> DataSeriesList { get; set; } = new List<DataSeries>();
        public int DataSeriesIndex { get; set; }

        public int Add(DataSeries ds)
        {
            this.DataSeriesList.Add(ds);

            return this.DataSeriesList.Count;
        }

        public void Insert(int DataSeriesIndex, DataSeries ds)
        {
            this.DataSeriesList.Insert(DataSeriesIndex, ds);
        }

        public void Remove(int DataSeriesIndex)
        {
            this.DataSeriesList.RemoveAt(DataSeriesIndex);
        }

        public void RemoveAll()
        {
            this.DataSeriesList.Clear();
        }

        public void AddLines(Graphics g, LineChart cs)
        {
            foreach (DataSeries ds in this.DataSeriesList)
            {

                if (ds.LineStyle.IsVisible)
                {
                    Pen aPen = new Pen(ds.LineStyle.LineColor, ds.LineStyle.LineThickness);

                    aPen.DashStyle = ds.LineStyle.LinePattern;

                    for (int i = 1; i < ds.PointList.Count; i++)
                    {
                        g.DrawLine(aPen, cs.Point2D(ds.PointList[i - 1]), cs.Point2D(ds.PointList[i]));
                    }

                    aPen.Dispose();
                }
            }

            //Plot Symbols
            foreach (DataSeries ds in this.DataSeriesList)
            {
                for (int i = 0; i < ds.PointList.Count; i++)
                {
                    PointF pt = ds.PointList[i];

                    if (pt.X >= cs.XLimitMin && pt.X <= cs.XLimitMax &&
                        pt.Y >= cs.YLimitMin && pt.Y <= cs.YLimitMax)
                    {
                        ds.Symbol.DrawSymbol(g, cs.Point2D(pt));
                    }
                }
            }
        }
    }
}
