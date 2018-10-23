using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using HPCsharp;

namespace Sorter.Core
{
    public class Source
    {
        public const int DefaultBatchSize = 10000000;
        protected readonly Func<IEnumerable<string>> ReadLines;
        protected readonly Func<IEnumerable<string>, Source> SaveLines;
        protected readonly Comparer<string> Comparer;
        protected readonly int LinesInBatch;

        public Source(
            Func<IEnumerable<string>> readLines,
            int linesInBatch = DefaultBatchSize,
            Func<IEnumerable<string>, Source> saveLines = null,
            Comparer<string> comparer = null)
        {
            this.ReadLines = readLines;
            this.SaveLines = saveLines;
            this.LinesInBatch = linesInBatch;
            this.Comparer = comparer;
        }

        public IEnumerable<string> OrderLines()
        {
            // todo: add cleanup
            // todo plug sorting alg here
            var orderedLines =
                this.ReadLines()
                    .Batch(LinesInBatch)
                    .Select(batch => batch.ToArray())
                    .Select(batch => batch.SortMergePar(this.Comparer))
                    .Select(batch => this.SaveLines != null
                        ? this.SaveLines(batch)
                        : new Source(() => batch))
                    .Select(source => source.ReadLines())
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .Aggregate(LinqExtensions.Merge);

            return orderedLines;
        }
    }
}
