using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter
{
    public class Sorter
    {
        public IEnumerable<string> SortContents(Func<Stream> getContetns)
        {
            var lines = ReadLines(getContetns);

            var result =
                lines
                    .Select(l => TestStruct.Parse(l))
                    .OrderBy(t => t)
                    .Select(t => t.ToString());

            return result;
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
    }
}
