using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет обременение/ограничение права.
    /// </summary>
    [EnableFullTextSearch]
    public class Encumbrance : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Encumbrance.
        /// </summary>
        public Encumbrance():base()
        {

        }
        

        /// <summary>
        /// Получает или задает номер гос регистрации.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Order = 1)]
        [DetailView(Name = "Номер гос. регистрации", Order = 1)]
        public System.String RegNumber { get; set; }


        public int? EncumbranceTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип обременения.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Вид обременения/ограничения", Order = 3)]
        public EncumbranceType EncumbranceType { get; set; }



        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Order = 3)]
        [DetailView(Name = "Код вида ограничения/обременения", Visible = false, Order = 4)]
        public System.String EncumbranceTypeCode { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Order = 4)]
        [DetailView(Name = "Наименование ограничения/обременения",  Order = 5)]
        public System.String Name { get; set; }



        /// <summary>
        /// Получает или задает лиц, в пользу которых ограничиваются права.
        /// </summary>        
        [ListView(Order = 5)]
        [DetailView(Name = "Описание лица в пользу которого установлены ограничения", Order = 6)]
        [PropertyDataType(PropertyDataType.Text)]
        public System.String Owner { get; set; }

        [ListView(Order = 6)]
        [DetailView(Name = "Наименование лица в пользу которого установлены ограничения", Order = 7)]
        [PropertyDataType(PropertyDataType.Text)]
        public System.String OwnerName { get; set; }

        [ListView(Order = 7)]
        [DetailView(Name = "ИНН лица в пользу которого установлены ограничения", Order = 8)]
        [PropertyDataType(PropertyDataType.Text)]
        public System.String OwnerINN { get; set; }


        /// <summary>
        /// Получает или задает предмет ограничения текстом.
        /// </summary>
        [MaxLength(4000)]
        [ListView(Order = 8)]
        [DetailView(Name = "Описание объекта ограничения/обременения", Order = 9)]
        [PropertyDataType(PropertyDataType.Text)]
        public System.String ShareText { get; set; }


        /// <summary>
        /// Получает или задает дату гос регистрации.
        /// </summary>
        [ListView(Order = 9)]
        [DetailView(Name = "Дата гос. регистрации ограничения/обременения", Order = 10)]
        public System.DateTime? RegDate { get; set; }


        /// <summary>
        /// Получает или задает дату начала.
        /// </summary>
        [ListView(Order = 10)]
        [DetailView(Name = "Дата начала действия", Order = 11)]
        public System.DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания.
        /// </summary>
        [ListView(Order = 11)]
        [DetailView(Name = "Дата прекращения действия", Order = 12)]
        public System.DateTime? EndDate { get; set; }


        /// <summary>
        /// Получает или задает заголовок.
        /// </summary>
        [MaxLength(1000)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Hidden = true)]
        [DetailView(Name ="Название", Visible = false, Order = 11)]
        public System.String Title { get; set; }


        /// <summary>
        /// Получает или задает продолжительность текстом.
        /// </summary>
        [MaxLength(1000)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Hidden = true)]
        [DetailView(Name = "Продолжительность", Order = 13)]
        public System.String Term { get; set; }

        /// <summary>
        /// Получает или задает участников долевого строительства по договорам участия в долевом строительстве.
        /// </summary>
        [MaxLength(1000)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Hidden = true)]
        [DetailView(Name = "Участники долевого строительства по договорам участия в долевом строительстве", Order = 14)]
        public System.String AllShareOwner { get; set; }

     

        /// <summary>
        /// Получает или задает ИД права.
        /// </summary>
        public int? RightID { get; set; }

        /// <summary>
        /// Получает или задает право субъекта.
        /// </summary>      
        [ListView(Order =7)]
        [DetailView(Name = "Право", Required = true, Order = 14, Visible = false)]
        public Right Right { get; set; }

        public int? EstateID { get; set; }

        /// <summary>
        /// Получает или задает право субъекта.
        /// </summary> 
        [DetailView(Name = "Объект имущества", Visible = false)]
        public Estate.Estate Estate { get; set; }

    }
}
