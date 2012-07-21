using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using StaticVoid.OrmPerformance.Messaging;

namespace StaticVoid.OrmPerformance.Runner.Wiring
{
    public class EventAggregatorForwarder : ISendMessages
    {
        private readonly IEventAggregator _eventAggregator;

        public EventAggregatorForwarder(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Send<T>(T message) where T : IOrmPerformanceMessage
        {
            _eventAggregator.Publish(message);
        }
    }
}
