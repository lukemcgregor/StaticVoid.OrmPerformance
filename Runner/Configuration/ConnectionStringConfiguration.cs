using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerfomance.Runner.Configuration
{
    public class ConnectionStringConfiguration : IConnectionString
    {
        public string Database{ get {return new System.Data.SqlClient.SqlConnectionStringBuilder(FormattedConnectionString).InitialCatalog; } }

        public string FormattedConnectionString { get; set; }
    }
}
