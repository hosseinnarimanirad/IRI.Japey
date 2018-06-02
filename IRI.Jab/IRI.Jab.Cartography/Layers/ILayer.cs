using IRI.Msh.Common.Primitives;
using IRI.Jab.Cartography.Model;
using IRI.Jab.Common;
using IRI.Ket.DataManagement;

using IRI.Jab.Common.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace IRI.Jab.Cartography
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

        bool IsValid { get; set; }

        void Invalidate();

        event EventHandler<CustomEventArgs<Visibility>> OnVisibilityChanged; 
        //event EventHandler<CustomEventArgs<bool>> OnVisibilityChanged;
    }
}
