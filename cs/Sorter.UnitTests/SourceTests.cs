using System.IO;
using System.Linq;
using System.Text;
using Sorter.Core;
using Xunit;

namespace Sorter.UnitTests
{
    public class SourceTests
    {
        [Fact]
        public void OrderLines_ReturnsOrderedLines()
        {
            var data = new MemoryStream(Encoding.UTF8.GetBytes(
                "415. Apple\r\n" +
                "30432. Something something something\r\n" +
                "1. Apple\r\n" +
                "32. Cherry is the best\r\n" +
                "2. Banana is yellow\r\n"));

            var source = new InMemorySource(() => data, linesInBatch: 2);

            var result = source.OrderLines().ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("30432. Something something something", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("415. Apple", item));
        }
    }
}
