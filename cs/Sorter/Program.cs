using System;
using System.IO;
using Sorter.Core;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = args.Length == 1 ? args[0] : @"d:\test.data";

            //var contents = new Contents(() => new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None));

            //var lines = contents.OrderLines(4);

            //foreach (var line in lines)
            //{
            //    Console.WriteLine(line);
            //}
        }
    }
}
