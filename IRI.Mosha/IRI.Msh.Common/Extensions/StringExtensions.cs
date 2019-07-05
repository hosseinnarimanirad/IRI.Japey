namespace System
{
    public static class StringExtensions
    {
        const char arabicYa01 = '\u0649';
        const char arabicYa02 = '\u064A';
        const char arabicKaf = '\u0643';

        const char farsiYa = '\u06CC';
        const char farsiKaf = '\u06A9';

        public static string ArabicToFarsi(this string arabicString)
        {
            if (string.IsNullOrWhiteSpace(arabicString))
                return arabicString;

            return arabicString
                .Replace(arabicYa01, farsiYa)
                .Replace(arabicYa02, farsiYa)
                .Replace(arabicKaf, farsiKaf);
        }

        public static string LatinNumbersToFarsiNumbers(this string value)
        {
            return value.Replace('1', '۱')
                    .Replace('2', '۲')
                    .Replace('3', '۳')
                    .Replace('4', '۴')
                    .Replace('5', '۵')
                    .Replace('6', '۶')
                    .Replace('7', '۷')
                    .Replace('8', '۸')
                    .Replace('9', '۹')
                    .Replace('0', '۰')
                    .Replace('.', '\u066B');
        }

        public static string FarsiNumbersToLatinNumbers(this string value)
        {
            return value.Replace('۱', '1')
                        .Replace('۲', '2')
                        .Replace('۳', '3')
                        .Replace('۴', '4')
                        .Replace('۵', '5')
                        .Replace('۶', '6')
                        .Replace('۷', '7')
                        .Replace('۸', '8')
                        .Replace('۹', '9')
                        .Replace('۰', '0')
                    .Replace('\u066B', '.');
        }

        public static bool ContainsIgnoreCase(this string value, string term)
        {
            return value.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool EqualsIgnoreCase(this string theString, string value)
        {
            return theString.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ToBase64String(this string value)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        public static string Base64ToNormalString(this string base64String)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64String));

            
        }

        
    }
}
