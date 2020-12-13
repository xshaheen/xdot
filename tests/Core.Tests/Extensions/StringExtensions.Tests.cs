using System;
using System.Text;
using FluentAssertions;
using X.Core.Extensions;
using X.Core.Utils;
using Xunit;

namespace Core.Tests.Extensions {
    public class StringExtensionsTests : IDisposable {
        private readonly IDisposable _cultureScope;

        public StringExtensionsTests() => _cultureScope = CultureHelper.Use("en-US");

        public void Dispose() => _cultureScope.Dispose();

        [Fact]
        public void Right_Test() {
            const string str = "This is a test string";

            str.Right(3).Should().Be("ing");
            str.Right(0).Should().Be("");
            str.Right(str.Length).Should().Be(str);
        }

        [Fact]
        public void Left_Test() {
            const string str = "This is a test string";

            str.Left(3).Should().Be("Thi");
            str.Left(0).Should().Be("");
            str.Left(str.Length).Should().Be(str);
        }

        [Fact]
        public void NormalizeLineEndings_Test() {
            const string str = "This\r\n is a\r test \n string";

            var normalized = str.NormalizeLineEndings();
            var lines      = normalized.SplitToLines();

            lines.Length.Should().Be(4);
        }

        [Fact]
        public void OneSpace() {
            "   ".OneSpace().Should().Be(" ");
            "\n\n\n".OneSpace().Should().Be(" ");
            "This\r\n is a\r test \n string".OneSpace().Should().Be("This is a test string");
        }

        [Fact]
        public void EnsureEndsWith_Test() {
            // Expected use-cases
            "Test".EnsureEndsWith('!').Should().Be("Test!");
            "Test!".EnsureEndsWith('!').Should().Be("Test!");
            @"C:\test\folderName".EnsureEndsWith('\\').Should().Be(@"C:\test\folderName\");
            @"C:\test\folderName\".EnsureEndsWith('\\').Should().Be(@"C:\test\folderName\");
            "Sarı".EnsureEndsWith('ı').Should().Be("Sarı");

            // Case differences
            "Egypt".EnsureEndsWith('T').Should().Be("EgyptT");
        }

        [Fact]
        public void EnsureEndsWith_CultureSpecific_Test() {
            using (CultureHelper.Use("tr-TR"))
                "Kırmızı".EnsureEndsWith('I', StringComparison.CurrentCultureIgnoreCase).Should()
                    .Be("Kırmızı");
        }

        [Fact]
        public void EnsureStartsWith_Test() {
            // Expected use-cases
            "Test".EnsureStartsWith('~').Should().Be("~Test");
            "~Test".EnsureStartsWith('~').Should().Be("~Test");

            // Case differences
            "Egypt".EnsureStartsWith('t').Should().Be("tEgypt");
        }

        [Fact]
        public void NthIndexOf_Test() {
            const string str = "This is a test string";

            str.NthIndexOf('i', 0).Should().Be(-1);
            str.NthIndexOf('i', 1).Should().Be(2);
            str.NthIndexOf('i', 2).Should().Be(5);
            str.NthIndexOf('i', 3).Should().Be(18);
            str.NthIndexOf('i', 4).Should().Be(-1);
        }

        [Fact]
        public void Truncate_Test() {
            const string  str       = "This is a test string";
            const string? nullValue = null;

            str.Truncate(7).Should().Be("This is");
            str.Truncate(0).Should().Be("");
            str.Truncate(100).Should().Be(str);

            nullValue.Truncate(5).Should().Be(null);
        }

        [Fact]
        public void TruncateWithPostfix_Test() {
            const string  str       = "This is a test string";
            const string? nullValue = null;

            str.TruncateWithPostfix(3).Should().Be("...");
            str.TruncateWithPostfix(12).Should().Be("This is a...");
            str.TruncateWithPostfix(0).Should().Be("");
            str.TruncateWithPostfix(100).Should().Be(str);

            nullValue.Truncate(5).Should().Be(null);

            str.TruncateWithPostfix(3, "~").Should().Be("Th~");
            str.TruncateWithPostfix(12, "~").Should().Be("This is a t~");
            str.TruncateWithPostfix(0, "~").Should().Be("");
            str.TruncateWithPostfix(100, "~").Should().Be(str);

            nullValue.TruncateWithPostfix(5, "~").Should().Be(null);
        }

        [Fact]
        public void RemovePostfix_Tests() {
            // null case
            (null as string).RemovePrefix("Test").Should().BeNull();

            // Simple case
            "MyTestAppService".RemovePostfix("AppService").Should().Be("MyTest");
            "MyTestAppService".RemovePostfix("Service").Should().Be("MyTestApp");

            // Multiple postfix (orders of postfixes are important)
            "MyTestAppService".RemovePostfix("AppService", "Service").Should().Be("MyTest");
            "MyTestAppService".RemovePostfix("Service", "AppService").Should().Be("MyTestApp");

            // Ignore case
            "TestString".RemovePostfix(StringComparison.OrdinalIgnoreCase, "string").Should()
                .Be("Test");

            // Unmatched case
            "MyTestAppService".RemovePostfix("Unmatched").Should().Be("MyTestAppService");
        }

        [Fact]
        public void RemovePrefix_Tests() {
            "Home.Index".RemovePrefix("NotMatchedPostfix").Should().Be("Home.Index");
            "Home.About".RemovePrefix("Home.").Should().Be("About");

            //Ignore case
            "Https://abp.io".RemovePrefix(StringComparison.OrdinalIgnoreCase, "https://").Should()
                .Be("abp.io");
        }

        [Fact]
        public void ReplaceFirst_Tests() {
            "Test string".ReplaceFirst("s", "X").Should().Be("TeXt string");
            "Test test test".ReplaceFirst("test", "XX").Should().Be("Test XX test");
            "Test test test".ReplaceFirst("test", "XX", StringComparison.OrdinalIgnoreCase).Should()
                .Be("XX test test");
        }

        [Theory]
        [InlineData("")]
        [InlineData("MyStringİ")]
        public void GetBytes_Test(string str) {
            var bytes = str.GetBytes();
            bytes.Should().NotBeNull();
            bytes.Length.Should().BeGreaterOrEqualTo(str.Length);
            Encoding.UTF8.GetString(bytes).Should().Be(str);
        }

        [Theory]
        [InlineData("")]
        [InlineData("MyString")]
        public void GetBytes_With_Encoding_Test(string str) {
            var bytes = str.GetBytes(Encoding.ASCII);
            bytes.Should().NotBeNull();
            bytes.Length.Should().BeGreaterOrEqualTo(str.Length);
            Encoding.ASCII.GetString(bytes).Should().Be(str);
        }

        [Fact]
        public void ToEnum_Test() {
            "MyValue1".ToEnum<MyEnum>().Should().Be(MyEnum.MyValue1);
            "MyValue2".ToEnum<MyEnum>().Should().Be(MyEnum.MyValue2);
        }

        private enum MyEnum { MyValue1, MyValue2 }
    }
}