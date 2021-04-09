using System;
using JetBrains.Annotations;
using X.Core.Extensions;

namespace X.Sitemap {
    /// <summary>Represents sitemap alternate URL node.</summary>
    [PublicAPI]
    public record SitemapAlternateUrl {
        private readonly string? _location;

        /// <summary>
        /// Alternate url.
        /// </summary>
        public string Location {
            get => _location!;
            init => _location = Uri.EscapeUriString(value.ToLowerInvariant().RemoveHiddenChars());
        }

        /// <summary>
        /// Language/region codes (in ISO 639-1 format) and optionally
        /// a region (in ISO 3166-1 Alpha 2 format) of an alternate URL
        /// Example ar-eg
        /// </summary>
        /// <remarks>
        /// ISO 639-1: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
        /// ISO 3166-1 Alpha 2: https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
        /// </remarks>
        public string LanguageCode { get; init; } = default!;
    }
}
