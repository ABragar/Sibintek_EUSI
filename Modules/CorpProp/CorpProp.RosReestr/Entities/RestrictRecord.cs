using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Ограничение права и обременение объекта недвижимости
    /// </summary>    
    public class RestrictRecord : BaseObject
    {
        protected const string TabName2 = "[002]Характеристики недвижимости";
        protected const string TabName3 = "[003]Реквизиты выписки";
       

        public RestrictRecord()
        {
            //Restricting_rights = new List<RightRecordNumber>();
            //Restricted_rights_parties = new List<RestrictedRightsPartyOut>();
            //DocumentRecords = new List<DocumentRecord>();
            //DealRecords = new List<DealRecord>();

        }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>     
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата государственной регистрации")]      
        public System.DateTime? Registration_date { get; set; }


        #region Общие сведения об ограничениях и обременениях RestrictionsEncumbrancesData
        /// <summary>
        /// Уникальный идентификатор записи об ограничении
        /// </summary>      
        public string ID_Record { get; set; }

        /// <summary>
        /// Дата модификации
        /// </summary>       
        public string MdfDate { get; set; }

        /// <summary>
        /// Номер регистрации ограничения права или обременения объекта недвижимости
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер регистрации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegNumber { get; set; }

        /// <summary>
        /// Предмет ограничения
        /// </summary>      
        public string ShareText { get; set; }
        /// <summary>
        /// Дата государственной регистрации
        /// </summary>      
        public string RegDateStr { get; set; }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>    
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// Вид зарегистрированного ограничения права или обременения объекта недвижимости
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Код вида ограничения/обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Вид ограничения/обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeName { get; set; }

        #region Период Period
        /// <summary>
        /// Дата начала действия
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата начала действия")]       
        public System.DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата прекращения действия
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата прекращения действия")]     
        public System.DateTime? EndDate { get; set; }              

        /// <summary>
        /// Дата начала действия
        /// </summary>       
        public string Started { get; set; }

        /// <summary>
        /// Дата прекращения действия
        /// </summary>      
        public string Stopped { get; set; }

        /// <summary>
        /// Продолжительность
        /// </summary>      
        [ListView]
        [DetailView(ReadOnly = true, Name = "Срок действия")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Term { get; set; }

        #endregion

        #region Дополнительная информация в зависимости от вида зарегистрированного ограничения права или обременения объекта недвижимости AdditionalEncumbranceInfoType

        /// <summary>
        /// Вид сервитута
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Код вида сервитута")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Servitude_kindCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Вид сервитута")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Servitude_kindName { get; set; }

        /// <summary>
        /// Условия сервитута
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Условия сервитута")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Servitude_condition { get; set; }
        #endregion

                
        ///// <summary>
        ///// Ограничиваемые права
        ///// </summary>
        //public virtual ICollection<RightRecordNumber> Restricting_rights { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Ограничиваемые права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightsStr { get; set; }


        #endregion //Общие сведения об ограничениях и обременениях RestrictionsEncumbrancesData

        
        ///// <summary>
        ///// Сведения о лицах, в пользу которых установлены ограничения права и обременения объекта недвижимости
        ///// </summary>
        //public virtual ICollection<RestrictedRightsPartyOut> Restricted_rights_parties { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Сведения о лицах, в пользу которых установлены ограничения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Owner { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name = "Участники долевого строительства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AllShareOwner { get; set; }

        ///// <summary>
        ///// Документы-основания
        ///// </summary>
        //public virtual ICollection<DocumentRecord> DocumentRecords { get; set; }



        ///// <summary>
        ///// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, совершенных без необходимого в силу закона согласия третьего лица, органа
        ///// </summary>
        //public virtual ICollection<DealRecord> DealRecords { get; set; }



        #region  Изъятие для государственных или муниципальных нужд StateExpropriation
        /// <summary>
        /// Сведения о решении об изъятии земельного участка и (или) расположенного на нем объекта недвижимости для государственных или муниципальных нужд
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name=" Сведения об изъятии",
          Description = "Сведения о решении об изъятии земельного участка и (или) расположенного на нем объекта недвижимости для государственных или муниципальных нужд")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Expropriation_info_type { get; set; }

        /// <summary>
        /// Содержание отметки при возникновении
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Содержание отметки при возникновении")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Origin_content { get; set; }
        #endregion      

      


        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Права", Visible = false)]
        public int? RightRecordID { get; set; }

        [DetailView(ReadOnly = true, Name="Право", Visible = false)]
        public RightRecord RightRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }


        [DetailView(ReadOnly = true, Name="Выписка", TabName = TabName3)]
        public Extract Extract { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }


        [DetailView(ReadOnly = true, Name="ОНИ", TabName = TabName2)]
        public ObjectRecord ObjectRecord { get; set; }

    }
}
