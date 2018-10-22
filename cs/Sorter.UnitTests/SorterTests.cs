using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Sorter.Core;

namespace Sorter.UnitTests
{
    public class SorterTests
    {
        [Fact]
        public void SortContents_ForInputStream_ReturnsOrderedSequence()
        {
            var data = new MemoryStream(Encoding.UTF8.GetBytes(
                "415. Apple\r\n" +
                "30432. Something something something\r\n" +
                "1. Apple\r\n" +
                "32. Cherry is the best\r\n" +
                "2. Banana is yellow\r\n"));

            var contents = new Contents(() => data);

            var result = contents.SortLines().ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        [Fact]
        public void SortContents_ForSequence_ReturnsOrderedSequence()
        {
            var data = new string[]
            {
                "415. Apple",
                "30432. Something something something",
                "1. Apple",
                "32. Cherry is the best",
                "2. Banana is yellow"
            };

            var contents = new Contents(() => null);

            var result = contents.SortLines(data).ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        [Fact]
        public void SortContents2_ForInputStream_ReturnsOrderedSequence()
        {
            var data = new MemoryStream(Encoding.UTF8.GetBytes(
                "415. Apple\r\n" +
                "30432. Something something something\r\n" +
                "1. Apple\r\n" +
                "32. Cherry is the best\r\n" +
                "2. Banana is yellow\r\n"));

            var sorter = new Contents(() => data);

            var result = sorter.SortLines(3).ToArray();

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
