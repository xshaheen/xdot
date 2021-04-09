using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using X.Core.Extensions;

namespace X.Sitemap {
    /// <summary>Represents sitemap URL node.</summary>
    [PublicAPI]
    public record SitemapUrl {
        /// <summary>
        /// Create a sitemap URL.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="lastModified"></param>
        /// <param name="changeFrequency"></param>
        /// <param name="priority"></param>
        public SitemapUrl(
            string location,
            DateTime? lastModified = null,
            ChangeFrequency? changeFrequency = null,
            float? priority = null
        ) {
            Location        = Uri.EscapeUriString(location.ToLowerInvariant().RemoveHiddenChars());
            LastModified    = lastModified;
            ChangeFrequency = changeFrequency;
            Priority        = priority;
        }

        /// <summary>
        /// Create a sitemap URL that with its alternates
        /// </summary>
        /// <param name="alternateLocations"></param>
        /// <param name="lastModified"></param>
        /// <param name="changeFrequency"></param>
        /// <param name="priority"></param>
        public SitemapUrl(
            ICollection<SitemapAlternateUrl> alternateLocations,
            DateTime? lastModified = null,
            ChangeFrequency? changeFrequency = null,
            float? priority = null
        ) {
            AlternateLocations = alternateLocations;
            LastModified       = lastModified;
            ChangeFrequency    = changeFrequency;
            Priority           = priority;
        }

        /// <summary>
        /// The full URL of the page.
        /// </summary>
        public string? Location { get; }

        /// <summary>
        /// Alternate localized URLs of the page
        /// </summary>
        public IEnumerable<SitemapAlternateUrl>? AlternateLocations { get; }

        /// <summary>
        /// The date of last modification of the page. Currently (2021) google ignore it.
        /// </summary>
        public float? Priority { get; }

        /// <summary>
        /// How frequently the page is likely to change
        /// </summary>
        public DateTime? LastModified { get; }

        /// <summary>
        /// The priority of that URL relative to other URLs on the site. This allows webmasters
        /// to suggest to crawlers which pages are considered more important.
        /// Currently (2021) google ignore it.
        /// </summary>
        public ChangeFrequency? ChangeFrequency { get; }
    }
}
