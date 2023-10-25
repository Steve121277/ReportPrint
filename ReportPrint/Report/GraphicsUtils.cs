using System.Drawing;
using System.Drawing.Drawing2D;

namespace ReportPrint.Report
{
    internal static class GraphicsUtils
    {
        public static void DrawHalfRoundedRectangle(this Graphics graphics, int x, int y, int width, int height, int radius, Brush brush)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = 2 * radius;
            Rectangle arcRect = new Rectangle(x, y, diameter, diameter);

            // Top left arc
            path.AddArc(arcRect, 180, 90);
            path.AddLine(x + radius, y, x + width - radius, y);
            arcRect.X += width - diameter;

            // Top right arc
            path.AddArc(arcRect, 270, 90);
            path.AddLine(x + width, y + radius, x + width, y + height);

            path.AddLine(x + width, y + height, x, y + height);
            path.AddLine(x, y + height, x, y + radius);


            // Close the path
            path.CloseFigure();

            // Fill the rounded rectangle path
            graphics.FillPath(brush, path);
        }

        public static void FillRoundRect(this Graphics graphics, Brush brush, int x, int y, int width, int height, int xThick, int yThick, int outerCornerRadius, int innerConnerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            FillRoundRect(path, x, y, width, height, outerCornerRadius);
            FillRoundRect(path, x + xThick, y + yThick, width - 2 * xThick, height - 2 * yThick, innerConnerRadius);

            // Close the path
            path.CloseFigure();

            // Fill the rounded rectangle path
            graphics.FillPath(brush, path);
        }

        private static void FillRoundRect(this GraphicsPath path, int x, int y, int width, int height, int cornerRadius)
        {
            int diameter = 2 * cornerRadius;
            Rectangle arcRect = new Rectangle(x, y, diameter, diameter);

            // Top left arc
            path.AddArc(arcRect, 180, 90);
            path.AddLine(x + cornerRadius, y, x + width - cornerRadius, y);
            arcRect.X += width - diameter;

            // Top right arc
            path.AddArc(arcRect, 270, 90);
            path.AddLine(x + width, y + cornerRadius, x + width, y + height - cornerRadius);
            arcRect.Y += height - diameter;

            // Bottom right arc
            path.AddArc(arcRect, 0, 90);
            arcRect.X -= width - diameter;
            path.AddLine(x + width - cornerRadius, y + height, x + cornerRadius, y + height);

            // Bottom left arc
            path.AddArc(arcRect, 90, 90);
            path.AddLine(x, y + height - cornerRadius, x, y + cornerRadius);
        }
    }
}
