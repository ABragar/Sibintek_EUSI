using AutoMapper;

namespace Common.Data
{
    public static class AutoMapperCommonConfig
    {


        public static void Init(IMapperConfigurationExpression config)
        {
            config.CreateMissingTypeMaps = true;
            config.ShouldMapProperty = fi => true;
            AutoMapperConfigBase.Init(config);
            AutoMapperConfigSecurity.Init(config);
            AutoMapperConfigUI.Init(config);
            AutoMapperConfigTest.Init(config);
        }

    }
}