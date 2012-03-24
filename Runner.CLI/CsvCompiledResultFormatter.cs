﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public class CsvCompiledResultFormatter : IResultFormatter<CompiledScenarioResult>
    {
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
                        row[headerRow.IndexOf(run.FormattedName)] = (run.AverageCommitTime + run.AverageApplicationTime + run.AverageSetupTime).ToString();
                    }

                    csv.AppendLine(string.Join(",", row));
                }
            }

            File.WriteAllText("test.csv", csv.ToString());
        }
    }
}
