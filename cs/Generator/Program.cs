using System;
using System.IO;
using CommandLine;
using Generator.Core;
using MoreLinq;

namespace Generator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    if (options.AreValid)
                    {
                        var lines = options.Lines;
                        var gen = new Core.Generator(RandomFactory.Create);

                        if (options.IsOutputFileSpecified)
                        {
                            Stream OpenOutput() => File.OpenWrite(options.FilePath);
                            gen.GenerateTo(lines, OpenOutput);
                        }
                        else
                        {
                            var seq = gen.CreateSequence(lines);
                            seq.ForEach(line => Console.WriteLine(line));
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
