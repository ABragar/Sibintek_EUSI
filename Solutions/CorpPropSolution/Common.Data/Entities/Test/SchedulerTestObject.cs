using System;
using Base;
using Base.Attributes;

namespace Common.Data.Entities.Test
{
    public class SchedulerTestObject : BaseObject, IScheduler
    {
        [DetailView, ListView]
        public string Title { get; set; }

        [DetailView, ListView]
        public DateTime Start { get; set; }

        [DetailView, ListView]
        public DateTime End { get; set; }

        [DetailView, ListView]
        public string Description { get; set; }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public string Color { get; set; }
    }
}