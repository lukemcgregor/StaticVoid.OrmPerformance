using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Runner.Config;

namespace StaticVoid.OrmPerformance.Formatters
{
    public class ResultCompiler : IResultFormatter<ScenarioInRunResult>
    {
        private IEnumerable<IResultFormatter<CompiledScenarioResult>> _compiledResultFormatters;
        private IRunnerConfig _config;

        public ResultCompiler(IEnumerable<IResultFormatter<CompiledScenarioResult>> compiledResultFormatters,IRunnerConfig config)
        {
            _compiledResultFormatters = compiledResultFormatters;
            _config = config;
        }

        public void FormatResults(IEnumerable<ScenarioInRunResult> inRunResults)
        {
            var resultsByRun = inRunResults.GroupBy(result => result.RunNumber);

            var compiledResults = new List<CompiledScenarioResult>();
            foreach (var result in resultsByRun.First())
            {
                var results = from set in resultsByRun
                              from res in set
                              where res.ConfigurationName == result.ConfigurationName
                              where res.SampleSize == result.SampleSize
                              where res.ScenarioName == result.ScenarioName
                              where res.Technology == result.Technology
                              select res;
                compiledResults.Add(new CompiledScenarioResult
                {
                    ConfigurationName = result.ConfigurationName,
                    SampleSize = result.SampleSize,
                    ScenarioName = result.ScenarioName,
                    Technology = result.Technology,
                    MinSetupTime            = results.OrderBy(r => r.SetupTime)         .Take(results.Count() - _config.DiscardWorst).Min     (r => r.SetupTime),
                    AverageSetupTime        = results.OrderBy(r => r.SetupTime)         .Take(results.Count() - _config.DiscardWorst).Average (r => r.SetupTime),
                    MaxSetupTime            = results.OrderBy(r => r.SetupTime)         .Take(results.Count() - _config.DiscardWorst).Max     (r => r.SetupTime),
                    MinApplicationTime      = results.OrderBy(r => r.ApplicationTime)   .Take(results.Count() - _config.DiscardWorst).Min     (r => r.ApplicationTime),
                    AverageApplicationTime  = results.OrderBy(r => r.ApplicationTime)   .Take(results.Count() - _config.DiscardWorst).Average (r => r.ApplicationTime),
                    MaxApplicationTime      = results.OrderBy(r => r.ApplicationTime)   .Take(results.Count() - _config.DiscardWorst).Max     (r => r.ApplicationTime),
                    MinCommitTime           = results.OrderBy(r => r.CommitTime)        .Take(results.Count() - _config.DiscardWorst).Min     (r => r.CommitTime),
                    AverageCommitTime       = results.OrderBy(r => r.CommitTime)        .Take(results.Count() - _config.DiscardWorst).Average (r => r.CommitTime),
                    MaxCommitTime           = results.OrderBy(r => r.CommitTime)        .Take(results.Count() - _config.DiscardWorst).Max     (r => r.CommitTime),
                    Status                  = results.Any(r => r.Status == "Failed") ? "Failed" : "Passed",
                    MemoryUsage             = results.Count() > _config.DiscardWorst + _config.DiscardHighestMemory
                                                ? results.OrderBy(r => r.MemoryUsage).Take(results.Count() - _config.DiscardWorst)
                                                    .OrderByDescending(r => r.MemoryUsage).Take(results.Count() - _config.DiscardWorst - _config.DiscardHighestMemory).Average(r => r.MemoryUsage)
                                                : results.Average(r => r.MemoryUsage)
                });

            }

            foreach (var formatter in _compiledResultFormatters)
            {
                formatter.FormatResults(compiledResults);
            }
        }
    }
}
