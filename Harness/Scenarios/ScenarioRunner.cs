using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Scenarios;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Messaging.Messages;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioRunner
    {
        private readonly IEnumerable<IRunnableScenario> _scenarios;
        private readonly ISendMessages _sender;
        private readonly ISelectedScenarios _selectedScenarios;
        private readonly ISampleSizeStep _sampleSizeStep;

        public ScenarioRunner(IEnumerable<IRunnableScenario> scenarios, ISendMessages sender, ISampleSizeStep sampleSizeStep)
        {
            _scenarios = scenarios;
            _sender = sender;
            _sampleSizeStep = sampleSizeStep;
        }

        public ScenarioRunner(ISelectedScenarios selectedScenarios, IEnumerable<IRunnableScenario> scenarios, ISendMessages sender, ISampleSizeStep sampleSizeStep): this(scenarios,sender,sampleSizeStep)
        {
            _selectedScenarios = selectedScenarios;
        }

        public List<ScenarioResult> Run(int maxSample, CancellationToken cancellationToken)
        {
            var scenarios = _scenarios;

            if (_selectedScenarios != null)
            {
                scenarios = _scenarios.Where(s=>_selectedScenarios.SelectedScenarios.Any(sel =>sel.Name == s.Name)); 
            }

            List<ScenarioResult> results = new List<ScenarioResult>();

            for (int i = 1; i <= maxSample; i = _sampleSizeStep.Increment(i))
            {
                _sender.Send(new SampleSizeChanged { SampleSize = i });
                foreach (var scenario in scenarios)
                {
                    _sender.Send(new ScenarioChanged { Scenario = scenario.Name });

                    results.AddRange(scenario.Run(i, cancellationToken));
                }
            }
            return results;
        }
    }
}
