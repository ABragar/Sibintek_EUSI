using AutoMapper;
using Base.Macros.Entities;
using Common.Data.Extensions;

namespace Common.Data
{
    public static class AutoMapperConfigBase
    {
        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMapForCopy<ConditionItem>();
        }
    }
}