﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeMS_Key_Plus.Events
{
    internal class EventAggregatorRepository
    {
        private static EventAggregator _instance;
        private EventAggregatorRepository()
        {

        }
        public static EventAggregator GetInstance()
        {
            if(_instance is null)
            {
                _instance = new EventAggregator();
            }
            return _instance;
        }
    }
}
