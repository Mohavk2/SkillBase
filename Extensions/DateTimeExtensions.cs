using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime GetTime(this DateTime dateTime)
        {
            return new(dateTime.Ticks - dateTime.Date.Ticks);
        }
        public static DateTime SetTime(this DateTime date, DateTime time)
        {
            return new(date.Date.Ticks + time.GetTime().Ticks);
        }
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new(date.Year, date.Month, 1);
        }
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            DateTime monthStart = new(date.Year, date.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);
            return nextMonthStart.AddDays(-1);
        }
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
        public static DateTime GetFirstDayOfYear(this DateTime date)
        {
            return new(date.Year, 1, 1);
        }
        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
    }
}
