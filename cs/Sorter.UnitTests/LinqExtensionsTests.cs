﻿using Sorter.Core;
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

            var result = LinqExtensions.Merge(arr1, arr2);

            Assert.Collection(result,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item));
        }

        [Fact]
        public void QuasyPerfTest_Merge_For2Sequences_ReturnsSingleOrderedSequence()
        {
            int n = 1000;
            var arr1 = Enumerable.Range(1, n).Select(x => x);
            var arr2 = Enumerable.Range(1, n).Select(x => x + x);

            var result = LinqExtensions.Merge(arr1, arr2);

            Assert.Equal(n * 2, result.Count());
        }

        //[Fact]
        //public void QuasyPerfTest_Seq_Merge_For2Sequences_ReturnsSingleOrderedSequence()
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
        //public void QuasyPerfTest_Par_Merge_For2Sequences_ReturnsSingleOrderedSequence()
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
