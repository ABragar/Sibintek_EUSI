using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.DAL.EF;
using Base.Links.Entities;
using Data.EF;

namespace Data
{
    public class LinksConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<LinkGroupConfig>()
                .Entity<LinkItem>()
                .Entity<LinkItemBaseObject>()
                ;
        }
    }
}