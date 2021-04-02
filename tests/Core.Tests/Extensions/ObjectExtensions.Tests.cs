using System;
using FluentAssertions;
using X.Core.Extensions;
using Xunit;

namespace Core.Tests.Extensions {
    public class ObjectExtensionsTests {
        [Fact]
        public void As_Test() {
            var o1 = (object) new ObjectExtensionsTests();
            ObjectExtensions.As<ObjectExtensionsTests>(o1).Should().NotBe(null);

            object? o2 = null;
            ObjectExtensions.As<ObjectExtensionsTests>(o2!).Should().Be(null);
        }

        [Fact]
        public void To_Tests() {
            "42".To<int>().Should().Be(42);
            "42".To<int>().Should().Be(42);

            "28173829281734".To<long>().Should().Be(28173829281734);
            "28173829281734".To<long>().Should().Be(28173829281734);

            "2.0".To<double>().Should().Be(2.0);
            "0.2".To<double>().Should().Be(0.2);
            2.0.To<int>().Should().Be(2);

            "false".To<bool>().Should().Be(false);
            "True".To<bool>().Should().Be(true);
            "False".To<bool>().Should().Be(false);
            "TrUE".To<bool>().Should().Be(true);

            Assert.Throws<FormatException>(() => "test".To<bool>());
            Assert.Throws<FormatException>(() => "test".To<int>());

            "2260AFEC-BBFD-42D4-A91A-DCB11E09B17F".To<Guid>()
                .Should().Be(new Guid("2260afec-bbfd-42d4-a91a-dcb11e09b17f"));
        }

        [Fact]
        public void IsIn_Test() {
            5.IsIn(1, 3, 5, 7).Should().Be(true);
            6.IsIn(1, 3, 5, 7).Should().Be(false);

            int? number = null;
            number.IsIn(2, 3, 5).Should().Be(false);

            var str = "a";
            str.IsIn("a", "b", "c").Should().Be(true);

            str = null;
            str.IsIn("a", "b", "c").Should().Be(false);
        }

        [Fact]
        public void If_Tests() {
            var value = 0;

            value = value.If(true, v => v + 1);
            value.Should().Be(1);

            value = value.If(false, v => v + 1);
            value.Should().Be(1);

            value = value
                .If(true, v => v + 3)
                .If(false, v => v + 5);
            value.Should().Be(4);
        }
    }
}
