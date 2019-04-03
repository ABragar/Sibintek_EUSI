using Base.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace Base
{
    [Serializable]
    [DataContract]
    [JsonObject]
    public abstract class BaseObject : IBaseObject
    {
        protected BaseObject()
        {
            SortOrder = -1;
        }

        [Key]
        [DataMember]
        [SystemProperty]
        [DetailView("Идентификатор", Visible = false, Order = -1, ReadOnly = true), ListView(Visible = false)]
        public int ID { get; set; }

        [DataMember]
        [SystemProperty]
        public bool Hidden { get; set; }
        
        [DataMember]
        [SystemProperty]
        public double SortOrder { get; set; }

        [Timestamp]
        [SystemProperty]
        public byte[] RowVersion { get; set; }
        
    }
}
