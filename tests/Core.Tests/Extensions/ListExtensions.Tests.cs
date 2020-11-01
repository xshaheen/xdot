using System.Linq;
using FluentAssertions;
using X.Core.Extensions;
using Xunit;

namespace Core.Tests.Extensions
{
    public class ListExtensionsTests
    {
        [Fact]
        public void InsertRange()
        {
            var list = Enumerable.Range(1, 3).ToList();
            list.InsertRange(1, new[] { 7, 8, 9 });

            list[0].Should().Be(1);
            list[1].Should().Be(7);
            list[2].Should().Be(8);
            list[3].Should().Be(9);
            list[4].Should().Be(2);
            list[5].Should().Be(3);
        }

        [Fact]
        public void InsertAfter()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(2, 42);

            list.Count.Should().Be(4);
            list[0].Should().Be(1);
            list[1].Should().Be(2);
            list[2].Should().Be(42);
            list[3].Should().Be(3);

            list.InsertAfter(3, 43);

            list.Count.Should().Be(5);
            list[0].Should().Be(1);
            list[1].Should().Be(2);
            list[2].Should().Be(42);
            list[3].Should().Be(3);
            list[4].Should().Be(43);
        }

        [Fact]
        public void InsertAfter_With_Predicate()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(i => i == 2, 42);

            list.Count.Should().Be(4);
            list[0].Should().Be(1);
            list[1].Should().Be(2);
            list[2].Should().Be(42);
            list[3].Should().Be(3);

            list.InsertAfter(i => i == 3, 43);

            list.Count.Should().Be(5);
            list[0].Should().Be(1);
            list[1].Should().Be(2);
            list[2].Should().Be(42);
            list[3].Should().Be(3);
            list[4].Should().Be(43);
        }

        [Fact]
        public void InsertAfter_With_Predicate_Should_Insert_To_First_If_Not_Found()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(i => i == 999, 42);

            list.Count.Should().Be(4);
            list[0].Should().Be(42);
            list[1].Should().Be(1);
            list[2].Should().Be(2);
            list[3].Should().Be(3);
        }

        [Fact]
        public void InsertBefore()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertBefore(2, 42);

            list.Count.Should().Be(4);
            list[0].Should().Be(1);
            list[1].Should().Be(42);
            list[2].Should().Be(2);
            list[3].Should().Be(3);

            list.InsertBefore(1, 43);

            list.Count.Should().Be(5);
            list[0].Should().Be(43);
            list[1].Should().Be(1);
            list[2].Should().Be(42);
            list[3].Should().Be(2);
            list[4].Should().Be(3);
        }

        [Fact]
        public void InsertBefore_With_Predicate()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertBefore(i => i == 2, 42);

            list.Count.Should().Be(4);
            list[0].Should().Be(1);
            list[1].Should().Be(42);
            list[2].Should().Be(2);
            list[3].Should().Be(3);

            list.InsertBefore(i => i == 1, 43);

            list.Count.Should().Be(5);
            list[0].Should().Be(43);
            list[1].Should().Be(1);
            list[2].Should().Be(42);
            list[3].Should().Be(2);
            list[4].Should().Be(3);
        }

        [Fact]
        public void ReplaceWhile_WithValue()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceWhile(i => i >= 2, 42);

            list[0].Should().Be(1);
            list[1].Should().Be(42);
            list[2].Should().Be(42);
        }

        [Fact]
        public void ReplaceWhile_WithFactory()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceWhile(i => i >= 2, i => i + 1);

            list[0].Should().Be(1);
            list[1].Should().Be(3);
            list[2].Should().Be(4);
        }

        [Fact]
        public void ReplaceFirst_WithValue()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceFirst(i => i >= 2, 42);

            list[0].Should().Be(1);
            list[1].Should().Be(42);
            list[2].Should().Be(3);
        }

        [Fact]
        public void ReplaceFirst_WithFactory()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceFirst(i => i >= 2, i => i + 1);

            list[0].Should().Be(1);
            list[1].Should().Be(3);
            list[2].Should().Be(3);
        }

        [Fact]
        public void ReplaceFirst_With_Item()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceFirst(2, 42);

            list[0].Should().Be(1);
            list[1].Should().Be(42);
            list[2].Should().Be(3);
        }
    }
}
