using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;

namespace StaticVoid.OrmPerformance.Harness
{
    public class UpdateContext : DbContext
    {
        public UpdateContext(IConnectionString connectionString) : base(connectionString.FormattedConnectionString + "MultipleActiveResultSets=true;")
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<TestEntity> TestEntities { get; set; }

        public AssertionStatus AssertDatabaseState(List<TestEntity> expectedState)
        {
            var dbEntities = this.TestEntities.ToArray();

            foreach (var entity in expectedState)
            {
                if (!dbEntities.Where(t => t.TestDate == entity.TestDate && t.TestInt == entity.TestInt && t.TestString == entity.TestString).Any())
                {
					return new AssertionFailForMismatch() {
						Expected = entity,
						Actual = dbEntities.Where(t => t.Id == entity.Id).FirstOrDefault()
					};
                }
            }
			
			if (dbEntities.Count() != expectedState.Count)
			{
				return new AssertionFailForRecordCount() { ActualCount = expectedState.Count, ExpectedCount = dbEntities.Count() };
			}

			return new AssertionPass();
        }
    }
}
