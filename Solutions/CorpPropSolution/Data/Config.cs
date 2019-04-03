using Base.DAL;
using Common.Data;
using Data.EF;

namespace Data
{
    public class Config
    {
        public static IEntityConfiguration BuildConfig()
        {
            var builder = new EntityConfigurationBuilder();

            CommonDataConfig.Init<DataContext>(builder);

            DataConfig.Init(builder);

            //доп. модули
            //ForumConfig.Init(builder);
            //NomenclatureConfig.Init(builder);

            return builder.Build();
        }
    }
}