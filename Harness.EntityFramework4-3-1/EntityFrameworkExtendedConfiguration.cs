using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using System.Data.Entity;
using StaticVoid.OrmPerformance.Harness.Models;
using EntityFramework.Extensions;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_3_1
{
    public class EntityFrameworkExtendedConfiguration : IRunnableDeleteConfiguration
    {
        public string Name { get { return "EntityFramework.Extended Batching"; } }

        public string Technology { get { return "Entity Framework 4.3.1"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public EntityFrameworkExtendedConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            Database.SetInitializer<TestContext>(null);// so it doesnt think db is different and try and recreate/migrate it
            _context = new TestContext(_connectionString);
            _toDelete = new List<int>();
        }
        List<int> _toDelete;
        public void Commit()
        {
            var e = _context.TestEntities.Delete(t => _toDelete.Contains(t.Id));
            _context.SaveChanges();        
        }

        public void TearDown()
        {
            _context.Dispose();
        }

        public void Delete(int id)
        {
            _toDelete.Add(id);
        }
    }
}
