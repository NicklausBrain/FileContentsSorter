using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter
{
    public class Sorter
    {
        public IEnumerable<string> SortContents(Func<Stream> getContetns)
        {
            return SortContents(ReadLines(getContetns));
        }

        public IEnumerable<string> SortContents(IEnumerable<string> lines)
        {
            var result =
                lines
                    .Select(l => TestStruct.Parse(l))
                    .OrderBy(t => t)
                    .Select(t => t.ToString());

            return result;
        }

        public IEnumerable<string> SortContents2(Func<Stream> getContetns, int parts)
        {
            var lines = ReadLines(getContetns);
            var linesParts = lines.Split(parts);
            IEnumerable<IOrderedEnumerable<TestStruct>> structParts = linesParts.Select(part => part.Select(l => TestStruct.Parse(l)).OrderBy(t => t));
            var tempfiles = structParts.Select(p => WriteLines(p.Select(s => s.ToString())));

            var sequences = tempfiles.Select(f => ReadLines(f.OpenRead).Select(l => TestStruct.Parse(l))).ToArray();
            var result = Merge(sequences);
            return result.Select(s => s.ToString());
        }

        public IEnumerable<T> Merge<T>(params IEnumerable<T>[] sequences) where T : struct
        {
            IEnumerator<T>[] enumerators =
                sequences.Select(s => s.GetEnumerator()).ToArray();

            IDictionary<IEnumerator<T>, T?> enums =
                enumerators.ToDictionary(
                    e => e,
                    v => v.MoveNext()
                        ? v.Current
                        : (T?)null);

            while (enums.Values.Any(v => v.HasValue))
            {
                var min = enums.OrderBy(e => e.Value).First(e => e.Value.HasValue);

                if (min.Value.HasValue)
                {
                    yield return min.Value.Value;
                }

                enums[min.Key] = min.Key.MoveNext()
                    ? min.Key.Current
                    : (T?)null;
            }
        }

        public IEnumerable<T> Merge2<T>(IEnumerable<T> seqA, IEnumerable<T> seqB) where T : struct, IComparable
        {
            IEnumerator<T> enumeratorA = seqA.GetEnumerator();
            IEnumerator<T> enumeratorB = seqB.GetEnumerator();

            T? a = null;
            T? b = null;

            a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
            b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;

            do
            {
                if (a.HasValue && b.HasValue)
                {
                    if(a.Value.CompareTo(b.Value) < 0)
                    {
                        yield return a.Value;
                        a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
                    }
                    else
                    {
                        yield return b.Value;
                        b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;
                    }
                }
                else if (a.HasValue)
                {
                    yield return a.Value;
                    a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
                }
                else if (b.HasValue)
                {
                    yield return b.Value;
                    b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;
                }
            }
            while(a.HasValue || b.HasValue);
        }

        private IEnumerable<string> ReadLines(Func<Stream> getContetns)
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

        private FileInfo WriteLines(IEnumerable<string> lines)
        {
            var temp = Path.GetTempFileName();

            using (var stream = File.Create(temp))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

            return new FileInfo(temp);
        }
    }
}
