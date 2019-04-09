using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Jab.Common;
using Microsoft.SqlServer.Types;
using System.Windows;
using IRI.Msh.Common.Primitives;
using System.Collections.ObjectModel;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.TileServices;

namespace IRI.Jab.MapViewer
{
    public class LayerManager : Notifier
    {
        List<ILayer> allLayers;

        private ObservableCollection<ILayer> _currentLayers;

        public ObservableCollection<ILayer> CurrentLayers
        {
            get { return _currentLayers; }
            set
            {
                _currentLayers = value;
                RaisePropertyChanged();
            }
        }

        public LayerManager()
        {
            this.allLayers = new List<ILayer>();

            this.CurrentLayers = new ObservableCollection<ILayer>();
        }

        public void Add(ILayer layer)
        {
            layer.OnVisibilityChanged -= RefreshLayerVisibility;

            layer.OnVisibilityChanged += RefreshLayerVisibility;

            this.CurrentLayers.Add(layer);

            //98.01.20
            //layer.ZIndex = this.allLayers.Count(i => !i.Type.HasFlag(LayerType.Complex) && !i.Type.HasFlag(LayerType.Drawing));

            this.allLayers.Add(layer);

            this.allLayers = this.allLayers.OrderBy(i => i.Type == LayerType.Complex).ToList();

            if (layer.IsGroupLayer)
            {
                foreach (var item in layer.SubLayers)
                {
                    Add(item);
                }
            }
        }

        public void Remove(ILayer layer, bool forceRemove)
        {
            Clear(lyr => lyr == layer, forceRemove);
            //if (forceRemove || layer.CanUserDelete)
            //{
            //    this.allLayers.Remove(layer);

            //    this.CurrentLayers.Remove(layer);
            //}
        }

        public void Remove(string layerName, bool forceRemove)
        {
            Clear(layer => layer?.LayerName == layerName, forceRemove);

            //this.allLayers.RemoveAll(i => (forceRemove || i.CanUserDelete) && i.LayerName == layerName);

            //for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            //{
            //    if ( CurrentLayers[i].LayerName == layerName)
            //    {
            //        this.CurrentLayers.Remove(CurrentLayers[i]);
            //    }
            //}
        }

        internal void Remove(LayerType type, bool forceRemove)
        {
            Clear(layer => layer.Type.HasFlag(type), forceRemove);

            //lock (allLayers)
            //{
            //    lock (CurrentLayers)
            //    {
            //        this.allLayers.RemoveAll(i => (forceRemove || i.CanUserDelete) && i.Type.HasFlag(type));

            //        for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            //        {
            //            if (CurrentLayers[i].Type.HasFlag(type))
            //            {
            //                this.CurrentLayers.Remove(CurrentLayers[i]);
            //            }
            //        }
            //    }
            //}
        }

        public void Remove(Predicate<ILayer> rule, bool forceRemove)
        {
            Clear(rule, forceRemove);

            //this.allLayers.RemoveAll(rule);


            //for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            //{
            //    if (rule(CurrentLayers[i]))
            //    {
            //        this.CurrentLayers.Remove(CurrentLayers[i]);
            //    }
            //}
        }

        public void RemoveTile(string providerFullName, /*TileType type, */bool forceRemove)
        {
            //for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            //{
            //    if (CurrentLayers[i] is TileServiceLayer)
            //    {
            //        var layer = CurrentLayers[i] as TileServiceLayer;

            //        if (layer.Provider == provider && layer.TileType == type)
            //        {
            //            this.CurrentLayers.Remove(layer);

            //            //map.Remove(layer);
            //        }
            //    }
            //}

            Clear(layer => (layer as TileServiceLayer)?.ProviderFullName?.ToUpper() == providerFullName?.ToUpper() /*&& (layer as TileServiceLayer)?.TileType == type*/, forceRemove);
        }


        private void Clear(Predicate<ILayer> rule, bool forceRemove)
        {
            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                if ((forceRemove || CurrentLayers[i]?.CanUserDelete == true) && rule(CurrentLayers[i]))
                {
                    this.CurrentLayers.Remove(CurrentLayers[i]);
                }
            }

            this.allLayers.RemoveAll(layer => (forceRemove || layer?.CanUserDelete == true) && rule(layer));
        }
         

        public void Clear()
        {
            this.allLayers.Clear();

            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                this.CurrentLayers.Remove(CurrentLayers[i]);
            }
        }

        public IEnumerable<ILayer> UpdateAndGetLayers(double inverseMapScale, RenderingApproach rendering)
        {
            //var newLayers = map.Where((var) => (var.VisibleRange.Upper >= inverseMapScale && var.VisibleRange.Lower < inverseMapScale)).OrderBy(i => i.Type != LayerType.BaseMap);
            if (rendering == RenderingApproach.Default)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateAndGetLayers:{  inverseMapScale}");
                System.Diagnostics.Debug.WriteLine($"map count:{allLayers.Count}");
            }


            var newLayers = allLayers.Where(l => l.VisibleRange.IsInRange(inverseMapScale) && l.Rendering == rendering).OrderBy(i => i.Type != LayerType.BaseMap).ThenBy(i => i.ZIndex);

            if (rendering == RenderingApproach.Default)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateAndGetLayers layercounts:{  newLayers.Count()}");
            }

            var toBeRemovedLayers = this.CurrentLayers.Where(i => i.Rendering == rendering && newLayers.All(l => l.LayerId != i.LayerId)).ToList();

            for (int i = 0; i < toBeRemovedLayers.Count; i++)
            {
                this.CurrentLayers.Remove(toBeRemovedLayers[i]);
            }

            var toBeAdded = newLayers.Where(i => i.Rendering == rendering && this.CurrentLayers.All(l => l.LayerId != i.LayerId)).ToList();

            for (int i = 0; i < toBeAdded.Count; i++)
            {
                this.CurrentLayers.Add(toBeAdded[i]);
            }

            return newLayers;
        }

        internal void UpdateIsInRange(double inverseMapScale)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateIsInRange:{  inverseMapScale}");

            foreach (var layer in allLayers)
            {
                layer.VisualParameters.IsInScaleRange = layer.VisibleRange.IsInRange(inverseMapScale);

                if (layer.Labels != null)
                {
                    System.Diagnostics.Debug.WriteLine($"scale:{  inverseMapScale}");

                    layer.Labels.IsInScaleRange = layer.Labels.VisibleRange.IsInRange(inverseMapScale);
                }

            }

        }

        public BoundingBox CalculateCurrentMapExtent()
        {
            return CalculateExtent(this.CurrentLayers);
        }

        public BoundingBox CalculateMapExtent()
        {
            return CalculateExtent(this.allLayers);
        }

        private BoundingBox CalculateExtent(IList<ILayer> layers)
        {
            if (layers == null || layers.Count < 1)
            {
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            var extents = layers.Where(i => !i.Type.HasFlag(LayerType.BaseMap) && !i.Type.HasFlag(LayerType.ImagePyramid))
                                .Select(i => i.Extent)
                                .Where(i => !double.IsNaN(i.Width) && !double.IsNaN(i.Height));

            return BoundingBox.GetMergedBoundingBox(extents);
        }

        private void RefreshLayerVisibility(object sender, EventArgs e)
        {
            this.RequestRefreshVisibility?.Invoke(sender as BaseLayer);
        }

        public Action<BaseLayer> RequestRefreshVisibility;

    }


}
