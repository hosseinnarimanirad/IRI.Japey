using IRI.Jab.Common;
using IRI.Jab.Common.Localization;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace IRI.Jab.Controls.View.Input;

/// <summary>
/// Interaction logic for GoToGeodetic.xaml
/// </summary>
public partial class GoToGeodeticView : NotifiableUserControl, IDisposable
{

    public GoToGeodeticView()
    {
        InitializeComponent();

        //this.UILanguage = LanguageMode.Persian;
        LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }

    private void Instance_LanguageChanged()
    { 
        RaisePropertyChanged(nameof(XLabel));
        RaisePropertyChanged(nameof(YLabel));
        RaisePropertyChanged(nameof(PanToLabel));
        RaisePropertyChanged(nameof(ZoomToLabel));
    }

    public string XLabel => LocalizationManager.Instance[LocalizationResourceKeys.srs_defaultLongitude.ToString()];

    //private string _xLabel;

    //public string XLabel
    //{
    //    get { return _xLabel; }
    //    set
    //    {
    //        _xLabel = value;
    //        RaisePropertyChanged();
    //    }
    //}
    public string YLabel => LocalizationManager.Instance[LocalizationResourceKeys.srs_defaultLatitude.ToString()];

    //private string _yLabel;

    //public string YLabel
    //{
    //    get { return _yLabel; }
    //    set
    //    {
    //        _yLabel = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string PanToLabel => LocalizationManager.Instance[LocalizationResourceKeys.map_pan.ToString()];

    //private string _panToLabel;

    //public string PanToLabel
    //{
    //    get { return _panToLabel; }
    //    set
    //    {
    //        _panToLabel = value;
    //        RaisePropertyChanged();
    //    }
    //}

    public string ZoomToLabel => LocalizationManager.Instance[LocalizationResourceKeys.map_zoomTo.ToString()];

    //private string _zoomToLabel;

    //public string ZoomToLabel
    //{
    //    get { return _zoomToLabel; }
    //    set
    //    {
    //        _zoomToLabel = value;
    //        RaisePropertyChanged();
    //    }
    //}


    //public string Note { get => string.Empty; }

    //private LanguageMode _uiLanguage;

    //public LanguageMode UILanguage
    //{
    //    get { return _uiLanguage; }
    //    set
    //    {
    //        _uiLanguage = value;
    //        RaisePropertyChanged();

    //        UpdateUI();
    //    }
    //}

    //private void UpdateUI()
    //{
    //    //bool isPersian = this.UILanguage == LanguageMode.Persian;

    //    //this.FlowDirection = isPersian ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

    //    //this.XLabel = isPersian ? "طول جغرافیایی" : "Longitude";

    //    //this.YLabel = isPersian ? "عرض جغرافیایی" : "Latitude";

    //    //this.PanToLabel = isPersian ? "حرکت" : "Pan To";

    //    //this.ZoomToLabel = isPersian ? "بزرگ‌نمایی" : "Zoom To";
    //}

    //public event PropertyChangedEventHandler PropertyChanged;

    //protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}

    //public IRI.Jab.Common.Model.Language Language
    //{
    //    get { return (IRI.Jab.Common.Model.Language)GetValue(LanguageProperty); }
    //    set { SetValue(LanguageProperty, value); }
    //}

    //// Using a DependencyProperty as the backing store for Language.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty LanguageProperty =
    //    DependencyProperty.Register("Language", typeof(IRI.Jab.Common.Model.Language), typeof(GoToGeodeticView), new PropertyMetadata(Common.Model.Language.Persian));


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
