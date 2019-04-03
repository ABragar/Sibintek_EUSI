using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Data.EF;
using Microsoft.Build.Framework;

namespace Data
{
    public partial class CorpPropConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<Data.EF.DataContext>.Instance)
            #region CorpProp
            #region CorpProp.Entities.Accounting
                        .Entity<CorpProp.Entities.Accounting.AccountingObject>()
                        .Entity<CorpProp.Entities.Accounting.AccountingObjectMoving>()                        
                        .Entity<CorpProp.Entities.Accounting.BIK>()
            #endregion
            #region CorpProp.Entities.Asset
                         .Entity<CorpProp.Entities.Asset.NonCoreAsset>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetAppraisal>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetList>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetListItem>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSale>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleAccept>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleOffer>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetStatus>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetCriteria>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetInventory>()
            #endregion
            #region CorpProp.Entities.Base
                    .Entity<CorpProp.Entities.Base.DictObject>()                   
            #endregion  
            #region CorpProp.Entities.CorporateGovernance
                        .Entity<CorpProp.Entities.CorporateGovernance.Appraisal>(e => e.Save(s => s.SaveOneObject(x => x.SibRegion)
                                                                                                    .SaveOneObject(x => x.Customer)
                                                                                                    .SaveOneObject(x => x.Owner)
                                                                                                    .SaveOneObject(x => x.Appraiser)
                                                                                                    .SaveOneToMany(x => x.NonCoreAssetAppraisals, x => x.SaveOneObject(ss => ss.Appraisal))
                                                                                                    .SaveOneObject(x => x.Deal)
                                                                                                    .SaveManyToMany(x => x.FileCards)
                                                                                                    .SaveManyToMany(x => x.Tasks)
                                                                                                    .SaveManyToMany(x => x.TaskReports)
                                                                                ))
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraisalType>()

                        .Entity<CorpProp.Entities.CorporateGovernance.EstateAppraisal>(e => e.Save(s => s.SaveOneObject(x => x.Appraisal)
                                                                                                         .SaveOneObject(x => x.AppraisalType)
                                                                                                         .SaveOneObject(x => x.EstateAppraisalType)
                                                                                                         .SaveManyToMany(x => x.Estates)
                                                                                       ))
                        .Entity<CorpProp.Entities.CorporateGovernance.IndicateEstateAppraisalView>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Investment>()
                        .Entity<CorpProp.Entities.CorporateGovernance.InvestmentType>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Predecessor>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Shareholder>()
                        .Entity<CorpProp.Entities.CorporateGovernance.SuccessionType>()
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraiserDataFinYear>()
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraisalOrgData>()
            #endregion
            #region CorpProp.Entities.Document
                        .Entity<CorpProp.Entities.Document.CardFolder>()
                        .Entity<CorpProp.Entities.Document.FileCard>()
                        .Entity<CorpProp.Entities.Document.FileCardMany>(builder => builder.Save(saver => saver.SaveManyToMany(many => many.FileOneCards)
                                                                                                               .SaveOneObject(card => card.PersonFullName)))
                        .Entity<CorpProp.Entities.Document.FileCardOne>(builder => builder.Save(saver => saver.SaveOneObject(one => one.File)
                                                                                                              .SaveOneObject(card => card.PersonFullName)))
            #endregion
            #region CorpProp.Entities.DocumentFlow
                        .Entity<CorpProp.Entities.DocumentFlow.SibDeal>(builder => builder.Save(saver => saver.SaveOneObject(deal => deal.SourseInformation)
                                                                                                              .SaveOneObject(deal => deal.DocType)
                                                                                                              .SaveManyToMany(deal => deal.TaskReports)
                                                                                                              .SaveManyToMany(deal => deal.FileCards)
                                                                                                              .SaveOneObject(deal => deal.Currency)
                                                                                                              .SaveManyToMany(deal => deal.Appraisals)
                                                                                                              .SaveOneToMany(deal => deal.NonCoreAssetSales, entrySaverDelegate => entrySaverDelegate.SaveOneObject(sale => sale.SibDeal))
                                                                                                              .SaveManyToMany(deal => deal.Tasks)))
                        .Entity<CorpProp.Entities.DocumentFlow.SibDealStatus>()
                        .Entity<CorpProp.Entities.DocumentFlow.DealParticipant>()
                        .Entity<CorpProp.Entities.DocumentFlow.Doc>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocKind>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocStatus>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocTask>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocType>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocTypeOperation>()
            #endregion
            #region CorpProp.Entities.Estate                      
                        .Entity<CorpProp.Entities.Estate.Aircraft>()
                        .Entity<CorpProp.Entities.Estate.AircraftKind>()
                        .Entity<CorpProp.Entities.Estate.AircraftType>()
                        .Entity<CorpProp.Entities.Estate.BuildingStructure>()
                        .Entity<CorpProp.Entities.Estate.Cadastral>()
                        .Entity<CorpProp.Entities.Estate.CadastralPart>()
                        .Entity<CorpProp.Entities.Estate.CadastralValue>()
                        .Entity<CorpProp.Entities.Estate.CarParkingSpace>()
                        .Entity<CorpProp.Entities.Estate.Coordinate>()
                        .Entity<CorpProp.Entities.Estate.Estate>()
                        .Entity<CorpProp.Entities.Estate.EstateDeal>()
                        .Entity<CorpProp.Entities.Estate.IntangibleAsset>()                     
                        .Entity<CorpProp.Entities.Estate.InventoryObject>()
                        .Entity<CorpProp.Entities.Estate.Land>()
                        .Entity<CorpProp.Entities.Estate.MovableEstate>()
                        .Entity<CorpProp.Entities.Estate.NonCadastral>()
                        .Entity<CorpProp.Entities.Estate.PropertyComplex>()
                        .Entity<CorpProp.Entities.Estate.RealEstate>()
                        .Entity<CorpProp.Entities.Estate.Room>()
                        .Entity<CorpProp.Entities.Estate.Ship>()
                        .Entity<CorpProp.Entities.Estate.ShipClass>()
                        .Entity<CorpProp.Entities.Estate.ShipKind>()
                        .Entity<CorpProp.Entities.Estate.ShipType>()
                        .Entity<CorpProp.Entities.Estate.SpaceShip>()
                        .Entity<CorpProp.Entities.Estate.UnfinishedConstruction>()
                        .Entity<CorpProp.Entities.Estate.Vehicle>()
                        .Entity<CorpProp.Entities.Estate.VehicleCategory>()
                        .Entity<CorpProp.Entities.Estate.VehicleType>()
            #endregion
            #region CorpProp.Entities.FIAS
                    .Entity<CorpProp.Entities.FIAS.SibAddress>()
                    .Entity<CorpProp.Entities.FIAS.SibCountry>(x=>x.Save(tt=>tt.SaveManyToMany(kk=>kk.IntangibleAssets)))
                    .Entity<CorpProp.Entities.FIAS.SibRegion>()
            #endregion

            #region Import
                    .Entity<CorpProp.Entities.Import.ImportErrorLog>()
                    .Entity<CorpProp.Entities.Import.ImportHistory>()
                    .Entity<CorpProp.Entities.Import.ImportTemplate>(x => x.Save(saver => saver.SaveOneToMany(many => many.Items)))
                    .Entity<CorpProp.Entities.Import.ImportTemplateItem>(x => x.Save(saver => saver.SaveOneObject(many => many.ImportTemplate)))

            #endregion

            #region CorpProp.Entities.Law
                        .Entity<CorpProp.Entities.Law.DuplicateRightView>()
                        .Entity<CorpProp.Entities.Law.Encumbrance>()
                        .Entity<CorpProp.Entities.Law.EncumbranceType>()
                        .Entity<CorpProp.Entities.Law.Extract>()
                        .Entity<CorpProp.Entities.Law.ExtractFormat>()
                        .Entity<CorpProp.Entities.Law.ExtractItem>()
                        .Entity<CorpProp.Entities.Law.ExtractType>()
                        .Entity<CorpProp.Entities.Law.IntangibleAssetRight>()
                        .Entity<CorpProp.Entities.Law.RegistrationBasis>()
                        .Entity<CorpProp.Entities.Law.Right>()
                        .Entity<CorpProp.Entities.Law.RightCostView>()
                        .Entity<CorpProp.Entities.Law.RightKind>()                        
                        .Entity<CorpProp.Entities.Law.RightType>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistration>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistrationRecord>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistrationStatus>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistrationType>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminate>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminateRecord>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateYear>()

            #endregion
            #region CorpProp.Entities.Mapping
                        .Entity<CorpProp.Entities.Mapping.EstateRulesCteation>()
                        .Entity<CorpProp.Entities.Mapping.AccountingEstates>(x=>x.Save(saver => saver.SaveOneObject(obj=>obj.ClassFixedAsset)))
                        .Entity<CorpProp.Entities.Mapping.Mapping>()
                        .Entity<CorpProp.Entities.Mapping.ExternalMappingSystem>()
                        .Entity<CorpProp.Entities.Mapping.OKOFEstates>(x => x.Save(saver => saver.SaveOneObject(obj => obj.OKOF2014)))
            #endregion
            #region CorpProp.Entities.NSI
                        .Entity<CorpProp.Entities.NSI.AccountingMovingType>()
                        .Entity<CorpProp.Entities.NSI.AccountingStatus>()
                        .Entity<CorpProp.Entities.NSI.ActualKindActivity>()
                        .Entity<CorpProp.Entities.NSI.AddonAttributeGroundCategory>()
                        .Entity<CorpProp.Entities.NSI.AppraisalGoal>()
                        .Entity<CorpProp.Entities.NSI.AppraisalPurpose>()
                        .Entity<CorpProp.Entities.NSI.BaseExclusionFromPerimeter>()
                        .Entity<CorpProp.Entities.NSI.BaseInclusionInPerimeter>()
                        .Entity<CorpProp.Entities.NSI.BasisForInvestments>()
                        .Entity<CorpProp.Entities.NSI.BusinessArea>()
                        .Entity<CorpProp.Entities.NSI.BusinessDirection>()
                        .Entity<CorpProp.Entities.NSI.BusinessSegment>()
                        .Entity<CorpProp.Entities.NSI.ClassFixedAsset>()
                        .Entity<CorpProp.Entities.NSI.Consolidation>()
                        .Entity<CorpProp.Entities.NSI.ContourType>()
                        .Entity<CorpProp.Entities.NSI.ContragentKind>()
                        .Entity<CorpProp.Entities.NSI.Currency>()
                        .Entity<CorpProp.Entities.NSI.DealType>()
                        .Entity<CorpProp.Entities.NSI.DepreciationGroup>()
                        .Entity<CorpProp.Entities.NSI.EstateAppraisalType>()
                        .Entity<CorpProp.Entities.NSI.ExchangeRate>(e => e.Save(s => s.SaveOneObject(x => x.Currency)))
                        .Entity<CorpProp.Entities.NSI.FeatureType>()
                        .Entity<CorpProp.Entities.NSI.GroundCategory>()
                        .Entity<CorpProp.Entities.NSI.ImplementationWay>()
                        .Entity<CorpProp.Entities.NSI.InformationSource>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetRightType>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetStatus>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetType>()
                        .Entity<CorpProp.Entities.NSI.EstateType>()
                        .Entity<CorpProp.Entities.NSI.LandType>()
                        .Entity<CorpProp.Entities.NSI.LayingType>()
                        .Entity<CorpProp.Entities.NSI.LeavingReason>()
                        .Entity<CorpProp.Entities.NSI.SibLocation>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetAppraisalType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListKind>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetSaleAcceptType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetType>()
                        .Entity<CorpProp.Entities.NSI.OKATO>()
                        .Entity<CorpProp.Entities.NSI.OKOF94>()
                        .Entity<CorpProp.Entities.NSI.OKOF2014>()
                        .Entity<CorpProp.Entities.NSI.AddonOKOF>()
                        .Entity<CorpProp.Entities.NSI.OKOFS>()
                        .Entity<CorpProp.Entities.NSI.OKOGU>()
                        .Entity<CorpProp.Entities.NSI.OKOPF>()
                        .Entity<CorpProp.Entities.NSI.OKPO>()
                        .Entity<CorpProp.Entities.NSI.OKTMO>()
                        .Entity<CorpProp.Entities.NSI.OPF>()
                        .Entity<CorpProp.Entities.NSI.OwnershipType>()
                        .Entity<CorpProp.Entities.NSI.ProductionBlock>()
                        .Entity<CorpProp.Entities.NSI.PropertyComplexKind>()
                        .Entity<CorpProp.Entities.NSI.RealEstateKind>()
                        .Entity<CorpProp.Entities.NSI.ReceiptReason>()
                        .Entity<CorpProp.Entities.NSI.RequestStatus>()
                        .Entity<CorpProp.Entities.NSI.ResponseStatus>()
                        .Entity<CorpProp.Entities.NSI.RealEstatePurpose>()
                        .Entity<CorpProp.Entities.NSI.RightHolderKind>()
                        .Entity<CorpProp.Entities.NSI.ShipAssignment>()
                        .Entity<CorpProp.Entities.NSI.SibBank>()
                        .Entity<CorpProp.Entities.NSI.SibFederalDistrict>()
                        .Entity<CorpProp.Entities.NSI.SibKBK>()
                        .Entity<CorpProp.Entities.NSI.SibMeasure>()
                        .Entity<CorpProp.Entities.NSI.SibProjectType>()
                        .Entity<CorpProp.Entities.NSI.SignType>()
                        .Entity<CorpProp.Entities.NSI.SibOKVED>()
                        .Entity<CorpProp.Entities.NSI.SocietyCategory1>()
                        .Entity<CorpProp.Entities.NSI.SocietyCategory2>()
                        .Entity<CorpProp.Entities.NSI.SourceInformation>()
                        .Entity<CorpProp.Entities.NSI.SourceInformationType>()
                        .Entity<CorpProp.Entities.NSI.StageOfCompletion>()
                        .Entity<CorpProp.Entities.NSI.StatusConstruction>()
                        .Entity<CorpProp.Entities.NSI.TaxNumberInSheet>()
                        .Entity<CorpProp.Entities.NSI.TypeData>()
                        .Entity<CorpProp.Entities.NSI.UnitOfCompany>()
                        .Entity<CorpProp.Entities.NSI.VehicleKindCode>()
                        .Entity<CorpProp.Entities.NSI.WellCategory>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListItemState>()
                        .Entity<CorpProp.Entities.NSI.DictObjectState>()
                        .Entity<CorpProp.Entities.NSI.DictObjectStatus>()
                        .Entity<CorpProp.Entities.NSI.ResponseRowState>()
            #endregion
            #region CorpProp.Entities.ProjectActivity
                        .Entity<CorpProp.Entities.ProjectActivity.SibProject>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibProjectStatus>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibProjectTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTask>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReport>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReportStatus>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskStatus>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskGanttDependency>()

            #endregion
            #region CorpProp.Entities.RosReestr
                        .Entity<CorpProp.Entities.RosReestr.AnotherSubject>()
                        .Entity<CorpProp.Entities.RosReestr.BuildRecord>()
                        .Entity<CorpProp.Entities.RosReestr.CadNumber>()
                        .Entity<CorpProp.Entities.RosReestr.CarParkingSpaceLocationInBuildPlans>()
                        .Entity<CorpProp.Entities.RosReestr.ContourOKSOut>()
                        .Entity<CorpProp.Entities.RosReestr.DealRecord>()
                        .Entity<CorpProp.Entities.RosReestr.DocumentRecord>()
                        .Entity<CorpProp.Entities.RosReestr.ExtractBuild>()
                        .Entity<CorpProp.Entities.RosReestr.ExtractLand>()
                        .Entity<CorpProp.Entities.RosReestr.IndividualSubject>()
                        .Entity<CorpProp.Entities.RosReestr.LandRecord>()
                        .Entity<CorpProp.Entities.RosReestr.LegalSubject>()
                        .Entity<CorpProp.Entities.RosReestr.NameRecord>()
                        .Entity<CorpProp.Entities.RosReestr.ObjectPartNumberRestrictions>()
                        .Entity<CorpProp.Entities.RosReestr.ObjectRecord>()
                        .Entity<CorpProp.Entities.RosReestr.OldNumber>()
                        .Entity<CorpProp.Entities.RosReestr.PermittedUse>()
                        .Entity<CorpProp.Entities.RosReestr.PublicSubject>()
                        .Entity<CorpProp.Entities.RosReestr.RestrictedRightsPartyOut>()
                        .Entity<CorpProp.Entities.RosReestr.RestrictRecord>()                       
                        .Entity<CorpProp.Entities.RosReestr.RightHolder>()
                        .Entity<CorpProp.Entities.RosReestr.RightRecord>()
                        .Entity<CorpProp.Entities.RosReestr.RightRecordNumber>()
                        .Entity<CorpProp.Entities.RosReestr.RoomLocationInBuildPlans>()
                        .Entity<CorpProp.Entities.RosReestr.SubjectRecord>()

            #endregion
            #region CorpProp.Entities.Security
                        .Entity<CorpProp.Entities.Security.SibRole>()
                        .Entity<CorpProp.Entities.Security.SibUser>(e => e.Save(s => s.SaveOneObject(x => x.Society)))
            #endregion
            #region CorpProp.Entities.Subject
                        .Entity<CorpProp.Entities.Subject.Appraiser>()
                        .Entity<CorpProp.Entities.Subject.BankingDetail>()
                        .Entity<CorpProp.Entities.Subject.Society>()
                        .Entity<CorpProp.Entities.Subject.Subject>()
                        .Entity<CorpProp.Entities.Subject.SubjectActivityKind>()
                        .Entity<CorpProp.Entities.Subject.SubjectKind>()
                        .Entity<CorpProp.Entities.Subject.SubjectType>()
            #endregion
            #endregion

            ;

            InitRequest(config);
        }
    }
}