using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class InsertSingleStatementUnBatched : IRunnableInsertConfiguration
    {
        public string Name { get { return "Insert with single statement (un-batched)"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private List<Models.TestEntity> _entities = new List<Models.TestEntity>();

        public InsertSingleStatementUnBatched(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup() { }

        public void Add(Models.TestEntity entity)
        {
            _entities.Add(entity);
        }

        public void Commit()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString))
            {
                connection.Open();

                string sql = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES"
                            + String.Join(",", _entities.Select(e => String.Format("('{0}',{1},'{2}')", e.TestDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), e.TestInt, e.TestString)));

                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public void TearDown() { }

    }
}
