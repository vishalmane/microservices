using System;

namespace EventBus
{
    public class Event<T> where T : class
    {
        public Event(T eventData)
        {
            EventData = eventData;
            RequestId = new Guid();
        }
        

        T EventData { get; set; }
        public Guid RequestId { get; set; }
    }
}
