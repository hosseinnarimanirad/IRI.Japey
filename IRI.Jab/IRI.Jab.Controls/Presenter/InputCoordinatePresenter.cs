using IRI.Jab.Common;
using IRI.Ham.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase.CoordinateSystems;

namespace IRI.Jab.Controls.Presenters
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
                RaisePropertyChanged("PointCollection");
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
                RaisePropertyChanged("Zone");
            }
        }

        public event EventHandler PointAdded;

        private SpatialReferenceType _inputType;

        public SpatialReferenceType InputType
        {
            get { return _inputType; }
            set
            {
                _inputType = value;
                RaisePropertyChanged("InputType");

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

            this._pointCollection = new ObservableCollection<IRI.Ham.SpatialBase.Point>();

            FeedGeographicPoints(geographicPoints);
        }

        public void FeedGeographicPoints(List<IRI.Ham.SpatialBase.Point> geographicPoints)
        {
            foreach (var item in geographicPoints)
            {
                this.PointCollection.Add(item);
            }
        }

        internal void AddPoint(IRI.Ham.SpatialBase.Point point)
        {
            this.PointCollection.Add(MapFunction(point));

            if (PointAdded != null)
            {
                PointAdded(null, null);
            }
        }

        internal void RemovePoint(IRI.Ham.SpatialBase.Point point)
        {
            this.PointCollection.Remove(point);
        }
    }
}
