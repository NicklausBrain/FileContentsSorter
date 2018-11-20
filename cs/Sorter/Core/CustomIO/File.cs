using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sorter.Core.CustomIO
{
    public class File
    {
        private const int DefaultBufferSize = 4096;

        public static IEnumerable<string> ReadLines(string path, int bufferSize = DefaultBufferSize)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (path.Length == 0)
                throw new ArgumentException(nameof(path));

            var encoding = Encoding.UTF8;

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);

            var sr = new StreamReader(fs, encoding, false, bufferSize, false);

            return ReadLinesIterator.CreateIterator(sr, encoding);
        }
    }
}
