using System;
using System.Security.Cryptography.X509Certificates;

namespace Base.Event
{
    public interface ICalendar
    {
        DateTime Start { get; set; }

        DateTime End { get; set; }

        string RecurrenceRule { get; set; }

        string RecurrenceException { get; set; }
    }
}