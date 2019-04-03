﻿using Base.DAL;
using Base.DAL.EF;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.Category;
using Base.Nomenclature.Entities.NomenclatureType;
using Common.Data.EF;

namespace Common.Data
{
    public class NomenclatureConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Nomenclature>()
                .Entity<TmcNomenclature>()
                .Entity<ServicesNomenclature>()
                .Entity<NomenclatureFile>()
                .Entity<ServicesCategory>()
                .Entity<TmcCategory>()
                .Entity<NomenclatureCategory>()
                .Entity<Tarif>(
                    x =>
                        x.Save(
                            c =>
                                c.SaveOneObject(o => o.Vender)
                                    .SaveOneObject(o => o.Measure)
                                    .SaveOneObject(o => o.NomCategory)))
                .Entity<OkpdHierarchy>(c => c.Name("Okpd"))
                .Entity<Price>(x => x.Save(x1 => x1
                    .SaveOneObject(x2 => x2.Measure)
                    .SaveOneObject(x2 => x2.Product)
                    .SaveOneObject(x2 => x2.Vender)))
                .Entity<Stowage>(x => x.Save(x1 => x1
                    .SaveOneObject(x2 => x2.Measure)
                    .SaveOneObject(x2 => x2.NomCategory)
                    .SaveOneObject(x2 => x2.Vender)
                    .SaveOneObject(x2 => x2.StowageType)))
                .Entity<Transportation>(x => x.Save(x1 => x1
                    .SaveOneObject(x2 => x2.Measure)
                    .SaveOneObject(x2 => x2.NomCategory)
                    .SaveOneObject(x2 => x2.Vender)))
                .Entity<StowageType>()
                .Entity<GroupAccounting>()
                .Entity<OKPD2>();
        }
    }
}