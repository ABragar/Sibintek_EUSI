using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Ранее присвоенный номер.
    /// </summary>    
    public class OldNumber : BaseObject
    {
        public OldNumber() { }

        /// <summary>
        /// Код вида номера.
        /// </summary>        
        [ListView]
        [DetailView(ReadOnly = true, Name="Код вида номера")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number_typeCode { get; set; }

        /// <summary>
        /// Код вида номера.
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Вид номера")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number_typeName { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Дата присвоения.
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата присвоения")]        
        public System.DateTime? Assignment_date { get; set; }

        /// <summary>
        /// Организация, присвоившая номер.
        /// </summary>        
        [ListView]
        [DetailView(ReadOnly = true, Name="Организация, присвоившая номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Assigner { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }

        
        [DetailView(ReadOnly = true, Name="ОНИ", Visible = false)]
        public ObjectRecord ObjectRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }

        
        [DetailView(ReadOnly = true, Name="Выписка", Visible = false)]
        public Extract Extract { get; set; }
    }
}
