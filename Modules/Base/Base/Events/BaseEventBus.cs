using System;
using System.Collections.Generic;
using System.Linq;
using Base.Service;

namespace Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        private readonly IServiceLocator _locator;

        protected BaseEventBus(IServiceLocator locator)
        {
            _locator = locator;
        }


        private class EventTrigger<TEvent> : IEventTrigger<TEvent>
            where TEvent : BaseEvent
        {
            private readonly BaseEventBus _event_bus;
            private readonly IEventSource _source;
            private IReadOnlyCollection<Action<TEvent>> _handlers;
            private int _stamp;
            internal EventTrigger(BaseEventBus event_bus, IEventSource source)
            {
                _event_bus = event_bus;
                _source = source;
                _stamp = event_bus._stamp;
                _handlers = event_bus.GetHandlers<TEvent>();
            }

            public void Raise(Func<TEvent> event_func)
            {
                if (_event_bus._stamp != _stamp)
                {
                    _stamp = _event_bus._stamp;
                    _handlers = _event_bus.GetHandlers<TEvent>();
                }

                _event_bus.Raise(_source,event_func,_handlers);
            }
        }


        protected virtual IServiceFactory<TService> GetServiceFactory<TService>() where TService : class
        {
            return _locator.GetService<IServiceFactory<TService>>();
        }

        private readonly List<object> _handlers = new List<object>();

        private void AddHandler<TEvent>(Action<TEvent> action)
            where TEvent : class, IEvent
        {
            _handlers.Add(action);

            _stamp++;
        }

        private int _stamp;
        protected void RegisterHandler<TEventHandler, TEvent>(Func<TEvent, bool> filter = null)
            where TEventHandler : class, IEventBusHandler<TEvent>
            where TEvent : class, IEvent

        {
            var factory = GetServiceFactory<TEventHandler>();

            if (filter == null)
            {
                AddHandler<TEvent>(x =>
                {
                    var service = factory.GetService();
                    service.OnEvent(x);
                });
            }
            else
            {
                AddHandler<TEvent>(x =>
                {
                    if (filter(x))
                    {
                        var service = factory.GetService();
                        service.OnEvent(x); 
                    }
                });
            }

        }

        protected void RegisterAction<THandler, TEvent>(Action<THandler, TEvent> handler, Func<TEvent, bool> filter = null)
            where THandler : class
            where TEvent : class, IEvent
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var factory = GetServiceFactory<THandler>();

            if (filter == null)
            {
                AddHandler<TEvent>(x =>
                {
                    var service = factory.GetService();
                    handler(service, x);
                });
            }
            else
            {
                AddHandler<TEvent>(x =>
                {
                    if (filter(x))
                    {
                        var service = factory.GetService();
                        handler(service, x);
                    }
                });
            }
        }


     

        public IEventTrigger<TEvent> GetTrigger<TEvent>(IEventSource source)
            where TEvent : BaseEvent
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new EventTrigger<TEvent>(this, source);
        }

        private IReadOnlyCollection<Action<TEvent>> GetHandlers<TEvent>() where TEvent : BaseEvent
        {
            return _handlers.OfType<Action<TEvent>>().ToArray();
        }

        public void Raise<TEvent>(IEventSource source, Func<TEvent> event_func)
            where TEvent : BaseEvent
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Raise(source,event_func,GetHandlers<TEvent>());
        }

        private void Raise<TEvent>(IEventSource source, Func<TEvent> event_func,IReadOnlyCollection<Action<TEvent>> handlers)
            where TEvent : BaseEvent
        {

            if (handlers.Any() && !source.DisableEvents)
            {
                var evnt = event_func();

                evnt.Source = source;

                foreach (var handler in handlers)
                {
                    handler(evnt);
                }

            }
            
        }
    }
}