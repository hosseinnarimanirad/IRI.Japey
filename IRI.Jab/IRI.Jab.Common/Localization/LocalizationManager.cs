using IRI.Jab.Common.Properties;
using System;
using System.ComponentModel;
using System.Globalization;

namespace IRI.Jab.Common;

public class LocalizationManager //: INotifyPropertyChanged
{
    private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

    public static LocalizationManager Instance { get; } = new LocalizationManager();

    // Custom event to avoid PropertyChanged overhead
    public event Action LanguageChanged;


    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (_currentCulture != value)
            {
                _currentCulture = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.CurrentCulture = value;
                LanguageChanged?.Invoke();
            }
        }
    }


    //public string this[string key] => Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);

    public string this[string key]
    {
        get
        {
            // Explicitly use the current culture and prevent caching issues
            var resourceSet = Resources.ResourceManager.GetResourceSet(
                CurrentCulture,
                true,  // load if not found
                false); // don't use cached resources
            return resourceSet?.GetString(key) ?? $"#{key}#";
        }
    }

    //public void SetCulture(CultureInfo culture)
    //{
    //    CultureInfo.CurrentUICulture = culture;
    //    LanguageChanged?.Invoke();
    //}
}