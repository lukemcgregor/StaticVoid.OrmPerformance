using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class BasicSelectConfiguration : IRunnableDiscreteSelectConfiguration, IRunnableBulkSelectConfiguration
    {
        public string Name { get { return "Basic"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;

        private System.Data.SqlClient.SqlCommand _findByTestIntCommand;
        private System.Data.SqlClient.SqlCommand _findByIdCommand;
        private SqlParameter _testId;
        private SqlParameter _testInt;

        public BasicSelectConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {

            _findByTestIntCommand = new System.Data.SqlClient.SqlCommand();
            _findByTestIntCommand.CommandText = "SELECT * FROM TestEntities WHERE TestInt=@TestInt";
            _testInt = _findByTestIntCommand.Parameters.Add("@TestInt", System.Data.SqlDbType.Int);

            _findByIdCommand = new System.Data.SqlClient.SqlCommand();
            _findByIdCommand.CommandText = "SELECT TOP 1 * FROM TestEntities WHERE Id=@TestId";
            _testId = _findByIdCommand.Parameters.Add("@TestId", System.Data.SqlDbType.Int);
        }
                
        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
            _findByTestIntCommand.Connection = _connection;

            _testInt.Value = testInt;

            var result = _findByTestIntCommand.ExecuteReader();

            List<Models.TestEntity> res = new List<Models.TestEntity>();

            while (result.Read())
            {
                res.Add( new Models.TestEntity 
                { 
                    Id = (int)result["Id"], 
                    TestDate = (DateTime)result["TestDate"], 
                    TestInt = (int)result["TestInt"], 
                    TestString = result["TestString"].ToString() 
                });
            }
            result.Close();
            _connection.Close();

            return res;
        }

        public Models.TestEntity Find(int id)
        {
            _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
            _findByIdCommand.Connection = _connection;
            _testId.Value = id;
            var result = _findByIdCommand.ExecuteReader();

            result.Read();

            var res = new Models.TestEntity
            {
                Id = (int)result["Id"],
                TestDate = (DateTime)result["TestDate"],
                TestInt = (int)result["TestInt"],
                TestString = result["TestString"].ToString()
            };
            result.Close();
            _connection.Close();
            return res;
        }

        public void TearDown()
        {
        }
    }
}
