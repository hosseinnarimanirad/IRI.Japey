using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model;
using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;

namespace IRI.Jab.Common
{
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
}
