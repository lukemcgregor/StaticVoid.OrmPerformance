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
using StaticVoid.OrmPerformance.Messaging.Messages;
using System.Threading;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableBulkSelectScenario : IRunnableScenario
    {
        public string Name { get { return "Bulk Select"; } }

        private readonly IProvideRunnableConfigurations _configurationProvider;
        private readonly IPerformanceScenarioBuilder<SelectContext> _builder;
        private readonly InsertContext _textContext;
        private readonly ISendMessages _sender;

        public RunnableBulkSelectScenario(
            IPerformanceScenarioBuilder<SelectContext> builder,
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
            testEntities = testEntities.Select(t => new TestEntity { TestDate = t.TestDate, TestString = t.TestString, TestInt = 1337 }).ToList() ;

            List<ScenarioResult> runs = new List<ScenarioResult>();
            Stopwatch timer = new Stopwatch();
            foreach (var config in _configurationProvider.GetRandomisedRunnableConfigurations<IRunnableBulkSelectConfiguration>())
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
                    Status = "Passed",
                    CommitTime = 0
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
                var foundEntities = config.FindWhereTestIntIs(1337);
                timer.Stop();

                run.ApplicationTime = timer.ElapsedMilliseconds;

                run.MemoryUsage = (System.GC.GetTotalMemory(true) - startMem);
                runs.Add(run);
                _sender.Send(new TimeResult { ElapsedMilliseconds = run.ApplicationTime + run.CommitTime + run.SetupTime });
                _sender.Send(new MemoryResult { ConsumedMemory = run.MemoryUsage });

                config.TearDown();

                Console.WriteLine("Asserting Database State");

                //no changes too slow for large numbers
                //if (!_textContext.AssertDatabaseState(testEntities))
                //{
                //    run.Status = "Failed";
                //}

                cancellationToken.ThrowIfCancellationRequested();
                if (testEntities.Count() != foundEntities.Count())
                {
                    run.Status = "Failed";
                }

                var fe = foundEntities.ToArray();
                for (int i = 0; i < testEntities.Count(); i++)
                {
                    if (fe[i].TestDate != testEntities[i].TestDate ||
                        fe[i].TestInt != testEntities[i].TestInt ||
                        fe[i].TestString != testEntities[i].TestString)
                    {
                        run.Status = "Failed";
                    }
                }

                _sender.Send(new ValidationResult { Status = run.Status });

                //foreach (var entity in testEntities)
                //{
                //    if (!foundEntities.Where(t => t.TestDate == entity.TestDate && t.TestInt == entity.TestInt && t.TestString == entity.TestString).Any())
                //    {
                //        run.Status = "Failed";
                //    }
                //}

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }


    }
}
