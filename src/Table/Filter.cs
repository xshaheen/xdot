using System.Collections.Generic;
using X.Table;

namespace X.Table
{
    public sealed class Filter
    {
        public Filter(string property, string comparison, string value)
        {
            Property   = property;
            Comparison = comparison;
            Value      = value;
        }

        public string Property { get; init; }

        public string Comparison { get; init; }

        public string Value { get; init; }
    }

    public sealed class Filters : List<Filter> { }
}
