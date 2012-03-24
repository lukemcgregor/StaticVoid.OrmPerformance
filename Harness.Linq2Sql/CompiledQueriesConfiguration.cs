using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.Linq2Sql
{
    public class CompiledQueriesConfiguration : 
        IRunnableSelectConfiguration
    {
        public string Name { get { return "Compiled Queries Configuration"; } }
        public string Technology { get { return "Linq2SQL"; } }

        private TestDataContext _context = null;
        private Func<TestDataContext, int, IEnumerable<TestEntity>> _findWhereTestIntIs;
        private Func<TestDataContext, int, TestEntity> _find;

        private IConnectionString _connectionString;
        public CompiledQueriesConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestDataContext(_connectionString.FormattedConnectionString);
            _context.DeferredLoadingEnabled = false;

            _findWhereTestIntIs = CompiledQuery.Compile<TestDataContext, int, IEnumerable<TestEntity>>
            (
                (context, testInt) => context.TestEntities
                .Where<TestEntity>(t => t.TestInt == testInt)
            );

            _find = CompiledQuery.Compile<TestDataContext, int, TestEntity>
            (
                (context, id) => context.TestEntities
                .SingleOrDefault(t => t.Id == id)
            );
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _findWhereTestIntIs.Invoke(_context, testInt).Select(t =>
                new Models.TestEntity
                {
                    Id = t.Id,
                    TestDate = t.TestDate,
                    TestInt = t.TestInt,
                    TestString = t.TestString
                }).ToArray();
        }

        public Models.TestEntity Find(int id)
        {
            var t = _find.Invoke(_context, id);
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
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
