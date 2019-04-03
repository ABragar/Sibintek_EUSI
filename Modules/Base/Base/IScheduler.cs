using System;

namespace Base
{
    public interface IDateTimeRange
    {
        DateTime Start { get; }
        DateTime End { get; }
    }

    public interface IScheduler: IDateTimeRange
    {   
        string Title { get; }
        
        string Description { get; }
        string StartTimezone { get; }
        string EndTimezone { get; }
        string RecurrenceRule { get; }
        int? RecurrenceID { get; }
        string RecurrenceException { get; }
        bool IsAllDay { get; }
        string Color { get; }
    }
}
