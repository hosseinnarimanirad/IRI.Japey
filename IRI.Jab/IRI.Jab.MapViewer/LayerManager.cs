using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Jab.Cartography;
using Microsoft.SqlServer.Types;
using IRI.Jab.Common;
using System.Windows;
using IRI.Ham.SpatialBase;
using System.Collections.ObjectModel;
using IRI.Jab.Common.Model;

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
            layer.OnVisibilityChanged -= Refresh;

            layer.OnVisibilityChanged += Refresh;

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
            this.map.RemoveAll(i => i.Type.HasFlag(type));

            for (int i = CurrentLayers.Count - 1; i >= 0; i--)
            {
                if (CurrentLayers[i].Type.HasFlag(type))
                {
                    this.CurrentLayers.Remove(CurrentLayers[i]);
                }
            }
        }

        public void Remove(Cartography.TileServices.MapProviderType provider, Cartography.TileServices.TileType type)
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

        public IEnumerable<ILayer> UpdateAndGetLayers(double inverseMapScale)
        {
            var newLayers = map.Where((var) => (var.VisibleRange.Upper >= inverseMapScale && var.VisibleRange.Lower < inverseMapScale)).OrderBy(i => i.Type != LayerType.BaseMap);

            var toBeRemovedLayers = this.CurrentLayers.Where(i => newLayers.All(l => l.Id != i.Id)).ToList();

            for (int i = 0; i < toBeRemovedLayers.Count; i++)
            {
                this.CurrentLayers.Remove(toBeRemovedLayers[i]);
            }

            var toBeAdded = newLayers.Where(i => this.CurrentLayers.All(l => l.Id != i.Id)).ToList();

            for (int i = 0; i < toBeAdded.Count; i++)
            {
                this.CurrentLayers.Add(toBeAdded[i]);
            }

            return newLayers;
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

        private void Refresh(object sender, EventArgs e)
        {
            this.OnRequestRefresh.SafeInvoke(null);
        }

        public event EventHandler OnRequestRefresh;
    }


}
