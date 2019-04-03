using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Контур ОКС
    /// </summary>   
    public class ContourOKSOut : BaseObject
    {
        public ContourOKSOut()
        {

        }

        /// <summary>
        /// Номер контура
        /// </summary>      
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер контура")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number_pp { get; set; }

        /// <summary>
        /// Кадастровый номер участка, входящего в состав единого землепользования
        /// </summary>
        [ListView]
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер участка, входящего в состав единого землепользования")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Cad_number { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Система координат")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Sk_id { get; set; }

        /// <summary>
        /// Тип контура (уровень расположения)
        /// </summary>    
        [ListView]
        [DetailView(ReadOnly = true, Name="Код типа контура (уровень расположения)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Level_contourCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Тип контура (уровень расположения)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Level_contourName { get; set; }

        /// <summary>
        /// Координата X
        /// </summary> 
        [ListView]
        [DetailView(ReadOnly = true, Name="Координата X")]
        public decimal X { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary> 
        [ListView]
        [DetailView(ReadOnly = true, Name="Координата Y")]
        public decimal Y { get; set; }

        /// <summary>
        /// Координата Z
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Координата Z")]
        public decimal Z { get; set; }

        /// <summary>
        /// Номер точки (порядок обхода)
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер точки (порядок обхода)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ord_nmb { get; set; }

        /// <summary>
        /// Номер точки
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер точки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Num_geopoint { get; set; }

        /// <summary>
        /// Погрешность
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Погрешность")]
        public decimal Delta_geopoint { get; set; }

        /// <summary>
        /// Радиус
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Радиус")]
        public decimal R { get; set; }

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
