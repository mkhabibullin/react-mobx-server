using System;
using System.Text.RegularExpressions;

namespace TimeReport.Extensions
{
    public static class StringExtensions
    {
        public static float ParseFloat(this string value)
        {
            var match = Regex.Match(value, @"([-+]?[0-9]*\.?[0-9]+)");
            if (match.Success)
                return Convert.ToSingle(match.Groups[1].Value);
            return 0;
        }

        public static DateTime ParseDate(this string value)
        {
            var monthsRu = new[] { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
            var monthsEn = new[] { "january ", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };

            var todayRu = "сегодня";
            var todayEn = "today";

            var yesterdayRu = "вчера";
            var yesterdayEn = "yesterday";

            var date = DateTime.UtcNow;

            if(value.Contains(yesterdayRu) || value.Contains(yesterdayEn))
            {
                date = date.AddDays(-1);
            }
            else if(!value.Contains(todayRu) && !value.Contains(todayEn))
            {
                string month;
                int day;

                var regexMatchesRu = Regex.Match(value, @".*?(\d+) (.+)");
                if (regexMatchesRu.Success)
                {
                    month = regexMatchesRu.Groups[2].Value;
                    int.TryParse(regexMatchesRu.Groups[1].Value, out day);
                }
                else
                {
                    var regexMatchesEn = Regex.Match(value, @".*?(\w+) (\d+)");
                    if (regexMatchesEn.Success)
                    {
                        month = regexMatchesEn.Groups[1].Value;
                        int.TryParse(regexMatchesEn.Groups[2].Value, out day);
                    }
                    else
                    {
                        throw new Exception($"Fail to extract month from {value}");
                    }

                }
                var monthIndexRu = Array.IndexOf(monthsRu, month.ToLower().Trim());
                var monthIndexEn = Array.IndexOf(monthsEn, month.ToLower().Trim());
                var monthIndex = monthIndexRu >= 0 ? monthIndexRu : monthIndexEn;
                if (monthIndex < 0) throw new Exception($"Month cannot be defined by '{month}'. The source is '{value}'");

                date = new DateTime(date.Year, monthIndex + 1, day);
            }

            return date;
        }
    }
}
