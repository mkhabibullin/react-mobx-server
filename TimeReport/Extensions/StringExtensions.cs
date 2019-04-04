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
    }
}
