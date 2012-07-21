using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new OrmConfigurationModule(), new HarnessModule());

            var config = kernel.Get<IRunnerConfig>();

            List<List<ScenarioResult>> allRunResults = new List<List<ScenarioResult>>();

            for (int i = 0; i < config.NumberOfRuns; i++)
            {
                Console.WriteLine(String.Format("Starting run number {0} at {1}", i, DateTime.Now.ToShortTimeString()));
                allRunResults.Add(kernel.Get<ScenarioRunner>().Run(config.MaximumSampleSize));
            }

            var compiledResults = new List<CompiledScenarioResult>();
            foreach (var result in allRunResults[0])
            {
                var results = from set in allRunResults
                              from res in set
                              where res.ConfigurationName == result.ConfigurationName
                              where res.SampleSize == result.SampleSize
                              where res.ScenarioName == result.ScenarioName
                              where res.Technology == result.Technology
                              select res;
                compiledResults.Add(new CompiledScenarioResult
                {
                    ConfigurationName       = result.ConfigurationName,
                    SampleSize              = result.SampleSize,
                    ScenarioName            = result.ScenarioName,
                    Technology              = result.Technology,
                    MinSetupTime            = results.OrderBy(r => r.SetupTime).Take(results.Count()             - config.DiscardWorst).Min(r=>r.SetupTime),
                    AverageSetupTime        = results.OrderBy(r => r.SetupTime).Take(results.Count()             - config.DiscardWorst).Average(r => r.SetupTime),
                    MaxSetupTime            = results.OrderBy(r => r.SetupTime).Take(results.Count()             - config.DiscardWorst).Max(r => r.SetupTime),
                    MinApplicationTime      = results.OrderBy(r => r.ApplicationTime).Take(results.Count()       - config.DiscardWorst).Min(r => r.ApplicationTime),
                    AverageApplicationTime  = results.OrderBy(r => r.ApplicationTime).Take(results.Count()       - config.DiscardWorst).Average(r => r.ApplicationTime),
                    MaxApplicationTime      = results.OrderBy(r => r.ApplicationTime).Take(results.Count()       - config.DiscardWorst).Max(r => r.ApplicationTime),
                    MinCommitTime           = results.OrderBy(r => r.CommitTime).Take(results.Count()            - config.DiscardWorst).Min(r => r.CommitTime),
                    AverageCommitTime       = results.OrderBy(r => r.CommitTime).Take(results.Count()            - config.DiscardWorst).Average(r => r.CommitTime),
                    MaxCommitTime           = results.OrderBy(r => r.CommitTime).Take(results.Count()            - config.DiscardWorst).Max(r => r.CommitTime),
                    Status                  = results.OrderByDescending(r => (int) r.Status.State).Select(r=> r.Status).FirstOrDefault() ?? new AssertionPass(),
                    MemoryUsage             = results.Count() > config.DiscardWorst + config.DiscardHighestMemory 
                                                ? results.OrderBy(r => r.MemoryUsage).Take(results.Count() - config.DiscardWorst)
                                                    .OrderByDescending(r => r.MemoryUsage).Take(results.Count() - config.DiscardWorst -config.DiscardHighestMemory).Average(r => r.MemoryUsage)
                                                : results.Average(r=>r.MemoryUsage)
                });

            }

            foreach (var formatter in kernel.GetAll<IResultFormatter<CompiledScenarioResult>>())
            {
                formatter.FormatResults(compiledResults);
            }

            Console.ReadLine();
        }
    }
}
