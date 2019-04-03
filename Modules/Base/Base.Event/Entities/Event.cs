using Base.Attributes;
using Base.Entities.Complex;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.ComplexKeyObjects.Superb;
using Base.Contact.Entities;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Event.Entities
{
    [EnableFullTextSearch]
    public class Event : BaseObject, IScheduler, IAccessibleObject, ICalendar, ICreateObject, ISuperObject<Event>
    {
        private static readonly CompiledExpression<Event, string> _color =
            DefaultTranslationOf<Event>.Property(x => x.Color).Is(x => x.ColorPicker.Value);

     
        public Event()
        {
            ColorPicker = new Color();
        }

        [ListView]
        [FullTextSearchProperty]
        [MaxLength(255)]
        [DetailView(Name = "Наименование", Order = 0)]
        public string Title { get; set; }

        public int CreatorID { get; set; }
        [DetailView("Автор", ReadOnly = true, Order = 1), ListView]
        public virtual User Creator { get; set; }

        [DetailView("Приоритет", Order = 3), ListView]
        public Prority Prority { get; set; }

        [DetailView("Статус события", Order = 4), ListView]
        public Status Status { get; set; }

        public bool IsAllDay { get; set; }

        [SystemProperty]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime Start { get; set; }

        [SystemProperty]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime End { get; set; }

        [SystemProperty]
        public string StartTimezone { get; set; }
        [SystemProperty]
        public string EndTimezone { get; set; }

        [ListView("Описание")]
        [FullTextSearchProperty]
        [DetailView("Описание")]
        public string Description { get; set; }

        [DetailView(TabName = "[3]Файлы")]
        [PropertyDataType(PropertyDataType.Files)]
        public virtual ICollection<EventFile> Files { get; set; }

        [SystemProperty]
        public string Color => _color.Evaluate(this);

        [DetailView("Цвет")]
        public Color ColorPicker { get; set; }

        //[ListView]
        //[DetailView(TabName = "[6]Периодичность")]
        //[PropertyDataType("Scheduler_RecurrenceRule")]
        public string RecurrenceRule { get; set; }

        [SystemProperty]
        public int? RecurrenceID { get; set; }

        [SystemProperty]
        public string RecurrenceException { get; set; }

        //Location
        //[DetailView(TabName = "[5]Местоположение")]
        //[PropertyDataType(PropertyDataType.LocationPoint)]
        //public Location Location { get; set; }

        public string ExtraID { get; }

        public virtual IEnumerable<User> GetStakeHolders()
        {
            return new List<User>();
        }

        [DetailView("Напоминание")]
        public RemindPeriod RemindPeriod { get; set; } = new RemindPeriod();

        public DateTime? RemindDate { get; set; }
        
    }

    [UiEnum]
    public enum Prority
    {
        [UiEnumValue("Низкий")]
        Low,
        [UiEnumValue("Средний")]
        Medium,
        [UiEnumValue("Высокий")]
        High
    }

    [UiEnum]
    public enum Status
    {
        [UiEnumValue("Новое")]
        New = 0,
        [UiEnumValue("В работе")]
        InProcess = 10,
        [UiEnumValue("Завершено")]
        Complete = 20,
        [UiEnumValue("Неактуально")]
        NotRelevant = 30,
    }

    public class EventFile : FileCollectionItem
    {
    }

    public class Participant : EasyCollectionEntry<BaseContact>
    {
    }

}
