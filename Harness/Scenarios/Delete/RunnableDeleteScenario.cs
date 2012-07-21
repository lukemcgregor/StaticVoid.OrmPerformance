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
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableDeleteScenario : IRunnableScenario
    {
        public string Name { get { return "Delete"; } }

        private IEnumerable<IRunnableDeleteConfiguration> _configurations;
        private IPerformanceScenarioBuilder<DeleteContext> _builder;
        private InsertContext _textContext;

        public RunnableDeleteScenario(
            IPerformanceScenarioBuilder<DeleteContext> builder,
            RunnableConfigurationCollection<IRunnableDeleteConfiguration> configurationsToRun,
            InsertContext insertTestContext)
        {
            _configurations = configurationsToRun;
            _builder = builder;
            _textContext = insertTestContext;
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
                    config.Delete(i+1);
                }
                timer.Stop();
                run.ApplicationTime = timer.ElapsedMilliseconds;

                //commit
                timer.Restart();
                config.Commit();
                timer.Stop();
                run.CommitTime = timer.ElapsedMilliseconds;


                run.MemoryUsage = (System.GC.GetTotalMemory(true) - startMem);
                runs.Add(run);

                config.TearDown();

                Console.WriteLine("Asserting Database State");

                run.Status = _textContext.AssertDatabaseState(new List<TestEntity>());

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }
    }
}
