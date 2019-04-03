using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Audit.Entities;
using Base.DAL;
using Base.DAL.EF;
using Base.Entities;
using Data.EF;

namespace Data
{
    public class AppConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<AppSetting>(c => c
                    .Save(saver => saver
                        .SaveOneObject(x => x.Logo)
                        .SaveOneObject(x => x.DashboardImage)));
        }
    }
}