using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Sorter.Core
{
    public abstract class Source
    {
        private readonly Func<string, IComparable> toComparable;
        protected readonly int LinesInBatch;

        public Source(int linesInBatch)
        {
            this.LinesInBatch = linesInBatch;
        }

        public Source(Func<string, IComparable> toComparable)
        {
            this.toComparable = toComparable;
        }

        public abstract IEnumerable<string> ReadLines();

        public abstract Source SaveLines(IEnumerable<string> lines);

        public IEnumerable<string> OrderLines()
        {
            // todo: add cleanup

            var orderedLines =
                this.ReadLines()
                    .Batch(LinesInBatch)
                    .Select(batch => this.SaveLines(batch.OrderBy(l => l)))
                    .Select(source => source.ReadLines())
                    .Aggregate(LinqExtensions.Merge);

            return orderedLines;
        }
    }
}
