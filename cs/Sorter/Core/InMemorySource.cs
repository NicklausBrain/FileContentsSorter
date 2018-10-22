using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sorter.Core
{
    public class InMemorySource : Source
    {
        private readonly Func<MemoryStream> getStream;

        public InMemorySource(
            Func<MemoryStream> getStream,
            int linesInBatch) : base(linesInBatch)
        {
            this.getStream = getStream;
        }

        public override IEnumerable<string> ReadLines()
        {
            return ReadLines(this.getStream);
        }

        public override Source SaveLines(IEnumerable<string> lines)
        {
            var dataStr = string.Join("\r\n", lines);

            return new InMemorySource(
                () => new MemoryStream(Encoding.UTF8.GetBytes(dataStr)),
                this.LinesInBatch);
        }

        public static IEnumerable<string> ReadLines(Func<Stream> getContetns)
        {
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
