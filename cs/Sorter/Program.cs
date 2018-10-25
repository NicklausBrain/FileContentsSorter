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
        private const int BatchToSave = 1000000;

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
                            sortingResult
                                .Batch(BatchToSave)
                                .Select(batch =>
                                {
                                    var sb = new StringBuilder();
                                    batch.ForEach(line => sb.AppendLine(line));
                                    return sb.ToString();
                                })
                                .ForEach(batch =>
                                {
                                    File.AppendAllText(options.OutputPath, batch);
                                });

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