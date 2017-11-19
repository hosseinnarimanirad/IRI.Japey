using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static System.Globalization.PersianCalendar _calendar = new System.Globalization.PersianCalendar();

        public static string ToPersianAlphabeticDate(this DateTime dateTime)
        {
            try
            {
                var year = _calendar.GetYear(dateTime);

                var month = _calendar.GetPersianMonthName(dateTime);

                var day = _calendar.GetDayOfMonth(dateTime);

                return $"{day} {month} {year}";
            }
            catch //ArgumentOutOfRangeException
            {
                return string.Empty;
            }
        }

        public static string ToLongPersianDateTime(this DateTime time)
        {
            try
            {
                var year = _calendar.GetYear(time);

                var month = _calendar.GetMonth(time);

                var day = _calendar.GetDayOfMonth(time);

                return $" تاریخ {day:00}-{month:00}-{year}  ساعت {time.Hour:00}:{time.Minute:00}  {time.GetPersianAmPm()} ";
            }
            catch //ArgumentOutOfRangeException
            {
                return string.Empty;
            }
        }

        public static bool IsAM(this DateTime time)
        {
            return time.ToString("tt").ToUpper() == "AM";
        }

        public static string GetPersianAmPm(this DateTime time)
        {
            return time.IsAM() ? "ق.ظ" : "ب.ظ";
        }
    }
}
