using IRI.Jab.Common.Properties;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace IRI.Jab.Common;

public class LocalizationManager //: INotifyPropertyChanged
{
    private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

    public static LocalizationManager Instance { get; } = new LocalizationManager();

    // Custom event to avoid PropertyChanged overhead
    public event Action LanguageChanged;

    public event Action FlowDirectionChanged;

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        private set
        {
            if (_currentCulture != value)
            {
                _currentCulture = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.CurrentCulture = value;
                LanguageChanged?.Invoke();
                FlowDirectionChanged?.Invoke(); // New event
            }
        }
    }

    public FlowDirection CurrentFlowDirection =>
      IsRightToLeftLanguage(CurrentCulture) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

    private bool IsRightToLeftLanguage(CultureInfo culture)
    {
        // List of RTL language codes
        var rtlLanguages = new[] { "fa", "ar", "he" }; // Persian, Arabic, Hebrew
        return rtlLanguages.Contains(culture.TwoLetterISOLanguageName);
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
     
    //public bool IsFrench => CurrentCulture.Name.Equals("fr-FR", StringComparison.OrdinalIgnoreCase);
    
    public bool IsPersian => CurrentCulture.Name.Equals("fa-IR", StringComparison.OrdinalIgnoreCase);

    //public void SetCulture(CultureInfo culture)
    //{
    //    CultureInfo.CurrentUICulture = culture;
    //    LanguageChanged?.Invoke();
    //}

    public void SetCulture(CultureInfo culture)
    {
        CurrentCulture = culture;
    }

}