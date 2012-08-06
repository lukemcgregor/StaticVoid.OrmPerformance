using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.IO;

namespace StaticVoid.OrmPerformance.Formatters
{
    public class CsvRawResultFormatter: IResultFormatter<ScenarioInRunResult>
    {
        private IFileOutputLocation _outputLocation;
        public CsvRawResultFormatter(IFileOutputLocation outputLocation)
        {
            _outputLocation = outputLocation;
        }

        public void FormatResults(IEnumerable<ScenarioInRunResult> results)
        {
            StringBuilder csv = new StringBuilder();

            List<String> headerRow = new List<string>{"",""}; // title column
            headerRow.AddRange(results.SelectMany(r => new String[] { r.SetupFormattedName, r.ApplicationFormattedName, r.CommitFormattedName }).Distinct().OrderBy(g => g));

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
                        row[headerRow.IndexOf(run.SetupFormattedName)] = run.SetupTime.ToString();
                        row[headerRow.IndexOf(run.ApplicationFormattedName)] = run.ApplicationTime.ToString();
                        row[headerRow.IndexOf(run.CommitFormattedName)] = run.CommitTime.ToString();
                    }

                    csv.AppendLine(string.Join(",", row));
                }
            }

            File.WriteAllText(_outputLocation.OutputDirectory.GetFilePath("raw-times.csv"), csv.ToString());
        }

        public string Name
        {
            get { return "Csv Raw Result Formatter"; }
        }
    }
}
