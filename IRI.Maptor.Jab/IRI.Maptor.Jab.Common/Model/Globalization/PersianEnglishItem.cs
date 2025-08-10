//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using IRI.Maptor.Extensions;

//namespace IRI.Maptor.Jab.Common.Model.Globalization;

//public class PersianEnglishItem : ValueObjectNotifier
//{
//    private readonly string _persianTitle;

//    public string PersianTitle
//    {
//        get { return _persianTitle; }
//        //private set
//        //{
//        //    if (_persianTitle == value)
//        //        return;

//        //    _persianTitle = value;
//        //    RaisePropertyChanged();
//        //    RaisePropertyChanged(nameof(Title));
//        //}
//    }

//    private readonly string _englishTitle;

//    public string EnglishTitle
//    {
//        get { return _englishTitle; }
//        //private set
//        //{
//        //    if (_englishTitle == value)
//        //        return;

//        //    _englishTitle = value;
//        //    RaisePropertyChanged();
//        //    RaisePropertyChanged(nameof(Title));
//        //}
//    }

//    public string Title
//    {
//        get { return UILanguage == LanguageMode.Persian ? PersianTitle : EnglishTitle; }
//    }

//    private LanguageMode _uiLanguage;

//    public LanguageMode UILanguage
//    {
//        get { return _uiLanguage; }
//        set
//        {
//            if (_uiLanguage == value)
//                return;

//            _uiLanguage = value;
//            RaisePropertyChanged();
//            RaisePropertyChanged(nameof(Title));
//        }
//    }

//    public PersianEnglishItem(string persianTitle, string englishTitle, LanguageMode uiLanguage = LanguageMode.Persian)
//    {
//        this._persianTitle = persianTitle?.ArabicToFarsi()?.Trim();

//        this._englishTitle = englishTitle?.Trim();

//        this.UILanguage = uiLanguage;
//    }

//    public override string ToString()
//    {
//        return Title;
//    }

//    public static PersianEnglishItem CreateEnglish(string persianTitle, string englishTitle)
//    {
//        return new PersianEnglishItem(persianTitle, englishTitle, LanguageMode.English);
//    }

//    public static PersianEnglishItem CreateUpperCasedEnglish(string persianTitle, string englishTitle)
//    {
//        return new PersianEnglishItem(persianTitle, englishTitle?.ToUpperInvariant(), LanguageMode.English);
//    }


//    public static PersianEnglishItem CreateFarsi(string persianTitle, string englishTitle)
//    {
//        return new PersianEnglishItem(persianTitle, englishTitle, LanguageMode.Persian);
//    }

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        yield return PersianTitle;
//        yield return EnglishTitle?.ToUpperInvariant();
//    }
//}
