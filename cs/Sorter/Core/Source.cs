using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Sorter.Core
{
    public abstract class Source
    {
        private readonly Func<string, IComparable> toComparable;

        public Source()
        {

        }

        public Source(Func<string, IComparable> toComparable)
        {
            this.toComparable = toComparable;
        }

        public abstract IEnumerable<string> ReadLines();

        public abstract Source SaveLines(IEnumerable<string> lines);

        public IEnumerable<string> OrderLines()
        {
            const int linesInBatch = 10000;

            // todo: add cleanup

            var orderedLines =
                this.ReadLines()
                    .Batch(linesInBatch)
                    .Select(batch => this.SaveLines(batch.OrderBy(l => l)))
                    .Select(source => source.ReadLines())
                    .Aggregate(LinqExtensions.Merge);

            return orderedLines;
        }
    }
}
