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
    public class DapperTransactionsChaosConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration
    {
        public string Name { get { return "Transaction (Chaos)"; } }

        public string Technology { get { return "Dapper 1.8"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public DapperTransactionsChaosConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.Chaos);
        }

        private List<dynamic> _entitiesToInsert = new List<dynamic>();
        public void Add(Models.TestEntity entity)
        {
            _connection.Execute("INSERT TestEntities (TestDate, TestInt, TestString) VALUES (@TestDate, @TestInt, @TestString)",
                new { TestDate = entity.TestDate, TestInt = entity.TestInt, TestString = entity.TestString }, transaction: _transaction);
        }
        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            _connection.Execute("UPDATE TestEntities SET TestDate=@TestDate, TestInt=@TestInt, TestString=@TestString WHERE Id=@Id",
                new { Id = id, TestDate = testDateTime, TestInt = testInt, TestString = testString }, transaction: _transaction);
        }

        public void Commit()
        {
            _transaction.Commit();
        }
        public void TearDown()
        {
            _connection.Close();
            SqlConnection.ClearPool(_connection);
        }
    }
}
