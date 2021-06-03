//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.Common.Extensions
//{
//    public static class PersianCalendarExtensions
//    {
//        //internal static readonly DateTime _midValidPersianDateTime = new DateTime(622, 3, 22);

//        //public static DateTime GetBeginningOfThePersianYear(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetBeginningOfThePersianYear(calendar, _midValidPersianDateTime);

//        //    var year = calendar.GetYear(dateTime);

//        //    return new DateTime(year, 1, 1, calendar);
//        //}

//        //public static DateTime GetBeginningOfThePersianMonth(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetBeginningOfThePersianMonth(calendar, _midValidPersianDateTime);

//        //    var year = calendar.GetYear(dateTime);

//        //    var month = calendar.GetMonth(dateTime);

//        //    return new DateTime(year, month, 1, calendar);
//        //}

//        //public static string GetPersianMonthName(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetPersianMonthName(calendar, _midValidPersianDateTime);

//        //    var monthIndex = calendar.GetMonth(dateTime) - 1;

//        //    return persianMonths[monthIndex];
//        //}

//        //public static string GetYearMonthString(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetYearMonthString(calendar, _midValidPersianDateTime);

//        //    return $"{calendar.GetPersianMonthName(dateTime)} {calendar.GetYear(dateTime)}";
//        //}

//        //public static string GetDateTimeString(this Calendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetDateTimeString(calendar, _midValidPersianDateTime);

//        //    return FormattableString.Invariant(
//        //        $"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00}-{calendar.GetHour(dateTime):00}:{calendar.GetMinute(dateTime):00}:{calendar.GetSecond(dateTime):00}");
//        //}

//        //public static string GetShortDateString(this Calendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetShortDateString(calendar, _midValidPersianDateTime);

//        //    return FormattableString.Invariant($"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00}");
//        //}

//        //public static string GetLongDateString(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetLongDateString(calendar, _midValidPersianDateTime);

//        //    return FormattableString.Invariant($"{calendar.GetYear(dateTime):0000}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00}");
//        //}

//        //public static DateTime GetBeginningOfThePersianWeek(this PersianCalendar calendar, DateTime dateTime)
//        //{
//        //    //to avoid ArgumentOutOfRangeException
//        //    if (dateTime < _midValidPersianDateTime)
//        //        return GetBeginningOfThePersianWeek(calendar, _midValidPersianDateTime);

//        //    var dayOfWeek = ((int)calendar.GetDayOfWeek(dateTime) + 1) % 7;

//        //    return dateTime.Date.AddDays(-dayOfWeek);
//        //}

//        //static readonly string[] persianMonths = new string[]
//        //{
//        //    "فروردین",
//        //    "اردیبهشت",
//        //    "خرداد",
//        //    "تیر",
//        //    "مرداد",
//        //    "شهریور",
//        //    "مهر",
//        //    "آبان",
//        //    "آذر",
//        //    "دی",
//        //    "بهمن",
//        //    "اسفند",
//        //};
//    }
//}
