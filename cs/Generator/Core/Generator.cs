using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Generator.Core
{
    public class Generator
    {
        private const int BatchSize = 10000000;
        private static readonly object Lock = new object();
        private readonly Lazy<string[]> lazyStrings = new Lazy<string[]>(Data.RandomPoemLines);
        private string[] Strings => lazyStrings.Value;
        protected readonly Func<Random> CreateRandom;

        public Generator(Func<Random> createRandom)
        {
            this.CreateRandom = createRandom;
        }

        public IEnumerable<string> CreateSequence(ulong count)
        {
            var random = this.CreateRandom();

            for (ulong i = 0; i < count; i++)
            {
                yield return GenerateString(random);
            }
        }

        public string GenerateString(Random random)
        {
            var rsi = random.Next(0, Strings.Length);
            var rs = Strings[rsi];
            var ri = random.Next(short.MinValue, short.MaxValue);
            return $"{ri}. {rs}";
        }

        public string GenerateBatch(int linesInBatch)
        {
            var random = this.CreateRandom();
            var presumableSize = 48 * linesInBatch;
            var sb = new StringBuilder(presumableSize);
            for (int i = 0; i < linesInBatch; i++)
            {
                sb.AppendLine(GenerateString(random));
            }
            return sb.ToString();
        }

        public void GenerateTo(ulong linesCount, Func<Stream> openOutput)
        {
            var batches = linesCount > BatchSize
                ? Enumerable
                    .Range(0, (int)(linesCount / BatchSize))
                    .Select(lines => BatchSize)
                    .Append((int)(linesCount % BatchSize))
                    .Where(lines => lines != 0)
                    .ToArray()
                : new[] { (int)linesCount };

            batches
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(this.GenerateBatch)
                .Select(Encoding.UTF8.GetBytes)
                .ForAll(dataSet => SaveToStream(dataSet, openOutput));
        }

        private void SaveToStream(byte[] data, Func<Stream> openOutput)
        {
            lock (Lock)
            {
                using (var stream = openOutput())
                using (var memory = new MemoryStream(data, false))
                {
                    stream.Seek(0, SeekOrigin.End);
                    memory.CopyTo(stream);
                }
            }
        }
    }
}
