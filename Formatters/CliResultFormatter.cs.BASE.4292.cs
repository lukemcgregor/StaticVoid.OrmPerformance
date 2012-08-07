using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public class CliResultFormatter : IResultFormatter<CompiledScenarioResult>
    {
        private IRunnerConfig _config;
        public CliResultFormatter(IRunnerConfig config)
        {
            _config = config;
        }

        public void FormatResults(IEnumerable<CompiledScenarioResult> results)
        {
            Console.WriteLine(String.Format("Stats calculated on the best {0} of {1} runs", _config.NumberOfRuns - _config.DiscardWorst, _config.NumberOfRuns));

            foreach (var techGroup in results.GroupBy(r => r.Technology))
            {
                Console.WriteLine(techGroup.Key);
                foreach (var scenarioGroup in techGroup.GroupBy(r => r.ScenarioName))
                {
                    Console.WriteLine("\t{0}", scenarioGroup.Key);
                    foreach (var configGroup in scenarioGroup.GroupBy(r => r.ConfigurationName))
                    {
                        Console.WriteLine("\t\t{0}", configGroup.Key);
                        foreach (var result in configGroup)
                        {
                            Console.WriteLine(String.Format("\t\t\t {14} with a sample size of {3} and the following times: {4} \t\t\t\tSetup Time: MIN {5}ms, AVG {6}ms, MAX {7}ms {4} \t\t\t\tApplication Time: MIN {8}ms, AVG {9}ms, MAX {10}ms {4} \t\t\t\tCommit Time:  MIN {11}ms, AVG {12}ms, MAX {13}ms",
                                result.Technology,
                                result.ConfigurationName,
                                result.ScenarioName,
                                result.SampleSize,
                                Environment.NewLine,
                                result.MinSetupTime,
                                result.AverageSetupTime,
                                result.MaxSetupTime,
                                result.MinApplicationTime,
                                result.AverageApplicationTime,
                                result.MaxApplicationTime,
                                result.MinCommitTime,
                                result.AverageCommitTime,
                                result.MaxCommitTime,
                                result.Status));
                        }
                    }
                }
            }
        }
    }
}
