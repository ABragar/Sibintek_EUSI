using AutoMapper;
using Base.Macros.Entities;

namespace Data
{
    public static class AutoMapperConfigBase
    {
        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMapForCopy<ConditionItem>();
        }
    }
}