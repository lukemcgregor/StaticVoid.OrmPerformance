using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Runner.Config;

namespace StaticVoid.OrmPerformance.Runner.Wiring
{
    public class ConnectionString : IConnectionString
    {
        private readonly IRunnerConfig _config;

        public ConnectionString(IRunnerConfig config)
        {
            _config = config;
        }

		public string Database 
        { 
            get 
            {
                return new SqlConnectionStringBuilder(_config.ConnectionString).InitialCatalog; 
            }
        }

        public string FormattedConnectionString
        {
            get 
            {
                return _config.ConnectionString;
            }
        }

        //TODO implement this properly for tarwn's PetaPoco tests
		public string Provider {
            get { return "System.Data.SqlClient"; }
		}
	}
}
