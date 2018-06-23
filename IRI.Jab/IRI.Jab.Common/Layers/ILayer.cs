using IRI.Msh.Common.Primitives;
using IRI.Jab.Common.Model;

using System;
using System.Windows;
using System.Collections.Generic;
using IRI.Jab.Common.Model.Legend;

namespace IRI.Jab.Common
{
    public interface ILayer
    {
        Guid Id { get; }

        string LayerName { get; set; }

        LayerType Type { get; }

        BoundingBox Extent { get; }

        int ZIndex { get; set; }

        ScaleInterval VisibleRange { get; set; }

        RenderingApproach Rendering { get; }

        RasterizationApproach ToRasterTechnique { get; }

        VisualParameters VisualParameters { get; set; }

        LabelParameters Labels { get; set; }

        bool IsValid { get; set; }

        void Invalidate();

        bool IsSelectedInToc { get; set; }
         
        bool ShowInToc { get; set; }
         
        bool CanUserDelete { get; }

        List<ILegendCommand> Commands { get; set; }

        event EventHandler<CustomEventArgs<Visibility>> OnVisibilityChanged;

        event EventHandler<CustomEventArgs<LabelParameters>> OnLabelChanged;
    }
}
