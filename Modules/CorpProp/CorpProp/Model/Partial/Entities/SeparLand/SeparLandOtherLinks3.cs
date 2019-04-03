using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandOtherLinks3 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает Кадастровые номера иных объектов недвижимости.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string OtherCadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает ранее присвоенный гос. учетный номер.
        /// </summary>
//        [Historical]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string OldRegNumbers { get; set; }
    }
}
