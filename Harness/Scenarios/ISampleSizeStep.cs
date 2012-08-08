using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public interface ISampleSizeStep
    {
        int Increment(int i);
    }
}
