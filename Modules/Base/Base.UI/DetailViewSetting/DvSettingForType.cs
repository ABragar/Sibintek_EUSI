using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace Base.UI.DetailViewSetting
{
    [EnableFullTextSearch]
    public class DvSettingForType : DvSetting , ITransform
    {
        
        [ListView]
        [DetailView("Тип", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string ObjectType { get; set; }
    }
}