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

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableBulkSelectScenario : IRunnableScenario
    {
        public string Name { get { return "Bulk Select"; } }

        private IEnumerable<IRunnableSelectConfiguration> _configurations;
        private IPerformanceScenarioBuilder<SelectContext> _builder;
        private InsertContext _textContext;

        public RunnableBulkSelectScenario(
            IPerformanceScenarioBuilder<SelectContext> builder,
            RunnableConfigurationCollection<IRunnableSelectConfiguration> configurationsToRun,
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
            testEntities = testEntities.Select(t => new TestEntity { TestDate = t.TestDate, TestString = t.TestString, TestInt = 1337 }).ToList() ;

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
                    Status = "Passed",
                    CommitTime = 0
                };

                //set up
                timer.Restart();
                config.Setup();
                timer.Stop();
                run.SetupTime = timer.ElapsedMilliseconds;

                //execute
                timer.Restart();
                var foundEntities = config.FindWhereTestIntIs(1337);
                timer.Stop();

                run.ApplicationTime = timer.ElapsedMilliseconds;
                
                runs.Add(run);
                config.TearDown();

                Console.WriteLine("Asserting Database State");

                //no changes
                if (!_textContext.AssertDatabaseState(testEntities))
                {
                    run.Status = "Failed";
                }

                foreach (var entity in testEntities)
                {
                    if (!foundEntities.Where(t => t.TestDate == entity.TestDate && t.TestInt == entity.TestInt && t.TestString == entity.TestString).Any())
                    {
                        run.Status = "Failed";
                    }
                }

                Console.WriteLine("Tearing down");
                _builder.TearDown();
            }

            return runs;
        }


    }
}
