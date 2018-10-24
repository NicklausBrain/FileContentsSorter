﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using HPCsharp;

namespace Sorter.Core
{
    public class Source
    {
        public const int DefaultBatchSize = 10000000;
        protected readonly Func<IEnumerable<string>, Source> SaveLines;
        protected readonly Func<bool> DeleteSource;
        protected readonly Comparer<string> Comparer;
        protected readonly int LinesInBatch;

        public Source(
            Func<IEnumerable<string>> readLines,
            int linesInBatch = DefaultBatchSize,
            Func<IEnumerable<string>, Source> saveLines = null,
            Func<bool> deleteSource = null,
            Comparer<string> comparer = null)
        {
            this.ReadLines = readLines ?? (() => Enumerable.Empty<string>());
            this.SaveLines = saveLines;
            this.DeleteSource = deleteSource;
            this.LinesInBatch = linesInBatch <= 0
                ? DefaultBatchSize
                : linesInBatch;
            this.Comparer = comparer;
        }

        public Func<IEnumerable<string>> ReadLines { get; }

        public SortingResult OrderLines()
        {
            var tempSources =
                this.ReadLines()
                    .Batch(LinesInBatch)
                    .Select(batch => batch.ToArray())
                    .Select(batch => batch.SortMergePar(this.Comparer))
                    .Select(batch => this.SaveLines != null
                        ? this.SaveLines(batch)
                        : new Source(() => batch))
                     .ToArray();

            return new SortingResult(tempSources);
        }

        public bool Delete()
        {
            return DeleteSource != null ? DeleteSource() : false;
        }
    }
}
