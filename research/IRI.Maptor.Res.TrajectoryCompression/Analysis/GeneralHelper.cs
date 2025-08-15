﻿using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

using IRI.Maptor.Jab.Common;
using IRI.Maptor.Jab.Common.Models;
using IRI.Maptor.Jab.Common.Enums;
using IRI.Maptor.Sta.Spatial.Helpers;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives; 


namespace IRI.Maptor.Res.TrajectoryCompression;

public static class GeneralHelper
{
    public static async Task<(Bitmap image, double percent)> CreateImages(
        BoundingBox groundBoundingBox,
        int level,
        Bitmap originalBitmap,
        VectorLayer vectorLayer,
        string outputDirectory,
        string fileName,
        string simplificationTypeMethod,
        double coef = 1,
        bool saveImages = true)
    {
        var currentScreenSize = WebMercatorUtility.ToScreenSize(level, groundBoundingBox);

        var scale = WebMercatorUtility.GetGoogleMapScale(level);

        var bitmap = await vectorLayer.AsGdiBitmapAsync(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

        var diff = IRI.Maptor.Ket.GdiPlus.Helpers.ImageHelper.CalculateDifPlusBitmaps(originalBitmap, bitmap, true);

        if (saveImages)
        {
            bitmap.Save($"{outputDirectory}\\{fileName}-{level}-{coef:N2}-{simplificationTypeMethod}.png", System.Drawing.Imaging.ImageFormat.Tiff);
            diff.image.Save($"{outputDirectory}\\{fileName}-{level}-{coef:N2}-{simplificationTypeMethod}-diff.png", System.Drawing.Imaging.ImageFormat.Tiff);
        }

        return diff;
    }
      


    public static VectorLayer GetAsLayer(string layerName, List<Geometry<IRI.Maptor.Sta.Common.Primitives.Point>> geometries)
    {
        var vectorLayer = new VectorLayer(layerName,
                                            geometries,
                                            VisualParameters.GetStroke(Colors.Blue, 1),
                                            LayerType.VectorLayer,
                                            RenderingApproach.Default,
                                            RasterizationApproach.DrawingVisual);

        vectorLayer.VisualParameters.Visibility = Visibility.Hidden;

        //this.Presenter.AddLayer(vectorLayer);

        return vectorLayer;
    }

}
