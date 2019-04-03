using System;
using Base.Security;

namespace Base.Social.Entities.Components
{
    public class Сomments : BaseObject
    {
        public string Type { get; set; }
        public int TypeId { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }

    }
}
