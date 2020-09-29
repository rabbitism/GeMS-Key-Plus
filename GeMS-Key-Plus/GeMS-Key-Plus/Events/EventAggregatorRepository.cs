using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeMS_Key_Plus.Events
{
    internal class EventAggregatorRepository
    {
        private static EventAggregatorRepository _instance;

        private EventAggregatorRepository()
        {

        }
        public static EventAggregatorRepository GetInstance()
        {
            if(_instance is null)
            {
                _instance = new EventAggregatorRepository();
            }
            return _instance;
        }

        public EventAggregator EventAggregator { get; } = new EventAggregator();
    }
}
