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
    }
}
