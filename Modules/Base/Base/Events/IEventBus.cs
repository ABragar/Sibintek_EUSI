using System;

namespace Base.Events
{
    public interface IEventBus
    {
        IEventTrigger<TEvent> GetTrigger<TEvent>(IEventSource source) where TEvent: BaseEvent;
        void Raise<TEvent>(IEventSource source,Func<TEvent> event_func) where TEvent: BaseEvent;
    }

}