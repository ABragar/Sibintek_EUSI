namespace Base.Events
{
    public interface IEvent
    {
        IEventSource Source { get; }
    }
}