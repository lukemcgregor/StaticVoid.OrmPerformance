using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using StaticVoid.OrmPerformance.Formatters;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Messaging.Messages;
using StaticVoid.OrmPerformance.Runner.Config;
using StaticVoid.OrmPerformance.Runner.Wiring;

namespace StaticVoid.OrmPerformance.Runner
{
    public class OrmPerformanceWindowViewModel : 
        Conductor<ITab>.Collection.OneActive,
        IRunOrmTests,
        IHandle<TestStarted>,
        IHandle<TestStopped>
    {
        private readonly IRunnerConfig _config;
        private readonly ScenarioRunner _runner;
        private readonly IEnumerable<IResultFormatter<ScenarioInRunResult>> _formatters;
        private readonly ISendMessages _sender;
        private CancellationTokenSource _cancelationToken;
        private ISelectableFormatters _selectableFormatters;

        private List<ScenarioInRunResult> _allRunResults;

        public OrmPerformanceWindowViewModel(
            IEnumerable<ITab> tabs, 
            IEventAggregator eventAggregator,
            IEnumerable<IResultFormatter<ScenarioInRunResult>> formatters,
            IRunnerConfig config, 
            ScenarioRunner runner, 
            ISendMessages sender,
            ISelectableFormatters selectableFormatters)
        {
            Items.AddRange(tabs);
            if (tabs.Any())
            {
                ActivateItem(Items.First(t => t.TabTitle == "Configuration"));
            }

            _config = config;
            _runner = runner;
            _formatters = formatters;
            _sender = sender;
            _selectableFormatters = selectableFormatters;

            eventAggregator.Subscribe(this);
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
                NotifyOfPropertyChange(() => IsNotRunning);
            }
        }

        public bool IsNotRunning
        {
            get
            {
                return !_isRunning;
            }
        }

        public void Handle(TestStarted message)
        {
            IsRunning = true;
            ActivateItem(Items.First(t => t.TabTitle != "Configuration"));
        }

        public void Handle(TestStopped message)
        {
            IsRunning = false;
        }

        public void RunTests()
        {
            _cancelationToken = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                _allRunResults = new List<ScenarioInRunResult>();
                _sender.Send(new TestStarted());

                for (int i = 0; i < _config.NumberOfRuns; i++)
                {
                    _sender.Send(new IterationChanged());
                    _allRunResults.AddRange(_runner.Run(_config.MaximumSampleSize, _cancelationToken.Token)
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
                    formatter.FormatResults(_allRunResults);
                }
                _sender.Send(new TestStopped());
            }, _cancelationToken.Token);
        }

        public void StopTests()
        {
            _cancelationToken.Cancel();
            try
            {
                foreach (var formatter in _formatters)
                {
                    formatter.FormatResults(_allRunResults);
                }
            }
            catch { }
            _sender.Send(new TestStopped());
        }
    }
}
