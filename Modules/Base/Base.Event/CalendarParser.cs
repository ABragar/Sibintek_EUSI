using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Event
{


    public class CalendarParser
    {
        private readonly Lazy<DateTime[]> _recurrence_exception;

        private readonly DateTime _start;
        private readonly DateTime _end;

        private readonly bool _is_recurrent;

        private readonly Lazy<Dictionary<string, string>> _rrule;
        public CalendarParser(ICalendar calendar)
        {
            _start = calendar.Start;
            _end = calendar.End;
            _recurrence_exception = new Lazy<DateTime[]>(() =>
                string.IsNullOrWhiteSpace(calendar.RecurrenceException) ? new DateTime[0] : 
                    calendar.RecurrenceException.Split(';')
                        .Select(x=>DateTime.Parse(x)).ToArray()
                );


            _is_recurrent = calendar.RecurrenceRule != null;

            _rrule = new Lazy<Dictionary<string, string>>(()=>
                !_is_recurrent ? null :
                    calendar.RecurrenceRule.Split(';').Select(x=>new { Index = x.IndexOf("=") ,String = x})
                        .ToDictionary(x=> x.String.Substring(0,x.Index),x=>x.String.Substring(x.Index+1),StringComparer.OrdinalIgnoreCase));

        }

        private int?[] GetCron()
        {
            var result = new int?[] {_start.Minute, _start.Hour, _start.Day, _start.Month, null};

            if (!_is_recurrent)
                return result;

            var freq = _rrule.Value["FREQ"];
            switch (freq)
            {
                
                case "DAILY":
                    result[2] = null;
                    result[3] = null;
                    result[4] = null;
                    break;
                case "WEEKLY":
                    result[2] = null;
                    result[3] = null;
                    result[4] = 1;
                    break;
                case "MONTHLY":
                    result[4] = null;
                    break;
                case "YEARLY":
                    result[4] = null;
                    break;
                default:
                    throw new NotSupportedException();
            }
            
            

            return result;
        }

        public string ToCron(DateTime current,bool raise)
        {
            var cron = GetCron();

            

            return string.Join(" ", cron.Select(x => x?.ToString() ?? "*" ));
        }

        public bool IsCurrent(DateTime current)
        {
            if (_is_recurrent)
            {
                throw new NotImplementedException();
            }
            else
            {
                return true;
            }

            
        }

        public bool IsEnded(DateTime current, bool raise)
        {
            
            throw new NotImplementedException();
        }
    }
}