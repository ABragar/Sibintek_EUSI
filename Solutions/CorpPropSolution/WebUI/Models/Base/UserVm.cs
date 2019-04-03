using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base;
using Base.Security;

namespace WebUI.Models.Base
{
    public class UserVm
    {
        public string FullName { get; set; }
        public FileData Image { get; set; }
        public int ID { get; set; }

    }
}