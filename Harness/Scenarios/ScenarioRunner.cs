using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Messaging.Messages;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioRunner
    {
        private readonly IEnumerable<IRunnableScenario> _scenarios;
        private readonly ISendMessages _sender;

        public ScenarioRunner(IEnumerable<IRunnableScenario> scenarios, ISendMessages sender)
        {
            _scenarios = scenarios;
            _sender = sender;
        }

        public List<ScenarioResult> Run(int maxSample, CancellationToken cancellationToken)
        {
            List<ScenarioResult> results = new List<ScenarioResult>();

            for (int i = 1; i <= maxSample; i = i * 10)
            {
                _sender.Send(new SampleSizeChanged { SampleSize = i });
                foreach (var scenario in _scenarios)
                {
                    _sender.Send(new ScenarioChanged { Scenario = scenario.Name });

                    results.AddRange(scenario.Run(i, cancellationToken));
                }
            }
            return results;
        }
    }
}
