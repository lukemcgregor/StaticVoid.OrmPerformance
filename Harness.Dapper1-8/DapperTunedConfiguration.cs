using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using Dapper;


namespace StaticVoid.OrmPerformance.Harness.Dapper1_8
{
    public class DapperTunedConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration, IRunnableBulkSelectConfiguration, IRunnableDiscreteSelectConfiguration, IRunnableDeleteConfiguration
    {
        public string Name { get { return "Tuned"; } }

        public string Technology { get { return "Dapper 1.8"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        public DapperTunedConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
        }

        private List<dynamic> _entitiesToInsert = new List<dynamic>();
        public void Add(Models.TestEntity entity)
        {
            _entitiesToInsert.Add(new { TestDate = entity.TestDate, TestInt = entity.TestInt, TestString = entity.TestString });
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            //TODO can you batch this?
            _connection.Execute("UPDATE TestEntities SET TestDate=@TestDate, TestInt=@TestInt, TestString=@TestString WHERE Id=@Id", new { Id = id, TestDate = testDateTime, TestInt = testInt, TestString = testString });
        }

        public void Commit()
        {
            if (_entitiesToInsert.Any())
            {
                _connection.Execute("INSERT INTO TestEntities (TestDate, TestInt, TestString) VALUES (@TestDate, @TestInt, @TestString)",
                    _entitiesToInsert.ToArray());
            }
        }
        public void TearDown()
        {
            _connection.Close();
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _connection.Query("SELECT * FROM TestEntities WHERE TestInt=@TestInt", new { TestInt = testInt })
                .Select(e => new Models.TestEntity { Id = e.Id, TestDate = e.TestDate, TestInt = e.TestInt, TestString = e.TestString });
        }


        public Models.TestEntity Find(int id)
        {
            var a = _connection.Query("SELECT TOP 1 * FROM TestEntities WHERE Id=@Id", new { Id = id });
            return a     .Select(e => new Models.TestEntity { Id = e.Id, TestDate = e.TestDate, TestInt = e.TestInt, TestString = e.TestString })
                 .SingleOrDefault();
        }

        public void Delete(int id)
        {
            _connection.Query<int>("DELETE FROM TestEntities WHERE Id=@Id", new { Id = id });
        }
    }
}
