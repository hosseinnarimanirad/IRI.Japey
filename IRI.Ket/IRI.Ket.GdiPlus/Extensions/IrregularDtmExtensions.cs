using IRI.Ket.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DigitalTerrainModeling.Extensions;

public static class IrregularDtmExtensions
{
    public static System.Drawing.Bitmap DrawSlopeMap(this IrregularDtm dtm, int scale)
    {
        if (dtm.triangulation.triangles.Count < 1)
        {
            throw new NotImplementedException();
        }

        double minX = dtm.LowerLeft.X; double minY = dtm.LowerLeft.Y;

        int width = (int)(dtm.UpperRight.X - minX);

        int height = (int)(dtm.UpperRight.Y - minY);

        System.Drawing.Bitmap result = new System.Drawing.Bitmap(width / scale, height / scale);

        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

        foreach (QuasiTriangle item in dtm.triangulation.triangles)
        {
            Point first = dtm.collection.GetPoint(item.First);

            Point second = dtm.collection.GetPoint(item.Second);

            Point third = dtm.collection.GetPoint(item.Third);

            Triangle temp = new Triangle(first, second, third);

            double tempSlope = dtm.CalculateSlope(temp);

            //int color = (int)((tempSlope + Math.PI / 2) * 255 / (Math.PI));

            int color = (int)((Math.Abs(tempSlope) * 250 / (Math.PI / 2)));

            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color, color, color));

            g.FillPolygon(brush,
                new System.Drawing.PointF[] {
                    new System.Drawing.PointF((float)(first.X - minX), (float)(result.Height - (first.Y - minY))),
                    new System.Drawing.PointF((float)(second.X - minX), (float)(result.Height - (second.Y - minY))),
                    new System.Drawing.PointF((float)(third.X - minX), (float)(result.Height - (third.Y - minY)))});
        }

        return result;
    }

    public static System.Drawing.Bitmap DrawAspectMap(this IrregularDtm dtm, int scale)
    {
        if (dtm.triangulation.triangles.Count < 1)
        {
            throw new NotImplementedException();
        }

        double minX = dtm.LowerLeft.X; double minY = dtm.LowerLeft.Y;

        int width = (int)(dtm.UpperRight.X - minX);

        int height = (int)(dtm.UpperRight.Y - minY);

        System.Drawing.Bitmap result = new System.Drawing.Bitmap(width / scale, height / scale);

        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

        foreach (QuasiTriangle item in dtm.triangulation.triangles)
        {
            Point first = dtm.collection.GetPoint(item.First);

            Point second = dtm.collection.GetPoint(item.Second);

            Point third = dtm.collection.GetPoint(item.Third);

            Triangle temp = new Triangle(first, second, third);

            double tempSlope = dtm.CalculateAspect(temp);

            //int color = (int)((tempSlope + Math.PI / 2) * 255 / (Math.PI));

            int color = (int)(tempSlope * 255 / (2 * Math.PI));

            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color, color, color));

            g.FillPolygon(brush,
                new System.Drawing.PointF[] {
                    new System.Drawing.PointF((float)(first.X - minX), (float)(result.Height - (first.Y - minY))),
                    new System.Drawing.PointF((float)(second.X - minX), (float)(result.Height - (second.Y - minY))),
                    new System.Drawing.PointF((float)(third.X - minX), (float)(result.Height - (third.Y - minY)))});
        }

        return result;
    }
}
