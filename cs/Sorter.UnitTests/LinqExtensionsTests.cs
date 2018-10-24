using Sorter.Core;
using System.Linq;
using Xunit;

namespace Sorter.UnitTests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void Merge_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var arr1 = new[] { 1, 2, 3 };
            var arr2 = new[] { 2, 5 };

            var result = arr1.Merge(arr2);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item));
        }

        [Fact]
        public void Merge_For2Sequences_UsingCustomComparer_ReturnsSequenceOrderedInSpecificManner()
        {
            var arr1 = new[]
            {
                "415. Apple", "2. Banana is yellow"
            };
            var arr2 = new[]
            {
                "1. Apple", "32. Cherry is the best", "30432. Something something something"
            };

            var result = arr1.Merge(arr2, new DefaultComparer());

            Assert.Collection(
                result,
                item => Assert.Equal("1. Apple", item),
                item => Assert.Equal("415. Apple", item),
                item => Assert.Equal("2. Banana is yellow", item),
                item => Assert.Equal("32. Cherry is the best", item),
                item => Assert.Equal("30432. Something something something", item));
        }

        [Fact]
        public void QuasiPerfTest_Merge_For2Sequences_ReturnsSingleOrderedSequence()
        {
            int n = 10000;
            var arr1 = Enumerable.Range(1, n).Select(x => x);
            var arr2 = Enumerable.Range(1, n).Select(x => x + x);

            var result = arr1.Merge(arr2);

            Assert.Equal(n * 2, result.Count());
        }

        //[Fact]
        //public void QuasiPerfTest_Seq_Merge_For2Sequences_ReturnsSingleOrderedSequence()
        //{
        //    int n = 10000000;
        //    var arr1 = Enumerable.Range(1, n).Select(x => x);
        //    var arr2 = Enumerable.Range(1, n).Select(x => x + x);
        //    var arr3 = Enumerable.Range(1, n).Select(x => x + 1);
        //    var arr4 = Enumerable.Range(1, n).Select(x => x + 3);

        //    var result = new[] { arr1, arr2, arr3, arr4 }.Aggregate(LinqExtensions.Merge);

        //    Assert.Equal(n * 4, result.Count());
        //}

        //[Fact]
        //public void QuasiPerfTest_Par_Merge_For2Sequences_ReturnsSingleOrderedSequence()
        //{
        //    int n = 10000000;
        //    var arr1 = Enumerable.Range(1, n).Select(x => x);
        //    var arr2 = Enumerable.Range(1, n).Select(x => x + x);
        //    var arr3 = Enumerable.Range(1, n).Select(x => x + 1);
        //    var arr4 = Enumerable.Range(1, n).Select(x => x + 3);

        //    var result = new[] { arr1, arr2, arr3, arr4 }
        //        .AsParallel()
        //        .WithDegreeOfParallelism(4)
        //        .WithMergeOptions(ParallelMergeOptions.NotBuffered)
        //        .Aggregate(LinqExtensions.Merge);

        //    Assert.Equal(n * 4, result.Count());
        //}
    }
}
