using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sorter.UnitTests
{
    public class SorterTests
    {
        [Fact]
        public void SortContents_ReturnsOrderedSequence()
        {
            var sorter = new Sorter();

            var data = new MemoryStream(Encoding.UTF8.GetBytes(
                "415. Apple\r\n" +
                "30432. Something something something\r\n" +
                "1. Apple\r\n" +
                "32. Cherry is the best\r\n" +
                "2. Banana is yellow\r\n"));

            var result = sorter.SortContents(() => data).ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }
    }
}
