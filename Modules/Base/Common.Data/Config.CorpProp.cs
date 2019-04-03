using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Common.Data.EF;
using CorpProp.Entities.Access;

namespace Common.Data
{
    public partial class CorpPropConfig
    {
       
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
            #region CorpProp
            #region CorpProp.Entities.Accounting
                        .Entity<CorpProp.Entities.Accounting.AccountingEstateRightView>()
                        .Entity<CorpProp.Entities.Accounting.AccountingObject>(
                ss=>ss.Save(xx => 
                    xx.SaveOneObject(x => x.ClassFixedAsset)
                    .SaveOneObject(x => x.BusinessArea)
                    .SaveOneObject(x => x.WhoUse)
                    .SaveOneObject(x => x.ReceiptReason)
                    .SaveOneObject(x => x.LeavingReason)
                    .SaveOneObject(x => x.EstateType)
                    .SaveOneObject(x => x.OKOF94)
                    .SaveOneObject(x => x.OKOF2014)
                    .SaveOneObject(x => x.OKTMO)
                    .SaveOneObject(x => x.OKTMORegion)
                    .SaveOneObject(x => x.OKATO)
                    .SaveOneObject(x => x.OKATORegion)
                    .SaveOneObject(x => x.Status)
                    .SaveOneObject(x => x.Region)
                    .SaveOneObject(x => x.RightKind)
                    .SaveOneObject(x => x.LayingType)
                    .SaveOneObject(x => x.GroundCategory)
                    .SaveOneObject(x => x.VehicleType)
                    .SaveOneObject(x => x.VehicleCategory)
                    .SaveOneObject(x => x.SibMeasure)
                    .SaveOneObject(x => x.ShipType)
                    .SaveOneObject(x => x.ShipClass)
                    .SaveOneObject(x => x.PowerUnit)
                    .SaveOneObject(x => x.LengthUnit)
                    .SaveOneObject(x => x.WidthUnit)
                    .SaveOneObject(x => x.DraughtHardUnit)
                    .SaveOneObject(x => x.DraughtLightUnit)
                    .SaveOneObject(x => x.MostHeightUnit)
                    .SaveOneObject(x => x.DeadWeightUnit)
                    .SaveOneObject(x => x.AircraftKind)
                    .SaveOneObject(x => x.AircraftType)
                    .SaveOneObject(x => x.PropertyComplex)
                    .SaveOneObject(x => x.Estate)
                    .SaveOneObject(x => x.Owner)
                    .SaveOneObject(x => x.MainOwner)
                    .SaveOneObject(x => x.RSBUAccountNumber))
                )                       
                        .Entity<CorpProp.Entities.Accounting.BIK>()
            #endregion
            #region CorpProp.Entities.Asset
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetTbl>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAsset>()                                             
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetList>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetAndListTbl>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetAndList>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSale>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleAccept>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleOffer>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetStatus>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetInventory>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetInventoryType>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetOwnerCategory>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetListState>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleStatus>()
                         .Entity<CorpProp.Entities.Asset.NonCoreAssetInventoryState>()

            

            #endregion
            #region CorpProp.Entities.Base
                    .Entity<CorpProp.Entities.Base.DictObject>()
            #endregion
            #region CorpProp.Entities.CorporateGovernance
                        .Entity<CorpProp.Entities.CorporateGovernance.Appraisal>(e => e.Save(s => s.SaveOneObject(x => x.SibRegion)
                                                                                                    .SaveOneObject(x => x.Customer)
                                                                                                    .SaveOneObject(x => x.Owner)
                                                                                                    .SaveOneObject(x => x.Appraiser)
                                                                                                    .SaveOneObject(x => x.Deal)


                                                                                ))
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraisalType>()

