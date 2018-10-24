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
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    var dataSource = DataSourceFactory.Create(options);

                    var sortingResult = dataSource.OrderLines();

                    if (options.IsOutputPathSpecified)
                    {
                        sortingResult.ForEach(line => Console.WriteLine(line));
                    }
                    else
                    {
                        sortingResult
                            .Batch(dataSource.LinesInBatch)
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

                        sortingResult.ClearTempSources();
                    }
                });
        }
    }
}