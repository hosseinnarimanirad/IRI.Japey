using IRI.Jab.Common;
using IRI.Ham.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase.CoordinateSystems;
using IRI.Jab.Common.Assets.Commands;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Presenter
{
    public class InputCoordinatePresenter : Notifier
    {
        private ObservableCollection<IRI.Ham.SpatialBase.Point> _pointCollection;

        /// <summary>
        /// Geodetic points
        /// </summary>
        public ObservableCollection<IRI.Ham.SpatialBase.Point> PointCollection
        {
            get { return _pointCollection; }
            set
            {
                _pointCollection = value;
                RaisePropertyChanged();
            }
        }

        Func<IRI.Ham.SpatialBase.Point, IRI.Ham.SpatialBase.Point> MapFunction = p => p;

        private int _zone;

        public int Zone
        {
            get { return _zone; }
            set
            {
                _zone = value;
                RaisePropertyChanged();
            }
        }

        public event EventHandler PointCollectionChanged;

        private SpatialReferenceType _inputType;

        public SpatialReferenceType InputType
        {
            get { return _inputType; }
            set
            {
                _inputType = value;
                RaisePropertyChanged();

                switch (value)
                {
                    case SpatialReferenceType.Geodetic:
                        MapFunction = p => p;
                        break;
                    case SpatialReferenceType.UTM:
                        MapFunction = p => (IRI.Ham.SpatialBase.Point)IRI.Ham.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(p, Zone);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public InputCoordinatePresenter()
            : this(new List<IRI.Ham.SpatialBase.Point>())
        {

        }

        public InputCoordinatePresenter(List<IRI.Ham.SpatialBase.Point> geographicPoints)
        {
            this.Zone = 39;

            FeedGeographicPoints(geographicPoints);
        }

        public void FeedGeographicPoints(List<IRI.Ham.SpatialBase.Point> geographicPoints)
        {
            this.PointCollection = new ObservableCollection<IRI.Ham.SpatialBase.Point>();

            this.PointCollection.CollectionChanged += (sender, e) => PointCollectionChanged?.Invoke(null, null);

            foreach (var item in geographicPoints)
            {
                this.PointCollection.Add(item);
            }
        }

        //internal void AddPoint(IRI.Ham.SpatialBase.Point point)
        //{
        //    this.PointCollection.Add(MapFunction(point));

        //    if (PointCollectionChanged != null)
        //    {
        //        PointCollectionChanged(null, null);
        //    }
        //}

        //internal void RemovePoint(IRI.Ham.SpatialBase.Point point)
        //{
        //    this.PointCollection.Remove(point);
        //}

        public Func<Task<Ham.SpatialBase.Primitives.Geometry>> RequestGetGeometry;

        private RelayCommand _drawGeometryCommand;

        public RelayCommand DrawGeometryCommand
        {
            get
            {
                if (_drawGeometryCommand == null)
                {
                    _drawGeometryCommand = new RelayCommand(param => DrawGeometry());
                }

                return _drawGeometryCommand;
            }
        }

        private async void DrawGeometry()
        {
            var geometry = await RequestGetGeometry?.Invoke();

            if (geometry == null)
            {
                return;
            }

            FeedGeographicPoints(geometry.Transform(IRI.Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84).GetAllPoints().Cast<IRI.Ham.SpatialBase.Point>().ToList());
        }
    }
}
