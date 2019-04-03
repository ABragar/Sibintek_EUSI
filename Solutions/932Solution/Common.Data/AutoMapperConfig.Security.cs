using AutoMapper;
using Base.Security;
using Common.Data.Extensions;

namespace Common.Data
{
    public static class AutoMapperConfigSecurity
    {
        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMapForCopy<ProfilePhone>()
                .ForMember(x => x.BaseProfile, opt => opt.Ignore())
                .ForMember(x => x.BaseProfileID, opt => opt.Ignore());

            config.CreateMapForCopy<ProfileEmail>()
                .ForMember(x => x.BaseProfile, opt => opt.Ignore())
                .ForMember(x => x.BaseProfileID, opt => opt.Ignore());
        }
    }
}