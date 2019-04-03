using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace Base.UI.DetailViewSetting
{
    [EnableFullTextSearch]
    public class DvSettingForMnemonic : DvSetting
    {
        [SystemProperty]
        [ListView("Мнемоника")]
        public string Mnemonic { get; set; }
    }
}