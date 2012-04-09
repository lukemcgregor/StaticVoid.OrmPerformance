using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class UpdateBasicConfiguration : IRunnableUpdateConfiguration
    {
        public string Name { get { return "Basic"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;

        private System.Data.SqlClient.SqlCommand _updateCommand;
        private SqlParameter _testId;
        private SqlParameter _testDate;
        private SqlParameter _testInt;
        private SqlParameter _testString;

        public UpdateBasicConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
            
            _updateCommand = new System.Data.SqlClient.SqlCommand();
            _updateCommand.Connection = _connection;
            _updateCommand.CommandText = "UPDATE TestEntities SET TestDate=@TestDate , TestInt=@TestInt, TestString=@TestString WHERE Id=@Id";
            _testId       = _updateCommand.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            _testInt      = _updateCommand.Parameters.Add("@TestInt", System.Data.SqlDbType.Int);
            _testDate     = _updateCommand.Parameters.Add("@TestDate", System.Data.SqlDbType.DateTime);
            _testString   = _updateCommand.Parameters.Add("@TestString", System.Data.SqlDbType.NVarChar);

            _connection.Open();
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            _testId.Value       = id;
            _testInt.Value      = testInt;
            _testDate.Value     = testDateTime;
            _testString.Value   = testString;

            _updateCommand.ExecuteNonQuery();
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
