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
    public class DapperDeleteWhereInConfiguration : IRunnableDeleteConfiguration
    {
        public string Name { get { return "Delete Where In"; } }

        public string Technology { get { return "Dapper 1.8"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        public DapperDeleteWhereInConfiguration(IConnectionString connectionString)
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
                 _connection.Query<int>("DELETE FROM TestEntities WHERE Id IN (" + String.Join(",",_toDelete) + ")");
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
