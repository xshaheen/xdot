﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using X.Sitemap;
using Xunit;

namespace Sitemap.Tests {
    public class SitemapUrlsTests : TestBase {
        public static List<object[]> TestData => new() {
            // basic
            new object[] {
                new List<SitemapUrl> {
                    new("https://www.example.com"),
                    new("https://www.example.com/contact-us"),
                },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com</loc>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/contact-us</loc>" +
                "  </url>" +
                "</urlset>",
            },
            // with priority, last modified, change frequency
            new object[] {
                new List<SitemapUrl> {
                    new("https://www.example.com",
                        new DateTime(2021, 3, 15),
                        ChangeFrequency.Daily,
                        0.8f),
                },
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "   <loc>https://www.example.com</loc>" +
                "   <priority>0.8</priority>" +
                "   <changefreq>daily</changefreq>" +
                "   <lastmod>2021-03-15</lastmod>" +
                "  </url>" +
                "</urlset>",
            },
            // Urls follow RFC-3986
            new object[] {
                new List<SitemapUrl> {
                    new("https://www.Example.com/ümlaT.html"),
                    new("https://www.example.com/اداره-اعلانات"),
                },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/%C3%BCmlat.html</loc>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/%D8%A7%D8%AF%D8%A7%D8%B1%D9%87-%D8%A7%D8%B9%D9%84%D8%A7%D9%86%D8%A7%D8%AA</loc>" +
                "  </url>" +
                "</urlset>",
            },
            // XML entity escape URLs
            new object[] {
                new List<SitemapUrl> { new("https://www.example.com/ümlat.html&q=name"), },
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/%C3%BCmlat.html&amp;q=name</loc>" +
                "  </url>" +
                "</urlset>",
            },
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task Write_to_stream_test(List<SitemapUrl> urls, string expected) {
            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            AssertEquivalentXml(result, expected);
        }

        [Fact]
        public async Task Write__should_add_xhtml_namespace__when_define_alternatives() {
            var urls = new List<SitemapUrl> {
                new(new SitemapAlternateUrl[] {
                    new() {
                        Location     = "https://www.example.com/ar/page.html",
                        LanguageCode = "ar",
                    },
                    new() {
                        Location     = "https://www.example.com/en/page.html",
                        LanguageCode = "en",
                    },
                }),
            };

            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            result.Should().Contain("xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"");
        }

        [Fact]
        public async Task Write__should_write_alternative_urls__when_provide_any() {
            var urls = new List<SitemapUrl> {
                new(new SitemapAlternateUrl[] {
                    new() {
                        Location     = "https://www.example.com/english/page.html",
                        LanguageCode = "en",
                    },
                    new() {
                        Location     = "https://www.example.com/deutsch/page.html",
                        LanguageCode = "de",
                    },
                    new() {
                        Location     = "https://www.example.com/schweiz-deutsch/page.html",
                        LanguageCode = "de-ch",
                    },
                }),
            };

            const string expected =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<urlset xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                "  <url>" +
                "    <loc>https://www.example.com/english/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/deutsch/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "  <url>" +
                "    <loc>https://www.example.com/schweiz-deutsch/page.html</loc>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"https://www.example.com/english/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"https://www.example.com/deutsch/page.html\"/>" +
                "    <xhtml:link rel=\"alternate\" hreflang=\"de-ch\" href=\"https://www.example.com/schweiz-deutsch/page.html\"/>" +
                "  </url>" +
                "</urlset>";

            string result;

            await using (var stream = new MemoryStream()) {
                await urls.WriteAsync(stream);
                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            AssertEquivalentXml(result, expected);
        }
    }
}
