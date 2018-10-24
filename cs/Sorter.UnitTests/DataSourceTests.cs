using System.Collections.Generic;
using System.Linq;
using Sorter.Core;
using Xunit;

namespace Sorter.UnitTests
{
    public class DataSourceTests
    {
        [Fact]
        public void OrderLines_UsingNoCustomComparer_ReturnsOrderedLines()
        {
            var lines = new[]
            {
                "415. Apple",
                "30432. Something something something",
                "1. Apple",
                "32. Cherry is the best",
                "2. Banana is yellow"
            };

            var source = new DataSource(() => lines);

            var result = source.OrderLines().ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("30432. Something something something", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("415. Apple", item));
        }

        [Fact]
        public void OrderLines_UsingDefaultComparer_ReturnsLinesOrderedInSpecificManner()
        {
            var lines = new[]
            {
                "415. Apple",
                "30432. Something something something",
                "1. Apple",
                "32. Cherry is the best",
                "2. Banana is yellow"
            };

            var source = new DataSource(() => lines, comparer: new DefaultComparer());

            var result = source.OrderLines().ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        [Fact]
        public void QuasyPerfTest_OrderLines_ReturnsOrderedLines()
        {
            var lines = Enumerable
                .Range(0, 1000)
                .Select(i => int.MaxValue - i)
                .Select(i => i.ToString());

            var source = new DataSource(() => lines, linesInBatch: 100);

            var result = source.OrderLines().ToArray();

            Assert.Equal<IEnumerable<string>>(
                lines.OrderBy(l => l),
                result);
        }
    }
}
