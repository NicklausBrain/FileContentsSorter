using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sorter.Core
{
    public class SortingResult : IEnumerable<string>
    {
        private readonly DataSource[] temporarySources;
        private readonly Comparer<string> comparer;

        public SortingResult(
            DataSource[] temporarySources,
            Comparer<string> comparer = null)
        {
            this.temporarySources = temporarySources;
            this.comparer = comparer;
        }

        public IEnumerable<string> MergeSources()
        {
            return
                this.temporarySources
                    .AsParallel()
                    .Select(source => source.ReadLines())
                    .Aggregate((seqA, seqB) => seqA.Merge(seqB, this.comparer));
        }

        public bool ClearTempSources()
        {
            return this.temporarySources.All(source => source.Delete());
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
