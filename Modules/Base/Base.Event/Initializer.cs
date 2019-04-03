using System.ComponentModel;
using Base.Contact.Entities;
using Base.Entities.Complex;
using Base.Event.Entities;
using Base.Event.Service;
using Base.Event.UI.Presets;
using Base.Links;
using Base.Links.Entities;
using Base.UI;
using Base.UI.ViewModal;
using Base.Word;

namespace Base.Event
{

    public class Initializer : IModuleInitializer
    {
        private readonly ILinkBuilder _linkBuilder;

        public Initializer(ILinkBuilder linkBuilder)
        {
            _linkBuilder = linkBuilder;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.Event>()
                .Title("События (список)")
                .Service<IEventService<Entities.Event>>()
                .ListView(x => x.Type(ListViewType.Grid))                
                .DetailView(x => x
                    .Title("Событие")
                    .Editors(edts => edts
                        .Add<Period>(action: e => e.Title("Период"))));

            context.CreateVmConfigOnBase<Entities.Event>(baseMnemonic: nameof(Entities.Event), createMnemonic: "EventScheduler")
                .Title("События (календарь)")
                .Service<IEventService<Entities.Event>>()
                .ListView(x => x.Type(ListViewType.Scheduler).HiddenActions(new[] { LvAction.Search, LvAction.Settings, LvAction.Export }))
                .DetailView(x => x.Title("Событие").IsMaximized(true));

            context.CreateVmConfigOnBase<Call>(nameof(Entities.Event))
                .Title("События - Звонок")
                .Service<ICallService>()
                .DetailView(d => d.Title("Звонок"))
                .ListView(l => l.Title("Звонки"));

            context.CreateVmConfigOnBase<SimpleEvent>(nameof(Entities.Event))
                .Title("События - Уведомление")
                .Service<IEventService<SimpleEvent>>()
                .DetailView(d => d.Title("Уведомление"))
                .ListView(l => l.Title("Уведомления"));

            context.CreateVmConfigOnBase<Meeting>(nameof(Entities.Event))
                .Title("События - Встреча")
                .Service<IEventService<Meeting>>()
                .DetailView(d => d.Title("Встреча"))
                .TemplateConfig(t => t
                    .AddValue("title", x => x.Item.Title)
                    .AddValue("description", x => x.Item.Description)
                )
                .ListView(l => l.Title("Встречи"));


            context.CreateVmConfig<SchedulerPreset>()
                 .Title("Пресет - Календарь")
                 .DetailView(
                     x =>
                         x.Title("Настройка - Календарь")
                             .IsMaximized(true)
                             .Toolbar(
                                 t =>
                                     t.Add("GetToolbarPreset", "View",
                                         d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "SchedulerPreset")))));

            context.CreateVmConfigOnBase<Call>(baseMnemonic: nameof(Call), createMnemonic: "CallCenter")
                .Title("Коллцентр")
                .ListView(x => x.Title("Коллцентр")
                    .DataSource(d => d
                        .Sort(s => s.Add(e => e.Start, ListSortDirection.Descending)))
                    .Columns(c => c
                        .Add(p => p.Title, d => d.Visible(false))
                        .Add(p => p.Description, d => d.Visible(false))
                        .Add(p => p.Creator, d => d.Visible(false))
                        .Add(p => p.Status, d => d.Visible(false))
                        .Add(p => p.Start, d => d.Title("Время").Visible(true)))
                    .Type(ListViewType.Grid)
                    .HiddenActions(new LvAction[] { LvAction.Create }))
                .DetailView(x => x.Title("Звонок").HideToolbar(true));


            InitLinks();
        }

        private void InitLinks()
        {
            _linkBuilder.Register<Employee, Call>().Config((source, dest) =>
            {
                dest.Title = $"Звонок: {source.Title}";
                dest.Contact = source;
            });

            _linkBuilder.Register<Employee, Meeting>().Config((source, dest) =>
            {
                dest.Title = $"Встреча: {source.Title}";
                dest.Participants.Add(new Participant()
                {
                    ObjectID = source.ID
                });
            });

            _linkBuilder.Register<Company, Call>().Config((source, dest) =>
            {
                dest.Title = $"Звонок: {source.Title}";
                dest.Contact = source;
            });

            _linkBuilder.Register<Company, Meeting>().Config((source, dest) =>
            {
                dest.Title = $"Встреча: {source.Title}";
                dest.Participants.Add(new Participant()
                {
                    ObjectID = source.ID
                });
            });

        }
    }
}
