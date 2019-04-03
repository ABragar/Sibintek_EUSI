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
    /// Сведения о части объекта недвижимости (номер, содержание ограничений)
    /// </summary>   
    public class ObjectPartNumberRestrictions : BaseObject
    {
        public ObjectPartNumberRestrictions() { }

        /// <summary>
        /// Порядковый номер части
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Порядковый номер части")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Part_number { get; set; }

        /// <summary>
        /// Номер реестровой записи
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер реестровой записи")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Номер регистрации ограничения права или обременения ОН
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер регистрации ограничения права или обременения ОН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Right_number { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>        
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата регистрации")]
        [PropertyDataType(PropertyDataType.Text)]
        public System.DateTime? Registration_date { get; set; }

        /// <summary>
        /// Реестровый номер границы 
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Реестровый номер границы")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Reg_number { get; set; }


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
