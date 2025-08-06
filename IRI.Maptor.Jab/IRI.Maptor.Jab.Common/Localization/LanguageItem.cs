using System;
using System.Globalization;
using System.Windows;

namespace IRI.Maptor.Jab.Common.Localization;

public class LanguageItem
{
    public CultureInfo Culture { get; }
    public string NativeName => Culture.NativeName;
    //public string FlagPath => $"/Assets/Images/Flags/{Culture.TwoLetterISOLanguageName}.png";
    public Uri FlagUri => new Uri(
         $"pack://application:,,,/IRI.Maptor.Jab.Common;component/Assets/Images/Flags/{Culture.TwoLetterISOLanguageName}.png",
         UriKind.Absolute);

    public FlowDirection TextFlowDirection =>
        Culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

    public LanguageItem(CultureInfo culture)
    {
        Culture = culture;
    }
} 