namespace X.Core.Utils {
    /// <summary>
    /// Arabic letters & classification
    /// </summary>
    public class Ar {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Arabic_script_in_Unicode
        /// <para>
        /// Arabic (0600–06FF, 255 characters) --  is a Unicode block, containing the
        /// standard letters and the most common diacritics of the Arabic script, and
        /// the Arabic-Indic digits.
        /// See: https://www.unicode.org/charts/PDF/U0600.pdf
        /// </para>
        /// <para>
        /// Arabic Supplement (0750–077F, 48 characters) -- is a Unicode block that
        /// encodes Arabic letter variants used for writing non-Arabic languages,
        /// including languages of Pakistan and Africa, and old Persian.
        /// See: https://www.unicode.org/charts/PDF/U0750.pdf
        /// </para>
        /// <para>
        /// Arabic Presentation Forms-A (FB50–FDFF, 611 characters) --  a Unicode block encoding
        /// contextual forms and ligatures of letter variants needed for Persian, Urdu, Sindhi
        /// and Central Asian languages. This block also encodes 32 noncharacters in Unicode.
        /// See: https://www.unicode.org/charts/PDF/UFB50.pdf
        /// </para>
        /// <para>
        /// Arabic Presentation Forms-B (FE70–FEFF, 141 characters) -- a Unicode block encoding
        /// spacing forms of Arabic diacritics, and contextual letter forms. The special codepoint,
        /// ZWNBSP is also here, which is used as a BOM.
        /// See: https://www.unicode.org/charts/PDF/UFE70.pdf
        /// </para>
        /// <para>
        /// Arabic Extended-A (08A0–08FF, 84 characters) is a Unicode block encoding Qur'anic
        /// annotations and letter variants used for various non-Arabic languages.
        /// See: https://www.unicode.org/charts/PDF/U08A0.pdf
        /// </para>
        /// </summary>
        public const string ArabicRange =
            @"[\u0600-\u06FF\u0750-\u077F\uFB50-\uFDFF\uFE70-\uFEFF\u08A0–\u08FF]";

        /// <summary>Arabic Comma '،'</summary>
        public const char Comma = '\u060C';

        /// <summary>Arabic Semicolon '؛'</summary>
        public const char Semicolon = '\u061B';

        /// <summary>Arabic Tatweel 'ـ'</summary>
        public const char Tatweel = '\u0640';
    }
}
