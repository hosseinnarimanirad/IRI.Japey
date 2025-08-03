using IRI.Jab.Common;
using IRI.Jab.Common.Localization;
using System;
using System.Windows;

namespace IRI.Jab.Controls;

/// <summary>
/// Interaction logic for MapInfoView.xaml
/// </summary>
public partial class MapInfoView : NotifiableUserControl, IDisposable
{ 
    public MapInfoView()
    {
        InitializeComponent();

        LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }

    private void Instance_LanguageChanged()
    {
        this.NotifyAllProperties();
    }

   

    public FlowDirection CurrentFlowDirection => LocalizationManager.Instance.CurrentFlowDirection;

    //public LanguageMode UILanguage
    //{
    //    get { return (LanguageMode)GetValue(UILanguageProperty); }
    //    set
    //    {
    //        SetValue(UILanguageProperty, value);

    //    }
    //}

    //// Using a DependencyProperty as the backing store for UILanguage.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty UILanguageProperty =
    //    DependencyProperty.Register(
    //        nameof(UILanguage),
    //        typeof(LanguageMode),
    //        typeof(MapInfoView),
    //        new PropertyMetadata(LanguageMode.Persian, (d, dp) =>
    //        {
    //            try
    //            {
    //                ((MapInfoView)d).SetLanguage((LanguageMode)dp.NewValue);
    //            }
    //            catch (Exception ex)
    //            {
    //                return;
    //            }
    //        }));


    public string MapPanel_CurrentPoint => LocalizationManager.Instance[LocalizationResourceKeys.mapPanel_currentPoint.ToString()];

    public string MapPanel_MultiPart => LocalizationManager.Instance[LocalizationResourceKeys.mapPanel_multiPart.ToString()];

    public string MapPanel_Srs => LocalizationManager.Instance[LocalizationResourceKeys.mapPanel_srs.ToString()];


    public string UtmZone => LocalizationManager.Instance[LocalizationResourceKeys.srs_utmZone.ToString()];

    //private PersianEnglishItem _utmZone = new PersianEnglishItem("ناحیهٔ UTM", "Zone", LanguageMode.Persian);
    //public PersianEnglishItem UtmZone
    //{
    //    get { return _utmZone; }
    //    set
    //    {
    //        _utmZone = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string UtmText => LocalizationManager.Instance[LocalizationResourceKeys.srs_utmTitle.ToString()];

    //private PersianEnglishItem _utmText = new PersianEnglishItem("سیستم تصویر UTM", "UTM", LanguageMode.Persian);
    //public PersianEnglishItem UTMText
    //{
    //    get { return _utmText; }
    //    set
    //    {
    //        _utmText = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string GeodeticWgs84Text => LocalizationManager.Instance[LocalizationResourceKeys.srs_geodeticTitle.ToString()];
    //private PersianEnglishItem _geodeticWgs84Text = new PersianEnglishItem("سیستم مختصات ژئودتیک - WGS84", "Geodetic (WGS84)", LanguageMode.Persian);
    //public PersianEnglishItem GeodeticWgs84Text
    //{
    //    get { return _geodeticWgs84Text; }
    //    set
    //    {
    //        _geodeticWgs84Text = value;
    //        RaisePropertyChanged();
    //    }
    //}

    //public PersianEnglishItem WebMercatorText
    //{
    //    get { return _webMercatorText; }
    //    set
    //    {
    //        _webMercatorText = value;
    //        RaisePropertyChanged();
    //    }
    //}
    //private PersianEnglishItem _webMercatorText = new PersianEnglishItem("سیستم تصویر وب‌مرکاتور", "Web Mercator", LanguageMode.Persian);

    public string NewDrawingText => LocalizationManager.Instance[LocalizationResourceKeys.draw_newDrawingText.ToString()];

    //private PersianEnglishItem _newDrawingText = new PersianEnglishItem("برای اتمام ترسیم روی نقطهٔ آخر مجدد کلیک کنید.", "Click on the last point to finish drawing.", LanguageMode.Persian);
    //public PersianEnglishItem NewDrawingText
    //{
    //    get { return _newDrawingText; }
    //    set
    //    {
    //        _newDrawingText = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string AddPointText => LocalizationManager.Instance[LocalizationResourceKeys.draw_addPointText.ToString()];

    //private PersianEnglishItem _addPointText = new PersianEnglishItem("افزودن نقطه", "Add Point", LanguageMode.Persian);

    //public PersianEnglishItem AddPointText
    //{
    //    get { return _addPointText; }
    //    set
    //    {
    //        _addPointText = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string CancelDrawingText => LocalizationManager.Instance[LocalizationResourceKeys.draw_cancelDrawingText.ToString()];

    //private PersianEnglishItem _cancelDrawingText = new PersianEnglishItem("لغو ترسیم", "Cancel Drawing", LanguageMode.Persian);
    //public PersianEnglishItem CancelDrawingText
    //{
    //    get { return _cancelDrawingText; }
    //    set
    //    {
    //        _cancelDrawingText = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string FinishDrawingText => LocalizationManager.Instance[LocalizationResourceKeys.draw_finishDrawingText.ToString()];

    //private PersianEnglishItem _finishDrawingText = new PersianEnglishItem("پایان ترسیم", "Finish Drawing", LanguageMode.Persian);
    //public PersianEnglishItem FinishDrawingText
    //{
    //    get { return _finishDrawingText; }
    //    set
    //    {
    //        _finishDrawingText = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string FinishDrawingPartText => LocalizationManager.Instance[LocalizationResourceKeys.draw_finishDrawingPartText.ToString()];

    //private PersianEnglishItem _finishDrawingPartText = new PersianEnglishItem("تکمیل بخش", "Finish Drawing Part", LanguageMode.Persian);
    //public PersianEnglishItem FinishDrawingPartText
    //{
    //    get { return _finishDrawingPartText; }
    //    set
    //    {
    //        _finishDrawingPartText = value;
    //        RaisePropertyChanged();
    //    }
    //}


    //private void SetLanguage(LanguageMode mode)
    //{
    //    //NewDrawingText.UILanguage = mode;
    //    var properties = (typeof(MapInfoView)).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
    //        .Where(p => p.DeclaringType == typeof(PersianEnglishItem));

    //    foreach (var property in properties)
    //    {
    //        property.SetValue(this, mode);
    //    }

    //}



    #region IDispose

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                LocalizationManager.Instance.LanguageChanged -= Instance_LanguageChanged;
            }

            // Dispose unmanaged resources here if any
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
