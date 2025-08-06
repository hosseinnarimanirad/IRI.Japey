using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace IRI.Maptor.Jab.Common.Localization;

public class LanguageSelectorViewModel : Notifier
{
    private readonly LocalizationManager _localization = LocalizationManager.Instance;

    public LanguageSelectorViewModel()
    {
        // Initialize available languages
        AvailableLanguages = new ObservableCollection<LanguageItem>
        {
            new LanguageItem(new CultureInfo("en-UK")), // English
            //new LanguageItem(new CultureInfo("fr-FR")), // French
            new LanguageItem(new CultureInfo("fa-IR")), // Persian
            //new LanguageItem(new CultureInfo("ar-SA"))  // Arabic
        };

        // Set current language
        _selectedLanguage = AvailableLanguages.FirstOrDefault(x =>
            x.Culture.Name == _localization.CurrentCulture.Name)
            ?? AvailableLanguages[0];

        _localization.LanguageChanged += OnLanguageChanged;
    }

    public ObservableCollection<LanguageItem> AvailableLanguages { get; }

    private LanguageItem _selectedLanguage;
    public LanguageItem SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage != value && value != null)
            {
                _selectedLanguage = value;
                _localization.SetCulture(value.Culture);
                RaisePropertyChanged(nameof(SelectedLanguage));
            }
        }
    }

    private void OnLanguageChanged()
    {
        // Update selected item when language changes externally
        var newSelected = AvailableLanguages.FirstOrDefault(x =>
            x.Culture.Name == _localization.CurrentCulture.Name);

        if (newSelected != null && _selectedLanguage != newSelected)
        {
            _selectedLanguage = newSelected;
            RaisePropertyChanged(nameof(SelectedLanguage));
        }
    }
     
}
