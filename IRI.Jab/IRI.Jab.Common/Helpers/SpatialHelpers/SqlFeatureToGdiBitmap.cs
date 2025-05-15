using System;
using System.Collections.Generic;
using drawing = System.Drawing;
using IRI.Sta.Spatial.Primitives;

using WpfPoint = System.Windows.Point;
using Point = IRI.Sta.Common.Primitives.Point;

namespace IRI.Jab.Common.Convertor;

public static class SqlFeatureToGdiBitmap
{
    internal static drawing.Bitmap ParseSqlGeometry(
       List<Feature<Point>> features,
       double width,
       double height,
       Func<WpfPoint, WpfPoint> mapToScreen,
       Func<Feature<Point>, VisualParameters> symbologyRule)
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
