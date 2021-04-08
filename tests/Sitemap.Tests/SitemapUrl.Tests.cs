using FluentAssertions;
using X.Sitemap;
using Xunit;

namespace Sitemap.Tests {
    public class SitemapUrlsTest {
        [Fact]
        public void AlternateUrls__should_include_location__when_it_has_any_alternate() {
            var sitemapUrl = new SitemapUrl {
                Location = "https://www.example.com",
                AlternateUrls = new SitemapAlternateUrls {
                    DefaultLanguageCode = "ar",
                    Urls = new SitemapAlternateUrl[] {
                        new() { Location = "https://www.example.com/en", LanguageCode = "en" },
                        new() { Location = "https://www.example.com/fr", LanguageCode = "fr" },
                    },
                },
            };

            sitemapUrl.AlternateUrls.Urls.Should().Contain(url
                => url.Location == "https://www.example.com" && url.LanguageCode == "ar");
        }
    }
}
