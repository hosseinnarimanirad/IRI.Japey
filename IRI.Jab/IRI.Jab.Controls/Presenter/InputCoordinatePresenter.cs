using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IRI.Jab.Common.Assets.Commands;
using System.Threading.Tasks;
using IRI.Jab.Controls.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.CoordinateSystem;

namespace IRI.Jab.Controls.Presenter
{
    public class InputCoordinatePresenter : Notifier
    {
        private ObservableCollection<IRI.Sta.Common.Primitives.Point> _pointCollection;

        /// <summary>
        /// Geodetic points
        /// </summary>
        public ObservableCollection<IRI.Sta.Common.Primitives.Point> PointCollection
        {
            get { return _pointCollection; }
            set
            {
                _pointCollection = value;
                RaisePropertyChanged();
            }
        }


        private Model.CoordinateEditor.CoordinateEditor _coordinates;

        public Model.CoordinateEditor.CoordinateEditor Coordinates
        {
            get { return _coordinates; }
            set
            {
                _coordinates = value;
                RaisePropertyChanged();
            }
        }


        Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> MapFunction = p => p;

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
                        MapFunction = p => (IRI.Sta.Common.Primitives.Point)IRI.Sta.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(p, Zone);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public InputCoordinatePresenter()
            : this(new List<IRI.Sta.Common.Primitives.Point>())
        {

        }

        public InputCoordinatePresenter(List<IRI.Sta.Common.Primitives.Point> geographicPoints)
        {
            this.Zone = 39;

            FeedGeographicPoints(geographicPoints);
        }

        public void FeedGeographicPoints(List<IRI.Sta.Common.Primitives.Point> geographicPoints)
        {
            this.PointCollection = new ObservableCollection<IRI.Sta.Common.Primitives.Point>();

            this.PointCollection.CollectionChanged += (sender, e) => PointCollectionChanged?.Invoke(null, null);

            foreach (var item in geographicPoints)
            {
                this.PointCollection.Add(item);
            }
        }

        //internal void AddPoint(IRI.Sta.Common.Primitives.Point point)
        //{
        //    this.PointCollection.Add(MapFunction(point));

        //    if (PointCollectionChanged != null)
        //    {
        //        PointCollectionChanged(null, null);
        //    }
        //}

        //internal void RemovePoint(IRI.Sta.Common.Primitives.Point point)
        //{
        //    this.PointCollection.Remove(point);
        //}

        public Func<Task<Sta.Common.Primitives.Geometry>> RequestGetGeometry;

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

        public Geometry Geometry
        {
            set
            {
                this.Coordinates = value.AsCoordinateEditor();
                 
            }
        }

        private async void DrawGeometry()
        {
            var geometry = await RequestGetGeometry?.Invoke();

            if (geometry == null)
            {
                return;
            }

            this.Coordinates = geometry.AsCoordinateEditor();

            //FeedGeographicPoints(geometry.Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84).GetAllPoints().Cast<IRI.Sta.Common.Primitives.Point>().ToList());
        }





    }
}
