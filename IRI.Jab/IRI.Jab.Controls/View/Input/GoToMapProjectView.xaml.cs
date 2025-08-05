using System;
using IRI.Jab.Common;
using IRI.Jab.Common.Localization;

namespace IRI.Jab.Controls.View;

/// <summary>
/// Interaction logic for GoToMapProjectView.xaml
/// </summary>
public partial class GoToMapProjectView : NotifiableUserControl, IDisposable
{
    public GoToMapProjectView()
    {
        InitializeComponent();

        LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }

    private void Instance_LanguageChanged()
    {
        RaisePropertyChanged(nameof(UtmZoneLabel));
    }

    public string UtmZoneLabel => LocalizationManager.Instance[LocalizationResourceKeys.srs_utmZone.ToString()];
    //public string UtmZone { get; set; }

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
