using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;
using X.Core.Extensions;
using X.Core.Utils;

namespace X.Sitemap {
    /// <summary>Sitemap file builder</summary>
    /// <remarks>https://developers.google.com/search/docs/advanced/sitemaps/build-sitemap</remarks>
    [PublicAPI]
    public static class SitemapUrls {
        /// <summary>
        /// Write sitemap file into the stream
        /// </summary>
        public static async Task WriteAsync(
            this IEnumerable<SitemapUrl> sitemapUrls,
            Stream stream
        ) {
            var settings = new XmlWriterSettings {
                Async        = true,
                Indent       = true,
                Encoding     = StringHelper.Utf8WithoutBom,
                NewLineChars = "\n",
            };

            await using var writer = XmlWriter.Create(stream, settings);

            await writer.WriteStartDocumentAsync();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            // writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            // writer.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml");
            // writer.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

            // write URLs to the sitemap
            foreach (var sitemapUrl in sitemapUrls) {
                // write base url
                await _WriteUrlNodeAsync(writer, sitemapUrl);

                // if not has alternates continue
                if (sitemapUrl.AlternateUrls is null) {
                    continue;
                }

                await _WriteAlternateUrlsAsync(writer, sitemapUrl);
            }

            await writer.WriteEndElementAsync();
        }

        /// <summary>
        /// Generate sitemap and separate it if exceeded max urls in single file.
        /// <see cref="SitemapConstants.MaxSitemapUrls"/>
        /// </summary>
        public static async Task<List<MemoryStream>> WriteAsync(
            this IEnumerable<SitemapUrl> sitemapUrls
        ) {
            //split URLs into separate lists based on the max size
            var sitemaps = sitemapUrls
                .Select((url, index) => new { Index = index, Value = url })
                .GroupBy(group => group.Index / SitemapConstants.MaxSitemapUrls)
                .Select(group => group.Select(url => url.Value)).ToList();

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

        private static async Task _WriteUrlNodeAsync(
            XmlWriter writer,
            SitemapUrl sitemapUrl
        ) {
            if (string.IsNullOrEmpty(sitemapUrl.Location)) {
                return;
            }

            writer.WriteStartElement("url");

            var loc = Uri.EscapeUriString(sitemapUrl.Location);

            // loc = await XmlHelper.XmlEncodeAsync(loc);

            writer.WriteElementString("loc", loc);

            if (sitemapUrl.AlternateUrls is not null) {
                await _WriteAlternateUrlsReferenceAsync(writer, sitemapUrl.AlternateUrls.Urls);
            }

            if (sitemapUrl.Priority is not null) {
                writer.WriteElementString(
                    "priority",
                    sitemapUrl.Priority.Value.ToString("N1")
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

            await writer.WriteEndElementAsync();
        }

        private static async Task _WriteAlternateUrlsAsync(
            XmlWriter writer,
            SitemapUrl sitemapUrl
        ) {
            var alternates = sitemapUrl.AlternateUrls!.Urls.Where(p => !p.Location.Equals(
                sitemapUrl.Location,
                StringComparison.InvariantCultureIgnoreCase)
            ).ToArray();

            // write all alternate URLs
            foreach (var alternate in alternates) {
                var urlNode = new SitemapUrl {
                    Location = alternate.Location,
                    AlternateUrls = new SitemapAlternateUrls {
                        DefaultLanguageCode = alternate.LanguageCode,
                        Urls                = sitemapUrl.AlternateUrls.Urls,
                    },
                };

                await _WriteUrlNodeAsync(writer, urlNode);
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

                // extract seo code
                var altLoc = await XmlHelper.XmlEncodeAsync(alternate.Location);

                writer.WriteStartElement("xhtml:link");
                writer.WriteAttributeString("rel", "alternate");
                writer.WriteAttributeString("hreflang", alternate.LanguageCode);
                writer.WriteAttributeString("href", altLoc);

                await writer.WriteEndElementAsync();
            }
        }
    }
}
