using System;
using System.IO;
using CommandLine;
using MoreLinq;
using Sorter.Core;

namespace Sorter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    if (options.AreValid)
                    {
                        var dataSource = DataSourceFactory.Create(options);

                        if (options.IsOutputPathSpecified)
                        {
                            Console.WriteLine($"Starting to process {options.SourcePath} at {DateTime.Now}");

                            var sortingResult = dataSource.OrderLines();

                            File.WriteAllLines(options.OutputPath, sortingResult);

                            Console.WriteLine($"Finished processing {options.SourcePath} at {DateTime.Now}");

                            Console.WriteLine("Cleaning temp files");

                            sortingResult.ClearTempSources();

                            Console.WriteLine("Done.");
                        }
                        else
                        {
                            dataSource.OrderLines().ForEach(line => Console.WriteLine(line));
                        }
                    }
                    else
                    {
                        options.ValidationErrors.ForEach(e => Console.WriteLine(e));
                    }
                });
        }
    }
}