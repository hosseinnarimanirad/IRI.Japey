using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Sta.Common.Primitives; 
using IRI.Jab.Common.Presenter.Map;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Extensions;
using IRI.Jab.Controls.Model.GoTo;
using IRI.Jab.Controls.View.Input;
using IRI.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.CoordinateSystem;
using IRI.Extensions;

namespace IRI.Jab.Controls.Presenter
{
    public class GoToPresenter : Notifier
    {
        private Action<Point> RequestZoomTo;

        private Action<Point> RequestPanTo;


        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                if (_x == value)
                    return;

                _x = value;
                RaisePropertyChanged();

                UpdateLatLong();
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                if (_y == value)
                    return;

                _y = value;
                RaisePropertyChanged();

                UpdateLatLong();
            }
        }

        private int _utmZone = 39;

        public int UtmZone
        {
            get { return _utmZone; }
            set
            {
                _utmZone = value;
                RaisePropertyChanged();
            }
        }

        private bool _isPaneOpen;

        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set
            {
                _isPaneOpen = value;
                RaisePropertyChanged();
            }
        }


        private Model.DegreeMinuteSecondModel _longitudeDms;

        public Model.DegreeMinuteSecondModel LongitudeDms
        {
            get { return _longitudeDms; }
            set
            {
                _longitudeDms = value;
                RaisePropertyChanged();
            }
        }

        private Model.DegreeMinuteSecondModel _latitudeDms;

        public Model.DegreeMinuteSecondModel LatitudeDms
        {
            get { return _latitudeDms; }
            set
            {
                _latitudeDms = value;
                RaisePropertyChanged();
            }
        }


        private List<HamburgerGoToMenuItem> _menuItems;

        public List<HamburgerGoToMenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                RaisePropertyChanged();
            }
        }

        private HamburgerGoToMenuItem _selectedItem;

        public HamburgerGoToMenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value)
                {
                    return;
                }

                _selectedItem = value;
                RaisePropertyChanged();
                //RaisePropertyChanged("SelectedItem.Content");

                this.LongitudeDms.SetValue(0);
                this.LatitudeDms.SetValue(0);
            }
        }


        public GoToPresenter(Action<Point> requestPanTo, Action<Point> requestZoomTo, List<HamburgerGoToMenuItem> items = null)
        {
            this.RequestZoomTo = requestZoomTo;

            this.RequestPanTo = requestPanTo;

            if (items == null || items.Count() < 1)
            {
                this.MenuItems = GetDefaultItems();
            }
            else
            {
                this.MenuItems = items;
            }

            //this.SelectedItem = MenuItems.First();

            this.LongitudeDms = new Model.DegreeMinuteSecondModel();

            this.LongitudeDms.OnValueChanged += (sender, e) => { UpdateXY(); };

            this.LatitudeDms = new Model.DegreeMinuteSecondModel();

            this.LatitudeDms.OnValueChanged += (sender, e) => { UpdateXY(); };

            this.IsPaneOpen = false;
        }

        public void ZoomTo()
        {
            this.RequestZoomTo?.Invoke(GetWgs84Point());
        }

        public void PanTo()
        {
            this.RequestPanTo?.Invoke(GetWgs84Point());
        }

        public Point GetWgs84Point()
        {
            var point = new Point(X, Y);

            switch (this.SelectedItem?.MenuType)
            {
                case SpatialReferenceType.Geodetic:
                    return point;//.Project( ,new IRI.Msh.CoordinateSystem.MapProjection.NoProjection());

                case SpatialReferenceType.UTM:
                    return point.Project(UTM.CreateForZone(UtmZone), new NoProjection());

                case SpatialReferenceType.Mercator:
                case SpatialReferenceType.TransverseMercator:
                case SpatialReferenceType.CylindricalEqualArea:
                case SpatialReferenceType.LambertConformalConic:
                case SpatialReferenceType.WebMercator:
                case SpatialReferenceType.AlbersEqualAreaConic:
                default:
                    throw new NotImplementedException();
            }
        }

        private List<HamburgerGoToMenuItem> GetDefaultItems()
        {
            return new List<HamburgerGoToMenuItem>()
            {
                new HamburgerGoToMenuItem(new GoToGeodeticView(), SpatialReferenceType.Geodetic){
                    Title = "Geodetic",
                    SubTitle ="WGS84",
                    Tooltip ="Geodetic",
                    Icon = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarGlobe,
                },
                new HamburgerGoToMenuItem(new GoToMapProjectView(), SpatialReferenceType.UTM){
                    Title = "Uiversal Transverse Mercator",
                    SubTitle ="UTM",
                    Tooltip ="UTM",
                    Icon = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMapTreasure,
                }
            };
        }

        private void UpdateXY()
        {
            //if (this.SelectedItem?.MenuType == SpatialReferenceType.Geodetic)
            //{
            this._x = this.LongitudeDms.GetDegreeValue();

            this._y = this.LatitudeDms.GetDegreeValue();

            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));

            RaisePropertyChanged(nameof(LongitudeDms));
            RaisePropertyChanged(nameof(LatitudeDms));
            //}
        }

        private void UpdateLatLong()
        {
            if (this.SelectedItem?.MenuType == SpatialReferenceType.Geodetic)
            {
                this.LongitudeDms.SetValue(this.X);

                this.LatitudeDms.SetValue(this.Y);
            }
        }

        public void SelectDefaultMenu()
        {
            if (this.MenuItems?.Count > 0)
            {
                this.SelectedItem = this.MenuItems.First();
                this.IsPaneOpen = false;
            }

        }

        #region Commands


        private RelayCommand _zoomToCommand;

        public RelayCommand ZoomToCommand
        {
            get
            {
                if (_zoomToCommand == null)
                {
                    _zoomToCommand = new RelayCommand(param => { this.ZoomTo(); });
                }

                return _zoomToCommand;
            }
        }


        private RelayCommand _panToCommand;

        public RelayCommand PanToCommand
        {
            get
            {
                if (_panToCommand == null)
                {
                    _panToCommand = new RelayCommand(param => { this.PanTo(); });
                }

                return _panToCommand;
            }
        }


        #endregion

        public static GoToPresenter Create(MapPresenter mapPresenter)
        {
            var gotoPresenter = new GoToPresenter(
               p =>
               {
                   var webMercatorPoint = MapProjects.GeodeticWgs84ToWebMercator(p);

                   mapPresenter.PanTo(webMercatorPoint, () =>
                   {
                       mapPresenter.FlashPoint(webMercatorPoint);
                   });

               },
               p =>
               {
                   var webMercatorPoint = MapProjects.GeodeticWgs84ToWebMercator(p);

                   mapPresenter.ZoomToLevelAndCenter(13, webMercatorPoint, () =>
                   {
                       mapPresenter.FlashPoint(webMercatorPoint);
                   });
               });
             
            return gotoPresenter;
        }
    }
}
