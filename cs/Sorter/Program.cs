using System;
using System.IO;
using Sorter.Core;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourcePath = args.Length == 1 ? args[0] : @"d:\10Mtest.data";

            var source = new FileSource(
                () => new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.None),
                1000000);

            var lines = source.OrderLines();

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
