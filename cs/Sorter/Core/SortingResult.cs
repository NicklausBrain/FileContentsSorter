using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sorter.Core
{
    public class SortingResult : IEnumerable<string>
    {
        private readonly Source[] temporarySources;

        public SortingResult(Source[] temporarySources)
        {
            this.temporarySources = temporarySources;
        }

        public IEnumerable<string> MergeSources()
        {
            return
                this.temporarySources
                    .Select(source => source.ReadLines())
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .Aggregate(LinqExtensions.Merge); // todo: imporve merge performance
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
