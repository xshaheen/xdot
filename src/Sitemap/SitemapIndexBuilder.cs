using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;

namespace X.Sitemap {
    /// <summary>Sitemap index file builder</summary>
    /// <remarks>https://developers.google.com/search/docs/advanced/sitemaps/large-sitemaps</remarks>
    [PublicAPI]
    public static class SitemapIndexBuilder {
        /// <summary>
        /// Write sitemap index file into the stream
        /// </summary>
        public static async Task WriteAsync(
            this List<SitemapReference> sitemapReferences,
            Stream stream
        ) {
            await using var writer = XmlWriter.Create(stream, SitemapConstants.WriterSettings);

            await writer.WriteStartDocumentAsync();

            await writer.WriteStartElementAsync(
                null,
                "sitemapindex",
                "http://www.sitemaps.org/schemas/sitemap/0.9"
            );

            // write sitemaps URL
            foreach (var sitemapRef in sitemapReferences) {
                await _WriteSitemapRefNodeAsync(writer, sitemapRef);
            }

            await writer.WriteEndElementAsync();
        }

        private static async Task _WriteSitemapRefNodeAsync(
            XmlWriter writer,
            SitemapReference sitemapRef
        ) {
            await writer.WriteStartElementAsync(null, "sitemap", null);

            await writer.WriteElementStringAsync(null, "loc", null, sitemapRef.Location);

            if (sitemapRef.LastModified.HasValue) {
                await writer.WriteElementStringAsync(
                    null,
                    "lastmod",
                    null,
                    sitemapRef.LastModified.Value.ToString(SitemapConstants.SitemapDateFormat, CultureInfo.InvariantCulture)
                );
            }

            await writer.WriteEndElementAsync();
        }
    }
}
