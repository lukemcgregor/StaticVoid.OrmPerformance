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
    public class RunnableUpdateScenario : IRunnableScenario
    {
        public string Name { get { return "Update"; } }

        private IEnumerable<IRunnableUpdateConfiguration> _configurations;
        private IPerformanceScenarioBuilder<UpdateContext> _builder;
        private InsertContext _textContext;
        private readonly ISendMessages _sender;

        public RunnableUpdateScenario(
            IPerformanceScenarioBuilder<UpdateContext> builder,
            RunnableConfigurationCollection<IRunnableUpdateConfiguration> configurationsToRun,
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
            List<TestEntity> updatedEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);
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
                    Status = "Passed"
                };


                long startMem = System.GC.GetTotalMemory(true);
                //set up
                timer.Restart();
                config.Setup();
                timer.Stop();
                run.SetupTime = timer.ElapsedMilliseconds;

                //execute
                timer.Restart();
                for(int i = 0; i < sampleSize; i++)
                {
                    config.Update(i+1, updatedEntities[i].TestString, updatedEntities[i].TestInt, updatedEntities[i].TestDate);
                }
                timer.Stop();
                run.ApplicationTime = timer.ElapsedMilliseconds;

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

                if (!_textContext.AssertDatabaseState(updatedEntities))
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
