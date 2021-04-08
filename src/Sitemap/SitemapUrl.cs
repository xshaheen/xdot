using System;
using System.Linq;
using JetBrains.Annotations;

namespace X.Sitemap {
    /// <summary>Represents sitemap URL node.</summary>
    [PublicAPI]
    public record SitemapUrl {
        private readonly SitemapAlternateUrls? _alternateUrls;
        private readonly string?               _location;

        /// <summary>
        /// The full URL of the page.
        /// </summary>
        public string Location {
            get => _location!;
            init => _location = value.ToLowerInvariant();
        }

        /// <summary>
        /// Alternate localized URLs of the page
        /// </summary>
        public SitemapAlternateUrls? AlternateUrls {
            get => _alternateUrls;
            init {
                if (value is not null && value.Urls.Any(u
                    => !u.Location.Equals(Location, StringComparison.InvariantCultureIgnoreCase))) {
                    // Make sure that base location is included as alternate to itself
                    // as well if there is any alternates (Google Guidelines).
                    _alternateUrls = new SitemapAlternateUrls {
                        DefaultLanguageCode = value.DefaultLanguageCode,
                        Urls = value.Urls.Append(new SitemapAlternateUrl {
                            Location     = Location,
                            LanguageCode = value.DefaultLanguageCode,
                        }),
                    };
                    return;
                }

                _alternateUrls = value;
            }
        }

        /// <summary>
        /// The date of last modification of the page. Currently (2021) google ignore it.
        /// </summary>
        public float? Priority { get; init; }

        /// <summary>
        /// How frequently the page is likely to change
        /// </summary>
        public DateTime? LastModified { get; init; }

        /// <summary>
        /// The priority of that URL relative to other URLs on the site. This allows webmasters
        /// to suggest to crawlers which pages are considered more important.
        /// Currently (2021) google ignore it.
        /// </summary>
        public ChangeFrequency? ChangeFrequency { get; init; }
    }
}
