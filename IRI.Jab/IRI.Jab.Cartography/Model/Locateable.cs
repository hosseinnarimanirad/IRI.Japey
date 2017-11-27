using IRI.Ham.SpatialBase.Primitives;
using IRI.Jab.Cartography.Model;
using IRI.Jab.Common;
using IRI.Jab.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using sb = IRI.Ham.SpatialBase;

namespace IRI.Jab.Cartography
{
    public class Locateable : Notifier
    {
        public Guid Id { get; set; }

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                if (_x == value)
                    return;

                var oldValue = new Point(_x, _y);

                _x = value;
                RaisePropertyChanged();

                this._location.X = value;

                this.OnPositionChanged.SafeInvoke(this, new ChangeEventArgs<Point>(oldValue, new Point(_x, _y)));
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

                var oldValue = new Point(_x, _y);

                _y = value;
                RaisePropertyChanged();

                this._location.Y = value;

                this.OnPositionChanged.SafeInvoke(this, new ChangeEventArgs<Point>(oldValue, new Point(_x, _y)));
            }
        }

        private Point _location;

        public Point Location
        {
            get { return _location; }
        }

        protected FrameworkElement _element;

        public virtual FrameworkElement Element
        {
            get { return _element; }
            set
            {
                this._element = value;
                this._element.MouseDown -= Element_MouseDown;
                this._element.MouseDown += Element_MouseDown;
            }
        }

        //public System.Windows.Controls.Primitives.Popup Popup
        //{
        //    get;
        //    set;
        //}

        public AncherFunctionHandler AncherFunction;

        public Locateable(AncherFunctionHandler ancherFunction = null)
        {
            if (ancherFunction == null)
            {
                this.AncherFunction = AncherFunctionHandlers.CenterCenter;
            }
            else
            {
                this.AncherFunction = ancherFunction;
            }

            this._location = new Point(0, 0);
        }

        public Locateable(sb.Point wgs84GeodeticPosition, AncherFunctionHandler ancherFunction = null) : this(ancherFunction)
        {
            var webMercator = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(wgs84GeodeticPosition);

            this.X = webMercator.X;

            this.Y = webMercator.Y;

            this._location = webMercator.AsWpfPoint();
        }

        //public Locateable(FrameworkElement element, Popup infoWindow, SpecialPointLayer.AncherFunctionHandler ancherFunction = null)
        //{
        //    this.Popup = infoWindow;

        //    if (ancherFunction == null)
        //    {
        //        this.AncherFunction = SpecialPointLayer.CenterCenter;
        //    }
        //    else
        //    {
        //        this.AncherFunction = ancherFunction;
        //    }

        //    this.Element = element;

        //    if (infoWindow != null)
        //    {
        //        infoWindow.AllowsTransparency = true;

        //        infoWindow.Child = new IRI.Jab.Common.UserControls.SimpleInfoControl();

        //        infoWindow.PopupAnimation = PopupAnimation.Slide;

        //        infoWindow.PlacementTarget = element;

        //        infoWindow.Placement = PlacementMode.Left;

        //        infoWindow.Focus();

        //        infoWindow.StaysOpen = false;

        //        //this.Element.MouseDown += Element_MouseDown;
        //    }
        //}

        void Element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (Popup != null)
            //{
            //    this.Popup.Focus();

            //    this.Popup.IsOpen = !this.Popup.IsOpen;
            //}

            this.OnRequestHandleMouseDown?.SafeInvoke(null);
        }

        public void Select()
        {
            if (this.Element == null)
                return;

            //var storyBoard = this.Element.FindResource("mapMarkerExpandOnMouseEnter") as Storyboard;

            //if (storyBoard == null)
            //    return;

            //storyBoard.Begin(this.Element);
            var element = ((Common.View.MapMarkers.MapMarker)(this.Element));

            element.BeginAnimation(FrameworkElement.HeightProperty, new DoubleAnimation(250, new Duration(new TimeSpan(0, 0, 1))) { FillBehavior = FillBehavior.HoldEnd });
        }

        public void Unselect()
        {
            if (this.Element == null)
                return;

            var storyBoard = this.Element.FindResource("mapMarkerResetOnMouseLeave") as Storyboard;

            if (storyBoard == null)
                return;

            storyBoard.Begin(this.Element);
        }

        public event EventHandler OnRequestHandleMouseDown;

        public event EventHandler<ChangeEventArgs<Point>> OnPositionChanged;
    }
}