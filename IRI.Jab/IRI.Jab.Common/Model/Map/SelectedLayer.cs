using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Map
{
    public class SelectedLayer<T> : Notifier, ISelectedLayer where T : class, IGeometryAware<Point>
    {
        public Guid Id { get { return AssociatedLayer?.LayerId ?? Guid.Empty; } }

        public ILayer AssociatedLayer { get; set; }

        public string LayerName { get { return AssociatedLayer?.LayerName; } }

        public bool ShowSelectedOnMap { get; set; } = false;

        private ObservableCollection<T> _features;
        public ObservableCollection<T> Features
        {
            get { return _features; }
            set
            {
                _features = value;
                RaisePropertyChanged();
            }
        }

       
        private ObservableCollection<T> _highlightedFeatures = new ObservableCollection<T>();
        public ObservableCollection<T> HighlightedFeatures
        {
            get { return _highlightedFeatures; }
            set
            {
                _highlightedFeatures = value;
                RaisePropertyChanged();

                _highlightedFeatures.CollectionChanged += (sender, e) =>
                {
                    this.UpdateHighlightedFeaturesOnMap(HighlightedFeatures.Cast<IGeometryAware<Point>>());

                    RaisePropertyChanged(nameof(IsSingleValueHighlighted));
                    //Update();
                };

                //Update();
                this.UpdateHighlightedFeaturesOnMap(HighlightedFeatures.Cast<IGeometryAware<Point>>());

                RaisePropertyChanged(nameof(IsSingleValueHighlighted));
            }
        }

        public bool IsSingleValueHighlighted
        {
            get
            {
                return HighlightedFeatures?.Count() == 1;
            }
        }

        //private void Update()
        //{
        //    this.UpdateHighlightedFeaturesOnMap(HighlightedFeatures.Cast<IGeometryAware<Point>>());
        //    RaisePropertyChanged(nameof(IsSingleValueHighlighted));
        //}


        public SelectedLayer(ILayer layer)
        {
            //this.Id = id;

            this.AssociatedLayer = layer;
        }

        public void UpdateSelectedFeatures(IEnumerable<IGeometryAware<Point>> items)
        {
            Features = new ObservableCollection<T>(items?.Cast<T>());
        }

        public void UpdateHighlightedFeatures(IEnumerable<IGeometryAware<Point>> items)
        {
            HighlightedFeatures = new ObservableCollection<T>(items.Cast<T>());
        }

        public void UpdateSelectedFeaturesOnMap(IEnumerable<IGeometryAware<Point>> enumerable)
        {
            FeaturesChangedAction?.Invoke(enumerable);
        }

        public void UpdateHighlightedFeaturesOnMap(IEnumerable<IGeometryAware<Point>> enumerable)
        {
            HighlightFeaturesChangedAction?.Invoke(enumerable);
        }

        public IEnumerable<IGeometryAware<Point>> GetSelectedFeatures()
        {
            return Features?.Cast<IGeometryAware<Point>>().ToList();

            //AssociatedLayer.
        }

        public int CountOfSelectedFeatures()
        {
            return Features.Count();
        }

        public IEnumerable<IGeometryAware<Point>> GetHighlightedFeatures()
        {
            return HighlightedFeatures?.Cast<IGeometryAware<Point>>().ToList();
        }

        private void TryFlashPoint(IEnumerable<IGeometryAware<Point>> point)
        {
            if (point?.Count() == 1 && point.First().TheGeometry.Type== GeometryType.Point/*.GetOpenGisType() == Microsoft.SqlServer.Types.OpenGisGeometryType.Point*/)
            {
                RequestFlashSinglePoint?.Invoke(point.First());
            }
        }

        public void Update(IGeometryAware<Point> oldGeometry, IGeometryAware<Point> newGeometry)
        {
            var dataSource = (this?.AssociatedLayer as VectorLayer)?.DataSource;

            dataSource.Update(newGeometry);

            var feature = this.Features.Single(f => f.Id == oldGeometry.Id);

            feature.TheGeometry = newGeometry.TheGeometry;

            //this.UpdateHighlightedFeatures(new List<IGeometryAware<Point>>() { feature });
            //var highlight = HighlightedFeatures.Single(h => h.Id == oldGeometry.Id)
        }

        public void UpdateFeature(object item)
        {
            var itemValue = item as T;

            var dataSource = (this?.AssociatedLayer as VectorLayer)?.DataSource;

            dataSource.UpdateFeature(itemValue);
        }

        public Action<IEnumerable<IGeometryAware<Point>>> FeaturesChangedAction { get; set; }

        public Action<IEnumerable<IGeometryAware<Point>>> HighlightFeaturesChangedAction { get; set; }

        public Action<IGeometryAware<Point>> RequestFlashSinglePoint { get; set; }

        public Action<IEnumerable<IGeometryAware<Point>>, Action> RequestZoomTo { get; set; }

        public Action<IGeometryAware<Point>> RequestEdit { get; set; }


        public void SaveChanges()
        {
            (this.AssociatedLayer as VectorLayer).DataSource.SaveChanges();
        }



        public Action RequestRemove { get; set; }


        private RelayCommand _zoomToCommand;
        public RelayCommand ZoomToCommand
        {
            get
            {
                if (_zoomToCommand == null)
                {
                    _zoomToCommand = new RelayCommand(param =>
                    {
                        var features = HighlightedFeatures.Cast<IGeometryAware<Point>>();
                        this.RequestZoomTo?.Invoke(features, () => { TryFlashPoint(features); });
                    });
                }

                return _zoomToCommand;
            }
        }


        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(param =>
                    {
                        var highlightedFeatures = GetHighlightedFeatures();

                        if (highlightedFeatures?.Count() == 1)
                        {
                            this.RequestEdit(highlightedFeatures.First());
                        }
                    });
                }

                return _editCommand;
            }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(param =>
                    {
                        this.SaveChanges();
                    });
                }

                return _saveCommand;
            }
        }


        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(param =>
                    {
                        this.RequestRemove?.Invoke();
                    });
                }

                return _removeCommand;
            }
        }
    }
}
