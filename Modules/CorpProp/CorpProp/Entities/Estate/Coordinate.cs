using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет координаты объекта.
    /// </summary>
    [Table("Coordinate")]
    [EnableFullTextSearch]
    public class Coordinate : TypeObject
    {
        public Coordinate() : base() { }

        /// <summary>
        /// Номер контура.
        /// </summary>
        [ListView(Order = 1)]
        [DetailView(Name = "Номер контура")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ContourNumber { get; set; }

        /// <summary>
        /// Система координат.
        /// </summary>
        [ListView(Order = 2)]
        [DetailView(Name = "Система координат")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Datum { get; set; }

        /// <summary>
        /// Получает или задает ИД типа контура.
        /// </summary>
        public int? ContourTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип контура.
        /// </summary>
        [ListView(Order = 3)]
        [DetailView(Name = "Тип контура")]
        public ContourType ContourType { get; set; }

        /// <summary>
        /// Координата X
        /// </summary>
        [ListView(Order = 4)]
        [DetailView(Name = "Координата X")]
        public decimal? X { get; set; }
        /// <summary>
        /// Координата Y
        /// </summary>
        [ListView(Order = 5)]
        [DetailView(Name = "Координата Y")]
        public decimal? Y { get; set; }
        /// <summary>
        /// Координата Z
        /// </summary>
        [ListView(Order = 6)]
        [DetailView(Name = "Координата Z")]
        public decimal? Z { get; set; }
        /// <summary>
        /// Номер точки (порядок обхода)
        /// </summary>
        [ListView(Order = 7)]
        [DetailView(Name = "Номер точки (порядок обхода)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string OrdNumber { get; set; }
        /// <summary>
        /// Номер точки
        /// </summary>
        [ListView(Order = 8)]
        [DetailView(Name = "Номер точки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string PointNumber { get; set; }

        /// <summary>
        /// Погрешность
        /// </summary>
        [ListView(Order = 9)]
        [DetailView(Name = "Погрешность")]
        public decimal? Delta { get; set; }

        /// <summary>
        /// Радиус
        /// </summary>
        [ListView(Order = 10)]
        [DetailView(Name = "Радиус")]
        public decimal? R { get; set; }


        [ListView(Hidden = true)]
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер участка, входящего в состав единого землепользования")]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadNumber { get; set; }


        /// <summary>
        /// Получает или задает ИД кадастровог ообъекта.
        /// </summary>
        public int? CadastralID { get; set; }

        /// <summary>
        /// Получает или задает кадастровый объект.
        /// </summary>
        public virtual Cadastral Cadastral { get; set; }

        /// <summary>
        /// Получает или задает тип объекта.
        /// </summary>
        [SystemProperty]       
        public EstateTyepLocation? EstateTyepLocation { get; set; }
    }


    [UiEnum]
    public enum EstateTyepLocation
    {
        [UiEnumValue("ЗУ")]
        Land,
        [UiEnumValue("Здание")]
        Build,
        [UiEnumValue("Сооружение")]
        Construction
    }
}
