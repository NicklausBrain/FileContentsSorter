using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Generator.Core
{
    public class Generator
    {
        private const int DefaultBatchSize = 1000000;

        private static readonly object Lock = new object();

        private readonly Lazy<string[]> lazyStrings = new Lazy<string[]>(Data.RandomPoemLines);
        private string[] Strings => this.lazyStrings.Value;
        private readonly Func<Random> createRandom;
        private readonly int batchSize;

        public Generator(Func<Random> createRandom, int batchSize)
        {
            this.createRandom = createRandom;
            this.batchSize = batchSize <= 0 ? DefaultBatchSize : batchSize;
        }

        public IEnumerable<string> CreateSequence(ulong count)
        {
            var random = this.createRandom();

            for (ulong i = 0; i < count; i++)
            {
                yield return this.GenerateString(random);
            }
        }

        public string GenerateString(Random random)
        {
            var rsi = random.Next(0, this.Strings.Length);
            var rs = this.Strings[rsi];
            var ri = random.Next(short.MinValue, short.MaxValue);
            return $"{ri}. {rs}";
        }

        public string GenerateBatch(int linesInBatch)
        {
            var random = this.createRandom();
            var presumableSize = 48 * linesInBatch;
            var sb = new StringBuilder(presumableSize);
            for (int i = 0; i < linesInBatch; i++)
            {
                sb.AppendLine(this.GenerateString(random));
            }
            return sb.ToString();
        }

        public void GenerateTo(ulong linesCount, Func<Stream> openOutput)
        {
            var batches = linesCount > (ulong)this.batchSize
                ? Enumerable
                    .Range(0, (int)(linesCount / (ulong)this.batchSize))
                    .Select(lines => this.batchSize)
                    .Append((int)(linesCount % (ulong)this.batchSize))
                    .Where(lines => lines != 0)
                    .ToArray()
                : new[] { (int)linesCount };

            batches
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(this.GenerateBatch)
                .Select(Encoding.UTF8.GetBytes)
                .ForAll(dataSet => this.SaveToStream(dataSet, openOutput));
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
