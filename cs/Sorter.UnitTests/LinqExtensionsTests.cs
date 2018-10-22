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
    }
}
