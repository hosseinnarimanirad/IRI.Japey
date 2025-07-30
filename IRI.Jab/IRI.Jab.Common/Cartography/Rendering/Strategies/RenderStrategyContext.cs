using IRI.Jab.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Cartography.Rendering;

public class RenderStrategyContext
{
    public RenderStrategy Create(VectorLayer layer)
    {
        if (layer == null)
            throw new ArgumentNullException(nameof(layer));

        return layer.ToRasterTechnique switch
        {
            RasterizationApproach.DrawingVisual => new DrawingVisualRenderStrategy(),
            RasterizationApproach.WriteableBitmap => new WriteableBitmapRenderStrategy(),
            RasterizationApproach.GdiPlus => new GdiBitmapRenderStrategy(),

            RasterizationApproach.StreamGeometry or
            RasterizationApproach.None or
            _ => throw new NotImplementedException(),
        };
    }
}
