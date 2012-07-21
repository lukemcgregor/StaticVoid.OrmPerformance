using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging
{
    public interface ISendMessages
    {
        void Send<T>(T message) where T : IOrmPerformanceMessage;
    }
}
