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

namespace StaticVoid.OrmPerformance.Runner
{
    public class OrmTestRunnerViewModel : 
        PropertyChangedBase, 
        IRunOrmTests,
        IHandle<IterationChanged>,
        IHandle<ConfigurationChanged>,
        IHandle<SampleSizeChanged>,
        IHandle<ScenarioChanged>,
        IHandle<ValidationResult>,
        IHandle<TimeResult>,
        IHandle<MemoryResult>
    {
        private readonly IRunnerConfig _config;
        private readonly ScenarioRunner _runner;
        private readonly IEnumerable<IResultFormatter<ScenarioInRunResult>> _formatters;
        private readonly ISendMessages _sender;

        public OrmTestRunnerViewModel(
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

        private string _databaseServer="chagall";
        public string DatabaseServer
        {
            get { return _databaseServer; }
            set
            {
                _databaseServer = value;
                NotifyOfPropertyChange(() => DatabaseServer);
                NotifyOfPropertyChange(() => ConnectionString);
            }
        }

        private string _manualConnectionString = "Data Source=MyServer;Initial Catalog=StaticVoid.OrmPerformance.TestDb;Integrated Security=true;";
        public string ConnectionString
        {
            get 
            { 
                return ManuallySetConnectionString ? 
                    _manualConnectionString : 
                    String.Format("Data Source={0};Initial Catalog=StaticVoid.OrmPerformance.TestDb;Integrated Security=true;", _databaseServer); 
            }
            set
            {
                _manualConnectionString = value;
                NotifyOfPropertyChange(() => ConnectionString);
            }
        }

        public bool CanConnectionString
        {
            get { return ManuallySetConnectionString; }
        }

        public bool IsDatabaseServerVisible
        {
            get { return !ManuallySetConnectionString; }
        }

        private bool _manuallySetConnectionString = false;
        public bool ManuallySetConnectionString
        {
            get
            {
                return _manuallySetConnectionString;
            }
            set
            {
                _manuallySetConnectionString = value;
                NotifyOfPropertyChange(() => ManuallySetConnectionString);
                NotifyOfPropertyChange(() => IsDatabaseServerVisible);
                NotifyOfPropertyChange(() => ManuallySetConnectionString);
                NotifyOfPropertyChange(() => ConnectionString);
            }
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

        private int _maximumNumberOfItems = 10000;
        public int MaximumNumberOfItems
        {
            get { return _maximumNumberOfItems; }
            set
            {
                _maximumNumberOfItems = value;
                NotifyOfPropertyChange(() => MaximumNumberOfItems);
            }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                NotifyOfPropertyChange(() => IsRunning);
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

        private string _output;
        public string Output
        {
            get { return _output; }
        }

        public void RunTests()
        {
            Task.Factory.StartNew(() =>
            {
                List<ScenarioInRunResult> allRunResults = new List<ScenarioInRunResult>();

                IsRunning = true;
                for (int i = 0; i < _config.NumberOfRuns; i++)
                {
                    _sender.Send(new IterationChanged());
                    Console.WriteLine(String.Format("Starting run number {0} at {1}", i, DateTime.Now.ToShortTimeString()));
                    allRunResults.AddRange(_runner.Run(_config.MaximumSampleSize)
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

                foreach (var formatter in _formatters)
                {
                    formatter.FormatResults(allRunResults);
                }
                IsRunning = false;
            });
        }

        private void AppendLineToOutput(string line)
        {
            _output = String.Format("{0}{1}{2}", _output, Environment.NewLine, line);
            NotifyOfPropertyChange(() => Output);
        }

        private void AppendToLastOutputLine(string line)
        {
            _output = String.Format("{0}, {1}", _output, line);
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
    }
}
