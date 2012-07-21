using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Runner.CLI
{
    public class ConnectionString : IConnectionString
    {
		public string Database { get { return System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]; } }

        public string FormattedConnectionString
        {
            get 
            {
				return System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            }
        }

		public string Provider {
			get { return System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ProviderName; }
		}
	}
}
