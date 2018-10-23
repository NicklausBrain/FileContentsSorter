using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using HPCsharp;

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
                    .Select(batch =>
                        this.SaveLines(
                            batch.ToArray().SortMergePar()))// plug sorting alg here
                    .Select(source => source.ReadLines())
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .Aggregate(LinqExtensions.Merge);

            return orderedLines;
        }

        public static IEnumerable<string> ReadLines(Func<Stream> getContetns)
        {
            //File.ReadLines() chage to this
            using (var stream = getContetns())
            using (var reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
