using IRI.Ham.SpatialBase;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace IRI.Jab.Common.Presenters
{
    public class MapPanelPresenter : Notifier
    {
        private bool _isDetailsVisible;

        public bool IsDetailsVisible
        {
            get { return _isDetailsVisible; }
            set
            {
                _isDetailsVisible = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsDetailsNotVisible));
            }
        }

        public bool IsDetailsNotVisible
        {
            get { return !IsDetailsVisible; }
            set
            {
                _isDetailsVisible = !value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsDetailsVisible));
            }
        }


        private bool _isUTMEditingMode;

        public bool IsUTMEditingMode
        {
            get { return _isUTMEditingMode; }
            set
            {
                _isUTMEditingMode = value;
                RaisePropertyChanged();
            }
        }


        private bool _isGeodeticWgs84EditingMode = true;

        public bool IsGeodeticWgs84EditingMode
        {
            get { return _isGeodeticWgs84EditingMode; }
            set
            {
                _isGeodeticWgs84EditingMode = value;
                RaisePropertyChanged();
            }
        }


        private NotifiablePoint _currentEditingPoint;

        public NotifiablePoint CurrentEditingPoint
        {
            get { return _currentEditingPoint; }
            set
            {
                _currentEditingPoint = value;
                RaisePropertyChanged();
            }
        }

        private int _currentEditingZone;

        public int CurrentEditingZone
        {
            get { return _currentEditingZone; }
            set
            {
                _currentEditingZone = value;
                RaisePropertyChanged();
            }
        }


        public Ham.SpatialBase.Point CurrentWebMercatorEditingPoint
        {
            get
            {
                if (IsUTMEditingMode)
                {
                    var geodetic = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(CurrentEditingPoint, CurrentEditingZone);

                    return IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(geodetic);
                }
                else if (IsGeodeticWgs84EditingMode)
                {
                    return IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(CurrentEditingPoint);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

        }

        private Ham.SpatialBase.Point FromWebMercator(Ham.SpatialBase.Point webMercatorPoint)
        {
            var geodetic = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84(webMercatorPoint);
       
            if (IsUTMEditingMode)
            {
                return IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(geodetic);
            }
            else if (IsGeodeticWgs84EditingMode)
            {
                return geodetic;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private RelayCommand _toggleDetailsVisibilityCommand;

        public RelayCommand ToggleDetailsVisibilityCommand
        {
            get
            {
                if (_toggleDetailsVisibilityCommand == null)
                {
                    _toggleDetailsVisibilityCommand = new RelayCommand(param => { IsDetailsVisible = !IsDetailsVisible; });
                }

                return _toggleDetailsVisibilityCommand;
            }
        }

        public void UpdateCurrentEditingPoint(Point webMercatorPoint)
        {
            lock (this.CurrentEditingPoint)
            {
                this.CurrentEditingPoint.IsRaiseCoordinateChangeEnabled = false;

                var point = FromWebMercator(webMercatorPoint);

                this.CurrentEditingPoint.X = point.X;

                this.CurrentEditingPoint.Y = point.Y;

                this.CurrentEditingPoint.IsRaiseCoordinateChangeEnabled = true;
            }

        }
    }
}
