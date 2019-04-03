using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Mapping
{
    /// <summary>
    /// Сопоставление объекта во внешней системе
    /// </summary>
    [EnableFullTextSearch]
    public class Mapping : TypeObject
    {
        /// <summary>
        /// Получает или задает идентификатор объекта в системе.      
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Идентификатор объекта в системе", Order = 0)]
        public int TypeObjectID { get; set; }

        /// <summary>
        /// Получает или задает тип объекта в системе.      
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Мнемоника", Order = 1)]
        public string Мnemonic { get; set; }

        /// <summary>
        /// Получает или задает ИД внешней системы.
        /// </summary>
        public int? ExternalSystemID { get; set; }

        /// <summary>
        /// Получает или задает внешнюю систему.
        /// </summary>      
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Внешняя система", Order = 2)]
        public virtual ExternalMappingSystem ExternalSystem { get; set; }

        /// <summary>
        /// Получает или задает идентификатор объекта во внешней системе.      
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Идентификатор объекта во внешней системе", Order = 3)]
        public Guid ExternalGuid { get; set; }

        /// <summary>
        /// Получает или задает идентифицирующий код объекта во внешней системе.
        /// </summary>
        /// <remarks>
        /// Здесь можно указывать строковое представление целочисленных идентификаторов или условные коды и обозначения.
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Код объекта во внешней системе", Order = 4)]
        public string ExternalCode { get; set; }

        /// <summary>
        /// Получает или задает наименование класса объекта во внешней системе.
        /// </summary>
        [MaxLength(500)]
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Класс объекта во внешней системе", Order = 5)]
        public string ExternalClassName { get; set; }
    }
}
