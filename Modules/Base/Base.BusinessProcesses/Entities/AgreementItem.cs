using Base.Security;
using System;

namespace Base.BusinessProcesses.Entities
{
    public class AgreementItem : BaseObject
    {
        public DateTime? Date { get; set; }
        public string Comment { get; set; }       

        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int? FromUserID { get; set; }
        public virtual User FromUser { get; set; }

        public int? ActionID { get; set; }
        public virtual StageAction Action { get; set; }
        public int? FileID { get; set; }
        public virtual FileData File { get; set; }
    }
}
