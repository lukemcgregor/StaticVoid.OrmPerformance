using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.Linq2Sql
{
    public class NoObjectTrackingConfiguration : 
        IRunnableDiscreteSelectConfiguration, IRunnableBulkSelectConfiguration
    {
        public string Name { get { return "No Object Tracking Configuration"; } }
        public string Technology { get { return "Linq2SQL"; } }

        private TestDataContext _context = null;

        private IConnectionString _connectionString;
        public NoObjectTrackingConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestDataContext(_connectionString.FormattedConnectionString);
            _context.DeferredLoadingEnabled = false;
            _context.ObjectTrackingEnabled = false;
        }

        public void TearDown()
        {
            _context.Dispose();
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            var res = _context.TestEntities.Where(t => t.TestInt == testInt).Select(t=>
                new Models.TestEntity 
                { 
                    Id = t.Id,
                    TestDate = t.TestDate, 
                    TestInt = t.TestInt, 
                    TestString = t.TestString 
                }).ToArray();
            return res;
        }

        public Models.TestEntity Find(int id)
        {
            var t = _context.TestEntities.SingleOrDefault(te => te.Id == id);
            if (t != null)
            {
                return new Models.TestEntity
                {
                    Id = t.Id,
                    TestDate = t.TestDate,
                    TestInt = t.TestInt,
                    TestString = t.TestString
                };
            }
            return null;
        }
    }
}
