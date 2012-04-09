using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class DeleteWhereInConfiguration : IRunnableDeleteConfiguration
    {
        public string Name { get { return "Delete Where In"; } }

        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        public DeleteWhereInConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new SqlConnection(_connectionString.FormattedConnectionString);
            _connection.Open();
        }

        public void Commit()
        {
            if (_toDelete.Any())
            {
                _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);

                var delCommand = new System.Data.SqlClient.SqlCommand();
                delCommand.Connection = _connection;
                delCommand.CommandText = "DELETE FROM TestEntities WHERE Id IN (" + String.Join(",", _toDelete) + ")";

                _connection.Open();

                delCommand.ExecuteNonQuery();

            }
        }
        public void TearDown()
        {
            _connection.Close();
        }

        List<string> _toDelete = new List<string>();
        public void Delete(int id)
        {
            _toDelete.Add(id.ToString());
        }
    }
}
