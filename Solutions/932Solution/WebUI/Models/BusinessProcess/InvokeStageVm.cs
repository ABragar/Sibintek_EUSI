using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.BusinessProcess
{
    public class InvokeStageVm
    {
        public string ObjectType { get; set; }
        public int ObjectID { get; set; }
        public int StageID { get; set; }
        public int ActionID { get; set; }
        public string SearchStr { get; set; }
    }
}