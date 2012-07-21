using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StaticVoid.OrmPerformance.Runner.Config;
using StaticVoid.OrmPerformance.Formatters;
using StaticVoid.OrmPerformance.Harness;

namespace StaticVoid.OrmPerformance.Runner.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new OrmConfigurationModule(), new HarnessModule());

            var config = kernel.Get<IRunnerConfig>();

            List<ScenarioInRunResult> allRunResults = new List<ScenarioInRunResult>();

            for (int i = 0; i < config.NumberOfRuns; i++)
            {
                Console.WriteLine(String.Format("Starting run number {0} at {1}", i, DateTime.Now.ToShortTimeString()));
                allRunResults.AddRange(kernel.Get<ScenarioRunner>().Run(config.MaximumSampleSize)
                    .Select(r =>
                        new ScenarioInRunResult
                        {
                            ApplicationTime = r.ApplicationTime,
                            CommitTime = r.CommitTime,
                            ConfigurationName = r.ConfigurationName,
                            MemoryUsage = r.MemoryUsage,
                            SampleSize = r.SampleSize,
                            ScenarioName = r.ScenarioName,
                            SetupTime = r.SetupTime,
                            Status = r.Status,
                            Technology = r.Technology,
                            RunNumber = i + 1
                        }));
            }

            foreach (var formatter in kernel.GetAll<IResultFormatter<ScenarioInRunResult>>())
            {
                formatter.FormatResults(allRunResults);
            }


            Console.ReadLine();
        }
    }
}
