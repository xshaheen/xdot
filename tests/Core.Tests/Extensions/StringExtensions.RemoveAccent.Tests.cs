using FluentAssertions;
using X.Core.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Core.Tests.Extensions {
    public class StringExtensionsRemoveDiacriticsTests {
        private readonly ITestOutputHelper _output;

        public StringExtensionsRemoveDiacriticsTests(ITestOutputHelper testOutputHelper)
            => _output = testOutputHelper;

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("  ", "  ")]
        public void RemoveAccent_Returns_WhiteSpaces_With_No_Changes(string value, string expected)
            => Test(value, expected);

        [Theory]
        [InlineData("ﺞ", "ج")]
        [InlineData("ة", "غ")]
        [InlineData("ﺑ", "ب")]
        [InlineData("ﻬ", "ه")]
        public void RemoveAccent_Converts_Arabic_Context_Characters_To_Regular_Characters(
            string value,
            string expected
        )
            => Test(value, expected);

        [Theory]
        [InlineData("ى", "ي")] // Alef maqsurah => ya'
        [InlineData("آ", "ا")] // Alef With Madda Above
        [InlineData("إ", "ا")] // Alef With Hamza Below
        [InlineData("أ", "ا")] // Alef With Hamza Above
        // [InlineData("۽", "ء")] // Arabic Sign Sindhi Ampersand
        // [InlineData("ۻ", "ض")] // Arabic Letter Dad With Dot Below
        // [InlineData("ڮ", "ك")] // Arabic Letter Kaf With Three Dots Below
        // [InlineData("ڪ", "ك")] // Arabic Letter Swash Kaf
        // [InlineData("ڥ", "ف")] // Arabic Letter Swash Kaf
        public void ArabicNormalization_Work_With_Arabic_Repeated_Characters(
            string value,
            string expected
        )
            => Test(value, expected);

        [Theory]
        [InlineData("ء", "ء")]
        [InlineData(" محمود ", " محمود ")]
        [InlineData("يمني", "يمني")] // ي is preserved
        [InlineData("ىمنى", "ىمنى")] // ى is preserved
        [InlineData("شاطئ", "شاطي")]
        [InlineData("لؤ", "لو")]
        [InlineData("بسم الله الرحمن الرحيم", "بسم الله الرحمن الرحيم")]
        [InlineData("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ", "بسم الله الرحمن الرحيم")]
        [InlineData("بِسْمِ اللَّـهِ الرَّحْمَـٰنِ الرَّحِيمِ", "بسم اللـه الرحمـن الرحيم")]
        [InlineData("۞", "بسم اللـه الرحمـن الرحيم")]
        public void RemoveAccent_Work_With_Arabic_Characters(string value, string expected)
            => Test(value, expected);

        [Theory]
        [InlineData("m", "m")]
        [InlineData("123", "123")]
        [InlineData(" Mahmoud 17 ", " Mahmoud 17 ")]
        [InlineData(" crème brûlée", " creme brulee")]
        public void RemoveAccent_Work_With_Latin_Characters(string value, string expected)
            => Test(value, expected);

        private void Test(string value, string expected) {
            // act
            var result = value.RemoveAccent();

            _output.WriteLine($"result   =>{result}");
            _output.WriteLine($"expected =>{expected}");

            // assert
            result.Should().Be(expected);
        }
    }
}