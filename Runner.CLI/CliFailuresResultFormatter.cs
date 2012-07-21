using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public class CliFailuresResultFormatter : IResultFormatter<CompiledScenarioResult>
    {
        private IRunnerConfig _config;
		public CliFailuresResultFormatter(IRunnerConfig config)
        {
            _config = config;
        }

        public void FormatResults(IEnumerable<CompiledScenarioResult> results)
        {
			if (results.All(r => r.Status.State == OrmPerformance.Harness.Scenarios.Assertion.AssertionResultState.Pass))
				Console.WriteLine("All Assertions passed successfully");
			else {
				Console.WriteLine("{0} Assertions Failed", results.Count(r => r.Status.State != OrmPerformance.Harness.Scenarios.Assertion.AssertionResultState.Pass));

				foreach(var result in results.Where(r => r.Status.State != OrmPerformance.Harness.Scenarios.Assertion.AssertionResultState.Pass))
				{
					 Console.WriteLine(String.Format("\t Tech: {0}, Config: {1}, Scenario: {2}, Sample size of {3}:\n\t Status: {4}",
                                result.Technology,
                                result.ConfigurationName,
                                result.ScenarioName,
                                result.SampleSize,
								result.Status));
				}
			}
        }
    }
}
