using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Extensions
{
    public static class PersianCalendarExtensions
    {

        public static DateTime GetBeginningOfThePersianYear(this PersianCalendar calendar, DateTime dateTime)
        {
            var year = calendar.GetYear(dateTime);

            return new DateTime(year, 1, 1, calendar);
        }

        public static DateTime GetBeginningOfThePersianMonth(this PersianCalendar calendar, DateTime dateTime)
        {
            var year = calendar.GetYear(dateTime);

            var month = calendar.GetMonth(dateTime);

            return new DateTime(year, month, 1, calendar);
        }

        public static string GetPersianMonthName(this PersianCalendar calendar, DateTime dateTime)
        {
            var monthIndex = calendar.GetMonth(dateTime) - 1;

            return persianMonths[monthIndex];
        }

        public static string GetYearMonthString(this PersianCalendar calendar, DateTime dateTime)
        {
            return $"{calendar.GetPersianMonthName(dateTime)} {calendar.GetYear(dateTime)}";
        }

        public static string GetDateTimeString(this Calendar calendar, DateTime dateTime)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                $"{calendar.GetYear(dateTime)}/{calendar.GetMonth(dateTime)}/{calendar.GetDayOfMonth(dateTime)}-{calendar.GetHour(dateTime)}:{calendar.GetMinute(dateTime)}:{calendar.GetSecond(dateTime)}");
        }

        public static string GetShortDateString(this Calendar calendar, DateTime dateTime)
        {
            return string.Format(CultureInfo.InvariantCulture, $"{calendar.GetYear(dateTime)}/{calendar.GetMonth(dateTime)}/{calendar.GetDayOfMonth(dateTime)}");
        }

        public static string GetLongDateString(this PersianCalendar calendar, DateTime dateTime)
        {
            return string.Format(CultureInfo.InvariantCulture, $"{calendar.GetYear(dateTime)}/{calendar.GetMonth(dateTime)}/{calendar.GetDayOfMonth(dateTime)}");
        }

        public static DateTime GetBeginningOfThePersianWeek(this PersianCalendar calendar, DateTime dateTime)
        {
            var dayOfWeek = ((int)calendar.GetDayOfWeek(dateTime) + 1) % 7;

            return dateTime.Date.AddDays(-dayOfWeek);
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
