using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Globalization
{
    public class PersianEnglishItem : Notifier
    {
        private string _persianTitle;

        public string PersianTitle
        {
            get { return _persianTitle; }
            set
            {
                if (_persianTitle == value)
                    return;

                _persianTitle = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _englishTitle;

        public string EnglishTitle
        {
            get { return _englishTitle; }
            set
            {
                if (_englishTitle == value)
                    return;

                _englishTitle = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        public string Title
        {
            get { return UILanguage == LanguageMode.Persian ? PersianTitle : EnglishTitle; }
        }


        private LanguageMode _uiLanguage;

        public LanguageMode UILanguage
        {
            get { return _uiLanguage; }
            set
            {
                if (_uiLanguage == value)
                    return;

                _uiLanguage = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        public PersianEnglishItem(string persianTitle, string englishTitle, LanguageMode uiLanguage = LanguageMode.Persian)
        {
            this._persianTitle = persianTitle;

            this._englishTitle = englishTitle;

            this.UILanguage = uiLanguage;
        }
    }
}
