using System;
using IRI.Maptor.Jab.Common.Enums;
using IRI.Maptor.Jab.Common.Model;
using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Maptor.Jab.Common;

public class GroupLayer : BaseLayer
{
    public override LayerType Type { get => LayerType.GroupLayer; protected set => throw new NotImplementedException(); }

    public override BoundingBox Extent { get => BoundingBox.NaN; protected set => throw new NotImplementedException(); }

    public override RenderingApproach Rendering { get => RenderingApproach.Default; protected set => throw new NotImplementedException(); }

    public GroupLayer(string title)
    {
        this.LayerName = title;

        this.IsGroupLayer = true;

        this.SubLayers = new System.Collections.ObjectModel.ObservableCollection<ILayer>();

        this.VisibleRange = ScaleInterval.All;

        this.ShowInToc = true;

        this.VisualParameters.Visibility = System.Windows.Visibility.Collapsed;
    }

    public void AddSubLayer(ILayer layer)
    {
        layer.ParentLayerId = this.LayerId;

        if (!this.SubLayers.Contains(layer))
        {
            this.SubLayers.Add(layer);
        }

    }

    public override string ToString()
    {
        return $"GROUP LAYER - {LayerName}";
    }
      
}
