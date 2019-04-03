using System;
using System.Text.RegularExpressions;

namespace Base.Utils.Common
{
    public static class StringExtensions
    {
        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;

            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);

            string ret = $"{input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()}...";

            return ret;
        }

        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = $"{(span.Duration().Days > 0 ? $"{span.Days:0} дней, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} часов, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} минут, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} секунд" : string.Empty)}";

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 секунд";

            return formatted;
        }

        public static string Sanitize(this string stringValue)
        {
            return stringValue?
                .Replace("'", "''")
                .RegexReplace("-{2,}", "-")  // transforms multiple --- in - use to comment in sql scripts
                .RegexReplace(@"[*/]+", string.Empty)      // removes / and * used also to comment in sql scripts
                .RegexReplace(@"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
        }

        private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith);
        }

        private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
        }
    }
}
