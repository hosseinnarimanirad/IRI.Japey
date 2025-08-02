using System;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model.Globalization;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.SpatialReferenceSystem;

namespace IRI.Jab.Common.Model.CoordinatePanel;

public class SpatialReferenceItem : Notifier
{
    const string defaultXLabel = "X";
    const string defaultYLabel = "Y";

    private Func<Point, Point> _fromWgs84Geodetic;

    private Func<double, string> _toString;

    public SpatialReferenceItem(
        Func<Point, Point> fromWgs84Geodetic,
        Func<double, string> toString,
        string titleItemResourceKey,
        string subTitleItemResourceKey,
        string xLabelResourceKey,
        string yLabelResourceKey)
    {
        this._fromWgs84Geodetic = fromWgs84Geodetic;

        this._toString = toString;

        this.TitleItemResourceKey = titleItemResourceKey;

        this.SubTitleItemResourceKey = subTitleItemResourceKey;

        this.XLabelItemResourceKey = xLabelResourceKey;

        this.YLabelItemResourceKey = yLabelResourceKey;

        LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        RaisePropertyChanged(nameof(TitleItem));
    }

    public void Dispose()
    {
        LocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
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

        this.ZoneNumber = MapProjects.FindUtmZone(geodeticPoint.X).ToString();

        //if (UILanguage == LanguageMode.Persian)
        if (LocalizationManager.Instance.IsPersian)
        {
            this.XValue = this.XValue.LatinNumbersToFarsiNumbers();
            this.YValue = this.YValue.LatinNumbersToFarsiNumbers();
            this.ZoneNumber = this.ZoneNumber.LatinNumbersToFarsiNumbers();
        }
    }


    public string TitleItemResourceKey { get; private set; }

    public string TitleItem => LocalizationManager.Instance[TitleItemResourceKey];

    //private PersianEnglishItem _titleItem;

    //public PersianEnglishItem TitleItem
    //{
    //    get { return _titleItem; }
    //    set
    //    {
    //        _titleItem = value;
    //        RaisePropertyChanged();
    //    }
    //}


    public string SubTitleItemResourceKey { get; private set; }

    public string SubTitleItem => LocalizationManager.Instance[SubTitleItemResourceKey];

    //private PersianEnglishItem _subTitleItem;

    //public PersianEnglishItem SubTitleItem
    //{
    //    get { return _subTitleItem; }
    //    set
    //    {
    //        _subTitleItem = value;
    //        RaisePropertyChanged();
    //    }
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


    public string ZoneItemResourceKey { get; private set; }

    public string ZoneItem => string.IsNullOrWhiteSpace(ZoneItemResourceKey) ? string.Empty : LocalizationManager.Instance[ZoneItemResourceKey];

    //private PersianEnglishItem _zoneItem = new PersianEnglishItem("ناحیه :", "Zone :", LanguageMode.Persian);

    //public PersianEnglishItem ZoneItem
    //{
    //    get { return _zoneItem; }
    //    set
    //    {
    //        _zoneItem = value;
    //        RaisePropertyChanged();
    //    }
    //}


    public string XLabelItemResourceKey { get; private set; }

    public string XLabelItem => LocalizationManager.Instance[XLabelItemResourceKey];

    //private PersianEnglishItem _xLabelItem;

    //public PersianEnglishItem XLabelItem
    //{
    //    get { return _xLabelItem; }
    //    set
    //    {
    //        _xLabelItem = value;
    //        RaisePropertyChanged();
    //    }
    //}


    public string YLabelItemResourceKey { get; private set; }

    public string YLabelItem => LocalizationManager.Instance[YLabelItemResourceKey];

    //private PersianEnglishItem _yLabelItem;

    //public PersianEnglishItem YLabelItem
    //{
    //    get { return _yLabelItem; }
    //    set
    //    {
    //        _yLabelItem = value;
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


    //private LanguageMode _uiLanguage;

    //public LanguageMode UILanguage
    //{
    //    get { return _uiLanguage; }
    //    set
    //    {
    //        _uiLanguage = value;
    //        RaisePropertyChanged();
    //        //this.TitleItem.UILanguage = value;
    //        this.SubTitleItem.UILanguage = value;
    //        this.ZoneItem.UILanguage = value;
    //        this.XLabelItem.UILanguage = value;
    //        this.YLabelItem.UILanguage = value;
    //    }
    //}

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
