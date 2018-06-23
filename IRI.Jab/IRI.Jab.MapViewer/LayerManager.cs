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
        List<ILayer> map;

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
            this.map = new List<ILayer>();

            this.CurrentLayers = new ObservableCollection<ILayer>();
        }

        public void Add(ILayer layer)
        {
            layer.OnVisibilityChanged -= RefreshLayerVisibility;

            layer.OnVisibilityChanged += RefreshLayerVisibility;

            this.CurrentLayers.Add(layer);

            layer.ZIndex = this.map.Count(i => !i.Type.HasFlag(LayerType.Complex) && !i.Type.HasFlag(LayerType.Drawing));

            this.map.Add(layer);

            this.map = this.map.OrderBy(i => i.Type == LayerType.Complex).ToList();
        }

        public void Remove(ILayer layer)
        {
            this.map.Remove(layer);

            this.CurrentLayers.Remove(layer);
        }

        public void Remove(string layerName)
        {
            this.map.RemoveAll(i => i.LayerName == layerName);

            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                if (CurrentLayers[i].LayerName == layerName)
                {
                    this.CurrentLayers.Remove(CurrentLayers[i]);
                }
            }
        }

        internal void Remove(LayerType type)
        {
            lock (map)
            {
                lock (CurrentLayers)
                {
                    this.map.RemoveAll(i => i.Type.HasFlag(type));

                    for (int i = CurrentLayers.Count - 1; i >= 0; i--)
                    {
                        if (CurrentLayers[i].Type.HasFlag(type))
                        {
                            this.CurrentLayers.Remove(CurrentLayers[i]);
                        }
                    }
                }
            }
        }

        internal void Remove(Predicate<ILayer> rule)
        {
            this.map.RemoveAll(rule);

            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                if (rule(CurrentLayers[i]))
                {
                    this.CurrentLayers.Remove(CurrentLayers[i]);
                }
            }
        }

        public void Remove(MapProviderType provider, TileType type)
        {
            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                if (CurrentLayers[i] is TileServiceLayer)
                {
                    var layer = CurrentLayers[i] as TileServiceLayer;

                    if (layer.Provider == provider && layer.TileType == type)
                    {
                        this.CurrentLayers.Remove(layer);

                        map.Remove(layer);
                    }
                }
            }
        }

        public void Clear()
        {
            this.map.Clear();

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
                System.Diagnostics.Debug.WriteLine($"map count:{map.Count}");
            }


            var newLayers = map.Where(l => l.VisibleRange.IsInRange(inverseMapScale) && l.Rendering == rendering).OrderBy(i => i.Type != LayerType.BaseMap).ThenBy(i => i.ZIndex);

            if (rendering == RenderingApproach.Default)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateAndGetLayers layercounts:{  newLayers.Count()}");
            }

            var toBeRemovedLayers = this.CurrentLayers.Where(i => i.Rendering == rendering && newLayers.All(l => l.Id != i.Id)).ToList();

            for (int i = 0; i < toBeRemovedLayers.Count; i++)
            {
                this.CurrentLayers.Remove(toBeRemovedLayers[i]);
            }

            var toBeAdded = newLayers.Where(i => i.Rendering == rendering && this.CurrentLayers.All(l => l.Id != i.Id)).ToList();

            for (int i = 0; i < toBeAdded.Count; i++)
            {
                this.CurrentLayers.Add(toBeAdded[i]);
            }

            return newLayers;
        }

        internal void UpdateIsInRange(double inverseMapScale)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateIsInRange:{  inverseMapScale}");

            foreach (var layer in map)
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
            return CalculateExtent(this.map);
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
