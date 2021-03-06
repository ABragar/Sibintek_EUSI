﻿using System;
using System.Text.RegularExpressions;

namespace WebUI.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Строка заканчивается точкой, воскл. знаком или вопр. знаком
        /// </summary>
        private static readonly Regex Reg = new Regex(@"(?:\.|\!|\?)$", RegexOptions.Compiled);

        /// <summary>
        /// Строка заканчивается запятой и/или словом из одного-двух символов
        /// </summary>
        private static readonly Regex _reg2 = new Regex(@"(\s*,\s?\w{0,2}|\s+\w{1,2})$", RegexOptions.Compiled);
        public static string Truncate(this string str, int length, bool breakWord = false)
        {
            if (str == null || str.Length < length)
                return str;

            if (breakWord)
            {
                return str.Substring(0, length - 3) + "...";
            }

            var iNextSpace = str.LastIndexOf(" ", length, StringComparison.Ordinal);

            var result = str.Substring(0, iNextSpace > 0 ? iNextSpace : length).Trim();

            // строка оканчивается завершающим предложение знаком препинания
            // . ? !
            if (Reg.IsMatch(result))
                return result;

            // убирает с конца запятую и очень короткие слова (один-два символа)
            result = $"{_reg2.Replace(result, "")}...";

            return result;
        }
    }
}