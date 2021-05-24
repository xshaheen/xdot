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
    /// <summary>
    /// Provides a set of extension methods for operations on String.
    /// </summary>
    [PublicAPI]
    public static partial class StringExtensions {
        /// <summary>
        /// Indicates whether the specified string is <c>null</c> or an <see cref="string.Empty">Empty</see> string.
        /// </summary>
        /// <param name="input">The string to test.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="input"/> is <c>null</c> or an empty string (""); otherwise, <c>false</c>.
        /// </returns>
        /// <seealso cref="string.IsNullOrEmpty" />
        [Pure]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? input) {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Indicates whether a specified string is <c>null</c>, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="input">A <see cref="string" /> value.</param>
        /// <returns>
        /// <c>true</c> if the value parameter is <c>null</c> or <see cref="string.Empty" />, or if <paramref name="input"/> consists exclusively of white-space characters.
        /// </returns>
        /// <seealso cref="string.IsNullOrWhiteSpace" />
        [Pure]
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? input) {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>Return the specified string if it is not <see cref="string.Empty">Empty</see>, or <c>null</c> otherwise.</summary>
        /// <param name="input">The string to test.</param>
        /// <example>
        /// <code>var displayName = name.NullIfEmpty() ?? "Unknown";</code>
        /// </example>
        /// <returns>
        /// <paramref name="input"/> if it is an empty string (""); otherwise, <c>null</c>.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? NullIfEmpty(this string? input) {
            return !string.IsNullOrEmpty(input) ? input : null;
        }

        /// <summary>Return the specified string if it is not white-space characters, <see cref="string.Empty">Empty</see>, or <c>null</c> otherwise.</summary>
        /// <param name="input">The string to test.</param>
        /// <example>
        /// <code>var displayName = name.NullIfWhiteSpace() ?? "Unknown";</code>
        /// </example>
        /// <returns>
        /// <paramref name="input"/> if the value parameter is <c>null</c> or <see cref="string.Empty" />, or if <paramref name="input"/> consists exclusively of white-space characters; otherwise, <c>null</c>.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? NullIfWhiteSpace(this string? input) {
            return !string.IsNullOrWhiteSpace(input) ? input : null;
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        [Pure]
        public static string NormalizeLineEndings(this string value) {
            return value.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        [Pure]
        public static string[] SplitToLines(
            this string input,
            StringSplitOptions options = StringSplitOptions.None
        ) {
            return input.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        [Pure]
        public static string EnsureStartsWith(
            this string input,
            char c,
            StringComparison comparisonType = StringComparison.Ordinal
        ) {
            if (input.StartsWith(c.ToString(), comparisonType)) {
                return input;
            }

            return c + input;
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        [Pure]
        public static string EnsureEndsWith(
            this string input,
            char c,
            StringComparison comparisonType = StringComparison.Ordinal
        ) {
            if (input.EndsWith(c.ToString(), comparisonType)) {
                return input;
            }

            return input + c;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="postfixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? RemovePostfix(this string? input, params string[]? postfixes) {
            return input.RemovePostfix(StringComparison.Ordinal, postfixes);
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="postfixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? RemovePostfix(
            this string? input,
            StringComparison comparisonType,
            params string[]? postfixes
        ) {
            if (string.IsNullOrEmpty(input)) {
                return null;
            }

            if (postfixes.IsNullOrEmpty()) {
                return input;
            }

            foreach (var postfix in postfixes) {
                if (input.EndsWith(postfix, comparisonType)) {
                    return input[..^postfix.Length];
                }
            }

            return input;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="prefixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? RemovePrefix(this string? input, params string[]? prefixes) {
            return input.RemovePrefix(StringComparison.Ordinal, prefixes);
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="prefixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? RemovePrefix(
            this string? input,
            StringComparison comparisonType,
            params string[]? prefixes
        ) {
            if (input.IsNullOrEmpty()) {
                return null;
            }

            if (prefixes.IsNullOrEmpty()) {
                return input;
            }

            foreach (var prefix in prefixes) {
                if (input.StartsWith(prefix, comparisonType)) {
                    var len = input.Length - prefix.Length;
                    return input.Substring(input.Length - len, len);
                }
            }

            return input;
        }

        /// <summary>
        /// Limits string length to a specified value by discarding any trailing characters after the specified length.
        /// </summary>
        /// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
        /// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
        /// <returns>
        /// The <paramref name="input" /> string if its length is lesser or equal than
        /// <paramref name="maxLength"/>; otherwise first <c>n</c> characters of the <paramref name="input" />,
        /// where <c>n</c> is equal to <paramref name="maxLength"/>.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? TruncateEnd(this string? input, [NonNegativeValue] int maxLength) {
            if (input is null) {
                return null;
            }

            return input.Length <= maxLength ? input : input[..maxLength];
        }

        /// <summary>
        /// Limits string length to a specified value by discarding a number of trailing characters and adds a specified
        /// suffix (ex "...") if any characters were discarded.
        /// </summary>
        /// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
        /// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
        /// <param name="suffix">The suffix added to the result if any characters are discarded.</param>
        /// <returns>
        /// The <paramref name="input" /> string if its length is lesser or equal than
        /// <paramref name="maxLength"/>; otherwise first <c>n</c> characters of the <paramref name="input" />
        /// followed by <paramref name="suffix" />, where <c>n</c> is equal to <paramref name="maxLength"/> - length of
        /// <see cref="suffix"/>.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? TruncateEnd(
            this string? input,
            [NonNegativeValue] int maxLength,
            string suffix
        ) {
            if (input == null) {
                return null;
            }

            if (input == string.Empty || maxLength == 0) {
                return string.Empty;
            }

            if (input.Length <= maxLength) {
                return input;
            }

            if (maxLength <= suffix.Length) {
                return suffix[..maxLength];
            }

            return input[..(maxLength - suffix.Length)] + suffix;
        }

        /// <summary>
        /// Limits string length to a specified value by discarding any starting characters before the specified length.
        /// </summary>
        /// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
        /// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
        /// <returns>
        /// The <paramref name="input" /> string if its length is lesser or equal than
        /// <paramref name="maxLength"/>; otherwise last <c>n</c> characters of the <paramref name="input" />,
        /// where <c>n</c> is equal to <paramref name="maxLength"/>.
        /// </returns>
        [Pure]
        [return: NotNullIfNotNull("input")]
        public static string? TruncateStart(this string? input, int maxLength) {
            if (input is null) {
                return null;
            }

            return input.Length <= maxLength
                ? input
                : input.Substring(input.Length - maxLength, maxLength);
        }

        /// <summary>
        /// Replace a new string applying on it <see cref="string.Replace(string, string)"/>
        /// using <paramref name="replaces"/>.
        /// </summary>
        [Pure]
        public static string Replace(
            this string input,
            IEnumerable<(string oldValue, string newValue)> replaces
        ) {
            var output = input[..];

            foreach (var (oldValue, newValue) in replaces) {
                output = output.Replace(oldValue, newValue);
            }

            return output;
        }

        /// <summary>
        /// Replace a new string applying on it <see cref="string.Replace(char, char)"/>
        /// using <paramref name="replaces"/>.
        /// </summary>
        [Pure]
        public static string Replace(
            this string input,
            IEnumerable<(char oldValue, char newValue)> replaces
        ) {
            var output = input[..];

            foreach (var (oldValue, newValue) in replaces) {
                output = output.Replace(oldValue, newValue);
            }

            return output;
        }

        /// <summary>
        /// Returns null if the string is null or empty or white spaces
        /// otherwise return a trim-ed string.
        /// </summary>
        [return: NotNullIfNotNull("input")]
        public static string? NullableTrim(this string? input) {
            return input.IsNullOrWhiteSpace() ? null : input.Trim();
        }

        /// <summary>
        /// Replace any white space characters [\r\n\t\f\v ] with one white space.
        /// </summary>
        [Pure]
        public static string OneSpace(this string input) {
            return Regex.Replace(input, @"\s+", " ");
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        [Pure]
        public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct {
            return (T) Enum.Parse(typeof(T), Guard.Against.Null(value, nameof(value)), ignoreCase);
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        [Pure]
        public static byte[] GetBytes(this string input) {
            return input.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>
        /// </summary>
        [Pure]
        public static byte[] GetBytes(this string input, Encoding encoding) {
            return encoding.GetBytes(input);
        }

        /// <summary>
        /// Gets index of nth occurrence of a char in a string.
        /// </summary>
        /// <param name="input">source string to be searched</param>
        /// <param name="c">Char to search in <see cref="input"/></param>
        /// <param name="n">Count of the occurrence</param>
        [Pure]
        public static int NthIndexOf(this string input, char c, int n) {
            var count = 0;
            for (var i = 0; i < input.Length; i++) {
                if (input[i] != c) {
                    continue;
                }

                if (++count == n) {
                    return i;
                }
            }

            return -1;
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
        public static int Diff(this string s1, string s2) {
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

        /// <summary>
        /// Strips any single quotes or double quotes from the beginning and end of a string.
        /// </summary>
        public static string StripQuotes(this string s) {
            return Regex.Replace(s, "^\\s*['\"]+|['\"]+\\s*$", "");
        }

        /// <summary>
        /// True if the given string is a valid IPv4 address.
        /// </summary>
        public static bool IsIp4(this string s) {
            // based on https://stackoverflow.com/a/29942932/62600
            if (string.IsNullOrEmpty(s)) {
                return false;
            }

            var parts = s.Split('.');
            return parts.Length == 4 && parts.All(x => byte.TryParse(x, out _));
        }

        /// <summary>
        /// Convert any digit in the string to the equivalent english digit.
        /// <para>"١٢٨" to "128"</para>
        /// <para>"١,٢٨" to "1,28"</para>
        /// <para>"١.٢٨" to "1.28"</para>
        /// <para>"This ١٢٨" to "This 128"</para>
        /// </summary>
        [Pure]
        public static string ToInvariantDigits(this string input) {
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
        [Pure]
        public static string ToInvariantDigits(this IEnumerable<char> input) {
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
        public static string RemoveAccentCharacters(this string input) {
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
        /// Remove control characters from string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string RemoveHiddenChars(this string val) {
            return Regex.Replace(
                val,
                @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]",
                string.Empty,
                RegexOptions.Compiled
            );
        }
    }
}
