using System.Collections.Generic;
using JetBrains.Annotations;

namespace X.Table {
    [PublicAPI]
    public sealed class Filter {
        public Filter(string property, string comparison, string value) {
            Property   = property;
            Comparison = comparison;
            Value      = value;
        }

        public string Property { get; init; }

        public string Comparison { get; init; }

        public string Value { get; init; }
    }

    [PublicAPI]
    public sealed class Filters : List<Filter> { }
}
