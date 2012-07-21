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
    public class RunnableBulkSelectScenario : IRunnableScenario
    {
        public string Name { get { return "Bulk Select"; } }

        private IEnumerable<IRunnableBulkSelectConfiguration> _configurations;
        private IPerformanceScenarioBuilder<SelectContext> _builder;
        private InsertContext _textContext;

        public RunnableBulkSelectScenario(
            IPerformanceScenarioBuilder<SelectContext> builder,
            RunnableConfigurationCollection<IRunnableBulkSelectConfiguration> configurationsToRun,
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
                    Status = new AssertionPass(),
                    CommitTime = 0
                };
                long startMem = System.GC.GetTotalMemory(true);
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

                run.MemoryUsage = (System.GC.GetTotalMemory(true) - startMem);
                runs.Add(run);
                config.TearDown();

                Console.WriteLine("Asserting Database State");

                //no changes too slow for large numbers
                //if (!_textContext.AssertDatabaseState(testEntities))
                //{
                //    run.Status = "Failed";
                //}
                if (testEntities.Count() != foundEntities.Count())
                {
					run.Status = new AssertionFailForRecordCount() { ActualCount = testEntities.Count, ExpectedCount = foundEntities.Count() };
                }

                var fe = foundEntities.ToArray();
                for (int i = 0; i < testEntities.Count(); i++)
                {
                    if (fe[i].TestDate != testEntities[i].TestDate ||
                        fe[i].TestInt != testEntities[i].TestInt ||
                        fe[i].TestString != testEntities[i].TestString)
                    {
						run.Status = new AssertionFailForMismatch() { Actual = fe[i], Expected = testEntities[i] };
                    }
                }
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
