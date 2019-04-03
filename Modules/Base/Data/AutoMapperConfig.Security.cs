using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Base.Security;
using Data.Entities.Test;

namespace Data
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