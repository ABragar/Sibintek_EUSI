using System;
using Base;

namespace WebUI.Models.Social
{
    public class CommentsModelVm
    {
        public string UserName { get; set; }
        public FileData Image { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
    }
}