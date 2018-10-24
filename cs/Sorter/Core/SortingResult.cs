using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sorter.Core
{
    public class SortingResult : IEnumerable<string>
    {
        private readonly DataSource[] temporarySources;

        public SortingResult(DataSource[] temporarySources)
        {
            this.temporarySources = temporarySources;
        }

        public IEnumerable<string> MergeSources()
        {
            return
                this.temporarySources
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                    .Select(source => source.ReadLines())
                    .Aggregate(LinqExtensions.Merge);
        }

        public bool ClearTempSources()
        {
            return temporarySources.All(source => source.Delete());
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.MergeSources().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
