using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common.Cartography.Rendering;

public static class RenderStrategyContext
{
    public static RenderStrategy Create(VectorLayer layer)
    {
        if (layer == null)
            throw new ArgumentNullException(nameof(layer));

        return layer.ToRasterTechnique switch
        {
            RasterizationApproach.DrawingVisual => new DrawingVisualRenderStrategy(layer.Symbolizers),
            RasterizationApproach.WriteableBitmap => new WriteableBitmapRenderStrategy(layer.Symbolizers),
            RasterizationApproach.GdiPlus => new GdiBitmapRenderStrategy(layer.Symbolizers),

            RasterizationApproach.StreamGeometry or
            RasterizationApproach.None or
            _ => throw new NotImplementedException(),
        };
    }
}
