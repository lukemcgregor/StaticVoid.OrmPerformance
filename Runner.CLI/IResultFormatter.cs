using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public interface IResultFormatter<T> where T: IPerformanceResult
    {
        void FormatResults(IEnumerable<T> results);
    }
}
