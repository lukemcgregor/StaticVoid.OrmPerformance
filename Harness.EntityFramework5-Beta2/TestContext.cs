using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework5_Beta2
{
    public class TestContext: System.Data.Entity.DbContext
    {
        public TestContext(IConnectionString connectionString)
            : base(connectionString.FormattedConnectionString + "MultipleActiveResultSets=true;")
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
