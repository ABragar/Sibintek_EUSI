using System;
using System.Linq;

namespace Base.Events
{

    public interface IEventTrigger<TEvent>
        where TEvent : BaseEvent
    {
        void Raise(Func<TEvent> event_func);
    }



    
    
}