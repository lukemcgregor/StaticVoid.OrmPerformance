using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1
{
    public class NoAsNoTrackingConfiguration: IRunnableBulkSelectConfiguration
    {
        public string Name { get { return "No AsNoTracking Configuration"; } }

        public string Technology{ get { return "Entity Framework 4.1.10331.0"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public NoAsNoTrackingConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestContext(_connectionString);
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _context.TestEntities.Where(t => t.TestInt == testInt).ToArray();
        }

        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
