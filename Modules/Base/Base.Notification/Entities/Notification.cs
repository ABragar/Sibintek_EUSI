using Base.Attributes;
using Base.Entities.Complex;
using Base.Security;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Base.Utils.Common.Attributes;
using Base.ComplexKeyObjects.Superb;

namespace Base.Notification.Entities
{
    [EnableFullTextSearch]
    public class Notification : BaseObject, ISuperObject<Notification>
    {
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; } = null;

        [SystemProperty]
        public NotificationStatus Status { get; set; }

        [ListView]
        [DetailView(Name = "Дата", Required = true)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? Date { get; set; }

        public int? UserID { get; set; }
        public User User { get; set; }

        [DetailView(Name = "Cущность")]
        [ListView]
        public LinkBaseObject Entity { get; set; } = new LinkBaseObject();

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование", Required = true)]
        [MaxLength(255)]
        public string Title { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Описание")]
        public string Description { get; set; }
    }
}
