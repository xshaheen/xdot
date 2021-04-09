using System;
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
            writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");

            // write sitemaps URL
            foreach (var sitemapRef in sitemapReferences) {
                await _WriteSitemapRefNode(writer, sitemapRef);
            }

            await writer.WriteEndElementAsync();
        }

        private static async Task _WriteSitemapRefNode(
            XmlWriter writer,
            SitemapReference sitemapRef
        ) {
            writer.WriteStartElement("sitemap");

            var loc = Uri.EscapeUriString(sitemapRef.Location);

            writer.WriteElementString("loc", loc);

            if (sitemapRef.LastModified.HasValue) {
                writer.WriteElementString(
                    "lastmod",
                    sitemapRef.LastModified.Value.ToString(SitemapConstants.SitemapDateFormat)
                );
            }

            await writer.WriteEndElementAsync();
        }
    }
}
