using IRI.Sta.CoordinateSystems;
using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using IRI.Jab.Common;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Extensions;

namespace IRI.Jab.Common.Model.CoordinatePanel
{
    public class SpatialReferenceItem : Notifier
    {
        const string defaultXLabel = "X";
        const string defaultYLabel = "Y";

        //static readonly IEllipsoid defaultEllipsoid = Ellipsoids.WGS84;

        //private IEllipsoid _ellipsoid = defaultEllipsoid;

        //public IEllipsoid Ellipsoid
        //{
        //    get { return _ellipsoid; }
        //    set
        //    {
        //        _ellipsoid = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private IRI.Sta.Common.CoordinateSystem.SpatialReferenceType _spatialReference;

        //public IRI.Sta.Common.CoordinateSystem.SpatialReferenceType SpatialReference
        //{
        //    get { return _spatialReference; }
        //    set
        //    {
        //        _spatialReference = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private Func<Point, Point> _fromWgs84Geodetic;

        private Func<double, string> _toString;

        public SpatialReferenceItem(Func<Point, Point> fromWgs84Geodetic, Func<double, string> toString, PersianEnglishItem titleItem, PersianEnglishItem subTitleItem, PersianEnglishItem xLabel, PersianEnglishItem yLabel)
        {
            this._fromWgs84Geodetic = fromWgs84Geodetic;

            this._toString = toString;

            this.TitleItem = titleItem;

            this.SubTitleItem = subTitleItem;

            this.XLabelItem = xLabel;

            this.YLabelItem = yLabel;
        }

        public IPoint FromWgs84Geodetic(Point geodeticPoint)
        {
            return _fromWgs84Geodetic(geodeticPoint);
        }

        public void Update(Point geodeticPoint)
        {
            var point = _fromWgs84Geodetic(geodeticPoint);

            this.XValue = _toString(point.X);

            this.YValue = _toString(point.Y);

            this.ZoneNumber = IRI.Sta.CoordinateSystems.MapProjection.MapProjects.FindUtmZone(geodeticPoint.X).ToString();

            if (UILanguage == LanguageMode.Persian)
            {
                this.XValue = this.XValue.LatinNumbersToFarsiNumbers();
                this.YValue = this.YValue.LatinNumbersToFarsiNumbers();
                this.ZoneNumber = this.ZoneNumber.LatinNumbersToFarsiNumbers();
            }
        }

        private PersianEnglishItem _titleItem;

        public PersianEnglishItem TitleItem
        {
            get { return _titleItem; }
            set
            {
                _titleItem = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _subTitleItem;

        public PersianEnglishItem SubTitleItem
        {
            get { return _subTitleItem; }
            set
            {
                _subTitleItem = value;
                RaisePropertyChanged();
            }
        }

        //private string _persianTitle;

        //public string PersianTitle
        //{
        //    get { return _persianTitle; }
        //    set
        //    {
        //        _persianTitle = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged(nameof(Title));
        //    }
        //}

        //private string _englishTitle;

        //public string EnglishTitle
        //{
        //    get { return _englishTitle; }
        //    set
        //    {
        //        _englishTitle = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged(nameof(Title));
        //    }
        //}

        //public string Title
        //{
        //    get { return UILanguage == LanguageMode.Persian ? PersianTitle : EnglishTitle; }
        //}



        private bool _isZoneVisible = false;

        public bool IsZoneVisible
        {
            get { return _isZoneVisible; }
            set
            {
                _isZoneVisible = value;
                RaisePropertyChanged();
            }
        }

        //private string _englishZoneLabel = "Zone";

        //public string EnglishZoneLabel
        //{
        //    get { return _englishZoneLabel; }
        //    set
        //    {
        //        _englishZoneLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string _persianZoneLabel = "ناحیه";

        //public string PersianZoneLabel
        //{
        //    get { return _persianZoneLabel; }
        //    set
        //    {
        //        _persianZoneLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public string ZoneLabel { get => UILanguage == LanguageMode.Persian ? PersianZoneLabel : EnglishZoneLabel; }

        private PersianEnglishItem _zoneItem = new PersianEnglishItem("ناحیه :", "Zone :", LanguageMode.Persian);

        public PersianEnglishItem ZoneItem
        {
            get { return _zoneItem; }
            set
            {
                _zoneItem = value;
                RaisePropertyChanged();
            }
        }


        private PersianEnglishItem _xLabelItem;

        public PersianEnglishItem XLabelItem
        {
            get { return _xLabelItem; }
            set
            {
                _xLabelItem = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _yLabelItem;

        public PersianEnglishItem YLabelItem
        {
            get { return _yLabelItem; }
            set
            {
                _yLabelItem = value;
                RaisePropertyChanged();
            }
        }

        //private string _xLabel = defaultXLabel;

        //public string XLabel
        //{
        //    get { return _xLabel; }
        //    set
        //    {
        //        _xLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string _yLabel = defaultYLabel;

        //public string YLabel
        //{
        //    get { return _yLabel; }
        //    set
        //    {
        //        _yLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private string _zoneNumber;

        public string ZoneNumber
        {
            get { return _zoneNumber; }
            set
            {
                _zoneNumber = value;
                RaisePropertyChanged();
            }
        }

        private string _xValue;

        public string XValue
        {
            get { return _xValue; }
            set
            {
                _xValue = value;
                RaisePropertyChanged();
            }
        }

        private string _yValue;

        public string YValue
        {
            get { return _yValue; }
            set
            {
                _yValue = value;
                RaisePropertyChanged();
            }
        }


        private bool _isVisible = true;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }


        private LanguageMode _uiLanguage;

        public LanguageMode UILanguage
        {
            get { return _uiLanguage; }
            set
            {
                _uiLanguage = value;
                RaisePropertyChanged();
                this.TitleItem.UILanguage = value;
                this.SubTitleItem.UILanguage = value;
                this.ZoneItem.UILanguage = value;
                this.XLabelItem.UILanguage = value;
                this.YLabelItem.UILanguage = value;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                RaisePropertyChanged();

                if (value)
                {
                    this.FireIsSelectedChanged?.Invoke(this);
                }

            }
        }

        public Action<SpatialReferenceItem> FireIsSelectedChanged;


        public string GetPositionString(Point geodeticPoint) //where T : IPoint, new()
        {
            var point = FromWgs84Geodetic(geodeticPoint);

            var x = _toString(point.X);

            var y = _toString(point.Y);

            return $"{x},{y}";
        }
    }
}
