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

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableDiscreetSelectScenario : IRunnableScenario
    {
        public string Name { get { return "Discrete Select"; } }

        private IEnumerable<IRunnableDiscreteSelectConfiguration> _configurations;
        private IPerformanceScenarioBuilder<SelectContext> _builder;
        private InsertContext _textContext;
        private readonly ISendMessages _sender;

        public RunnableDiscreetSelectScenario(
            IPerformanceScenarioBuilder<SelectContext> builder,
            RunnableConfigurationCollection<IRunnableDiscreteSelectConfiguration> configurationsToRun,
            InsertContext insertTestContext,
            ISendMessages sender)
        {
            _configurations = configurationsToRun;
            _builder = builder;
            _textContext = insertTestContext;
            _sender = sender;
        }

        public List<ScenarioResult> Run(int sampleSize)
        {
            Console.WriteLine("Generating Samples");
            List<TestEntity> testEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);

            List<ScenarioResult> runs = new List<ScenarioResult>();
            Stopwatch timer = new Stopwatch();
            foreach (var config in _configurations)
            {
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

                long startMem = System.GC.GetTotalMemory(true);
                //set up
                timer.Restart();
                config.Setup();
                timer.Stop();
                run.SetupTime = timer.ElapsedMilliseconds;

                ////Warmup
                //var w = config.Find(1);

                //execute
                timer.Restart();
                for (int i = 0; i < sampleSize; i++)
                {
                    timer.Start();
                    var entity = config.Find(i+1);
                    timer.Stop();

                    if (entity == null || entity.TestInt != testEntities[i].TestInt || entity.TestDate != testEntities[i].TestDate || entity.TestString != testEntities[i].TestString)
                    {
                        run.Status = "Failed";
                    }
                }
                timer.Stop();

                run.ApplicationTime = timer.ElapsedMilliseconds;

                run.MemoryUsage = (System.GC.GetTotalMemory(true) - startMem);

                _sender.Send(new TimeResult { ElapsedMilliseconds = run.ApplicationTime + run.CommitTime + run.SetupTime });
                _sender.Send(new MemoryResult { ConsumedMemory = run.MemoryUsage });
                runs.Add(run);
                config.TearDown();

                Console.WriteLine("Asserting Database State");

                //no changes
                if (!_textContext.AssertDatabaseState(testEntities))
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
