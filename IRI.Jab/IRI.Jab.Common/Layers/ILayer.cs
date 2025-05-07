using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model;

using System;
using System.Windows;
using System.Collections.Generic;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Assets.Commands;
using System.Collections.ObjectModel;

namespace IRI.Jab.Common
{
    public interface ILayer
    {
        Guid LayerId { get; }

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

        bool IsExpandedInToc { get; set; }

        bool IsGroupLayer { get; set; }

        // is layer discoverable in identify
        bool IsSearchable { get; set; }  

        Guid ParentLayerId { get; set; }

        bool ShowInToc { get; set; }

        bool CanUserDelete { get; }

        int NumberOfSelectedFeatures { get; set; }

        List<ILegendCommand> Commands { get; set; }

        ObservableCollection<ILayer> SubLayers { get; set; }

        void SetVisibility(Visibility visibility);

        List<IFeatureTableCommand> FeatureTableCommands { get; set; }

        RelayCommand ChangeSymbologyCommand { get;   }

        event EventHandler<CustomEventArgs<Visibility>> OnVisibilityChanged;

        event EventHandler<CustomEventArgs<LabelParameters>> OnLabelChanged;

        bool CanRenderLayer(double mapScale);

        bool CanRenderLabels(double mapScale);
    }
}
