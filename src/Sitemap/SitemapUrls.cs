using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;
using X.Core.Extensions;

namespace X.Sitemap {
    /// <summary>Sitemap file builder</summary>
    /// <remarks>https://developers.google.com/search/docs/advanced/sitemaps/build-sitemap</remarks>
    [PublicAPI]
    public static class SitemapUrls {
        /// <summary>
        /// Generate sitemap and separate it if exceeded max urls in single file.
        /// <see cref="SitemapConstants.MaxSitemapUrls"/>
        /// </summary>
        public static async Task<List<MemoryStream>> WriteAsync(
            this ICollection<SitemapUrl> sitemapUrls
        ) {
            //split URLs into separate lists based on the max size
            var sitemaps = sitemapUrls
                .Select((url, index) => new { Index = index, Value = url })
                .GroupBy(group => group.Index / SitemapConstants.MaxSitemapUrls)
                .Select(group => group.Select(url => url.Value).ToArray()).ToList();

            var streams = new List<MemoryStream>();

            if (!sitemaps.Any()) {
                return streams;
            }

            foreach (var sitemap in sitemaps) {
                await using var stream = new MemoryStream();
                await sitemap.WriteAsync(stream);
                streams.Add(stream);
            }

            return streams;
        }

        /// <summary>
        /// Write sitemap file into the stream
        /// </summary>
        public static async Task WriteAsync(
            this ICollection<SitemapUrl> sitemapUrls,
            Stream stream
        ) {
            await using var writer = XmlWriter.Create(stream, SitemapConstants.WriterSettings);

            await writer.WriteStartDocumentAsync();
            await writer.WriteStartElementAsync(null, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            if (sitemapUrls.Any(i => i.AlternateLocations is not null)) {
                await writer.WriteAttributeStringAsync("xmlns",
                    "xhtml",
                    null,
                    "http://www.w3.org/1999/xhtml"
                );
            }

            // write URLs to the sitemap
            foreach (var sitemapUrl in sitemapUrls) {
                await _WriteUrlNodeAsync(writer, sitemapUrl);
            }

            await writer.WriteEndElementAsync();
        }

        private static async Task _WriteUrlNodeAsync(
            XmlWriter writer,
            SitemapUrl sitemapUrl
        ) {
            var hasAlternates = sitemapUrl.AlternateLocations is not null;

            if (!hasAlternates) {
                await writer.WriteStartElementAsync(null, "url", null);
                await writer.WriteElementStringAsync(null, "loc", null, sitemapUrl.Location!);
                _WriteOtherNodes(writer, sitemapUrl);
                await writer.WriteEndElementAsync();
                return;
            }

            // write with alternates

            foreach (var baseLoc in sitemapUrl.AlternateLocations!) {
                await writer.WriteStartElementAsync(null, "url", null);
                await writer.WriteElementStringAsync(null, "loc", null, baseLoc.Location);
                await _WriteAlternateUrlsReferenceAsync(writer, sitemapUrl.AlternateLocations);
                _WriteOtherNodes(writer, sitemapUrl);
                await writer.WriteEndElementAsync();
            }
        }

        private static async Task _WriteAlternateUrlsReferenceAsync(
            XmlWriter writer,
            IEnumerable<SitemapAlternateUrl> alternateUrls
        ) {
            foreach (var alternate in alternateUrls) {
                if (alternate.Location.IsNullOrWhiteSpace() ||
                    alternate.LanguageCode.IsNullOrWhiteSpace()) {
                    continue;
                }

                await writer.WriteStartElementAsync("xhtml", "link", null);
                await writer.WriteAttributeStringAsync(null, "rel", null, "alternate");
                await writer.WriteAttributeStringAsync(null, "hreflang", null, alternate.LanguageCode);
                await writer.WriteAttributeStringAsync(null, "href", null, alternate.Location);

                await writer.WriteEndElementAsync();
            }
        }

        private static void _WriteOtherNodes(XmlWriter writer, SitemapUrl sitemapUrl) {
            if (sitemapUrl.Priority is not null) {
                writer.WriteElementString(
                    "priority",
                    sitemapUrl.Priority.Value.ToString("N1", CultureInfo.InvariantCulture)
                );
            }

            if (sitemapUrl.ChangeFrequency is not null) {
                writer.WriteElementString(
                    "changefreq",
                    sitemapUrl.ChangeFrequency.Value.ToString().ToLowerInvariant()
                );
            }

            if (sitemapUrl.LastModified is not null) {
                writer.WriteElementString(
                    "lastmod",
                    sitemapUrl.LastModified.Value
                        .ToString(SitemapConstants.SitemapDateFormat, CultureInfo.InvariantCulture)
                );
            }
        }
    }
}
