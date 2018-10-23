using System;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using MoreLinq;
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
                    var batchSize = options.BatchSize == 0
                        ? defaultBatchSize
                        : options.BatchSize;

                    var source = new Source(
                        readLines: () => File.ReadLines(sourcePath),
                        linesInBatch: batchSize,
                        saveLines: lines =>
                        {
                            var tempFile = Path.GetTempFileName();
                            File.WriteAllLines(tempFile, lines);
                            return new Source(() => File.ReadLines(tempFile));
                        });

                    var orderedLines = source.OrderLines();

                    if (string.IsNullOrWhiteSpace(options.OutputPath))
                    {
                        orderedLines.ForEach(line => Console.WriteLine(line));
                    }
                    else
                    {
                        orderedLines
                            .Batch(defaultBatchSize)
                            .Select(batch =>
                            {
                                var sb = new StringBuilder();
                                batch.ForEach(line => sb.AppendLine(line));
                                return sb.ToString();
                            })
                            .Select(batch => Encoding.UTF8.GetBytes(batch))
                            .ForEach(batch =>
                            {
                                using (var file = new FileStream(options.OutputPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                                using (var memory = new MemoryStream(batch))
                                {
                                    file.Seek(0, SeekOrigin.End);
                                    memory.CopyTo(file);
                                }
                            });
                    }
                });
        }
    }
}