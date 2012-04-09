using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.IO;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public class CsvCompiledBestResultFormatter : IResultFormatter<CompiledScenarioResult>
    {
        private IFileOutputLocation _outputLocation;
        public CsvCompiledBestResultFormatter(IFileOutputLocation outputLocation)
        {
            _outputLocation = outputLocation;
        }

        public void FormatResults(IEnumerable<CompiledScenarioResult> results)
        {
            StringBuilder csv = new StringBuilder();

            var bestConfigs = new List<CompiledScenarioResult>();

            foreach (var techRun in results.GroupBy(r => new { r.Technology, r.ScenarioName }))
            {
                bestConfigs.AddRange(techRun.GroupBy(tr => tr.FormattedName).OrderBy(r => r.Sum(run => run.AverageCommitTime + run.AverageApplicationTime + run.AverageSetupTime)).First());
            }

            List<String> headerRow = new List<string>{"",""}; // title column
            headerRow.AddRange(bestConfigs.Select(r => r.Technology).Distinct().OrderBy(g => g));

            csv.AppendLine(string.Join(",", headerRow));

            foreach (var scenario in bestConfigs.GroupBy(r => r.ScenarioName))
            {
                foreach (var sample in scenario.GroupBy(r => r.SampleSize))
                {
                    string[] row = new string[headerRow.Count];

                    row[0] = scenario.Key;
                    row[1] = sample.Key.ToString();
                    foreach (var run in sample)
                    {
                        row[headerRow.IndexOf(run.Technology)] = (run.AverageCommitTime + run.AverageApplicationTime + run.AverageSetupTime).ToString();
                    }

                    csv.AppendLine(string.Join(",", row));
                }
            }

            File.WriteAllText(_outputLocation.OutputDirectory.GetFilePath("best-times.csv"), csv.ToString());
        }
    }
}
