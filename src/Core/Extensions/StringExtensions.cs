using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace X.Core.Extensions {
    [PublicAPI]
    public static partial class StringExtensions {
        /// <summary>
        /// Returns null if the string is null or empty or white spaces
        /// otherwise return a trim-ed string.
        /// </summary>
        public static string? NullableTrim(this string? input) {
            return input.IsNullOrWhiteSpace() ? null : input.Trim();
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str) {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string str) {
            return str.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Replace any white space characters [\r\n\t\f\v ] with one white space.
        /// </summary>
        public static string OneSpace(this string input) {
            return Regex.Replace(input, @"\s+", " ");
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(
            this string str,
            char c,
            StringComparison comparisonType = StringComparison.Ordinal
        ) {
            Guard.Against.Null(str, nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType)) {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(
            this string str,
            char c,
            StringComparison comparisonType = StringComparison.Ordinal
        ) {
            Guard.Against.Null(str, nameof(str));
            if (str.EndsWith(c.ToString(), comparisonType)) {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str) {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options) {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct {
            return (T) Enum.Parse(typeof(T), Guard.Against.Null(value, nameof(value)), ignoreCase);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len) {
            Guard.Against.Null(str, nameof(str));

            if (str.Length < len) {
                throw new ArgumentException(
                    "len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len) {
            Guard.Against.Null(str, nameof(str));

            if (str.Length < len) {
                throw new ArgumentException(
                    "len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Replace first occurrence of <paramref name="search"/> in the string <paramref name="str"/>
        /// with <paramref name="replace"/>.
        /// </summary>
        public static string ReplaceFirst(
            this string str,
            string search,
            string replace,
            StringComparison comparisonType = StringComparison.Ordinal
        ) {
            Guard.Against.Null(str, nameof(str));

            var pos = str.IndexOf(search, comparisonType);

            return pos < 0
                ? str
                : str.Substring(0, pos) + replace + str[(pos + search.Length)..];
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string? RemovePostfix(this string? str, params string[]? postFixes) {
            return str.RemovePostfix(StringComparison.Ordinal, postFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="postfixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string? RemovePostfix(
            this string? str,
            StringComparison comparisonType,
            params string[]? postfixes
        ) {
            if (str.IsNullOrEmpty()) {
                return null;
            }

            if (postfixes.IsNullOrEmpty()) {
                return str;
            }

            foreach (var postFix in postfixes) {
                if (str.EndsWith(postFix, comparisonType)) {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="prefixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        public static string? RemovePrefix(this string? str, params string[]? prefixes) {
            return str.RemovePrefix(StringComparison.Ordinal, prefixes);
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="prefixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        public static string? RemovePrefix(
            this string? str,
            StringComparison comparisonType,
            params string[]? prefixes
        ) {
            if (str.IsNullOrEmpty()) {
                return null;
            }

            if (prefixes.IsNullOrEmpty()) {
                return str;
            }

            foreach (var preFix in prefixes) {
                if (str.StartsWith(preFix, comparisonType)) {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string? Truncate(this string? str, int maxLength) {
            if (str is null) {
                return null;
            }

            if (str.Length <= maxLength) {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from Ending of the string if it exceeds maximum length.
        /// </summary>
        public static string? TruncateFromBeginning(this string? str, int maxLength) {
            if (str is null) {
                return null;
            }

            if (str.Length <= maxLength) {
                return str;
            }

            return str.Right(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        public static string? TruncateWithPostfix(
            this string? str,
            int maxLength,
            string postfix = "..."
        ) {
            if (str == null) {
                return null;
            }

            if (str == string.Empty || maxLength == 0) {
                return string.Empty;
            }

            if (str.Length <= maxLength) {
                return str;
            }

            if (maxLength <= postfix.Length) {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static byte[] GetBytes(this string str) {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>
        /// </summary>
        public static byte[] GetBytes(this string str, Encoding encoding) {
            Guard.Against.Null(str, nameof(str));
            Guard.Against.Null(encoding, nameof(encoding));

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Gets index of nth occurrence of a char in a string.
        /// </summary>
        /// <param name="str">source string to be searched</param>
        /// <param name="c">Char to search in <see cref="str"/></param>
        /// <param name="n">Count of the occurrence</param>
        public static int NthIndexOf(this string str, char c, int n) {
            Guard.Against.Null(str, nameof(str));

            var count = 0;
            for (var i = 0; i < str.Length; i++) {
                if (str[i] != c) {
                    continue;
                }

                if (++count == n) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Replace a new string applying on it <see cref="string.Replace(string, string)"/>
        /// using <paramref name="replaces"/>.
        /// </summary>
        public static string Replace(
            this string str,
            IEnumerable<(string oldValue, string newValue)> replaces
        ) {
            var output = str[..];

            foreach (var (oldValue, newValue) in replaces) {
                output = output.Replace(oldValue, newValue);
            }

            return output;
        }

        /// <summary>
        /// Replace a new string applying on it <see cref="string.Replace(char, char)"/>
        /// using <paramref name="replaces"/>.
        /// </summary>
        public static string Replace(
            this string str,
            IEnumerable<(char oldValue, char newValue)> replaces
        ) {
            var output = str[..];

            foreach (var (oldValue, newValue) in replaces) {
                output = output.Replace(oldValue, newValue);
            }

            return output;
        }

        /// <summary>
        /// Convert any digit in the string to the equivalent english digit.
        /// <para>"١٢٨" to "128"</para>
        /// <para>"١,٢٨" to "1,28"</para>
        /// <para>"١.٢٨" to "1.28"</para>
        /// <para>"This ١٢٨" to "This 128"</para>
        /// </summary>
        public static string ConvertDigitsToEnglishDigits(this string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return input;
            }

            var sb = new StringBuilder();

            foreach (var c in input) {
                if (char.IsDigit(c)) {
                    sb.Append(char.GetNumericValue(c).ToString(CultureInfo.InvariantCulture));
                }
                else {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert any digit in the string to the equivalent english digit.
        /// <para>"١٢٨" to "128"</para>
        /// <para>"١,٢٨" to "1,28"</para>
        /// <para>"١.٢٨" to "1.28"</para>
        /// <para>"This ١٢٨" to "This 128"</para>
        /// </summary>
        public static string ConvertDigitsToEnglishDigits(this IEnumerable<char> input) {
            var sb = new StringBuilder();
            foreach (var c in input) {
                if (char.IsDigit(c)) {
                    sb.Append(char.GetNumericValue(c).ToString(CultureInfo.InvariantCulture));
                }
                else {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Remove accents (diacritics) from the string.
        /// <para>"crème brûlée" to "creme-brulee"</para>
        /// <para>"بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ" to "بسم الله الرحمن الرحيم"</para>
        /// </summary>
        /// <remarks>
        /// Remarks:
        /// <para>* Normalize to FormD splits accented letters in letters+accents.</para>
        /// <para>* Remove those accents (and other non-spacing characters).</para>
        /// <para>* Return a new string from the remaining chars.</para>
        /// </remarks>
        public static string RemoveAccent(this string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return input;
            }

            var cs =
                from ch in input.Normalize(NormalizationForm.FormD)
                let category = CharUnicodeInfo.GetUnicodeCategory(ch)
                where category is not UnicodeCategory.NonSpacingMark
                select ch;

            var sb = new StringBuilder();

            foreach (var c in cs) {
                sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Compares two strings, character by character, and returns the
        /// first position where the two strings differ from one another.
        /// </summary>
        /// <param name="s1">
        /// The first string to compare
        /// </param>
        /// <param name="s2">
        /// The second string to compare
        /// </param>
        /// <returns>
        /// The first position where the two strings differ.
        /// </returns>
        public static int StringDifference(this string s1, string s2) {
            var len1 = s1.Length;
            var len2 = s2.Length;
            var len = len1 < len2 ? len1 : len2;

            for (var i = 0; i < len; i++) {
                if (s1[i] != s2[i]) {
                    return i;
                }
            }

            return len;
        }
    }
}
