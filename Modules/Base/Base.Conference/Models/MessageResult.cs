using System.Collections.Generic;

namespace Base.Conference.Models
{
    public class MessageResult
    {
        public SimpleMessage Message { get; set; }
        public List<int> Targets { get; set; }
    }
}