using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter.Core
{
    public class Contents
    {
        Func<Stream> getContetns;

        public Contents(Func<Stream> getContetns)
        {
            this.getContetns = getContetns;
        }

        public IEnumerable<string> SortLines()
        {
            return SortLines(ReadLines(this.getContetns));
        }

        public IEnumerable<string> SortLines(IEnumerable<string> lines)
        {
            var result =
                lines
                    .Select(l => TestStruct.Parse(l))
                    .OrderBy(t => t)
                    .Select(t => t.ToString());

            return result;
        }

        public IEnumerable<string> SortLines(int parts)
        {
            var lines = SortLines(ReadLines(this.getContetns));
            var linesParts = lines.Split(parts);
            IEnumerable<IOrderedEnumerable<TestStruct>> structParts = linesParts.Select(part => part.Select(l => TestStruct.Parse(l)).OrderBy(t => t));
            var tempfiles = structParts.Select(p => WriteLines(p.Select(s => s.ToString())));

            var sequences = tempfiles.Select(f => ReadLines(f.OpenRead).Select(l => TestStruct.Parse(l))).ToArray();

            var result = sequences.Aggregate(LinqExtensions.Merge);
            return result.Select(s => s.ToString());
        }

        public IEnumerable<string> ReadLines(Func<Stream> getContetns)
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
