using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.IO;

namespace StaticVoid.OrmPerformance.Formatters
{
    public class CsvCompiledMemoryResultFormatter : IResultFormatter<CompiledScenarioResult>
    {
        private IFileOutputLocation _outputLocation;
        public CsvCompiledMemoryResultFormatter(IFileOutputLocation outputLocation)
        {
            _outputLocation = outputLocation;
        }

        public void FormatResults(IEnumerable<CompiledScenarioResult> results)
        {
            StringBuilder csv = new StringBuilder();

            List<String> headerRow = new List<string>{"",""}; // title column
            headerRow.AddRange(results.Select(r => r.FormattedName).Distinct().OrderBy(g => g));

            csv.AppendLine(string.Join(",", headerRow));

            foreach (var scenario in results.GroupBy(r => r.ScenarioName))
            {
                foreach (var sample in scenario.GroupBy(r => r.SampleSize))
                {
                    string[] row = new string[headerRow.Count];

                    row[0] = scenario.Key;
                    row[1] = sample.Key.ToString();
                    foreach (var run in sample)
                    {
                        row[headerRow.IndexOf(run.FormattedName)] = (run.MemoryUsage/1024).ToString();//log memory in kb
                    }

                    csv.AppendLine(string.Join(",", row));
                }
            }

            File.WriteAllText(_outputLocation.OutputDirectory.GetFilePath("memory.csv"), csv.ToString());
        }
    }
}
