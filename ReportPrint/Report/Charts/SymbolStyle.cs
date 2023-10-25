﻿using System.Drawing;

namespace ReportPrint.Report.Charts
{
    internal enum SymbolTypeEnum : int
    {
        None = 0,
        OpenTriangle = 1,
        Circle = 2,
        Triangle = 3,
    }

    internal class SymbolStyle
    {
        public SymbolTypeEnum SymbolType { get; set; } = SymbolTypeEnum.Triangle;
        public float SymbolSize { get; set; } = 20.0f;
        public float SymbolInnerCircleRudias { get; set; } = 4.0f;
        public Color BorderColor { get; set; } = Color.Black;
        public Color FillColor { get; set; } = Color.Black;
        public float BorderThickness { get; set; } = 1.0f;

        public void DrawSymbol(Graphics g, PointF pt)
        {
            Pen aPen = new Pen(this.BorderColor, BorderThickness);
            SolidBrush aBrush = new SolidBrush(this.FillColor);
            SolidBrush circleBrush = new SolidBrush(Color.White);

            float x = pt.X;
            float y = pt.Y;
            float size = SymbolSize;
            float halfSize = size / 2.0f;
            RectangleF aRectangle = new RectangleF(x - halfSize, y - halfSize, size, size);

            switch (SymbolType)
            {
                case SymbolTypeEnum.OpenTriangle:
                    g.DrawLine(aPen, x, y - halfSize, x + halfSize, y + halfSize);
                    g.DrawLine(aPen, x + halfSize, y + halfSize, x - halfSize, y + halfSize);
                    g.DrawLine(aPen, x - halfSize, y + halfSize, x, y - halfSize);
                    break;
                case SymbolTypeEnum.Circle:
                    g.FillEllipse(aBrush, x - halfSize, y - halfSize, size, size);
                    g.DrawEllipse(aPen, x - halfSize, y - halfSize, size, size);
                    break;
                case SymbolTypeEnum.Triangle:
                    PointF[] ptc = new PointF[3];
                    ptc[0].X = x;
                    ptc[0].Y = y - halfSize;
                    ptc[1].X = x + halfSize;
                    ptc[1].Y = y + halfSize;
                    ptc[2].X = x - halfSize;
                    ptc[2].Y = y + halfSize;
                    g.FillPolygon(aBrush, ptc);
                    g.DrawPolygon(aPen, ptc);
                    break;
            }

            g.FillEllipse(circleBrush, x - SymbolInnerCircleRudias, y - SymbolInnerCircleRudias, SymbolInnerCircleRudias * 2, SymbolInnerCircleRudias * 2);
        }

    }
}