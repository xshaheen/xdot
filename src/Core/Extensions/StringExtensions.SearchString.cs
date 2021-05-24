using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using X.Core.Utils;

namespace X.Core.Extensions {
    /// <summary>
    /// Cast to string implicitly or explicitly to get the string.
    /// </summary>
    public class SearchString {
        private readonly string _str;

        public SearchString(string str) {
            _str = str;
        }

        public static implicit operator string(SearchString s) {
            return s._str;
        }
    }

    /// <summary>Utility functions used by to prepare a text to search and index.</summary>
    [PublicAPI]
    public static class StringExtensionsSearchString {
        private static Dictionary<char, string> _replaces = new() {
            // Alef
            [Ar.AlefMadda]      = Ar.Alef.ToString(),
            [Ar.AlefHamzaAbove] = Ar.Alef.ToString(),
            [Ar.AlefHamzaBelow] = Ar.Alef.ToString(),
            [Ar.AlefWasla]      = Ar.Alef.ToString(),
            [Ar.HamzaAbove]     = Ar.Alef.ToString(),
            [Ar.HamzaBelow]     = Ar.Alef.ToString(),
            // Hamza
            [Ar.WawHamza] = Ar.Hamza.ToString(),
            [Ar.YehHamza] = Ar.Hamza.ToString(),
            // Lam alef
            [Ar.LamAlef]           = Ar.Lam + Ar.Alef.ToString(),
            [Ar.LamAlefHamzaAbove] = Ar.Lam + Ar.Alef.ToString(),
            [Ar.LamAlefHamzaBelow] = Ar.Lam + Ar.Alef.ToString(),
            [Ar.LamAlefMaddaAbove] = Ar.Lam + Ar.Alef.ToString(),
            // Uthmani symbols
            [Ar.SmallAlef] = "",
            [Ar.SmallWaw]  = "",
            [Ar.SmallYeh]  = "",
            // Common spell errors
            [Ar.TehMarbuta]  = Ar.Heh.ToString(),
            [Ar.AlefMaksura] = Ar.Yeh.ToString(),
            // Yeh like
            ['ی'] = Ar.Yeh.ToString(), // Farsi Yeh
            ['ۍ'] = Ar.Yeh.ToString(), // Yeh With Tail
            ['ێ'] = Ar.Yeh.ToString(), // Yeh With Small V
            ['ؠ'] = Ar.Yeh.ToString(), // Arabic Letter Kashmiri Yeh
            ['ې'] = Ar.Yeh.ToString(), // E
            ['ۑ'] = Ar.Yeh.ToString(), // Yeh With Three Dots Below
            ['ؽ'] = Ar.Yeh.ToString(), // Farsi Yeh With Inverted V
            ['ؾ'] = Ar.Yeh.ToString(), // Farsi Yeh With Two Dots Above
            ['ؿ'] = Ar.Yeh.ToString(), // Farsi Yeh With Three Dots Above
            // Waw like
            ['ۏ'] = Ar.Waw.ToString(), // Waw With Dot Above
            ['ۋ'] = Ar.Waw.ToString(), // Ve
            ['ۊ'] = Ar.Waw.ToString(), // Waw With Two Dots Above
            ['ۉ'] = Ar.Waw.ToString(), // Kirghiz Yu
            ['ۈ'] = Ar.Waw.ToString(), // Yu
            ['ۇ'] = Ar.Waw.ToString(), // U
            ['ۆ'] = Ar.Waw.ToString(), // Oe
            ['ۅ'] = Ar.Waw.ToString(), // Kirghiz Oe
            ['ۄ'] = Ar.Waw.ToString(), // Waw With Ring
            // Lam like
            ['ڵ'] = Ar.Lam.ToString(), // Lam With Small V
            ['ڶ'] = Ar.Lam.ToString(), // Lam With Dot Above
            ['ڷ'] = Ar.Lam.ToString(), // Lam With Three Dots Above
            ['ڸ'] = Ar.Lam.ToString(), // Lam With Three Dots Below
            // Kaf like
            ['ػ'] = Ar.Kaf.ToString(), // Keheh With Two Dots Above
            ['ؼ'] = Ar.Kaf.ToString(), // Keheh With Three Dots Below
            ['ک'] = Ar.Kaf.ToString(), // Keheh
            ['ڪ'] = Ar.Kaf.ToString(), // Swash Kaf
            ['ګ'] = Ar.Kaf.ToString(), // Kaf With Ring
            ['ڬ'] = Ar.Kaf.ToString(), // Kaf With Dot Above
            ['ڭ'] = Ar.Kaf.ToString(), // Ng
            ['ڮ'] = Ar.Kaf.ToString(), // Kaf With Three Dots Below
            ['گ'] = Ar.Kaf.ToString(), // Gaf
            ['ڰ'] = Ar.Kaf.ToString(), // Gaf With Ring
            ['ڱ'] = Ar.Kaf.ToString(), // Ngoeh
            ['ڲ'] = Ar.Kaf.ToString(), // Gaf With Two Dots Below
            ['ڳ'] = Ar.Kaf.ToString(), // Gueh
            ['ڴ'] = Ar.Kaf.ToString(), // Gaf With Three Dots Above
            // Hef like
            ['ۿ'] = Ar.Heh.ToString(), // Heh With Inverted V
            ['ھ'] = Ar.Heh.ToString(), // Heh Doachashmee
            ['ۀ'] = Ar.Heh.ToString(), // Heh With Yeh Above
            ['ہ'] = Ar.Heh.ToString(), // Heh Goal
            ['ۂ'] = Ar.Heh.ToString(), // Heh Goal With Hamza Above
            ['ۃ'] = Ar.Heh.ToString(), // Teh Marbuta Goal
            // Dal like
            ['ۮ'] = Ar.Dal.ToString(),  // Dal With Inverted V
            ['ڈ'] = Ar.Dal.ToString(),  // Ddal
            ['ډ'] = Ar.Dal.ToString(),  // Dal With Ring
            ['ڊ'] = Ar.Dal.ToString(),  // Dal With Dot Below
            ['ڋ'] = Ar.Dal.ToString(),  // Dal With Dot Below And Small Tah
            ['ڍ'] = Ar.Dal.ToString(),  // Ddahal
            ['ڌ'] = Ar.Thal.ToString(), // Dahal
            ['ڎ'] = Ar.Thal.ToString(), // Dul
            ['ڏ'] = Ar.Thal.ToString(), // Dal With Three Dots Above Downwards
            ['ڐ'] = Ar.Thal.ToString(), // Dal With Four Dots Above
            // Qaf like
            ['ٯ'] = Ar.Qaf.ToString(), // Dotless Qaf
            // Beh like
            ['ٮ'] = Ar.Beh.ToString(), // Dotless Beh
            ['ﺑ'] = Ar.Beh.ToString(),
            // Reh like
            ['ۯ'] = Ar.Reh.ToString(), // Reh With Inverted V
            // Feh like
            ['ڥ'] = Ar.Feh.ToString(),
            // Dad like
            ['ۻ'] = Ar.Dad.ToString(),
            // Jeem like
            ['ﺞ'] = Ar.Jeem.ToString(),
        };

