using FluentAssertions;
using X.Core.Utils;
using Xunit;

namespace Core.Tests.Utils {
    public class TypeHelperTests {
        [Fact]
        public void GetDefaultValue() {
            TypeHelper.GetDefaultValue(typeof(bool)).Should().Be(false);
            TypeHelper.GetDefaultValue(typeof(byte)).Should().Be(0);
            TypeHelper.GetDefaultValue(typeof(int)).Should().Be(0);
            TypeHelper.GetDefaultValue(typeof(string)).Should().BeNull();
            TypeHelper.GetDefaultValue(typeof(MyEnum)).Should().Be(MyEnum.MyValue0);
        }

        private enum MyEnum { MyValue0 }
    }
}
