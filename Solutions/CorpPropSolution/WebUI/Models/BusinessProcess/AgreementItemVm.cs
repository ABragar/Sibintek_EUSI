using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base;
using Base.BusinessProcesses.Entities;
using Base.Security;
using WebUI.Models.Base;

namespace WebUI.Models.BusinessProcess
{
    public class AgreementItemVm
    {
        public string ShortDate { get; set; }
        public string ShortTime { get; set; }
        public string Comment { get; set; }

        public UserVm User { get; set; }

        public  FileData File { get; set; }

        public StageAction Action { get; set; }
    }
}