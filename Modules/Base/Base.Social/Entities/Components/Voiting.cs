﻿using Base.Security;

namespace Base.Social.Entities.Components
{
    public class Voiting : BaseObject
    {
        public string Type { get; set; }
        public int TypeId { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public bool? Thumb { get; set; }
    }
}