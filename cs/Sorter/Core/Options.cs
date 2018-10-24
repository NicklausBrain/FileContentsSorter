using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using CommandLine;

namespace Sorter.Core
{
    public class Options : IValidatableObject
    {
        [Option('s', "source", Required = true, HelpText = "Source file with the contents to sort")]
        public string SourcePath { get; set; }

        [Option('o', "output", Required = false, HelpText = "File to save sorting results in")]
        public string OutputPath { get; set; }

        public bool IsOutputPathSpecified => !string.IsNullOrWhiteSpace(this.OutputPath);

        [Option('t', "temp", Required = false, HelpText = "Directory to save temporary files in")]
        public string TempDirectory { get; set; }

        public bool IsTempDirectorySpecified => !string.IsNullOrWhiteSpace(this.TempDirectory);

        [Option('b', "batch", Required = false, HelpText = "Batch size (count of items to sort in one step)")]
        public int BatchSize { get; set; }

        public bool AreValid => !this.ValidationErrors.Any();

        public IEnumerable<string> ValidationErrors => this
            .Validate(new ValidationContext(this))
            .Select(e => e.ErrorMessage);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!File.Exists(this.SourcePath))
            {
                yield return new ValidationResult("source file does not exist!");
            }

            if (this.IsOutputPathSpecified && !Directory.Exists(Path.GetDirectoryName(this.OutputPath)))
            {
                yield return new ValidationResult("output directory does not exist!");
            }

            if (this.IsTempDirectorySpecified && !Directory.Exists(this.TempDirectory))
            {
                yield return new ValidationResult("temp directory does not exist!");
            }
        }
    }
}
