using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using CommandLine;

namespace Generator.Core
{
    public class Options : IValidatableObject
    {
        [Option('l', "lines", Required = true, HelpText = "Lines to generate")]
        public ulong Lines { get; set; }

        [Option('f', "file", Required = false, HelpText = "Output file path")]
        public string FilePath { get; set; }

        public bool IsOutputFileSpecified => !string.IsNullOrWhiteSpace(this.FilePath);

        public bool AreValid => !this.ValidationErrors.Any();

        public IEnumerable<string> ValidationErrors => this
            .Validate(new ValidationContext(this))
            .Select(e => e.ErrorMessage);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Lines <= 0)
            {
                yield return new ValidationResult("incorrect number of lines!");
            }

            if (this.IsOutputFileSpecified && !Directory.Exists(Path.GetDirectoryName(this.FilePath)))
            {
                yield return new ValidationResult("output directory does not exist!");
            }
        }
    }
}
