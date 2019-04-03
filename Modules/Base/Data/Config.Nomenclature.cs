using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Entities;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.Category;
using Base.Nomenclature.Entities.NomenclatureType;
using Data.EF;

namespace Data
{
    public class NomenclatureConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
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
                .Entity<StowageType>();
        }
    }
}