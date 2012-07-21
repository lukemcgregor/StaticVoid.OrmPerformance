using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;
using StaticVoid.OrmPerformance.Harness.Util;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableInsertScenario : IRunnableScenario
    {
        public string Name { get { return "Insert"; } }

        private IEnumerable<IRunnableInsertConfiguration> _configurations;
        private IPerformanceScenarioBuilder<InsertContext> _builder;

        public RunnableInsertScenario(IPerformanceScenarioBuilder<InsertContext> builder, RunnableConfigurationCollection<IRunnableInsertConfiguration> configurationsToRun)
        {
            _configurations = configurationsToRun;
            _builder = builder;
        }

        public List<ScenarioResult> Run(int sampleSize)
        {
            Console.WriteLine("Generating Samples");
            List<TestEntity> testEntities = TestEntityHelpers.GenerateRandomTestEntities(sampleSize);
            List<ScenarioResult> runs = new List<ScenarioResult>();
            Stopwatch timer = new Stopwatch();
            foreach (var config in _configurations)
            {
                Console.WriteLine(String.Format("Starting configuration {0} - {1} at {2}",config.Technology, config.Name, DateTime.Now.ToShortTimeString()));
                _builder.SetUp((t) => { return true; });// no seed
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
                foreach (var e in testEntities)
                {
                    config.Add(e);
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

				run.Status = _builder.Context.AssertDatabaseState(testEntities);

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }
    }
}
