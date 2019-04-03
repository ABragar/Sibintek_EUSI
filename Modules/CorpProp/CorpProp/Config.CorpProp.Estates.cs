using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Common;
using CorpProp.Entities.Estate;

namespace CorpProp
{
    public partial class CorpPropConfig
    {
        private static void InitEstates<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)

            #region CorpProp.Entities.Estate

                .Entity<CorpProp.Entities.Estate.Aircraft>()
                .Entity<CorpProp.Entities.Estate.AircraftKind>()
                .Entity<CorpProp.Entities.Estate.AircraftType>()
                .Entity<CorpProp.Entities.Estate.BuildingStructure>()
                .Entity<CorpProp.Entities.Estate.Cadastral>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.WellCategory)
                ))
                .Entity<CorpProp.Entities.Estate.CadastralPart>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Cadastral)))
                .Entity<CorpProp.Entities.Estate.CadastralValue>()
                .Entity<CorpProp.Entities.Estate.CarParkingSpace>()
                .Entity<CorpProp.Entities.Estate.Coordinate>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Cadastral)))
                .Entity<CorpProp.Entities.Estate.Estate>(s => s.Save(ss => ss
                        .SaveOneObject(x => x.EstateType)
                        //.SaveOneObject(x => x.ClassFixedAsset)
                        //.SaveOneObject(x => x.Owner)
                        .SaveOneObject(x => x.BusinessArea)
                        //.SaveOneObject(x => x.WhoUse)
                        //.SaveOneObject(x => x.ReceiptReason)
                        //.SaveOneObject(x => x.LeavingReason)
                        .SaveOneObject(x => x.OKOF94)
                        .SaveOneObject(x => x.OKOF2014)
                        .SaveOneObject(x => x.OKTMO)
                        //.SaveOneObject(x => x.OKTMORegion)
                        //.SaveOneObject(x => x.OKATO)
                        //.SaveOneObject(x => x.OKATORegion)
                        //.SaveOneObject(x => x.Status)
                        .SaveOneObject(x => x.DepreciationMethodRSBU)
                        .SaveOneObject(x => x.DepreciationMethodNU)
                        .SaveOneObject(x => x.DepreciationMethodMSFO)
                        .SaveOneObject(x => x.AddonOKOF)
                        .SaveOneObject(x => x.DivisibleType)

                //.SaveOneToMany(x => x.Images, x => x.SaveOneObject(z => z.Object))
                ))
                .Entity<CorpProp.Entities.Estate.EstateImage>()
                .Entity<CorpProp.Entities.Estate.EstateDeal>(s =>
                    s.Save(ss => ss.SaveOneObject(w => w.Estate).SaveOneObject(w => w.SibDeal)))
                .Entity<CorpProp.Entities.Estate.IntangibleAsset>()
                .Entity<CorpProp.Entities.Estate.InventoryObject>(s => s.Save(ss => ss
                     .SaveOneObject(x => x.Deposit)
                    .SaveOneObject(x => x.DepreciationGroup)
                    .SaveOneObject(x => x.EstateMovableNSI)
                    .SaveOneObject(x => x.Fake)
                    .SaveOneObject(x => x.GroupConsolidationMSFO)
                    .SaveOneObject(x => x.GroupConsolidationRSBU)
                    .SaveOneObject(x => x.LayingType)
                    .SaveOneObject(x => x.LessorSubject)
                    .SaveOneObject(x => x.LicenseArea)
                    .SaveOneObject(x => x.OwnershipType)
                    .SaveOneObject(x => x.Parent)
                    .SaveOneObject(x => x.PositionConsolidation)
                    .SaveOneObject(x => x.PropertyComplex)
                    .SaveOneObject(x => x.ProprietorSubject)
                    .SaveOneObject(x => x.RentTypeMSFO)
                    .SaveOneObject(x => x.RentTypeRSBU)
                    .SaveOneObject(x => x.SibCityNSI)
                    .SaveOneObject(x => x.SibCountry)
                    .SaveOneObject(x => x.SibFederalDistrict)
                    .SaveOneObject(x => x.SibRegion)
                    .SaveOneObject(x => x.StageOfCompletion)
                    .SaveOneObject(x => x.StatusConstruction)
                ))
                .Entity<EstateTaxes>(s => s.Save(ss => ss
                      .SaveOneObject(x => x.DecisionsDetails)
                      .SaveOneObject(x => x.DecisionsDetailsLand)
                      .SaveOneObject(x => x.DecisionsDetailsTS)
                      .SaveOneObject(x => x.EnergyLabel)
                      .SaveOneObject(x => x.TaxBase)
                      .SaveOneObject(x => x.TaxExemption)
                      .SaveOneObject(x => x.TaxRateType)
                ))
                .Entity<CorpProp.Entities.Estate.PropertyComplexIO>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.SibUser)
                    .SaveOneObject(x => x.SibFederalDistrict)
                    .SaveOneObject(x => x.SibRegion)
                    .SaveOneObject(x => x.Land)
                    .SaveOneObject(x => x.Country)
                    .SaveOneObject(x => x.Parent)
                    .SaveOneObject(x => x.PropertyComplexIOType)
                ))
                .Entity<CorpProp.Entities.Estate.EstateCalculatedField>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Owner)
                    .SaveOneObject(x => x.WhoUse)
                ))
                .Entity<CorpProp.Entities.Estate.Land>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.AddonAttributeGroundCategory)
                    .SaveOneObject(x => x.LandPurpose)
                ))
                .Entity<CorpProp.Entities.Estate.MovableEstate>()
                .Entity<CorpProp.Entities.Estate.NonCadastral>()
                .Entity<CorpProp.Entities.Estate.PropertyComplex>()
                .Entity<CorpProp.Entities.Estate.RealEstate>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.PermittedUseKind)
                ))
                .Entity<CorpProp.Entities.Estate.RealEstateComplex>()
                .Entity<CorpProp.Entities.Estate.Room>()
                .Entity<CorpProp.Entities.Estate.Ship>()
                .Entity<CorpProp.Entities.Estate.ShipClass>()
                .Entity<CorpProp.Entities.Estate.ShipKind>()
                .Entity<CorpProp.Entities.Estate.ShipType>()
                .Entity<CorpProp.Entities.Estate.SpaceShip>()
                .Entity<CorpProp.Entities.Estate.UnfinishedConstruction>()
                .Entity<CorpProp.Entities.Estate.Vehicle>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.VehicleClass)
                    .SaveOneObject(x => x.VehicleLabel)
                    .SaveOneObject(x => x.EngineType)
                ))
                .Entity<CorpProp.Entities.Estate.VehicleCategory>()
                .Entity<CorpProp.Entities.Estate.VehicleType>()
                .Entity<AdditionalFeatures>();

            #endregion CorpProp.Entities.Estate
        }
    }
}