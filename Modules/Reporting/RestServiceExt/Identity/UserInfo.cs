﻿namespace RestService.Identity
{
    public class UserInfo
    {
        public int? UserId { get; set; }
        public bool IsAdmin { get; set; }
        public string CategoryIds { get; set; }
    }
}