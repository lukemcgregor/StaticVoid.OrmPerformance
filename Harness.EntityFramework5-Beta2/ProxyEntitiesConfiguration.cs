using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using System.Data.Entity;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework5_Beta2
{
    public class ProxyEntitiesConfiguration : 
        IRunnableInsertConfiguration, 
        IRunnableUpdateConfiguration, 
        IRunnableDeleteConfiguration
    {
        public string Name { get { return "Proxy Entities"; } }

        public string Technology { get { return "Entity Framework 5.0.0.0-Beta2"; } }

        private Proxy.TestContext _context = null;

        private IConnectionString _connectionString;
        public ProxyEntitiesConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new Proxy.TestContext(_connectionString);
            _context.Configuration.LazyLoadingEnabled = false;
        }

        public void Add(Models.TestEntity entity)
        {
            var e = _context.TestEntities.Create();
            e.TestDate = entity.TestDate;
            e.TestInt = entity.TestInt;
            e.TestString = entity.TestString;
            _context.TestEntities.Add(e);
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            var entity = _context.TestEntities.Create();
            entity.Id = id;
            _context.TestEntities.Attach(entity);
            entity.TestDate = testDateTime;
            entity.TestInt = testInt;
            entity.TestString = testString;
        }

        public void Commit()
        {
            _context.SaveChanges();        
        }

        public void TearDown()
        {
            _context.Dispose();
        }

        public void Delete(int id)
        {
            var entity = _context.TestEntities.Create();
            entity.Id = id;
            _context.TestEntities.Attach(entity);
            _context.TestEntities.Remove(entity);
        }
    }
}
