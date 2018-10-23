using System;
using System.Collections.Generic;
using System.IO;

namespace Sorter.Core
{
    public class FileSource : Source
    {
        private readonly Func<Stream> getContetns;

        public FileSource(
            Func<Stream> getContetns,
            int linesInBatch) : base(linesInBatch)
        {
            this.getContetns = getContetns;
        }

        public FileSource(Func<string, IComparable> toComparable) : base(toComparable)
        {
        }

        public override IEnumerable<string> ReadLines()
        {
            return ReadLines(this.getContetns);
        }

        public override Source SaveLines(IEnumerable<string> lines)
        {
            var temp = Path.GetTempFileName();

            File.WriteAllLines(temp, lines);

            return new FileSource(
                () => new FileStream(temp, FileMode.Open, FileAccess.Read, FileShare.None),
                this.LinesInBatch);
        }
    }
}
