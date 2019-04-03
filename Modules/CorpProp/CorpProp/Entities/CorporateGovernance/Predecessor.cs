using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет сведения о правопредшественнике.
    /// </summary>
    [EnableFullTextSearch]
    public class Predecessor : TypeObject
    {
        
        /// <summary>
        /// Инициализирует новый экземпляр класса Predecessor.
        /// </summary>
        public Predecessor(): base()
        {
        }
        

        /// <summary>
        /// Получает или задает ИНН.
        /// </summary>       
        [DetailView(Name = "ИНН", Order = 1, TabName = "[0]Основные данные")]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=INN")]
        [FullTextSearchProperty]
        [ListView(Order =1 )]
        public string INN { get; set; }

        /// <summary>
        /// Получает или задает краткое наименование.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView(Order = 2)]
        [DetailView(Name = "Краткое наименование", Order = 2,
         TabName = "[0]Основные данные", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShortName { get; set; }

        /// <summary>
        /// Получает или задает ИД правопреемника.
        /// </summary>
        [SystemProperty]
        public int? SocietySuccessorID { get; set; }

        /// <summary>
        /// Получает или задает правопреемника.
        /// </summary>
        [ListView(Order = 3, Hidden = true)]
        [DetailView(Name = "Правопреемник", Visible = false)]
        public virtual SubjectObject.Society SocietySuccessor { get; set; }

        /// <summary>
        /// Получает или задает ИД типа правопреемства.
        /// </summary>
        public int? SuccessionTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип правопреемства.
        /// </summary>
        [DetailView(Name = "Тип правопреемства", Order = 4, TabName = "[0]Основные данные", Required = true)]
        [ListView(Order = 4)]
        public virtual SuccessionType SuccessionType { get; set; }

        /// <summary>
        /// Получает или задает дату правопреемства.
        /// </summary>
        [ListView(Order = 5)]
        [DetailView(Name = "Дата правопреемства", Order = 5, TabName = "[0]Основные данные", Required = true)]
        public DateTime DateSuccession { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Комментарий", Order = 6, TabName = "[0]Основные данные")]
        [FullTextSearchProperty]        
        public string Description { get; set; }

       
    }
}
