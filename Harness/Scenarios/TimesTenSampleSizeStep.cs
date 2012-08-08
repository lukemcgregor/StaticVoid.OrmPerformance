using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public class TimesTenSampleSizeStep : ISampleSizeStep
    {
        public int Increment(int i)
        {
            return i * 10;
        }
    }
}
