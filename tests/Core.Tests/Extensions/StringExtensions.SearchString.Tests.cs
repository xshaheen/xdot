using FluentAssertions;
using X.Core.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Core.Tests.Extensions {
    public class StringExtensionsSearchStringTests {
        private readonly ITestOutputHelper _output;

        public StringExtensionsSearchStringTests(ITestOutputHelper testOutputHelper)
            => _output = testOutputHelper;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(" \n\n\r\n ")]
        public void SearchString_Returns_WhiteSpaces_With_No_Changes(string value)
            => Test(value, string.Empty);

        [Theory]
        [InlineData("٠", "0")]
        [InlineData("١", "1")]
        [InlineData("٢", "2")]
        [InlineData("٩", "9")]
        [InlineData("١٢٨", "128")]
        [InlineData("This ١٢٨", "this128")]
        public void SearchString_Change_Arabic_Numeral_To_Arabic_Latin_Numeral(
            string value,
            string expected
        )
            => Test(value, expected);

        [Theory]
        [InlineData("ﺞ", "ج")]
        [InlineData("ﺑ", "ب")]
        [InlineData("ﻬ", "ه")]
        public void SearchString_Converts_Arabic_Context_Characters_To_Regular_Characters(
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
        public void SearchString_Work_With_Arabic_Repeated_Characters(string value, string expected)
            => Test(value, expected);

        [Theory]
        [InlineData("ة", "ه")]
        [InlineData("ى", "ي")]
        public void SearchString_Work_With_Arabic_Popular_Equivalent_Characters(
            string value,
            string expected
        )
            => Test(value, expected);

        [Theory]
        [InlineData("،", "")]  // ARABIC COMMA
        [InlineData("؛‎", "")] // ARABIC SEMICOLON
        [InlineData("?", "")]  // ARABIC QUESTION MARK
        [InlineData("۩", "")]  // ARABIC PLACE OF SAJDAH
        [InlineData("﴾", "")]  // Arabic ornate left parenthesis
        [InlineData("؏", "")]  // ARABIC SIGN MISRA
        [InlineData("؁", "")]  // ARABIC SIGN MISRA
        [InlineData("؂", "")]  // ARABIC SIGN MISRA
        [InlineData("؃", "")]  // ARABIC SIGN MISRA
        public void SearchString_Remove_Punctuation_And_Ornaments(string value, string expected)
            => Test(value, expected);

        [Theory]
        [InlineData("ء", "ء")]
        [InlineData(" محمود ", "محمود")]
        [InlineData("أحمد", "احمد")]
        [InlineData("إحمد", "احمد")]
        [InlineData("آحمد", "احمد")]
        [InlineData("يمني", "يمني")]
        [InlineData("ىمنى", "يمني")]
        [InlineData("شاطئ", "شاطي")]
        [InlineData("لؤ", "لو")]
        [InlineData("۝", "")]
        [InlineData("؞", "")]
        [InlineData("؏", "")]
        [InlineData("،", "")]
        [InlineData("بسم الله الرحمن الرحيم", "بسماللهالرحمنالرحيم")]
        [InlineData("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ", "بسماللهالرحمنالرحيم")]
        [InlineData("بِسْمِ اللَّـهِ الرَّحْمَـٰنِ الرَّحِيمِ", "بسماللهالرحمنالرحيم")]
        public void SearchString_Work_With_Arabic_Characters(string value, string expected)
            => Test(value, expected);

        [Theory]
        [InlineData("m", "m")]
        [InlineData(" Mahmoud ", "mahmoud")]
        [InlineData("crème brûlée", "cremebrulee")]
        public void SearchString_Work_With_Latin_Characters(string value, string expected)
            => Test(value, expected);

        private void Test(string value, string expected) {
            // act
            var result = value.SearchString().SupportAr();

            _output.WriteLine($"result   =>{result}");
            _output.WriteLine($"expected =>{expected}");

            // assert
            result.Should().Be(expected);
        }
    }
}