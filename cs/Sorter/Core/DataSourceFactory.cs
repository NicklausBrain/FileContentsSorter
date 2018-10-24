using System.IO;

namespace Sorter.Core
{
    public class DataSourceFactory
    {
        public static DataSource Create(Options options)
        {
            var sourcePath = options.SourcePath;

            var dataSource = new DataSource(
                readLines: () => File.ReadLines(sourcePath),
                linesInBatch: options.BatchSize,
                saveLines: lines =>
                {
                    var tempFile = Path.GetTempFileName();
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
    }
}
