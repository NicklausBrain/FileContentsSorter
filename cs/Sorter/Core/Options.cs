using CommandLine;

namespace Sorter.Core
{
    public class Options
    {
        [Option('s', "source", Required = true, HelpText = "Source file with the contents to sort")]
        public string SourcePath { get; set; }

        [Option('o', "output", Required = false, HelpText = "File to save sorting results in")]
        public string OutputPath { get; set; }

        public bool IsOutputPathSpecified => string.IsNullOrWhiteSpace(this.OutputPath);

        [Option('е', "temp", Required = false, HelpText = "Directory to save temporary files in")]
        public string TempDirectory { get; set; }

        [Option('b', "batch", Required = false, HelpText = "Batch size...")]
        public int BatchSize { get; set; }
    }
}
