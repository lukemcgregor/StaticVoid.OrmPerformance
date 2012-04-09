using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class InsertBasicConfiguration : IRunnableInsertConfiguration
    {
        public string Name { get { return "Basic"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;

        private System.Data.SqlClient.SqlCommand _insertCommand;
        private SqlParameter _testDate;
        private SqlParameter _testInt;
        private SqlParameter _testString;

        public InsertBasicConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
            
            _insertCommand = new System.Data.SqlClient.SqlCommand();
            _insertCommand.Connection = _connection;
            _insertCommand.CommandText = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES (@TestDate,@TestInt,@TestString)";
            _testInt      = _insertCommand.Parameters.Add("@TestInt", System.Data.SqlDbType.Int);
            _testDate     = _insertCommand.Parameters.Add("@TestDate", System.Data.SqlDbType.DateTime);
            _testString   = _insertCommand.Parameters.Add("@TestString", System.Data.SqlDbType.NVarChar);

            _connection.Open();
        }

        public void Add(Models.TestEntity entity)
        {
            _testInt.Value      = entity.TestInt;
            _testDate.Value     = entity.TestDate;
            _testString.Value   = entity.TestString;

            _insertCommand.ExecuteNonQuery();
        }

        public void Commit()
        {
        }
                
        public void TearDown()
        {
            _connection.Close();
            _connection.Dispose();
        }

    }
}