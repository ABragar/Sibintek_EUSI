using AutoMapper;
using Base.UI.DetailViewSetting;
using Common.Data.Extensions;

namespace Common.Data
{
    public static class AutoMapperConfigUI
    {
        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMapForCopy<DvSettingForType>()
                .ForMember(p => p.Fields, a => a.MapFrom(m => m.Fields));

            config.CreateMapForCopy<EditorVmSetting>();
            config.CreateMapForCopy<RuleVisible>();
            config.CreateMapForCopy<RuleHidden>();
            config.CreateMapForCopy<RuleEnable>();
            config.CreateMapForCopy<ReadOnlyEnable>();
            config.CreateMapForCopy<BRuleVisible>();
            config.CreateMapForCopy<BRuleHidden>();
            config.CreateMapForCopy<BRuleEnable>();
            config.CreateMapForCopy<BRuleReadOnly>();
        }
    }
}