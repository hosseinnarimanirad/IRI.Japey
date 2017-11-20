using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Extensions
{
    public static class DateTimeExtensions
    {
        internal static readonly DateTime _midValidPersianDateTime = new DateTime(622, 3, 22);

        private static readonly System.Globalization.PersianCalendar _calendar = new System.Globalization.PersianCalendar();

        public static bool IsAM(this DateTime dateTime)
        {
            return dateTime.ToString("tt").ToUpper() == "AM";
        }

        public static string GetPersianAmPm(this DateTime dateTime)
        {
            return dateTime.IsAM() ? "ق.ظ" : "ب.ظ";
        }

        public static string ToPersianAlphabeticDate(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return ToPersianAlphabeticDate(_midValidPersianDateTime);

            var year = _calendar.GetYear(dateTime);

            var month = GetPersianMonthName(dateTime);

            var day = _calendar.GetDayOfMonth(dateTime);

            return $"{day:00} {month} {year:0000}";
        }

        public static string ToLongPersianDateTime(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return ToLongPersianDateTime(_midValidPersianDateTime);

            var year = _calendar.GetYear(dateTime);

            var month = _calendar.GetMonth(dateTime);

            var day = _calendar.GetDayOfMonth(dateTime);

            return $" تاریخ {day:00}-{month:00}-{year:0000}  ساعت {dateTime.Hour:00}:{dateTime.Minute:00}  {dateTime.GetPersianAmPm()} ";

        }

        public static string ToLongPersianDateTimeSimple(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return ToLongPersianDateTimeSimple(_midValidPersianDateTime);

            return FormattableString.Invariant(
                $"{_calendar.GetYear(dateTime):0000}/{_calendar.GetMonth(dateTime):00}/{_calendar.GetDayOfMonth(dateTime):00}-{_calendar.GetHour(dateTime):00}:{_calendar.GetMinute(dateTime):00}:{_calendar.GetSecond(dateTime):00}");
        }

        public static string ToPersianDate(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return ToPersianDate(_midValidPersianDateTime);

            return FormattableString.Invariant($"{_calendar.GetYear(dateTime):0000}/{_calendar.GetMonth(dateTime):00}/{_calendar.GetDayOfMonth(dateTime):00}");
        }

        public static string ToPersianYearMonth(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return ToPersianYearMonth(_midValidPersianDateTime);

            return $"{GetPersianMonthName(dateTime)} {_calendar.GetYear(dateTime)}";
        }

        public static DateTime GetBeginningOfThePersianYear(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return GetBeginningOfThePersianYear(_midValidPersianDateTime);

            var year = _calendar.GetYear(dateTime);

            return new DateTime(year, 1, 1, _calendar);
        }

        public static DateTime GetBeginningOfThePersianMonth(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return GetBeginningOfThePersianMonth(_midValidPersianDateTime);

            var year = _calendar.GetYear(dateTime);

            var month = _calendar.GetMonth(dateTime);

            return new DateTime(year, month, 1, _calendar);
        }

        public static DateTime GetBeginningOfThePersianWeek(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return GetBeginningOfThePersianWeek(_midValidPersianDateTime);

            var dayOfWeek = ((int)_calendar.GetDayOfWeek(dateTime) + 1) % 7;

            return dateTime.Date.AddDays(-dayOfWeek);
        }

        public static string GetPersianMonthName(this DateTime dateTime)
        {
            //to avoid ArgumentOutOfRangeException
            if (dateTime < _midValidPersianDateTime)
                return GetPersianMonthName(_midValidPersianDateTime);

            var monthIndex = _calendar.GetMonth(dateTime) - 1;

            return persianMonths[monthIndex];
        }

        static readonly string[] persianMonths = new string[]
        {
            "فروردین",
            "اردیبهشت",
            "خرداد",
            "تیر",
            "مرداد",
            "شهریور",
            "مهر",
            "آبان",
            "آذر",
            "دی",
            "بهمن",
            "اسفند",
        };
    }
}
