using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioRunner
    {
        private IEnumerable<IRunnableScenario> _scenarios;

        public ScenarioRunner(IEnumerable<IRunnableScenario> scenarios)
        {
            _scenarios = scenarios;
        }

        public List<ScenarioResult> Run(int maxSample)
        {
            List<ScenarioResult> results = new List<ScenarioResult>();

            for (int i = 1; i <= maxSample; i = i * 10)
            {
                foreach (var scenario in _scenarios)
                {
                    Console.WriteLine(String.Format("Starting sample size {0} for {1} at {2}", i, scenario.Name, DateTime.Now.ToShortTimeString()));
                    results.AddRange(scenario.Run(i));
                }
            }
            return results;
        }
    }
}
