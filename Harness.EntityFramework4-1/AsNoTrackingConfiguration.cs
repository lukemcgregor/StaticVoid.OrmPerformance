using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1
{
    public class AsNoTrackingConfiguration: IRunnableBulkSelectConfiguration
    {
        public string Name { get { return "AsNoTracking Configuration"; } }

        public string Technology{ get { return "Entity Framework 4.1.10331.0"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public AsNoTrackingConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestContext(_connectionString);
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _context.TestEntities.AsNoTracking().Where(t => t.TestInt == testInt).ToArray();
        }

        public Models.TestEntity Find(int id)
        {
            return _context.TestEntities.AsNoTracking().SingleOrDefault(t => t.Id == id);
        }
        
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
