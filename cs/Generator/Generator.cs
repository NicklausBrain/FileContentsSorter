using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Generator
{
    public class Generator
    {
        const string RandomPoems = @"
Whose seal is that? I think I know.
Its owner is quite happy though.
Full of joy like a vivid rainbow,
I watch him laugh. I cry hello.

He gives his seal a shake,
And laughs until her belly aches.
The only other sound's the break,
Of distant waves and birds awake.

The seal is acid, empty and deep,
But he has promises to keep,
After cake and lots of sleep.
Sweet dreams come to him cheap.

He rises from his gentle bed,
With thoughts of kittens in his head,
He eats his jam with lots of bread.
Ready for the day ahead.";

        private readonly Lazy<string[]> lazyStrings = new Lazy<string[]>(() => RandomPoems.Split('\n', '\r').ToArray());

        private string[] Strings => lazyStrings.Value;

        private const int BatchSize = 10000000;

        private int tickCountOnStart = Environment.TickCount;

        public IEnumerable<string> CreateSequence(ulong count)
        {
            Random random = new Random(Interlocked.Increment(ref tickCountOnStart));

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
            Random random = new Random(Interlocked.Increment(ref tickCountOnStart));

            var presumableSize = 48 * linesInBatch;
            var sb = new StringBuilder(presumableSize);
            for (int i = 0; i < linesInBatch; i++)
            {
                sb.AppendLine(GenerateString(random));
            }
            return sb.ToString();
        }

        private static readonly object Lock = new object();

        public void SaveToStream(byte[] data, Func<Stream> openOutput)
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

            batches.AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(GenerateBatch)
                .Select(Encoding.UTF8.GetBytes)
                .ForAll(dataSet => SaveToStream(dataSet, openOutput));
        }
    }
}
