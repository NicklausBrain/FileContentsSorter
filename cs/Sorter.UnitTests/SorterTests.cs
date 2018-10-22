using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sorter.UnitTests
{
    public class SorterTests
    {
        [Fact]
        public void SortContents_ForInputStream_ReturnsOrderedSequence()
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

        [Fact]
        public void SortContents_ForSequence_ReturnsOrderedSequence()
        {
            var sorter = new Sorter();

            var data = new string[]
            {
                "415. Apple",
                "30432. Something something something",
                "1. Apple",
                "32. Cherry is the best",
                "2. Banana is yellow"
            };

            var result = sorter.SortContents(data).ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        [Fact]
        public void Merge_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var sorter = new Sorter();

            var arr1 = new int[] { 1, 2, 3 };
            var arr2 = new int[] { 2, 5 };

            var result = sorter.Merge<int>(arr1, arr2);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item));
        }

        [Fact]
        public void Merge2_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var sorter = new Sorter();

            var arr1 = new int[] { 1, 2, 3 };
            var arr2 = new int[] { 2, 5 };

            var result = sorter.Merge2<int>(arr1, arr2);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item));
        }

        [Fact]
        public void Merge_For3Sequences_ReturnsSingleOrderedSequence()
        {
            var sorter = new Sorter();

            var arr1 = new int[] { 1, 2, 3 };
            var arr2 = new int[] { 2, 5 };
            var arr3 = new int[] { 1, 3, 7, 9 };

            var result = sorter.Merge<int>(arr1, arr2, arr3);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item),
                item => Assert.Equal(7, item),
                item => Assert.Equal(9, item));
        }

        [Fact]
        public void SortContents2_ForInputStream_ReturnsOrderedSequence()
        {
            var sorter = new Sorter();

            var data = new MemoryStream(Encoding.UTF8.GetBytes(
                "415. Apple\r\n" +
                "30432. Something something something\r\n" +
                "1. Apple\r\n" +
                "32. Cherry is the best\r\n" +
                "2. Banana is yellow\r\n"));

            var result = sorter.SortContents2(() => data, 3).ToArray();

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        //--------------------------------------------
        int n = 1000;

        [Fact]
        public void QuasyPerfTest_Merge_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var sorter = new Sorter();
            
            var arr1 = Enumerable.Range(1, n).Select(x => x);
            var arr2 = Enumerable.Range(1, n).Select(x => x + x);

            var result = sorter.Merge<int>(arr1, arr2);

            Assert.Equal(n * 2, result.Count());
        }

        [Fact]
        public void QuasyPerfTest_Merge2_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var sorter = new Sorter();
            var arr1 = Enumerable.Range(1, n).Select(x => x);
            var arr2 = Enumerable.Range(1, n).Select(x => x + x);

            var result = sorter.Merge2<int>(arr1, arr2);

            Assert.Equal(n * 2, result.Count());
        }
    }
}
