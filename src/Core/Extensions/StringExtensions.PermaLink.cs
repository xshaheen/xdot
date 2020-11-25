using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ardalis.GuardClauses;

namespace X.Core.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Convert the string to SEO optimized and valid url.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        public static string PermaLink(this string input) => ExcludeNonAlpha(input);

        /// <summary>
        /// Convert the string to SEO optimized and valid url.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <param name="key">A unique identifier to append at the end to make uri unique.</param>
        /// <returns></returns>
        public static string PermaLink(this string input, string key) => $"{ExcludeNonAlpha(input)}-{key.Truncate(10)}";

        private static string ExcludeNonAlpha(string input)
        {
            Guard.Against.NullOrEmpty(input, nameof(input));

            var result = input.Trim()
                .Replace(
                    new[]
                    {
                        ("&", " And "),
                        ("#", " Sharp "),
                        ("+", " Plus "),
                        ("%", " Percent "),
                        ("$", " Dollar "),
                    })
                .NoAccent()
                .NoSymbols()
                .FirstUpper();

            var temp = new Regex(@" +").Replace(string.Concat(result), " ").Trim();

            return new Regex(@" +").Replace(temp, "-");
        }

        private static IEnumerable<char> NoAccent(this string input)
        {
            var cs =
                from ch in input.Normalize(NormalizationForm.FormD)
                let category = CharUnicodeInfo.GetUnicodeCategory(ch)
                where category != UnicodeCategory.NonSpacingMark
                select ch;

            foreach (var c in cs) yield return c;
        }

        private static IEnumerable<char> NoSymbols(this IEnumerable<char> input)
            => input.Select(c => (char.IsPunctuation(c) || char.IsSymbol(c)) && c != '.' ? ' ' : c);

        private static IEnumerable<char> FirstUpper(this IEnumerable<char> str)
        {
            var newWord = true;

            foreach (var c in str)
            {
                if (char.IsDigit(c))
                {
                    yield return c;
                    continue;
                }

                if (!char.IsLetter(c))
                {
                    yield return c;
                    newWord = true;
                    continue;
                }

                if (newWord)
                {
                    yield return char.ToUpper(c);
                    newWord = false;
                }
                else
                {
                    yield return c;
                }
            }
        }
    }
}