        /// <summary>
        /// Normalize string to search optimized. Remove any accent from the
        /// string and Convert any digit to english equivalent.
        /// </summary>
        [Pure]
        public static SearchString SearchString(this string? input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return new SearchString("");
            }

            var withoutAccentAndSymbols =
                from ch in input[..]
                    .Trim()
                    .OneSpace()
                    .ToLowerInvariant()
                    .Normalize(NormalizationForm.FormD)
                let category = CharUnicodeInfo.GetUnicodeCategory(ch)
                where category
                    is UnicodeCategory.LowercaseLetter
                    or UnicodeCategory.OtherLetter
                    or UnicodeCategory.DecimalDigitNumber
                select ch;

            var str = withoutAccentAndSymbols.ToInvariantDigits();

            return new SearchString(str);
        }

        /// <summary>
        /// Opt in arabic normalization for search.
        /// <para>Extra normalization is:</para>
        /// <para>* Replace teh marbuta to heh.</para>
        /// <para>* Replace alef maksura (dotless yeh) to yeh.</para>
        /// <para>* Removal of tatweel (stretching character)..</para>
        /// </summary>
        [Pure]
        public static string SupportAr(this SearchString input) {
            var removes = new[] {
                Ar.Tatweel,
            };

            var sb = new StringBuilder();

            foreach (var cur in (string) input) {
                if (removes.Any(r => r == cur)) {
                    continue;
                }

                sb.Append(_replaces.TryGetValue(cur, out var replace) ? replace : cur);
            }

            return sb.ToString();
        }

        // /// <summary>Check that text not contain non Arabic characters.</summary>
        // /// <param name="text">Unicode text</param>
        // /// <returns>True if all characters are in Arabic block.</returns>
        // [Pure]
        // public static bool IsArabicText(this string text) {
        //     return !Regex.IsMatch(
        //         text,
        //         $@"[^\u0600-\u0652{Ar.LamAlef}{Ar.LamAlefHamzaAbove}{Ar.LamAlefMaddaAbove}\w]");
        // }
        //
        // /// <summary>Check that text is a valid Arabic word.</summary>
        // /// <param name="word">An unicode word.</param>
        // /// <returns>True if all characters are in Arabic block and has a valid Arabic word syntax.</returns>
        // [Pure]
        // public static bool IsArabicWord(this string word) {
        //     if (word.Length == 0) {
        //         return false;
        //     }
        //
        //     if (!word.IsArabicText()) {
        //         return false;
        //     }
        //
        //     // If word has Alef Maksura in the middle
        //     if (Regex.IsMatch(word, $@"^(.)*[{Ar.AlefMaksura}](.)+$")) {
        //         return false;
        //     }
        //
        //     // If word has Teh Marbuta in the middle
        //     if (Regex.IsMatch(word,
        //         $@"^(.)*[{Ar.TehMarbuta}]([^{Ar.Damma}{Ar.Kasra}{Ar.Fatha}])(.)+$")) {
        //         return false;
        //     }
        //
        //     return false;
        // }
    }
}
