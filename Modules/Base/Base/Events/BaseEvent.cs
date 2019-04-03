namespace Base.Events
{
    public abstract class BaseEvent : IEvent
    {
        public IEventSource Source { get; internal set; }
    }
}