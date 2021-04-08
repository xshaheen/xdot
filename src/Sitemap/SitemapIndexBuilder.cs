using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;
using X.Core.Utils;

namespace X.Sitemap {
    /// <summary>Sitemap index file builder</summary>
    /// <remarks>https://developers.google.com/search/docs/advanced/sitemaps/large-sitemaps</remarks>
    [PublicAPI]
    public static class SitemapIndexBuilder {
        /// <summary>
        /// Write sitemap index file into the stream
        /// </summary>
        public static async Task WriteAsync(
            Stream stream,
            List<SitemapReference> sitemapReferences
        ) {
            await using var writer = new XmlTextWriter(stream, Encoding.UTF8) {
                Formatting = Formatting.Indented,
            };

            writer.WriteStartDocument();
            writer.WriteStartElement("sitemapindex");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            // writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            // writer.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml");
            // writer.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

            // write sitemaps URL
            foreach (var sitemapRef in sitemapReferences) {
                await _WriteSitemapRefNode(writer, sitemapRef);
            }

            writer.WriteEndElement();
        }

        private static async Task _WriteSitemapRefNode(
            XmlTextWriter writer,
            SitemapReference sitemapRef
        ) {
            var location = await XmlHelper.XmlEncodeAsync(sitemapRef.Location);

            writer.WriteStartElement("sitemap");
            writer.WriteElementString("loc", location);

            if (sitemapRef.LastModified.HasValue) {
                writer.WriteElementString(
                    "lastmod",
                    sitemapRef.LastModified.Value.ToString(SitemapConstants.SitemapDateFormat)
                );
            }

            writer.WriteEndElement();
        }
    }
}
