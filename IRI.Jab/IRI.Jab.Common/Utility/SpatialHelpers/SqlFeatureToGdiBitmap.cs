using System;
using System.Collections.Generic;
using drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using sb = IRI.Msh.Common.Primitives;

namespace IRI.Jab.Common.Convertor
{
    public static class SqlFeatureToGdiBitmap
    {

        //internal static drawing.Bitmap ParseSqlGeometry(
        //    List<SqlFeature> features,
        //    double width,
        //    double height,
        //    Func<Point, Point> mapToScreen,
        //    Func<SqlFeature, VisualParameters> symbologyRule)
        //{
        //    var result = new drawing.Bitmap((int)width, (int)height);

        //    drawing.Graphics graphics = drawing.Graphics.FromImage(result);

        //    int p = 0;

        //    if (features != null)
        //    {
        //        foreach (SqlFeature item in features)
        //        {
        //            var symbology = symbologyRule(item);

        //            var pen = symbology.GetGdiPlusPen(symbology.Opacity);

        //            var brush = symbology.GetGdiPlusFillBrush(symbology.Opacity);

        //            SqlSpatialToGdiBitmap.WriteToImage(graphics, item.TheSqlGeometry, mapToScreen, pen, brush, symbology.PointSymbol);
        //        }
        //    }

        //    return result;
        //}

        internal static drawing.Bitmap ParseSqlGeometry(
           List<sb.Feature<sb.Point>> features,
           double width,
           double height,
           Func<Point, Point> mapToScreen,
           Func<sb.Feature<sb.Point>, VisualParameters> symbologyRule)
        {
            var result = new drawing.Bitmap((int)width, (int)height);

            drawing.Graphics graphics = drawing.Graphics.FromImage(result);

            int p = 0;

            if (features != null)
            {
                foreach (var item in features)
                {
                    var symbology = symbologyRule(item);

                    var pen = symbology.GetGdiPlusPen(symbology.Opacity);

                    var brush = symbology.GetGdiPlusFillBrush(symbology.Opacity);

                    SqlSpatialToGdiBitmap.WriteToImage(graphics, item.TheGeometry, mapToScreen, pen, brush, symbology.PointSymbol);
                }
            }

            return result;
        }
    }
}
