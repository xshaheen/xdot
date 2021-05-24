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
        // Alef
        [InlineData("آ", "ا")]
        [InlineData("إ", "ا")]
        [InlineData("أ", "ا")]
        // Common spelling error
        [InlineData("ة", "ه")]
        [InlineData("ى", "ي")]
        // Kaf like
        [InlineData("ڮ", "ك")]
        [InlineData("ػ", "ك")]
        [InlineData("ڪ", "ك")]
        [InlineData("ڴ", "ك")]
        // Waw like
        [InlineData("ۈ", "و")]
        //
        [InlineData("ؠ", "ي")]
        [InlineData("ۮ", "د")]
        [InlineData("ﺞ", "ج")]
        [InlineData("ﺑ", "ب")]
        [InlineData("ۻ", "ض")]
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
        [InlineData('?')]
        [InlineData('۩')]
        [InlineData('﴾')]
        [InlineData('؏')]
        [InlineData('؁')]
        [InlineData('؃')]
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
