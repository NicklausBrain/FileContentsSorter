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

        public static void WriteAllLines(string path, IEnumerable<string> contents, int bufferSize = DefaultBufferSize)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (contents == null)
                throw new ArgumentNullException(nameof(contents));
            if (path.Length == 0)
                throw new ArgumentException(nameof(path));

            var encoding = Encoding.UTF8;

            var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, bufferSize);

            var sw = new StreamWriter(fs, encoding, bufferSize, false);

            InternalWriteAllLines(sw, contents);
        }

        private static void InternalWriteAllLines(TextWriter writer, IEnumerable<string> contents)
        {
            using (writer)
            {
                foreach (string line in contents)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
