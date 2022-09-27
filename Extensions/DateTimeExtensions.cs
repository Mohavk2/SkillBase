using System;
using System.Collections.Generic;
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
    }
}
