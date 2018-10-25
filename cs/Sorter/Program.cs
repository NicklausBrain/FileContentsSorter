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

                        var sortingResult = dataSource.OrderLines();

                        if (options.IsOutputPathSpecified)
                        {
                            File.WriteAllLines(options.OutputPath, sortingResult);

                            sortingResult.ClearTempSources();
                        }
                        else
                        {
                            sortingResult.ForEach(line => Console.WriteLine(line));
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