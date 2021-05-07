using FluentAssertions;
using X.Core.Extensions;
using X.Core.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Core.Tests.Extensions {
    public class StringExtensionsSearchStringTests {
        private readonly ITestOutputHelper _output;

        public StringExtensionsSearchStringTests(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(" \n\n\r\n ")]
        public void SearchString__should_returns_white_spaces_with_no_changes(string value) {
            _Test(value, string.Empty);
        }

        [Theory]
        [InlineData("٠", "0")]
        [InlineData("١", "1")]
        [InlineData("٢", "2")]
        [InlineData("٩", "9")]
        [InlineData("١٢٨", "128")]
        [InlineData("This ١٢٨", "this128")]
        public void SearchString__should_change_arabic_numeral_to_arabic_latin_numeral(
            string value,
            string expected
        ) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData("ﺞ", "ج")]
        [InlineData("ﺑ", "ب")]
        [InlineData("ﻬ", "ه")]
        [InlineData("ى", "ي")] // Alef maqsurah => ya'
        [InlineData("آ", "ا")] // Alef With Madda Above
        [InlineData("إ", "ا")] // Alef With Hamza Below
        [InlineData("أ", "ا")] // Alef With Hamza Above
        [InlineData("ۻ", "ض")] // Arabic Letter Dad With Dot Below
        [InlineData("ڮ", "ك")] // Arabic Letter Kaf With Three Dots Below
        [InlineData("ڪ", "ك")] // Arabic Letter Swash Kaf
        [InlineData("ڥ", "ف")] // Arabic Letter Swash Kaf
        public void SearchString__should_convert_arabic_context_characters_to_regular_characters(
            string value,
            string expected
        ) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData("ة", "ه")]
        [InlineData("ى", "ي")]
        public void SearchString__should_replace_equivalent_characters_with_one_shape(
            string value,
            string expected
        ) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData(Ar.Semicolon)]
        [InlineData(Ar.StarOfRubElHizb)]
        [InlineData(Ar.EndOfAyah)]
        [InlineData(Ar.Comma)]
        [InlineData('?')] // ARABIC QUESTION MARK
        [InlineData('۩')] // ARABIC PLACE OF SAJDAH
        [InlineData('﴾')] // Arabic ornate left parenthesis
        [InlineData('؏')] // ARABIC SIGN MISRA
        [InlineData('؁')] // ARABIC SIGN MISRA
        [InlineData('؃')] // ARABIC SIGN MISRA
        public void SearchString__should_remove_punctuation_and_ornaments(char value) {
            _Test(value.ToString(), "");
        }

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
        [InlineData("بسم الله الرحمن الرحيم", "بسماللهالرحمنالرحيم")]
        [InlineData("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ", "بسماللهالرحمنالرحيم")]
        [InlineData("بِسْمِ اللَّـهِ الرَّحْمَـٰنِ الرَّحِيمِ", "بسماللهالرحمنالرحيم")]
        public void SearchString__should_work_with_arabic(string value, string expected) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData("m", "m")]
        [InlineData(" Mahmoud ", "mahmoud")]
        [InlineData(" Mahmoud Shaheen", "mahmoudshaheen")]
        [InlineData("crème brûlée", "cremebrulee")]
        public void SearchString__should_work_with_latin(string value, string expected) {
            _Test(value, expected);
        }

        private void _Test(string value, string expected) {
            // act
            var result = value.SearchString().SupportAr();

            _output.WriteLine($"result   =>{result}");
            _output.WriteLine($"expected =>{expected}");

            // assert
            result.Should().Be(expected);
        }
    }
}
