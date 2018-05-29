using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Utilities
{
    public static class DateTimeExtensions
    {
        public static String ToHumanReadable(this TimeSpan timespan)
        {
            if (Math.Round(timespan.TotalSeconds, 0) < 60) return $"{Math.Round(timespan.TotalSeconds, 0)} sec";
            else if (Math.Round(timespan.TotalMinutes, 0) < 60) return $"{Math.Round(timespan.TotalMinutes, 0)} min";
            else if (Math.Round(timespan.TotalHours, 0) < 24) return $"{Math.Round(timespan.TotalHours, 0)} hours";
            else if ((int)Math.Round(timespan.TotalDays, 0) == 1) return $"{Math.Round(timespan.TotalDays, 0)} day";
            else return $"{Math.Round(timespan.TotalDays, 0)} days";
        }

    }
}
