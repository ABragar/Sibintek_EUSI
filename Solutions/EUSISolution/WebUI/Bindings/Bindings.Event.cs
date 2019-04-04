using Base.App;
using Base.Event.Entities;
using Base.Event.Service;
using Base.Event.UI.Presets;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class EventBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Event.Initializer>();
            container.Register<IEventService<Event>, EventService<Event>>();
            container.Register<ICallService, CallService>();
            container.Register<IEventService<Meeting>, MeetingService>();
            container.Register<IEventService<SimpleEvent>, EventService<SimpleEvent>>();

            container.Register<IPresetService<SchedulerPreset>, PresetService<SchedulerPreset>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<SchedulerPreset>, DefaultPresetFactory<SchedulerPreset>>(Lifestyle.Singleton);
        }
    }
}