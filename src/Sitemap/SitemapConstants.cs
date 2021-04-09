using System.Xml;
using X.Core.Utils;

namespace X.Sitemap {
    public static class SitemapConstants {
        /// <summary>
        /// Gets a date and time format for the sitemap.
        /// </summary>
        public const string SitemapDateFormat = @"yyyy-MM-dd";

        /// <summary>
        /// Max urls in single sitemap according to Google.
        /// </summary>
        public const int MaxSitemapUrls = 50_000;

        internal static readonly XmlWriterSettings WriterSettings = new() {
            Async        = true,
            Indent       = true,
            Encoding     = StringHelper.Utf8WithoutBom,
            NewLineChars = "\n",
        };
    }
}
