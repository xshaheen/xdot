﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace X.Sitemap {
    /// <summary>Represents sitemap alternates URLs.</summary>
    [PublicAPI]
    public record SitemapAlternateUrls {
        /// <summary>
        /// Gets language/region codes (in ISO 639-1 format) and optionally
        /// a region (in ISO 3166-1 Alpha 2 format) of an alternate URL
        /// Example ar-eg
        /// </summary>
        /// <remarks>
        /// ISO 639-1: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
        /// ISO 3166-1 Alpha 2: https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
        /// </remarks>
        public string DefaultLanguageCode { get; init; } = default!;

        /// <summary>
        /// Gets alternate URLs.
        /// </summary>
        public IEnumerable<SitemapAlternateUrl> Urls { get; init; }  = default!;
    }
}
