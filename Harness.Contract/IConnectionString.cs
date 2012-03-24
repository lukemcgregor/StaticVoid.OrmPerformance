using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface IConnectionString
    {
        string Server { get; }
        string Database { get; }
        bool Trusted { get; }
        string FormattedConnectionString { get; }
    }
}
