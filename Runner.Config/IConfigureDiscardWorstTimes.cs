using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformace.Runner.Config
{
    public interface IConfigureDiscardWorstTimes
    {
        int NumberOfWorstTimesToDiscard { get; }
    }
}
