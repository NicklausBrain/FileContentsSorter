using System;
using System.IO;
using CommandLine;
using Sorter.Core;

namespace Sorter
{
    public class Program
    {
        public class Options
        {
            [Option('s', "source", Required = true, HelpText = "Source file with the contents to sort")]
            public string SourcePath { get; set; }

            [Option('o', "output", Required = false, HelpText = "File to save sorting results in")]
            public string OutputPath { get; set; }

            [Option('е', "temp", Required = false, HelpText = "Directory to save temporary files in")]
            public string TempDirectory { get; set; }

            [Option('b', "batch", Required = false, HelpText = "Batch size...")]
            public int BatchSize { get; set; }
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    const int defaultBatchSize = 10000000;
                    var sourcePath = options.SourcePath;
                    var batchSize = options.BatchSize == 0 ? defaultBatchSize : options.BatchSize;

                    var source = new FileSource(
                        () => new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.None),
                        batchSize);

                    var lines = source.OrderLines();

                    if (string.IsNullOrWhiteSpace(options.OutputPath))
                    {
                        foreach (var line in lines)
                        {
                            Console.WriteLine(line);
                        }
                    }
                    else
                    {
                        File.WriteAllLines(options.OutputPath, lines);
                    }
                });
        }
    }
}