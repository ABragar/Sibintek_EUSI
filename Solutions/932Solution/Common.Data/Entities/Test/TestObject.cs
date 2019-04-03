using System;
using System.Collections.Generic;
using Base;
using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.Content;
using Base.Entities.Complex;
using Base.EntityFrameworkTypes.Complex;
using Base.Enums;
using Base.Event;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Security.ObjectAccess.Policies;
using Base.Social.Entities.Components;
using Base.Translations;

namespace Common.Data.Entities.Test
{
    [AccessPolicy(typeof(EditCreatorOnlyAccessPolicy))]
    public class TestObject : BaseObject, IBPObject, ICommentsState
    {
        [DetailView("Метстоположение")]
        [ListView]
        public Location Location { get; set; } = new Location();


        [DetailView(Name = "Иконка")]
        public Icon Icon { get; set; } = new Icon();

        public int? CreatorID { get; set; }

        [ListView("Автор")]
        [DetailView]
        public virtual User Creator { get; set; }

        public int? ImageID { get; set; }

        [DetailView(Name = "Изображение")]
        [ListView]
        [Image(Width = 600, Height = 200, HeightForListView = 20, WidthForListView = 20, DefaultImage = DefaultImage.NoPhoto)]
        public virtual FileData Image { get; set; }

        public int? Image2ID { get; set; }

        [DetailView(Name = "Изображение2")]
        [ListView]
        [Image(DefaultImage = DefaultImage.NoImage, Crop = false)]
        public virtual FileData Image2 { get; set; }

        public int? Image3ID { get; set; }
        [DetailView(Name = "Изображение3")]
        [ListView]
        [Image(DefaultImage = DefaultImage.NoImage, Circle = true)]
        public virtual FileData Image3 { get; set; }

        [DetailView(Name = "Наименование документа", Required = true,
            Description =
                "Каждый веб-разработчик знает, что такое текст-«рыба». Текст этот, несмотря на название, не имеет никакого отношения к обитателям водоемов. Используется он веб-дизайнерами для вставки на интернет-страницы и демонстрации внешнего вида контента, просмотра шрифтов, абзацев, отступов и т.д. Так как цель применения такого текста исключительно демонстрационная, то и смысловую нагрузку ему нести совсем необязательно. Более того, нечитабельность текста сыграет на руку при оценке качества восприятия макета."
            )]
        [ListView(OneLine = false)]
        public string Title { get; set; }

        //[DetailView(Name = "Дата", Description = "Каждый веб-разработчик знает, что такое текст-«рыба».")]
        //public DateTime? Date { get; set; }

        [DetailView(Name = "Дабл", Description = "Каждый веб-разработчик знает, что такое текст-«рыба»."), ListView]
        public double Double { get; set; }

        [DetailView(Name = "Дабл 2", Description = "Каждый веб-разработчик знает, что такое текст-«рыба»."), ListView]
        public double Double2 { get; set; }

        [DetailView(Name = "Итерация")]
        [PropertyDataType(PropertyDataType.Number)]
        [ListView]
        public int Iteration { get; set; }

        [DetailView(Name = "Этап")]
        [ListView]
        public State? State { get; set; }

        [DetailView(Name = "Bullshit"), ListView]
        public bool Bullshit { get; set; }

        [DetailView("Записи")]
        public virtual ICollection<TestObjectEntry> TestObjectEntries { get; set; }

        [DetailView("UsersEntry"), ListView]
        public virtual ICollection<UsersEntry> UsersEntries { get; set; }

        [DetailView(Name = "Продолжительность следующего этапа")]
        [PropertyDataType(PropertyDataType.Duration)]
        public int? NextStageDuration { get; set; }

        public int? WorkflowContextID { get; set; }

        public WorkflowContext WorkflowContext { get; set; }

        public int? TestObjectFileID { get; set; }

        [DetailView("File")]
        [PropertyDataType(PropertyDataType.File)]
        public virtual FileData TestObjectFile { get; set; }

        [ListView]
        [DetailView("Дата и время")]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? DateTimeTest { get; set; }

