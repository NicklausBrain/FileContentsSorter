using System.IO;

namespace Sorter.Core
{
    public class DataSourceFactory
    {
        public static DataSource Create(Options options)
        {
            var sourcePath = options.SourcePath;

            var dataSource = new DataSource(
                readLines: () => CustomIO.File.ReadLines(sourcePath, 128 * 1024 * 1024),
                linesInBatch: options.BatchSize,
                saveLines: lines =>
                {
                    var tempFile = GetTempFilePath(options);

                    File.WriteAllLines(tempFile, lines);

                    return new DataSource(
                        readLines: () => File.ReadLines(tempFile),
                        linesInBatch: options.BatchSize,
                        deleteSource: () =>
                        {
                            File.Delete(tempFile);
                            return true;
                        });
                },
                comparer: new DefaultComparer());

            return dataSource;
        }

        private static string GetTempFilePath(Options options)
        {
            var tempFile = Path.GetTempFileName();

            return options.IsTempDirectorySpecified
                ? Path.Combine(options.TempDirectory, new FileInfo(tempFile).Name)
                : tempFile;
        }
    }
}
