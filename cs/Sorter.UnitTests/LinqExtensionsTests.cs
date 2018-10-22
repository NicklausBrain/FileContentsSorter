using Sorter.Core;
using System.Linq;
using Xunit;

namespace Sorter.UnitTests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void Merge2_For2Sequences_ReturnsSingleOrderedSequence()
        {
            var arr1 = new int[] { 1, 2, 3 };
            var arr2 = new int[] { 2, 5 };

            var result = LinqExtensions.Merge<int>(arr1, arr2);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item));
        }

        [Fact]
        public void QuasyPerfTest_Merge2_For2Sequences_ReturnsSingleOrderedSequence()
        {
            int n = 1000;
            var arr1 = Enumerable.Range(1, n).Select(x => x);
            var arr2 = Enumerable.Range(1, n).Select(x => x + x);

            var result = LinqExtensions.Merge<int>(arr1, arr2);

            Assert.Equal(n * 2, result.Count());
        }

        [Fact]
        public void Split_Into3_Returns3Sequence()
        {
            var seq = Enumerable.Range(1, 10);

            var result = seq.Split(3).ToArray();

            var s1 = result[0];
            var s2 = result[1];
            var s3 = result[2];

            Assert.Collection(s1,
                item => Assert.Equal(1, item),
                item => Assert.Equal(4, item),
                item => Assert.Equal(7, item),
                item => Assert.Equal(10, item));

            Assert.Collection(s2,
                item => Assert.Equal(2, item),
                item => Assert.Equal(5, item),
                item => Assert.Equal(8, item));

            Assert.Collection(s3,
                item => Assert.Equal(3, item),
                item => Assert.Equal(6, item),
                item => Assert.Equal(9, item));
        }

        //[Fact]
        //public void ParallelSplit_Into3_Returns3Sequence()
        //{
        //    var seq = Enumerable.Range(1, 10);

        //    var result = seq.ParallelSplit(3)
        //        .Select(x=>x.OrderBy(y=>y))
        //        .OrderBy(x=>x.First())
        //        .ToArray();

        //    var s1 = result[0].OrderBy(x => x).ToArray();
        //    var s2 = result[1].OrderBy(x => x).ToArray();
        //    var s3 = result[2].OrderBy(x => x).ToArray();

        //    Assert.Collection(s1,
        //        item => Assert.Equal(1, item),
        //        item => Assert.Equal(4, item),
        //        item => Assert.Equal(7, item),
        //        item => Assert.Equal(10, item));

        //    Assert.Collection(s2,
        //        item => Assert.Equal(2, item),
        //        item => Assert.Equal(5, item),
        //        item => Assert.Equal(8, item));

        //    Assert.Collection(s3,
        //        item => Assert.Equal(3, item),
        //        item => Assert.Equal(6, item),
        //        item => Assert.Equal(9, item));
        //}
    }
}