        [ListView]
        [DetailView("Дата")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime DateTest { get; set; }

        [ListView]
        [DetailView("Месяц")]
        [PropertyDataType(PropertyDataType.Month)]
        public DateTime? MonthTest { get; set; }

        [ListView]
        [DetailView("Год")]
        [PropertyDataType(PropertyDataType.Year)]
        public DateTime? YearTest { get; set; }


        [ListView]
        [DetailView("TestField")]
        public virtual TestObjectNestedEntry TestField { get; set; }

        [ListView]
        [DetailView("CheckBox")]
        public bool? Check { get; set; }

        //[DetailView(TabName = "[1]Items")]
        //public virtual ICollection<TestObjectItem> Items { get; set; }

        [DetailView, ListView("TimeTest")]
        [PropertyDataType(PropertyDataType.Time)]
        public DateTime? TimeTest { get; set; }

        [DetailView("Период")]
        public Period Period { get; set; } = new Period()
        {
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };

        [DetailView("Nullable int"), ListView]
        public int? NullableInt { get; set; }

        [DetailView(TabName = "[3]Content")]
        public Content Content { get; set; } = new Content();

        [DetailView(ReadOnly = true), ListView]
        public LinkBaseObject LinkBaseObject { get; set; } = new LinkBaseObject();
    }

    public class TestObjectAndTestObject : ManyToManyAssociation<TestObject, TestObject>
    {

    }

    public class TestObjectAndTestObject2 : ManyToManyAssociation<TestObject, TestObject2>
    {

    }

    public class TestObject2 : BaseCatalog.Entities.BaseCatalog

    {
        private static readonly CompiledExpression<TestObject2, string> _title =
            DefaultTranslationOf<TestObject2>.Property(x => x.Td).Is(x => x.Title + " " + x.Description);

        [DetailView, ListView]
        public string Td { get; set; }
    }

    public class UsersEntry : EasyCollectionEntry<User>
    {
    }




    public class TestObjectEntry : EasyCollectionEntry<TestObjectNestedEntry>
    {
    }

    public class TestScheduler : BaseObject, IScheduler, ICalendar
    {
        private static readonly CompiledExpression<TestScheduler, string> _color =
            DefaultTranslationOf<TestScheduler>.Property(x => x.Color).Is(x => x.ColorPicker.Value);

        public TestScheduler()
        {
            ColorPicker = new Color();
        }

        public int? TestObjectID { get; set; }
        [DetailView]
        public virtual TestObject TestObject { get; set; }

        [DetailView, ListView]
        public string Title { get; set; }

        [DetailView, ListView]
        public DateTime Start { get; set; }

        [DetailView, ListView]
        public DateTime End { get; set; }

        [DetailView, ListView]
        public string Description { get; set; }

        [SystemProperty]
        public string StartTimezone { get; set; }

        [SystemProperty]
        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }

        [SystemProperty]
        public string Color => _color.Evaluate(this);

        [DetailView("Цвет")]
        public Color ColorPicker { get; set; }
    }

    public class TestObjectItem : BaseObject
    {
        public int? ParentID { get; set; }
        [DetailView]
        public virtual TestObject Parent { get; set; }

        public int? ImageID { get; set; }
        [DetailView, ListView]
        [Image(Width = 200, Height = 200)]
        public virtual FileData Image { get; set; }

        [DetailView, ListView]
        public string Title { get; set; }

        public int? UserID { get; set; }

        [DetailView("User"), ListView]
        public virtual User User { get; set; }


        [DetailView("Items aggregate collection")]
        public virtual ICollection<TestObjectSubItem> Items { get; set; }
    }

    public class TestObjectSubItem : BaseObject
    {
        //public int ParentID { get; set; }
        //public TestObjectItem Parent { get; set; }

        [DetailView, ListView]
        public string Title { get; set; }
    }

    public class TestObjectNestedEntry : BaseObject
    {
        [DetailView(Name = "Title")]
        [ListView]
        public string Title { get; set; }

        [DetailView(Name = "Text1")]
        [ListView]
        public string Text1 { get; set; }

        [DetailView(Name = "Text2")]
        [ListView]
        public string Text2 { get; set; }

        [DetailView(Name = "Text3")]
        [ListView]
        public string Text3 { get; set; }

        [DetailView(Name = "Изображение2")]
        [ListView]
        [Image(DefaultImage = DefaultImage.NoImage, Crop = false)]
        public virtual FileData Image2 { get; set; }

        [DetailView(Name = "Enum1")]
        [ListView]
        public State Enum1 { get; set; }
    }

    [UiEnum]
    public enum State
    {
        [UiEnumValue("New")]
        New = 0,

        [UiEnumValue("Stage 1")]
        Stage1 = 1,

        [UiEnumValue("Stage 2")]
        Stage2 = 2
    }

    [Flags]
    public enum Days
    {
        None = 0x0,
        Sunday = 0x1,
        Monday = 0x2,
        Tuesday = 0x4,
        Wednesday = 0x8,
        Thursday = 0x10,
        Friday = 0x20,
        Saturday = 0x40
    }
}