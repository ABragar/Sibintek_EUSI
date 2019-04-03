using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Сведения о лице, в пользу которого установлены ограничения права и обременения объекта недвижимости
    /// </summary>
    public class RestrictedRightsPartyOut : BaseObject
    {
        public RestrictedRightsPartyOut() { }
        /// <summary>
        /// Тип лица, в пользу которых установлены ограничения права и обременения объекта недвижимости
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Код типа лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Тип лица, в пользу которых установлены ограничения права и обременения объекта недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "ИНН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "ОГРН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name= "Краткое наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Short_name { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Субъекта", Visible = false)]    
        public int? SubjectID { get; set; }

        /// <summary>
        /// Лицо, в пользу которого установлены ограничения права и обременения объекта недвижимости
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Лицо, в пользу которого установлены ограничения права и обременения объекта недвижимости")]       
        public SubjectRecord Subject { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Ограничения", Visible = false)]
        public int? RestrictRecordID { get; set; }

       
        [DetailView(ReadOnly = true, Name="Ограничение", Visible = false)]
        public RestrictRecord RestrictRecord { get; set; }
    }
}
