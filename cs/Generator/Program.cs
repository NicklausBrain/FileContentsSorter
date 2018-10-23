using System;
using System.IO;
using CommandLine;

namespace Generator
{
    public class Program
    {
        public class Options
        {
            [Option('l', "lines", Required = true, HelpText = "Lines to generate")]
            public ulong Lines { get; set; }

            [Option('f', "file", Required = false, HelpText = "Output file path")]
            public string FilePath { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    var lines = options.Lines;
                    var gen = new Generator();

                    if (string.IsNullOrWhiteSpace(options.FilePath))
                    {
                        var seq = gen.CreateSequence(lines);

                        foreach (var str in seq)
                        {
                            Console.WriteLine(str);
                        }
                    }
                    else
                    {
                        Stream OpenOutput() => File.OpenWrite(options.FilePath);
                        gen.GenerateTo(lines, OpenOutput);
                    }
                });
        }
    }
}
