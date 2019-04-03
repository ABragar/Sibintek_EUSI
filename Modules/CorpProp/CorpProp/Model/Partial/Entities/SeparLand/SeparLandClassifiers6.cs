using Base;
using Base.Attributes;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandClassifiers6 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает ИД класса объекта.
        /// </summary>
        public int? EstateTypeID { get; set; }

        /// <summary>
        /// Получает или задает класс объекта.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        public EstateType EstateType { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-94 наименование
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKOFName { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-94 код.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKOFCode { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKOFName2 { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKOFCode2 { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKTMOCode { get; set; }


        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKTMOName { get; set; }

        [DetailView(Visible = false)]
        public SibRegion OKTMORegion { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКАТО.
        /// </summary>        
        [SystemProperty]
        public int? OKATOID { get; set; }

        /// <summary>
        /// Получает или задает OKATO.
        /// </summary>        
        [DetailView(Visible = false)]
        public OKATO OKATO { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKATORegionCode { get; set; }

        [DetailView(Visible = false)]
        public SibRegion OKATORegion { get; set; }
    }
}
