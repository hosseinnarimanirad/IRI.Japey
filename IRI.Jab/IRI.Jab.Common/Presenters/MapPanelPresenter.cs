using IRI.Msh.Common.Primitives; 
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.CoordinateSystem;

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

        //private bool _isManualInputAvailable = true;

        //public bool IsManualInputAvailable
        //{
        //    get { return _isManualInputAvailable; }
        //    set
        //    {
        //        _isManualInputAvailable = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private bool _isMultiPartSupportAvailable = true;

        //public bool IsMultiPartSupportAvailable
        //{
        //    get { return _isMultiPartSupportAvailable; }
        //    set
        //    {
        //        _isMultiPartSupportAvailable = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private EditableFeatureLayerOptions _options;

        public EditableFeatureLayerOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                RaisePropertyChanged();
            }
        }


        private SpatialReferenceType _spatialReference = SpatialReferenceType.Geodetic;

        public SpatialReferenceType SpatialReference
        {
            get { return _spatialReference; }
            set
            {
                var currentPoint = CurrentWebMercatorEditingPoint;

                _spatialReference = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsUTMEditingMode));
                RaisePropertyChanged(nameof(IsGeodeticWgs84EditingMode));

                UpdateCurrentEditingPoint(currentPoint);
            }
        }


        private bool _isUTMEditingMode;

        public bool IsUTMEditingMode
        {
            get { return _isUTMEditingMode; }
            set
            {
                if (_isUTMEditingMode == value)
                {
                    return;
                }

                if (CurrentEditingPoint != null && value)
                {
                    this.SpatialReference = SpatialReferenceType.UTM;
                }

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
                if (_isGeodeticWgs84EditingMode == value)
                {
                    return;
                }

                if (CurrentEditingPoint != null && value)
                {
                    this.SpatialReference = SpatialReferenceType.Geodetic;
                }

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


        public Point CurrentWebMercatorEditingPoint
        {
            get
            {
                if (this.SpatialReference == SpatialReferenceType.UTM)
                {
                    var geodetic = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(CurrentEditingPoint, CurrentEditingZone);

                    return IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(geodetic);
                }
                else if (this.SpatialReference == SpatialReferenceType.Geodetic)
                {
                    return IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(CurrentEditingPoint);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

        }

        public MapPanelPresenter()
        {
            //RaisePropertyChanged(nameof(IsGeodeticWgs84EditingMode));
        }

        private Point FromWebMercator(Point webMercatorPoint)
        {
            var geodetic = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84(webMercatorPoint);

            if (this.SpatialReference == SpatialReferenceType.UTM)
            {
                this.CurrentEditingZone = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.FindZone(geodetic.X);

                return IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(geodetic);
            }
            else if (this.SpatialReference == SpatialReferenceType.Geodetic)
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
