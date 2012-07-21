using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Messaging;
using StaticVoid.OrmPerformance.Messaging.Messages;

namespace StaticVoid.OrmPerformance.Runner.CLI
{
    public class ConsoleForwarder: ISendMessages
    {
        public void Send<T>(T message) where T : IOrmPerformanceMessage
        {
            if (message is ILoggable)
            {
                Console.WriteLine((message as ILoggable).Message);
            }
        }
    }
}
