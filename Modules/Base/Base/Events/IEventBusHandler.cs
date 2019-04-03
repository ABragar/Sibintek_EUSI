namespace Base.Events
{
    public interface IEventBusHandler<in TEvent>
        where TEvent: class,IEvent
    {
        void OnEvent(TEvent evnt);

    }
}