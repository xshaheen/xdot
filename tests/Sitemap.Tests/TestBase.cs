using System.Xml.Linq;
using FluentAssertions;
using X.Core.Extensions;

namespace Sitemap.Tests {
    public class TestBase {
        protected static void AssertEquivalentXml(string result, string expected) {
            result   = XDocument.Parse(result).ToInvariantString();
            expected = XDocument.Parse(expected).ToInvariantString();

            result.Should().Be(expected);
        }
    }
}
