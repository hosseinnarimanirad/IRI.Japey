using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IRI.Jab.Common.Assets.Commands;
using System.Threading.Tasks;
using IRI.Jab.Controls.Extensions;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem;

namespace IRI.Jab.Controls.Presenter
{
    public class InputCoordinatePresenter : Notifier
    {
        private ObservableCollection<IRI.Msh.Common.Primitives.Point> _pointCollection;

        /// <summary>
        /// Geodetic points
        /// </summary>
        public ObservableCollection<IRI.Msh.Common.Primitives.Point> PointCollection
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


        Func<IRI.Msh.Common.Primitives.Point, IRI.Msh.Common.Primitives.Point> MapFunction = p => p;

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
                        MapFunction = p => (IRI.Msh.Common.Primitives.Point)IRI.Msh.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(p, Zone);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public InputCoordinatePresenter()
            : this(new List<IRI.Msh.Common.Primitives.Point>())
        {

        }

        public InputCoordinatePresenter(List<IRI.Msh.Common.Primitives.Point> geographicPoints)
        {
            this.Zone = 39;

            FeedGeographicPoints(geographicPoints);
        }

        public void FeedGeographicPoints(List<IRI.Msh.Common.Primitives.Point> geographicPoints)
        {
            this.PointCollection = new ObservableCollection<IRI.Msh.Common.Primitives.Point>();

            this.PointCollection.CollectionChanged += (sender, e) => PointCollectionChanged?.Invoke(null, null);

            foreach (var item in geographicPoints)
            {
                this.PointCollection.Add(item);
            }
        }

        //internal void AddPoint(IRI.Msh.Common.Primitives.Point point)
        //{
        //    this.PointCollection.Add(MapFunction(point));

        //    if (PointCollectionChanged != null)
        //    {
        //        PointCollectionChanged(null, null);
        //    }
        //}

        //internal void RemovePoint(IRI.Msh.Common.Primitives.Point point)
        //{
        //    this.PointCollection.Remove(point);
        //}

        public Func<Task<Geometry<Point>>> RequestGetGeometry;

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

        public Geometry<Point> Geometry
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

            //FeedGeographicPoints(geometry.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84).GetAllPoints().Cast<IRI.Msh.Common.Primitives.Point>().ToList());
        }





    }
}
