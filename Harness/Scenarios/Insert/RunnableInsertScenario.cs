using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;
using StaticVoid.OrmPerformance.Harness.Util;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Messaging.Messages;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableInsertScenario : IRunnableScenario
    {
        public string Name { get { return "Insert"; } }

        private IProvideRunnableConfigurations _configurationProvider;
        private IPerformanceScenarioBuilder<InsertContext> _builder;
        private readonly ISendMessages _sender;

        public RunnableInsertScenario(
            IPerformanceScenarioBuilder<InsertContext> builder,
            IProvideRunnableConfigurations configurationProvider,
            ISendMessages sender)
        {
            _configurationProvider = configurationProvider;
            _builder = builder;
            _sender = sender;
        }

        public List<ScenarioResult> Run(int sampleSize, CancellationToken cancellationToken)
        {
            Console.WriteLine("Generating Samples");
            List<TestEntity> testEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);
            List<ScenarioResult> runs = new List<ScenarioResult>();
            Stopwatch timer = new Stopwatch();
            foreach (var config in _configurationProvider.GetRandomisedRunnableConfigurations<IRunnableInsertConfiguration>())
            {
                cancellationToken.ThrowIfCancellationRequested();

                _sender.Send(new ConfigurationChanged { Technology = config.Technology, Name = config.Name });
                Console.WriteLine(String.Format("Starting configuration {0} - {1} at {2}",config.Technology, config.Name, DateTime.Now.ToShortTimeString()));
                _builder.SetUp((t) => { return true; });// no seed
                ScenarioResult run = new ScenarioResult {
                    SampleSize = testEntities.Count(),
                    ConfigurationName=config.Name,
                    Technology = config.Technology,
                    ScenarioName = Name,
                    Status = "Passed"
                };

                cancellationToken.ThrowIfCancellationRequested();

                long startMem = System.GC.GetTotalMemory(true);
                //set up
                timer.Restart();
                config.Setup();
                timer.Stop();
                run.SetupTime = timer.ElapsedMilliseconds;

                cancellationToken.ThrowIfCancellationRequested();

                //execute
                timer.Restart();
                foreach (var e in testEntities)
                {
                    config.Add(e);
                }
                timer.Stop();
                run.ApplicationTime = timer.ElapsedMilliseconds;

                cancellationToken.ThrowIfCancellationRequested();

                //commit
                timer.Restart();
                config.Commit();
                timer.Stop();
                run.CommitTime = timer.ElapsedMilliseconds;

                run.MemoryUsage = (System.GC.GetTotalMemory(true) - startMem);
                runs.Add(run);
                _sender.Send(new TimeResult { ElapsedMilliseconds = run.ApplicationTime + run.CommitTime + run.SetupTime });
                _sender.Send(new MemoryResult { ConsumedMemory = run.MemoryUsage });

                config.TearDown();

                Console.WriteLine("Asserting Database State");

                if (!_builder.Context.AssertDatabaseState(testEntities))
                {
                    run.Status = "Failed";
                }

                _sender.Send(new ValidationResult { Status = run.Status });

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }
    }
}
