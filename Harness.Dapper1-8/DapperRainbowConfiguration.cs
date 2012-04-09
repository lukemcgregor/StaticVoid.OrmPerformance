using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.Dapper1_8
{
    public class DapperRainbowConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration
    {
        public string Name { get { return "Rainbow ORM"; } }

        public string Technology { get { return "Dapper 1.8"; } }

        private TestDatabase _database;
        private IConnectionString _connectionString;
        private SqlConnection _connection;
        public DapperRainbowConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
            _database = TestDatabase.Init(_connection,30);
        }

        public void Add(Models.TestEntity entity)
        {
            _database.TestEntities.Insert(new { TestDate = entity.TestDate, TestInt = entity.TestInt, TestString = entity.TestString });
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        { 
            _database.TestEntities.Update(id, new { TestDate = testDateTime, TestInt = testInt, TestString = testString });
        }

        public void Commit()
        {
        }
        public void TearDown()
        {
            _database.Dispose();
            SqlConnection.ClearPool(_connection);
        }


    }
}
