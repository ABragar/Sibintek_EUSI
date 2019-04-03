using Base;
using Base.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.NSI;
using System.ComponentModel;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandCharacteristics2 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Плучает или задает категории земель.
        /// </summary>
//        [Historical]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        //[PropertyDataType(PropertyDataType.Text)]
        public GroundCategory GroundCategory { get; set; }

        /// <summary>
        /// вид разрешенного использования По документу
        /// </summary>              
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedByDoc { get; set; }

        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором
        /// </summary>           
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedLandUse { get; set; }

        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>   
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedLandUseMer { get; set; }

        /// <summary>
        /// Реестровый номер границы  градостроительному регламенту
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittesGradRegNumbBorder { get; set; }

        /// <summary> 
        /// Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittesGradLandUse { get; set; }

        /// <summary>
        /// Плучает или задает виды разрешенного использования.
        /// </summary>
//        [Historical]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string UsesKind { get; set; }

        /// <summary>
        /// Получает или задает площадь.
        /// </summary> 
//        [Historical]
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType("Sib_Decimal2")]
        [DefaultValue(0)]
        public decimal? Area { get; set; }

        /// <summary>
        /// Получает или задает эксплуатируемую площадь ЗУ.
        /// </summary>
//        [Historical]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType("Sib_Decimal2")]
        public string UseArea { get; set; }

        /// <summary>
        /// Получает или задает текущую кадастровая стоимость.
        /// </summary> 
        // TODO: добавить логику для вычисления текущей кадастровой стоимости.
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Текущая кадастровая стоимость", TabName = TabName6)]
        [DefaultValue(0)]
        [DetailView(Visible = false)]
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// Получает или задает вид объекта недвижимого имущества.
        /// </summary>
//        [Historical]
        //[ListView]
        [DetailView(Visible = false)]
        public RealEstateKind RealEstateKind { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public LandType LandType { get; set; }
    }
}
