using System;
using FluentAssertions;
using X.Core.Extensions;
using X.Core.Utils;
using Xunit;

namespace Core.Tests.Extensions {
    public class StringExtensionsConvertDigitsToEnglishDigitsTests : IDisposable {
        private readonly IDisposable _cultureScope;

        public StringExtensionsConvertDigitsToEnglishDigitsTests() {
            _cultureScope = CultureHelper.Use("en-Us");
        }

        public void Dispose() {
            _cultureScope.Dispose();
            GC.SuppressFinalize(this);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(" \n\n\r\n ")]
        public void ConvertDigitsToEnglishDigits__should_returns_white_spaces_as_it_is_tests(string value) {
            _Test(value, value);
        }

        [Theory]
        [InlineData("٠", "0")]
        [InlineData("١", "1")]
        [InlineData("٢", "2")]
        [InlineData("٩", "9")]
        [InlineData("١٢٨", "128")]
        [InlineData("١.٢٨", "1.28")]
        [InlineData("١,٢٨", "1,28")]
        public void ConvertDigitsToEnglishDigits_numerals_tests(string value, string expected) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData("This is numeral ١٢٨", "This is numeral 128")]
        [InlineData("This is numeral ١.٢٨", "This is numeral 1.28")]
        [InlineData("This is numeral ١,٢٨", "This is numeral 1,28")]
        public void ConvertDigitsToEnglishDigits_numeral_with_other_characters_tests(
            string value,
            string expected
        ) {
            _Test(value, expected);
        }

        [Theory]
        [InlineData("This is numeral ١٢٨", "This is numeral 128")]
        [InlineData("This is numeral ١.٢٨", "This is numeral 1.28")]
        [InlineData("This is numeral ١,٢٨", "This is numeral 1,28")]
        public void ConvertDigitsToEnglishDigits__should_work_independent_of_current_culture(
            string value,
            string expected
        ) {
            using (CultureHelper.Use("ar-eg")) {
                var result = value.ConvertDigitsToEnglishDigits();
                result.Should().Be(expected);
            }
        }

        private static void _Test(string value, string expected) {
            // act

            var result = value.ConvertDigitsToEnglishDigits();

            // assert

            result.Should().Be(expected);
        }
    }
}
