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
using System.Text;

namespace StaticVoid.OrmPerformance.Runner
{
    public class RunningOverviewTabViewModel : 
        PropertyChangedBase, 
        ITab,
        IHandle<IterationChanged>,
        IHandle<ConfigurationChanged>,
        IHandle<SampleSizeChanged>,
        IHandle<ScenarioChanged>,
        IHandle<ValidationResult>,
        IHandle<TimeResult>,
        IHandle<MemoryResult>,
        IHandle<TestStarted>
    {

        public String TabTitle { get { return "Overview"; } }

        private readonly IRunnerConfig _config;
        private readonly ScenarioRunner _runner;
        private readonly IEnumerable<IResultFormatter<ScenarioInRunResult>> _formatters;
        private readonly ISendMessages _sender;

        public RunningOverviewTabViewModel(
            IRunnerConfig config, 
            ScenarioRunner runner, 
            IEnumerable<IResultFormatter<ScenarioInRunResult>> formatters,
            IEventAggregator eventAggregator,
            ISendMessages sender)
        {
            _config = config;
            _runner = runner;
            _formatters = formatters;
            _sender = sender;
            NumberOfIterations = config.NumberOfRuns;
            eventAggregator.Subscribe(this);
        }

        private int _numberOfIterations = 3;
        public int NumberOfIterations
        {
            get { return _numberOfIterations; }
            set
            {
                _numberOfIterations = value;
                NotifyOfPropertyChange(() => NumberOfIterations);
            }
        }

        private string _currentScenario;
        public string CurrentScenario
        {
            get
            {
                return _currentScenario;
            }
            set
            {
                _currentScenario = value;
                NotifyOfPropertyChange(() => CurrentScenario);
            }
        }

        private string _currentTechnology;
        public string CurrentTechnology
        {
            get
            {
                return _currentTechnology;
            }
            set
            {
                _currentTechnology = value;
                NotifyOfPropertyChange(() => CurrentTechnology);
            }
        }

        private string _currentConfig;
        public string CurrentConfig
        {
            get
            {
                return _currentConfig;
            }
            set
            {
                _currentConfig = value;
                NotifyOfPropertyChange(() => CurrentConfig);
            }
        }

        private int _currentSampleSize;
        public int CurrentSampleSize
        {
            get
            {
                return _currentSampleSize;
            }
            set
            {
                _currentSampleSize = value;
                NotifyOfPropertyChange(() => CurrentSampleSize);
            }
        }

        private int _currentIteration = 0;
        public int CurrentIteration
        {
            get
            {
                return _currentIteration;
            }
            set
            {
                _currentIteration = value;
                NotifyOfPropertyChange(() => CurrentIteration);
                NotifyOfPropertyChange(() => Iteration);
            }
        }

        public string Iteration
        {
            get { return String.Format("{0} / {1}", CurrentIteration,NumberOfIterations); }
        }

        private StringBuilder _output = new StringBuilder();
        public string Output
        {
            get { return _output.ToString(); }
        }

        private void AppendLineToOutput(string line)
        {
            _output.Append(Environment.NewLine);
            _output.Append(line);
            NotifyOfPropertyChange(() => Output);
        }

        private void AppendToLastOutputLine(string line)
        {
            _output.Append(", ");
            _output.Append(line);
            NotifyOfPropertyChange(() => Output);
        }

        public void Handle(IterationChanged message)
        {
            CurrentIteration++;
            AppendLineToOutput(message.Message);
        }

        public void Handle(ConfigurationChanged message)
        {
            CurrentTechnology = message.Technology;
            CurrentConfig = message.Name;
            AppendLineToOutput(message.Message);
        }

        public void Handle(SampleSizeChanged message)
        {
            CurrentSampleSize = message.SampleSize;
            AppendLineToOutput(message.Message);
        }

        public void Handle(ScenarioChanged message)
        {
            CurrentScenario = message.Scenario;
            AppendLineToOutput(message.Message);
        }

        public void Handle(MemoryResult message)
        {
            AppendToLastOutputLine(message.ConsumedMemory/1024 + "KB");
        }

        public void Handle(TimeResult message)
        {
            AppendToLastOutputLine(message.ElapsedMilliseconds + "ms");
        }

        public void Handle(ValidationResult message)
        {
            AppendToLastOutputLine(message.Status);
        }

        public void Handle(TestStarted message)
        {
            CurrentIteration = 0;
            _output.Clear() ;
            NumberOfIterations = _config.NumberOfRuns;
        }
    }
}
