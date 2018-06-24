using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
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
    public class SelectedLayer<T> : Notifier, ISelectedLayer where T : ISqlGeometryAware
    {
        public Guid Id { get { return AssociatedLayer?.Id ?? Guid.Empty; } }

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

        //private DataTable _features;

        //public DataTable Features
        //{
        //    get { return _features; }
        //    set
        //    {
        //        _features = value;
        //        RaisePropertyChanged();
        //    }
        //}
         
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
                    this.UpdateHighlightedFeaturesOnMap(e.NewItems.Cast<ISqlGeometryAware>());
                };

                this.UpdateHighlightedFeaturesOnMap(HighlightedFeatures.Cast<ISqlGeometryAware>());
            }
        }

        public SelectedLayer(ILayer layer)
        {
            //this.Id = id;

            this.AssociatedLayer = layer;
        }

        public void UpdateSelectedFeatures(IEnumerable<ISqlGeometryAware> items)
        {
            Features = new ObservableCollection<T>(items?.Cast<T>());
        }

        public void UpdateHighlightedFeatures(IEnumerable<ISqlGeometryAware> items)
        {
            HighlightedFeatures = new ObservableCollection<T>(items.Cast<T>());
        }

        public IEnumerable<ISqlGeometryAware> GetSelectedFeatures()
        {
            return Features?.Cast<ISqlGeometryAware>().ToList();
        }

        public int CountOfSelectedFeatures()
        {
            return Features.Count();
        }

        public IEnumerable<ISqlGeometryAware> GetHighlightedFeatures()
        {
            return HighlightedFeatures?.Cast<ISqlGeometryAware>().ToList();
        }

        public void UpdateSelectedFeaturesOnMap(IEnumerable<ISqlGeometryAware> enumerable)
        {
            FeaturesChangedAction?.Invoke(enumerable);
        }

        public void UpdateHighlightedFeaturesOnMap(IEnumerable<ISqlGeometryAware> enumerable)
        {
            HighlightFeaturesChangedAction?.Invoke(enumerable);
        }


        public Action<IEnumerable<ISqlGeometryAware>> FeaturesChangedAction { get; set; }

        public Action<IEnumerable<ISqlGeometryAware>> HighlightFeaturesChangedAction { get; set; }

        public Action<IEnumerable<ISqlGeometryAware>> ZoomTo { get; set; }

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
                        this.ZoomTo?.Invoke(HighlightedFeatures.Cast<ISqlGeometryAware>());
                    });
                }

                return _zoomToCommand;
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
