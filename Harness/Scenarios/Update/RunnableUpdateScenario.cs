using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;
using System.Data.Entity;
using StaticVoid.OrmPerformance.Harness.Util;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;
using StaticVoid.OrmPerformance.Messaging.Messages;
using System.Threading;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableUpdateScenario : IRunnableScenario
    {
        public string Name { get { return "Update"; } }

        private IProvideRunnableConfigurations _configurationProvider;
        private IPerformanceScenarioBuilder<UpdateContext> _builder;
        private InsertContext _textContext;
        private readonly ISendMessages _sender;

        public RunnableUpdateScenario(
            IPerformanceScenarioBuilder<UpdateContext> builder,
            IProvideRunnableConfigurations configurationProvider,
            InsertContext insertTestContext,
            ISendMessages sender)
        {
            _configurationProvider = configurationProvider;
            _builder = builder;
            _textContext = insertTestContext;
            _sender = sender;
        }

        public List<ScenarioResult> Run(int sampleSize, CancellationToken cancellationToken)
        {
            Console.WriteLine("Generating Samples");
            List<TestEntity> testEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);
            List<TestEntity> updatedEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);
            List<ScenarioResult> runs = new List<ScenarioResult>();
            Stopwatch timer = new Stopwatch();
            foreach (var config in _configurationProvider.GetRandomisedRunnableConfigurations<IRunnableUpdateConfiguration>())
            {
                cancellationToken.ThrowIfCancellationRequested();
                _sender.Send(new ConfigurationChanged { Technology = config.Technology, Name = config.Name });
                Console.WriteLine(String.Format("Starting configuration {0} - {1} at {2}",config.Technology, config.Name, DateTime.Now.ToShortTimeString()));
                _builder.SetUp((t) => 
                {
                    foreach(var e in testEntities)
                    {
                        t.TestEntities.Add(e);
                    }
                    t.SaveChanges();
                    return true; 
                });// no seed
                ScenarioResult run = new ScenarioResult {
                    SampleSize = testEntities.Count(),
                    ConfigurationName=config.Name,
                    Technology = config.Technology,
                    ScenarioName = Name,
                    Status = new AssertionPass()
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
                for(int i = 0; i < sampleSize; i++)
                {
                    config.Update(i+1, updatedEntities[i].TestString, updatedEntities[i].TestInt, updatedEntities[i].TestDate);
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
                _sender.Send(new TimeResult { ElapsedMilliseconds = run.ApplicationTime + run.CommitTime + run.SetupTime });
                _sender.Send(new MemoryResult { ConsumedMemory = run.MemoryUsage });
                
                runs.Add(run);

                config.TearDown();

                Console.WriteLine("Asserting Database State");

				run.Status = _builder.Context.AssertDatabaseState(testEntities);

                _sender.Send(new ValidationResult { Status = run.Status.ToShortString() });

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }
    }
}
