using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using X.Core.Utils;

namespace X.Core.Extensions {
    /// <summary>
    /// Cast to string implicitly or explicitly to get the string.
    /// </summary>
    public class SearchString {
        private readonly string _str;

        public SearchString(string str) => _str = str;

        public static implicit operator string(SearchString s) {
            return s._str;
        }
    }

    /// <summary>
    /// Utility functions used by to prepare a text to search and index.
    /// </summary>
    public static class StringExtensionsSearchString {
        /// <summary>
        /// Normalize string to search optimized.
        /// <para>* Remove any accent from the string.</para>
        /// <para>* Convert any digit to english equivalent.</para>
        /// </summary>
        public static SearchString SearchString(this string? input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return new SearchString("");
            }

            var withoutAccentAndSymbols =
                from ch in input[..].Trim().OneSpace().ToLower().Normalize(NormalizationForm.FormD)
                let category = CharUnicodeInfo.GetUnicodeCategory(ch)
                where
                    category == UnicodeCategory.LowercaseLetter
                    || category == UnicodeCategory.OtherLetter
                    || category == UnicodeCategory.DecimalDigitNumber
                select ch;

            var str = withoutAccentAndSymbols.ConvertDigitsToEnglishDigits();

            return new SearchString(str);
        }

        /// <summary>
        /// Opt in arabic normalization for search. Normalization is done in-place for efficiency.
        /// <para>Extra normalization is:</para>
        /// <para>* Replace teh marbuta to heh.</para>
        /// <para>* Replace alef maksura (dotless yeh) to yeh.</para>
        /// <para>* Removal of tatweel (stretching character)..</para>
        /// </summary>
        public static string SupportAr(this SearchString input) {
            var removes = new[] { Ar.Tatweel };

            var replaces = new Dictionary<char, char> { ['ة'] = 'ه', ['ى'] = 'ي' };

            // ('ڪ', 'ك'),
            // ('ڮ', 'ك'),
            // ('ڥ', 'ف'),
            // ('ۻ', 'ض'),
            // ('۽', 'ء'),
            // ('؀', '\0'), // Arabic Number Sign
            // ('؁', '\0'), // Arabic Sign Sanah

            var sb = new StringBuilder();

            foreach (var cur in (string) input) {
                if (removes.Any(r => r == cur)) {
                    continue;
                }

                var toReplace = replaces.Keys.Any(k => k == cur);

                sb.Append(toReplace ? replaces[cur] : cur);
            }

            return sb.ToString();
        }
    }
}
