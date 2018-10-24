using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using HPCsharp;

namespace Sorter.Core
{
    public class DataSource
    {
        public const int DefaultBatchSize = 10000000;

        private readonly Func<IEnumerable<string>, DataSource> saveLines;
        private readonly Func<bool> deleteSource;
        private readonly Comparer<string> comparer;

        public DataSource(
            Func<IEnumerable<string>> readLines,
            int linesInBatch = DefaultBatchSize,
            Func<IEnumerable<string>, DataSource> saveLines = null,
            Func<bool> deleteSource = null,
            Comparer<string> comparer = null)
        {
            this.ReadLines = readLines ?? Enumerable.Empty<string>;
            this.saveLines = saveLines;
            this.deleteSource = deleteSource;
            this.LinesInBatch = linesInBatch <= 0
                ? DefaultBatchSize
                : linesInBatch;
            this.comparer = comparer;
        }

        public int LinesInBatch { get; }

        public Func<IEnumerable<string>> ReadLines { get; }

        public SortingResult OrderLines()
        {
            var tempSources =
                this.ReadLines()
                    .Batch(this.LinesInBatch)
                    .Select(batch => batch.ToArray())
                    .Select(batch => batch.SortMergePar(this.comparer))
                    .Select(batch => this.saveLines != null
                        ? this.saveLines(batch)
                        : new DataSource(() => batch))
                    .ToArray();

            return new SortingResult(tempSources, this.comparer);
        }

        public bool Delete()
        {
            return this.deleteSource?.Invoke() ?? false;
        }
    }
}
