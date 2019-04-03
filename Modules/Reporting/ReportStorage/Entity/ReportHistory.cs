using System;

namespace ReportStorage.Entity
{
    public class ReportHistory
    {
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatorID { get; set; }

        public int ReportID { get; set; }

        public virtual Report Report { get; set; }

//        public string Action { get; set; }
    }
}