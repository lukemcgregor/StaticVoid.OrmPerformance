using System;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Caliburn;
using System.ComponentModel;
using System.Collections.Generic;
using StaticVoid.OrmPerformance.Formatters;
using StaticVoid.OrmPerformance.Runner.Config;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Messaging.Messages;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Runner.Wiring;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Runner
{
    public class ConfigurationTabViewModel : 
        PropertyChangedBase, 
        ITab
    {
        public String TabTitle { get { return "Configuration"; } }

        private readonly IPersistedRunnerConfig _config;
        private readonly ScenarioRunner _runner;
        private readonly IEnumerable<IResultFormatter<ScenarioInRunResult>> _formatters;
        private readonly ISendMessages _sender;
        private readonly ISelectableScenarios _scenarios;
        private readonly IEnumerable<SelectableConfiguration> _selectableConfigurations;
        private readonly ISelectableFormatters _selectableFormatters;

        public ConfigurationTabViewModel(
            IPersistedRunnerConfig config, 
            ScenarioRunner runner, 
            IEnumerable<IResultFormatter<ScenarioInRunResult>> formatters,
            IEventAggregator eventAggregator,
            ISendMessages sender,
            ISelectableScenarios scenarios,
            ISelectableConfigurations configurations,
            ISelectableFormatters compiledFormatters)
        {
            _config = config;
            _runner = runner;
            _formatters = formatters;
            _sender = sender;
            _scenarios = scenarios;
            _selectableFormatters = compiledFormatters;
            _selectableConfigurations = configurations.SelectableConfigurations;
            foreach (var c in _selectableConfigurations)
            {
                c.IsSelected = !_config.IgnoredConfigurations.Contains(c.Name);
            }
            foreach (var f in _selectableFormatters.SelectableFormatters)
            {
                f.IsSelected = !_config.IgnoredFormatters.Contains(f.Formatter.Name);
            } 
            foreach (var s in _scenarios.SelectableScenarios)
            {
                s.IsSelected = !_config.IgnoredScenarios.Contains(s.Scenario.Name);
            }
            NumberOfIterations = config.NumberOfRuns;
            eventAggregator.Subscribe(this);
        }

        public int NumberOfIterations
        {
            get { return _config.NumberOfRuns; }
            set
            {
                _config.NumberOfRuns = value;
                NotifyOfPropertyChange(() => NumberOfIterations);
            }
        }

        public int MaximumNumberOfItems
        {
            get { return _config.MaximumSampleSize; }
            set
            {
                _config.MaximumSampleSize = value;
                NotifyOfPropertyChange(() => MaximumNumberOfItems);
            }
        }

        public string ConnectionString
        {
            get { return _config.ConnectionString; }
            set
            {
                _config.ConnectionString = value;
                NotifyOfPropertyChange(() => ConnectionString);
            }
        }

        public IEnumerable<SelectableScenario> Scenarios
        {
            get { return _scenarios.SelectableScenarios; }
        }

        public IEnumerable<SelectableConfiguration> SelectableConfigurations
        {
            get { return _selectableConfigurations; }
        }

        public IEnumerable<SelectableFormatter> Formatters  
        {
            get { return _selectableFormatters.SelectableFormatters; }
        }

        public void Save()
        {
            _config.IgnoredConfigurations = _selectableConfigurations.Where(c => !c.IsSelected).Select(c => c.Name).ToList();
            _config.IgnoredFormatters = _selectableFormatters.SelectableFormatters.Where(c => !c.IsSelected).Select(c => c.Formatter.Name).ToList();
            _config.IgnoredScenarios = _scenarios.SelectableScenarios.Where(c => !c.IsSelected).Select(c => c.Scenario.Name).ToList();
            _config.Save();
        }
    }


}