                        .Entity<CorpProp.Entities.CorporateGovernance.EstateAppraisal>(e => e.Save(s => s.SaveOneObject(x => x.Appraisal)
                                                                                                         .SaveOneObject(x => x.AppraisalType)
                                                                                                         .SaveOneObject(x => x.EstateAppraisalType)
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
                        .Entity<CorpProp.Entities.Document.FileCard>(builder => builder.Save(saver => saver.SaveOneObject(card => card.FileCardPermission)))
                        .Entity<CorpProp.Entities.Document.FileCardMany>(builder => builder.Save(saver => saver.SaveOneObject(card => card.PersonFullName)
                                                                                                               .SaveOneObject(card => card.FileCardPermission)))
                        .Entity<CorpProp.Entities.Document.FileCardOne>(builder => builder.Save(saver => saver.SaveOneObject(one => one.FileData)
                                                                                                              .SaveOneObject(card => card.PersonFullName)
                                                                                                              .SaveOneObject(card => card.FileCardPermission)))
                        .Entity<CorpProp.Entities.Document.FileCardPermission>()
                        .Entity<CorpProp.Entities.Document.ViewSettingsByMnemonic>()
            #endregion
            #region CorpProp.Entities.DocumentFlow
                        .Entity<CorpProp.Entities.DocumentFlow.SibDeal>(builder => builder.Save(saver => saver.SaveOneObject(deal => deal.SourseInformation)
                                                                                                              .SaveOneObject(deal => deal.DocType)
                                                                                                              .SaveOneObject(deal => deal.Currency)
                                                                                                           ))
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
                        .Entity<CorpProp.Entities.Estate.EstateImage>()
                        .Entity<CorpProp.Entities.Estate.EstateDeal>(s => s.Save(ss=>ss.SaveOneObject(w=>w.Estate).SaveOneObject(w => w.SibDeal)))
                        .Entity<CorpProp.Entities.Estate.IntangibleAsset>()
                        .Entity<CorpProp.Entities.Estate.InventoryObject>()
                        .Entity<CorpProp.Entities.Estate.Land>()                     
                        .Entity<CorpProp.Entities.Estate.MovableEstate>()
                        .Entity<CorpProp.Entities.Estate.NonCadastral>()
                        .Entity<CorpProp.Entities.Estate.PropertyComplex>()
                        .Entity<CorpProp.Entities.Estate.RealEstate>()
                        .Entity<CorpProp.Entities.Estate.RealEstateComplex>()
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
                    .Entity<CorpProp.Entities.FIAS.SibCountry>()
                    .Entity<CorpProp.Entities.FIAS.SibRegion>()
            #endregion
                     .Entity<CorpProp.Entities.History.HistoricalSettings>()
            #region Import
                    .Entity<CorpProp.Entities.Import.ImportHistory>()
                    .Entity<CorpProp.Entities.Import.ImportErrorLog>()
                    .Entity<CorpProp.Entities.Import.ImportObject>()
                    .Entity<CorpProp.Entities.Import.ImportTemplate>()
                    .Entity<CorpProp.Entities.Import.ImportTemplateItem>()
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
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminate>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminateRecord>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateYear>()

            #endregion
            #region CorpProp.Entities.Mapping
                        .Entity<CorpProp.Entities.Mapping.EstateRulesCteation>()
                        .Entity<CorpProp.Entities.Mapping.AccountingEstates>(x => x.Save(saver => saver.SaveOneObject(obj => obj.ClassFixedAsset)))
                        .Entity<CorpProp.Entities.Mapping.Mapping>()
                        .Entity<CorpProp.Entities.Mapping.ExternalMappingSystem>()
                        .Entity<CorpProp.Entities.Mapping.OKOFEstates>(x => x.Save(saver => saver.SaveOneObject(obj => obj.OKOF2014)))
                        .Entity<CorpProp.Entities.Mapping.RosReestrTypeEstate>()
            #endregion
            #region CorpProp.Entities.NSI
                        .Entity<CorpProp.Entities.NSI.AccountingMovingType>()
                        .Entity<CorpProp.Entities.NSI.AccountingStatus>()
                        .Entity<CorpProp.Entities.NSI.ActualKindActivity>()
                        .Entity<CorpProp.Entities.NSI.AddonAttributeGroundCategory>()
                        .Entity<CorpProp.Entities.NSI.AppraisalGoal>()
                        .Entity<CorpProp.Entities.NSI.AppraisalPurpose>()
                        .Entity<CorpProp.Entities.NSI.AppType>()

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
                        .Entity<CorpProp.Entities.NSI.HolidaysCalendar>()
                        .Entity<CorpProp.Entities.NSI.LandType>()
                        .Entity<CorpProp.Entities.NSI.LayingType>()
                        .Entity<CorpProp.Entities.NSI.LeavingReason>()
                        .Entity<CorpProp.Entities.NSI.SibLocation>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetAppraisalType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListKind>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetSaleAcceptType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetType>()
                        .Entity<CorpProp.Entities.NSI.NSI>()
                        .Entity<CorpProp.Entities.NSI.NSIType>()
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
                        .Entity<CorpProp.Entities.NSI.RSBU>()
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
                        .Entity<CorpProp.Entities.NSI.SocietyDept>()
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
                        //.Entity<CorpProp.Entities.ProjectActivity.SibProjectTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTask>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReport>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReportStatus>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskStatus>()
                        //.Entity<CorpProp.Entities.ProjectActivity.SibTaskTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskGanttDependency>()
            #endregion
            #region CorpProp.RosReestr.Entities
                        .Entity<CorpProp.RosReestr.Entities.AnotherSubject>()
                        .Entity<CorpProp.RosReestr.Entities.BuildRecord>()
                        .Entity<CorpProp.RosReestr.Entities.CadNumber>()
                        .Entity<CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans>()
                        .Entity<CorpProp.RosReestr.Entities.ContourOKSOut>()
                        .Entity<CorpProp.RosReestr.Entities.DealRecord>()
                        .Entity<CorpProp.RosReestr.Entities.DocumentRecord>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractBuild>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractLand>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractObject>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractSubj>()
                        .Entity<CorpProp.RosReestr.Entities.Governance>()
                        .Entity<CorpProp.RosReestr.Entities.IndividualSubject>()
                        .Entity<CorpProp.RosReestr.Entities.LandRecord>()
                        .Entity<CorpProp.RosReestr.Entities.LegalSubject>()
                        .Entity<CorpProp.RosReestr.Entities.NameRecord>()
                        .Entity<CorpProp.RosReestr.Entities.Notice>()
                        .Entity<CorpProp.RosReestr.Entities.NoticeSubj>()
                        .Entity<CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions>()
                        .Entity<CorpProp.RosReestr.Entities.ObjectRecord>()
                        .Entity<CorpProp.RosReestr.Entities.OldNumber>()
                        .Entity<CorpProp.RosReestr.Entities.Organization>()
                        .Entity<CorpProp.RosReestr.Entities.PermittedUse>()
                        .Entity<CorpProp.RosReestr.Entities.PublicSubject>()
                        .Entity<CorpProp.RosReestr.Entities.Refusal>()
                        .Entity<CorpProp.RosReestr.Entities.RefusalSubj>()
                        .Entity<CorpProp.RosReestr.Entities.RestrictedRightsPartyOut>()
                        .Entity<CorpProp.RosReestr.Entities.RestrictRecord>()
                        .Entity<CorpProp.RosReestr.Entities.RightHolder>()
                        .Entity<CorpProp.RosReestr.Entities.RightRecord>()
                        .Entity<CorpProp.RosReestr.Entities.RightRecordNumber>()
                        .Entity<CorpProp.RosReestr.Entities.RoomLocationInBuildPlans>()
                        .Entity<CorpProp.RosReestr.Entities.SubjectRecord>()
                        .Entity<CorpProp.RosReestr.Entities.SubjRight>()
                        .Entity<CorpProp.RosReestr.Entities.TPerson>()

                        .Entity<CorpProp.RosReestr.Migration.MigrateHistory>()
                        .Entity<CorpProp.RosReestr.Migration.MigrateLog>(e=>e.Save(s=>s.SaveOneObject(o=>o.MigrateHistory)
                                                                                        .SaveOneObject(o=>o.MigrateState)))
                        .Entity<CorpProp.RosReestr.Migration.MigrateState>()

            #endregion
            #region CorpProp.Entities.Security
                        .Entity<CorpProp.Entities.Security.SibRole>()
                        .Entity<CorpProp.Entities.Security.SibUser>(e => e.Save(s => s.SaveOneObject(x => x.Society)))
                        .Entity<CorpProp.Entities.Security.ObjectPermission>(e => e.Save(s => s
                        .SaveOneObject(x => x.TypePermission)
                        .SaveOneObject(x =>x .Role)))
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

            #region CorpProp.Entities.Settings
                        .Entity<CorpProp.Entities.Settings.SibNotification>()
            #endregion
            
            #region  SibPermission
            .Entity<SibPermission>()
            #endregion

            #endregion

            #region ManyToMany
                .Entity<CorpProp.Entities.ManyToMany.CadastralAndExtract>()
                .Entity<CorpProp.Entities.ManyToMany.EstateAndEstateAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndCertificateRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndDoc>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndExtract>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndLegalRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndNonCoreAsset>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndRequestContent>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndResponse>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateYear>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateRegistrationRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateTerminateRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardOneAndFileCardMany>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndSibProject>()

                .Entity<CorpProp.Entities.ManyToMany.IntangibleAssetAndSibCountry>()


                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndRight>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndSibUser>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndRight>()

                .Entity<CorpProp.Entities.ManyToMany.RequestAndSociety>()
                .Entity<CorpProp.Entities.ManyToMany.ResponseAndSibUser>(s => s.Save(os => os.SaveOneObject(obj=>obj.ObjLeft).SaveOneObject(obj => obj.ObjRigth)))

                .Entity<CorpProp.Entities.Asset.NonCoreAssetListItemAndNonCoreAssetSaleAccept>()               

                .Entity<CorpProp.Entities.ManyToMany.SibUserTerritory>()

            #endregion
            ;

            InitRequest<TContext>(config);
        }
    }
}