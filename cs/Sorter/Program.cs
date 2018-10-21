using System;
using System.IO;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = args.Length == 1 ? args[0] : @"d:\test.data";

            var s = new Sorter();

            var lines = s.SortContents2(() => new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None), 4);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
