using AutoMapper;
using Base.Security;
using Common.Data.Entities.Test;

namespace Common.Data
{
    public static class AutoMapperConfigTest
    {
        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMap<SimpleProfile, TestBaseProfile>()
                .ForMember(m => m.ID, m => m.Ignore())
                .ForMember(m => m.RowVersion, m => m.Ignore())
                .ForMember(m => m.Hidden, m => m.Ignore());
        }
    }
}