using Base;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Entities;
using Base.Links.Entities;
using Base.Reporting;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Settings;
using Base.UI;
using Base.UI.Service;
using Base.UI.ViewModal;
using CorpProp.DefaultData;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Export;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.History;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Settings;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using CorpProp.Services.Estate;
using CorpProp.Services.Mapping;
using CorpProp.Services.Security;
using CorpProp.Services.Subject;
using System;
using System.Collections.Generic;
using System.Linq;

using bbase = Base;
using SubjectObject = CorpProp.Entities.Subject;

namespace CorpProp
{
    /// <summary>
    /// Представляет инициализатор модуля CorpProp.
    /// </summary>
    public class Initializer : bbase.IModuleInitializer
    {
        private readonly IPresetRegistorService _presetRegistorService;
        private readonly ISettingService<AppSetting> _appSettingService;
        private readonly ISettingService<ReportingSetting> _reportingSettingService;
        private readonly ILoginProvider _loginProvider;
        private readonly ILinkBuilder _linkBuilder;
        private readonly IImageSettingService _imageSettingService;
        private readonly IViewModelConfigService _viewModelConfigService;

        public Initializer(IPresetRegistorService presetRegistorService, ISettingService<AppSetting> appSettingService,
            ILoginProvider loginProvider, ILinkBuilder linkBuilder,
            ISettingService<ReportingSetting> reportingSettingService, IImageSettingService imageSettingService
            , IViewModelConfigService viewModelConfigService)
        {
            _presetRegistorService = presetRegistorService;
            _appSettingService = appSettingService;
            _loginProvider = loginProvider;
            _linkBuilder = linkBuilder;
            _reportingSettingService = reportingSettingService;
            _imageSettingService = imageSettingService;
            _viewModelConfigService = viewModelConfigService;
        }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public Dictionary<string, object> sibProfileUser = new Dictionary<string, object>();

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="context"></param>
        public void Init(bbase.IInitializerContext context)
        {
            #region Funcs

            //Получает текущего пользователя в указанной сессии
            Func<IUnitOfWork, SibUser> getCurrentSibUser = (unitOfWork) =>
            {
                //var currentUserID = Base.Ambient.AppContext.SecurityUser.ID;
                var profileUserID = Base.Ambient.AppContext.SecurityUser.ProfileInfo?.ID;
                if (profileUserID != null)
                {
                    var currentSibUser = unitOfWork.GetRepository<SibUser>().Find(sUser => sUser.ID == profileUserID);
                    return currentSibUser;
                }
                else
                    return null;
            };

            //Получает текущего пользователя в указанной сессии
            Func<IUnitOfWork, bool> setSibProfileCurrentUser = (unitOfWork) =>
            {
                //var currentUserID = Base.Ambient.AppContext.SecurityUser.ID;
                var profileUserID = Base.Ambient.AppContext.SecurityUser.ProfileInfo?.ID;
                if (profileUserID != null)
                {
                    var currentSibUser = unitOfWork.GetRepository<SibUser>().Find(sUser => sUser.UserID == profileUserID);
                    sibProfileUser = new Dictionary<string, object>
                                     {
                                         {"ID", currentSibUser.ID},
                                         {"SibUser", currentSibUser},
                                         {"User", currentSibUser.User},
                                         {"UserID", currentSibUser.UserID},
                                         {"Image", currentSibUser.Image},
                                         {"IsActiv", currentSibUser.IsActiv},
                                         {
                                             "IsAdministrative",
                                             currentSibUser.IsAdministrative
                                         },
                                         {"LastName", currentSibUser.LastName},
                                         {"FirstName", currentSibUser.FirstName},
                                         {"MiddleName", currentSibUser.MiddleName},
                                         {"FullName", currentSibUser.FullName},
                                         {
                                             "FullNameAndOrg", currentSibUser.FullNameAndOrg
                                         },
                                         {"Boss", currentSibUser.Boss},
                                         {"BossID", currentSibUser.BossID},
                                         {"DeptName", currentSibUser.SocietyDeptName},
                                         {"PostName", currentSibUser.PostName},
                                         {"Society", currentSibUser.Society},
                                         {"SocietyID", currentSibUser.SocietyID},
                                         {"SocietyName", currentSibUser.SocietyName},
                                         {"Mobile", currentSibUser.Mobile},
                                         {"Phone", currentSibUser.Phone},
                                         {"Email", currentSibUser.Email},
                                         {"Vice", currentSibUser.Vice}
                                     };

                    return true;
                }
                else
                    return false;
            };

            //Получает ОГ от текущего пользователя в указанной сессии
            Func<IUnitOfWork, Society> getSocietyCurrentSibUser = (unitOfWork) =>
            {
                var currentSibUser = getCurrentSibUser(unitOfWork);
                if (currentSibUser != null && currentSibUser.Society != null)
                {
                    var societyCurrentSibUser = unitOfWork.GetRepository<Society>()
                    .Find(societySibUser => societySibUser.ID == currentSibUser.SocietyID);
                    return societyCurrentSibUser;
                }
                else
                    return null;
            };

            //Получает Наименование подразделения текущего пользователя в указанной сессии
            Func<IUnitOfWork, string> getDeptNameCurrentSibUser = (unitOfWork) =>
            {
                var currentSibUser = getCurrentSibUser(unitOfWork);
                if (currentSibUser != null)
                    return currentSibUser.SocietyDeptName;
                else
                    return "";
            };

            //Получает статус перечня ННА по указанному коду в указанной сессии
            Func<IUnitOfWork, string, NonCoreAssetListState> getStateByCodeNonCoreAssetList = (unitOfWork, stateCode) =>
            {
                var nonCorAssetListState = unitOfWork.GetRepository<NonCoreAssetListState>()
                .Filter(state => state != null && !state.Hidden && !state.IsHistory && state.Code == stateCode)
                .FirstOrDefault();
                return nonCorAssetListState;
            };

            #endregion Funcs

            #region Accounting

            context.CreateVmConfig<AccountingEstateRightView>()
                .Title("Основные характеристики ОБУ ОИ Росреестр")
                .IsReadOnly();

            context.CreateVmConfig<AccountingObject>()
                .Service<IAccountingObjectService>()
                .Title("Объект бухгалтерского учета")
                .IsReadOnly()
                .ListView(x => x.Title("Объекты бухгалтерского учета")
                        .IsMultiSelect(true)
#if DEBUG
                //.HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#else
   .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#endif

                )
                .LookupProperty(x => x.Text(t => t.Name))
                .DetailView(x => x.Title("Объект бухгалтерского учета")
                    .Editors(edt => edt
                        .AddOneToManyAssociation<CadastralValue>("AccountingObject_CadastralValue",
                            editor => editor
                                .TabName("[002]Стоимость")
                                .Title("Кадастровая стоимость")
                                .IsReadOnly(true)
                                .IsLabelVisible(true)
                                .Order(42)
                                .Filter((uofw, q, id, oid) =>
                                    q.Where(w => w.Cadastral != null
                                    //&& w.Cadastral.AccountingObjects.Any(s => s.ID == id)
                                    )
                                ))
                        .AddOneToManyAssociation<FileCard>("AccountingObject_FileCard1",
                            editor => editor
                                .TabName("[002]Стоимость")
                                .IsReadOnly(true)
                                .Title("Реквизиты отчета об оценке")
                                .IsLabelVisible(true)
                                .Order(43)
                                .Filter((uofw, q, id, oid) =>
                                        //TODO: Реквизиты отчета об оценке
                                        q.Where(w => id == -1))
                        )
                        .AddOneToManyAssociation<Right>("AccountingObject_Right",
                            editor => editor
                                .TabName("[011]Ссылки")
                                .IsReadOnly(true)
                                .Title("Права")
                                .IsLabelVisible(true)
                                .Order(138)
                                .FilterExtended((uofw, q, id, oid) =>
                                {
                                    var curr = uofw.GetRepository<AccountingObject>().Find(id);

                                    if (curr.Estate != null)
                                    {
                                        var items = uofw.GetRepository<Estate>()
                                            .Filter(ff => !ff.Hidden && ff.ID == curr.EstateID);

                                        return items.Join(q, e => e.ID, o => o.EstateID, (e, o) => o);
                                    }
                                    else
                                        return q.Where(w => w.EstateID == -1);
                                }
                                ))
                        .AddOneToManyAssociation<FileCard>("AccountingObject_FileCard2",
                            editor => editor
                                .TabName("[011]Ссылки")
                                .IsReadOnly(true)
                                .Title("Документы")
                                .IsLabelVisible(true)
                                .Order(139)
                                .Filter((uofw, q, id, oid) =>
                                        //TODO: Документы
                                        q.Where(w => w.ID == -1)
                                )
                        )
                        .AddOneToManyAssociation<Extract>("AccountingObject_Extract",
                            editor => editor
                                .TabName("[011]Ссылки")
                                .IsReadOnly(true)
                                .Title("Выписки ЕГРН")
                                .IsLabelVisible(true)
                                .Order(140)
                                .Filter((uofw, q, id, oid) =>
                                        //TODO: Выписки ЕГРН
                                        q.Where(w => w.ID == -1)
                                )
                        )
                    )
                );
            //.ImportConfirmName("БЕ", "БЕ");

            context.CreateVmConfigOnBase<AccountingObject>(nameof(AccountingObject), "IsRealEstateOBU").IsReadOnly()
                .ListView(lv => lv
                .Title("Спорные ОБУ")
                .DataSource(ds => ds
                    .Filter(f => f.IsRealEstate)
                ))
                ;

            context.CreateVmConfigOnBase<AccountingObject>(nameof(AccountingObject), "DisputeOBU").IsReadOnly()
                .ListView(lv => lv
                    .Title("Спорные ОБУ")
                    .DataSource(ds => ds
                        .Filter(f => f.IsDispute)
                    ))
                ;

            #endregion Accounting

            #region Asset

            context.CreateVmConfig<NonCoreAsset>()
              .Service<INonCoreAssetService>()
              .Title("Непрофильный или неэффективный актив")
              .ListView(x => x.Title("Непрофильный или неэффективный актив")
                .IsMultiSelect(true))
              .DetailView(x => x.Title("Непрофильный или неэффективный актив").Editors(edt => edt
                    //закладка "Характеристики Актива" карточки ННА

                    .Add(ed => ed.AssetOwner, ac => ac.Order(1).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.EstateObject, ac => ac.Order(2).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NonCoreAssetOwnerCategory, ac => ac.Order(3).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.AssetUser, ac => ac.Order(4).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NameAsset, ac => ac.Order(5).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.Supervising, ac => ac.Order(6).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NonCoreAssetType, ac => ac.Order(7).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NnamePropertyPartComplex, ac => ac.Order(8).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.ImplementationWay, ac => ac.Order(9).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NonCoreAssetStatus, ac => ac.Order(12).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.NonCoreAssetNameComplex, ac => ac.Order(13).Group("Основные данные").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.AssetMainOwner, ac => ac.Order(4).Group("Основные данные").TabName("[1]Характеристики актива"))

                    .Add(ed => ed.InventoryNumber, ac => ac.Order(12).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.PropertyCharacteristics, ac => ac.Order(13).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.EGRNNumber, ac => ac.Order(14).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.CadastralNumber, ac => ac.Order(15).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .AddEmpty(ac => ac.Order(16).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .AddEmpty(ac => ac.Order(17).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.LocationAssetText, ac => ac.Order(18).Group("Характеристики объекта").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.EncumbranceText, ac => ac.Order(19).Group("Характеристики объекта").TabName("[1]Характеристики актива"))

                    .Add(ed => ed.LeavingDate, ac => ac.Order(20).Group("Дополнительно").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.TaskNumber, ac => ac.Order(21).Group("Дополнительно").TabName("[1]Характеристики актива"))
                    .AddEmpty(ac => ac.Order(21).Group("Дополнительно").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.LeavingReason, ac => ac.Order(22).Group("Дополнительно").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.TaskDate, ac => ac.Order(23).Group("Дополнительно").TabName("[1]Характеристики актива"))
                    .Add(ed => ed.AppraisalAssignment, ac => ac.Order(24).Group("Дополнительно").TabName("[1]Характеристики актива"))

                   //закладка "Финансовая информация" карточки ННА
                   .Add(ed => ed.InitialCost, ac => ac.Order(17).Group("Стоимость по БУ").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(18).Group("Стоимость по БУ").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(19).Group("Стоимость по БУ").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.ResidualCost, ac => ac.Order(20).Group("Стоимость по БУ").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.AnnualCostContentWithoutVAT, ac => ac.Order(21).Group("Годовые затраты\\выручка").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(22).Group("Годовые затраты\\выручка").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(23).Group("Годовые затраты\\выручка").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.AnnualRevenueFromUseWithoutVAT, ac => ac.Order(24).Group("Годовые затраты\\выручка").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.IndicativeValuationWithoutVAT, ac => ac.Order(25).Group("Индикативная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeIndicateCosts(form, isChange);"))
                   .AddEmpty(ac => ac.Order(26).Group("Индикативная стоимость").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(27).Group("Индикативная стоимость").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.IndicativeValuationIncludingVAT, ac => ac.Order(28).Group("Индикативная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeIndicateCosts(form, isChange);"))
                   .AddEmpty(ac => ac.Order(29).Group("Индикативная стоимость").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(30).Group("Индикативная стоимость").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.IndicativeVAT, ac => ac.Order(31).Group("Индикативная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeIndicateCosts(form, isChange);"))
                   .Add(ed => ed.MarketValuationWithoutVAT, ac => ac.Order(32).Group("Рыночная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeMarketCosts(form, isChange);"))
                   .AddEmpty(ac => ac.Order(33).Group("Рыночная стоимость").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(34).Group("Рыночная стоимость").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.MarketValuationIncludingVAT, ac => ac.Order(35).Group("Рыночная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeMarketCosts(form, isChange);"))
                   .AddEmpty(ac => ac.Order(36).Group("Рыночная стоимость").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(37).Group("Рыночная стоимость").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.MarketValuationVAT, ac => ac.Order(38).Group("Рыночная стоимость").TabName("[2]Финансовая информация").OnClientEditorChange(@" corpProp.dv.editors.onChange.NNA_ChangeMarketCosts(form, isChange);"))
                   .Add(ed => ed.BudgetProposedProcedure, ac => ac.Order(39).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.BiddingOrganizersBenefits, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.NonCoreAsset_SumBudget(form, isChange);").Order(40).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(41).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.PublicationExpense, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.NonCoreAsset_SumBudget(form, isChange);").Order(42).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.OtherExpenses, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.NonCoreAsset_SumBudget(form, isChange);").Order(43).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .AddEmpty(ac => ac.Order(44).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))
                   .Add(ed => ed.AppraisalExpense, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.NonCoreAsset_SumBudget(form, isChange);").Order(45).Group("Бюджет процедуры").TabName("[2]Финансовая информация"))

                   //закладка "Реализация ННА" карточки ННА
                   .AddOneToManyAssociation<NonCoreAssetSale>("NonCoreAsset_Sale",
                         y => y.TabName("[6]Реализация ННА")
                         .Title("Реализация ННА")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.NonCoreAsset = uofw.GetRepository<NonCoreAsset>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.NonCoreAsset = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.NonCoreAssetID == id && !w.Hidden)))
                   .AddOneToManyAssociation<Appraisal>("NonCoreAsset_Appraisal",
                        y => y.TabName("[7]Оценки")
                            .Title("Оценки")
                            .IsLabelVisible(false)
                            .IsReadOnly()
                            .FilterExtended((uofw, q, id, oid) =>
                            {
                                return uofw.GetRepository<NonCoreAsset>().Filter(f => f.ID == id && !f.Hidden)
                                    .Join(uofw.GetRepository<AccountingObject>().Filter(f => !f.Hidden),
                                        e => e.EstateObjectID, o => o.EstateID, (e, o) => o)
                                    .Join(uofw.GetRepository<EstateAppraisal>().Filter(f => !f.Hidden), e => e.ID,
                                        o => o.AccountingObjectID, (e, o) => o)
                                    .Join(uofw.GetRepository<Appraisal>().Filter(f => !f.Hidden), e => e.AppraisalID,
                                        o => o.ID, (e, o) => o);
                            }))
                   .AddManyToManyRigthAssociation<FileCardAndNonCoreAsset>("FileCardAndNonCoreAsset", y => y.TabName("[8]Документы"))
                   .AddManyToManyLeftAssociation<NonCoreAssetAndList>("NonCoreAssetAndList", y => y.TabName("[8]Перечни"))
              )
              .DefaultSettings((uow, r, commonEditorViewModel) =>
              {
                  if (r.EstateObject != null) //Если атрибут "Объект имущества" заполнен
                  {
                      commonEditorViewModel.ReadOnly(nca => nca.EstateObject, true); // Cделать поле “Объектимущества” доступным только на чтение
                      commonEditorViewModel.ReadOnly(nca => nca.AssetMainOwner, true); // Cделать поле “Собственник актива” доступным только на чтение
                      commonEditorViewModel.ReadOnly(nca => nca.AssetOwner, true);
                  }

                  if ((r.IndicativeValuationWithoutVAT != null || r.IndicativeValuationIncludingVAT != null || r.IndicativeVAT != null)
                        && r.MarketValuationWithoutVAT == null && r.MarketValuationIncludingVAT == null && r.MarketValuationVAT == null
                  )
                  {
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationWithoutVAT);
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationIncludingVAT);
                      commonEditorViewModel.Required(rl => rl.IndicativeVAT);

                      commonEditorViewModel.Required(rl => rl.MarketValuationWithoutVAT, false);
                      commonEditorViewModel.Required(rl => rl.MarketValuationIncludingVAT, false);
                      commonEditorViewModel.Required(rl => rl.MarketValuationVAT, false);
                  }
                  else if ((r.MarketValuationWithoutVAT != null || r.MarketValuationIncludingVAT != null || r.MarketValuationVAT != null)
                        && r.IndicativeValuationWithoutVAT == null && r.IndicativeValuationIncludingVAT == null && r.IndicativeVAT == null
                        )
                  {
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationWithoutVAT, false);
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationIncludingVAT, false);
                      commonEditorViewModel.Required(rl => rl.IndicativeVAT, false);

                      commonEditorViewModel.Required(rl => rl.MarketValuationWithoutVAT);
                      commonEditorViewModel.Required(rl => rl.MarketValuationIncludingVAT);
                      commonEditorViewModel.Required(rl => rl.MarketValuationVAT);
                  }
                  else if (
                  r.IndicativeValuationWithoutVAT == null
                  && r.IndicativeValuationIncludingVAT == null
                  && r.IndicativeVAT == null
                  && r.MarketValuationWithoutVAT == null
                  && r.MarketValuationIncludingVAT == null
                  && r.MarketValuationVAT == null
                  )
                  {
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationWithoutVAT);
                      commonEditorViewModel.Required(rl => rl.IndicativeValuationIncludingVAT);
                      commonEditorViewModel.Required(rl => rl.IndicativeVAT);

                      commonEditorViewModel.Required(rl => rl.MarketValuationWithoutVAT);
                      commonEditorViewModel.Required(rl => rl.MarketValuationIncludingVAT);
                      commonEditorViewModel.Required(rl => rl.MarketValuationVAT);
                  }
              }))
              .LookupProperty(x => x.Text(t => t.NonCoreAssetName));

            context.CreateVmConfig<NonCoreAssetAndList>("NonCoreAssetAndList")
                .Service<INonCoreAssetListItemService>()
                .Title("Строка перечня ННА")
                .ListView(x => x.Title("Строки перечня ННА")
                    .IsMultiSelect(true))
                .DetailView(x => x.Title("Строка перечня ННА")
                    .Editors(edt => edt.Clear()
                        .Add(ed => ed.NonCoreAssetListItemState, ac => ac.Order(1).Mnemonic("NNAItemStates").OnChangeClientScript(@" corpProp.dv.editors.onChange.NNARow_State(form, isChange);"))
                        .Add(ed => ed.Offer, ac => ac.Order(2))
                        .Add(ed => ed.NoticeOG, ac => ac.Order(3))
                        .Add(ed => ed.NoticeCauk, ac => ac.Order(4))

                        .Add(ed => ed.ResidualCostAgreement, ac => ac.TabName("Стоимость").Group("{2}Согласовано Куратором").Order(1))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}Согласовано Куратором").Order(2))
                        .Add(ed => ed.ResidualCostDateAgreement, ac => ac.TabName("Стоимость").Group("{2}Согласовано Куратором").Order(3))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}Согласовано Куратором").Order(4))

                        .Add(ed => ed.ResidualCostMatching, ac => ac.TabName("Стоимость").Group("{2}В момент направления на согласование Кураторам").Order(5))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}В момент направления на согласование Кураторам").Order(6))
                        .Add(ed => ed.ResidualCostDateMatching, ac => ac.TabName("Стоимость").Group("{2}В момент направления на согласование Кураторам").Order(7))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}В момент направления на согласование Кураторам").Order(8))

                        .Add(ed => ed.ResidualCostStatement, ac => ac.TabName("Стоимость").Group("{2}Утверждение").Order(9))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}Утверждение").Order(10))
                        .Add(ed => ed.ResidualCostDateStatement, ac => ac.TabName("Стоимость").Group("{2}Утверждение").Order(11))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}Утверждение").Order(12))

                        .Add(ed => ed.ResidualCost, ac => ac.TabName("Стоимость").Group("{2}На последнюю отчетную дату").Order(13))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}На последнюю отчетную дату").Order(14))
                        .Add(ed => ed.InitialCost, ac => ac.TabName("Стоимость").Group("{2}На последнюю отчетную дату").Order(15))
                        .AddEmpty(ac => ac.TabName("Стоимость").Group("{2}На последнюю отчетную дату").Order(16))
                        .Add(ed => ed.BURegDate, ac => ac.TabName("Стоимость").Group("{2}На последнюю отчетную дату").Order(17))

                        .AddManyToManyLeftAssociation<NonCoreAssetListItemAndNonCoreAssetSaleAccept>("NonCoreAssetListItemAndNonCoreAssetSaleAccept", y => y.TabName("[1]Одобрения реализации ННА"))
                        )
                        .DefaultSettings((uow, r, commonEditorViewModel) =>
                        {
                            commonEditorViewModel.ReadOnly(nca => nca.ResidualCost);
                            commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDate);

                            if (Base.Ambient.AppContext.SecurityUser.IsFromCauk(uow))
                                commonEditorViewModel.ReadOnly(nca => nca.NoticeOG);
                            else
                                commonEditorViewModel.ReadOnly(nca => nca.NoticeCauk);

                            var ncaList = uow.GetRepository<NonCoreAssetList>().Find(f => f.ID == r.ObjRigthId);

                            var rstate = uow.GetRepository<NonCoreAssetListItemState>().Find(f => f.ID == r.NonCoreAssetListItemStateID);

                            if (rstate?.Code == "109")
                                commonEditorViewModel.Required(rl => rl.NoticeCauk);
                            else
                                commonEditorViewModel.Required(rl => rl.NoticeCauk, false);

                            if (ncaList?.NonCoreAssetListStateID != null)
                            {
                                var state = uow.GetRepository<NonCoreAssetListState>()
                                    .Find(f => f.ID == ncaList.NonCoreAssetListStateID);

                                if (state.Code == "105")
                                    commonEditorViewModel.SetReadOnlyAll();

                                //статус перечня = на доработке в ОГ
                                if (state.Code == "106")
                                {
                                    //статус строки != одобрено И пользователь не ЦАУК
                                    if (r.NonCoreAssetListItemStateID != null)
                                    {
                                        var st = uow.GetRepository<NonCoreAssetListItemState>()
                                                    .Find(f => f.ID == r.NonCoreAssetListItemStateID);
                                        if (st != null && st.Code != "108" && !bbase.Ambient.AppContext.SecurityUser.IsFromCauk(uow))
                                            commonEditorViewModel.SetReadOnlyAll(true);
                                    }
                                }
                            }

                            if (r.ResidualCostAgreement != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCostAgreement);
                            if (r.ResidualCostDateAgreement != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDateAgreement);
                            if (r.ResidualCostStatement != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCostStatement);
                            if (r.ResidualCostDateStatement != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDateStatement);

                            if (r.ObjRigth != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ObjRigth, false);
                            if (r.ObjLeft != null)
                                commonEditorViewModel.ReadOnly(nca => nca.ObjLeft, false);
                        })
                        )
                .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndList", "NonCoreAssetAndListMapping")
                    .Service<INonCoreAssetListItemService>()
                    .Title("Строка перечня ННА")
                    .ListView(x => x.Title("Строки перечня ННА")
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit })
                        .IsMultiSelect(true))
                    .DetailView(x => x.Title("Строка перечня ННА")
                        .Editors(edt => edt
                            .AddManyToManyLeftAssociation<NonCoreAssetAndList, NonCoreAssetListItemAndNonCoreAssetSaleAccept>("NonCoreAssetListItemAndNonCoreAssetSaleAccept", y => y.Mnemonic("NonCoreAssetAndListMapping").TabName("[1]Одобрения реализации ННА"))
                             )
                            .DefaultSettings((uow, r, commonEditorViewModel) =>
                            {
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCost);
                                commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDate);

                                if (r.ResidualCostAgreement != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ResidualCostAgreement);
                                if (r.ResidualCostDateAgreement != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDateAgreement);
                                if (r.ResidualCostStatement != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ResidualCostStatement);
                                if (r.ResidualCostDateStatement != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ResidualCostDateStatement);

                                if (r.ObjRigth != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ObjRigth, false);
                                if (r.ObjLeft != null)
                                    commonEditorViewModel.ReadOnly(nca => nca.ObjLeft, false);
                            })
                            )
                    .LookupProperty(x => x.Text(t => t.ID));

            //context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndList", "NonCoreAssetAndListMenu")
            //    .ListView(lv => lv
            //    .DataSource(ds => ds.Groups(gr => gr.Add(g => g.ObjRigth)))
            //    .Columns(col => col.Add(c => c.ObjRigth, ac => ac.Title("Перечень").Visible(true).Order(1))))
            //    ;

            context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndListMapping", "NonCoreAssetAndListMenu")
                .ListView(lv => lv
                .DataSource(ds => ds.Groups(gr => gr.Add(g => g.ObjRigth)))
                .Columns(col => col.Add(c => c.ObjRigth, ac => ac.Title("Перечень").Visible(true).Order(1))))
                ;

            context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndList", "NonCoreAssetAndListExcluded")
                .ListView(lv => lv
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit, LvAction.Link, LvAction.Unlink })
                    .DataSource(ds => ds.Filter(f => (f.NonCoreAssetListItemState != null) && (f.NonCoreAssetListItemState.Code == "109") && !f.Hidden))
                )
                ;

            context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndList", "NonCoreAssetAndListPreviousPeriod")
                .ListView(lv => lv
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit, LvAction.Link, LvAction.Unlink })
                    .DataSource(ds => ds.Filter(f => !f.Hidden))
                )
                ;

            context.CreateVmConfigOnBase<NonCoreAssetAndList>("NonCoreAssetAndList", "NonCoreAssetAndListRO")
              .Service<INonCoreAssetListItemService>()
              .Title("Строка перечня ННА")
              .ListView(x => x.Title("Строки перечня ННА")
                  .IsMultiSelect(true)
                  .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit, LvAction.Link, LvAction.Unlink })
                  .DataSource(ds => ds.Filter(f => !f.Hidden))
                  .Columns(cols => cols.Add(c => c.InventoryNumber, ac => ac.Visible(true))))
              .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfigOnBase<NonCoreAsset>("NonCoreAsset", "NCAList_NonCoreAssets")
               .ListView(lv => lv
               .Columns(col => col.Clear()
               .Add(c => c.ID, ac => ac.Visible(false).Order(0).Visible(true))
               .Add(c => c.AssetMainOwner, ac => ac.Title("Наименование собственника Актива").Order(1).Visible(true))
               .Add(c => c.NonCoreAssetName, ac => ac.Title("Наименование Актива/Комплекса (производственная база, нефтебаза, АЗС и т.д.)").Order(2).Visible(true))
               .Add(c => c.NnamePropertyPartComplex, ac => ac.Title("Наименование недвижимости, входящей в состав комплекса в соответствии с правоустанавливающими документами (в соответствии с утвержденным реестром)").Order(5).Visible(true))
               .Add(c => c.NameAsset, ac => ac.Title("Наименование ОС в соответствии с БУ").Order(6).Visible(true))
               .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер по БУ").Order(7).Visible(true))
               .Add(c => c.LocationAssetText, ac => ac.Title("Местонахождение Актива").Order(8).Visible(true))
               .Add(c => c.PropertyCharacteristics, ac => ac.Title("Характеристики объекта* (площадь, протяженность)").Order(9).Visible(true))
               .Add(c => c.EncumbranceText, ac => ac.Title("Ограничение / обременение").Order(10).Visible(true))
               .Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость (тыс. руб.)").Order(11).ClientTemplate("#= data.InitialCost != null ? kendo.toString(data.InitialCost/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.ResidualCost, ac => ac.Title("Балансовая (остаточная) стоимость (тыс. руб.)").Order(12).ClientTemplate("#= data.ResidualCost != null ? kendo.toString(data.ResidualCost/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.AnnualRevenueFromUseWithoutVAT, ac => ac.Title("Годовая выручка от использования без учета НДС (тыс. руб.)").Order(13).ClientTemplate("#= data.AnnualRevenueFromUseWithoutVAT != null ? kendo.toString(data.AnnualRevenueFromUseWithoutVAT/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.AnnualCostContentWithoutVAT, ac => ac.Title("Годовые затраты на содержание актива без учета НДС (тыс. руб.)").Order(14).ClientTemplate("#= data.AnnualCostContentWithoutVAT != null ? kendo.toString(data.AnnualCostContentWithoutVAT/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.IndicativeValuationWithoutVAT, ac => ac.Title("Индикативная оценка без учета НДС (тыс. руб.)").Order(15).ClientTemplate("#= data.IndicativeValuationWithoutVAT != null ? kendo.toString(data.IndicativeValuationWithoutVAT/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.MarketValuationWithoutVAT, ac => ac.Title("Рыночная оценка Актива без учета НДС (тыс. руб.)").Order(16).ClientTemplate("#= data.MarketValuationWithoutVAT != null ? kendo.toString(data.MarketValuationWithoutVAT/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.SpecialNonCoreAssetCriteria, ac => ac.Title(@"Критерии отнесения к непрофильному/неэффективному").Order(17).ClientTemplate("#= data.StrategicDiscrepancy ? 'Несоответствие стратегии развития Компании;' : '' # + #=data.NonCoreBusiness ? 'Осуществление непрофильных видов деятельности;' : '' #+ #=data.InexpedientInvestment ? 'Нецелесообразность инвестиций;' : '' #+ #=data.PreservationOfCompetitiveAdvantages ? 'Сохранение конкурентных преимуществ при отсутствии объекта в портфеле активов;' : '' #+ #=data.LackOfStrategicGoals ? 'Отсутствие стратегических интересов;' : '' #+ #=data.WorthSelling ? 'Ожидание рыночной цены за объект выше, чем экономический эффект от инвестиций;' : '' #+ #=data.InexpedientMaintence ? 'Необходимость отвлечения управленческих ресурсов Компании;' : '' #+ #=data.ReputationLoss ? 'Негативные экономические и репутационные последствиям;' : '' #").Visible(true))
               .Add(c => c.ProposedActionsMethodImplementation, ac => ac.Title("Предлагаемые действия (процедуры) Способ реализации").Order(18).Visible(true))
               .Add(c => c.BudgetProposedProcedure, ac => ac.Title("Бюджет предполагаемой процедуры (тыс. руб.)").Order(19).ClientTemplate("#= data.BudgetProposedProcedure != null ? kendo.toString(data.BudgetProposedProcedure/1000, 'n') : 0 # ").Visible(true))
               .Add(c => c.ForecastPeriod, ac => ac.Title("Прогнозный срок организации мероприятий по реализации").Order(20).Visible(true))
               .Add(c => c.RationaleProposals, ac => ac.Title("Обоснование предложений Примечания").Order(21).Visible(true))
               .Add(c => c.Supervising, ac => ac.Title("Курирующий ВП").Order(22))
               ));

            context.CreateVmConfig<NonCoreAssetList>()
                   .IsNotify()
                   .Service<INonCoreAssetListService>()
                   .Title("Перечень ННА")
                   .ListView(x => x.Title("Перечни ННА").DataSource(ds => ds.Groups(gr => gr.Add(g => g.Year))))
                   .DetailView(x => x.Title("Перечень ННА").Editors(edt => edt
                           .AddManyToManyRigthAssociation<NonCoreAssetAndList>("NCAList_NonCoreAssets", y => y
                           .TabName("ННА")
                           .Mnemonic("NCAList_NonCoreAssets"))
                           .AddOneToManyAssociation<NonCoreAssetAndList>("NonCoreAssetList_NonCoreAssetAndList", y => y
                               .TabName("Строки ННА")
                                .Mnemonic("NonCoreAssetAndListRO")
                               .Filter((uofw, q, id, oid) => q.Where(w => (((w.ObjRigthId == id) && (w.NonCoreAssetListItemState == null)) ||
                                                                           ((w.ObjRigthId == id) &&
                                                                            ((w.NonCoreAssetListItemState != null) &&
                                                                             (w.NonCoreAssetListItemState.Code != "109")))) &&
                                                                          !w.Hidden))
                        )
                        .AddOneToManyAssociation<NonCoreAssetAndList>("NonCoreAssetList_NonCoreAssetAndList_Excluded", y => y
                            .TabName("Строки ННА (Исключенные)")
                            .Mnemonic("NonCoreAssetAndListExcluded")
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ObjRigthId == id))
                        )
                        .AddOneToManyAssociation<NonCoreAssetAndList>("NonCoreAssetList_NonCoreAssetAndList_PreviousPeriod", y => y
                            .TabName("Строки ННА прошлых периодов")
                            .Mnemonic("NonCoreAssetAndListPreviousPeriod")
                            .FilterExtended((uofw, q, id, oid) =>
                            {
                                int? currentSocietyId = uofw.GetRepository<NonCoreAssetList>().Find(f => f.ID == id)
                                    .SocietyID;

                                if (currentSocietyId == null)
                                    return null;

                                int[] ncaIds = uofw.GetRepository<NonCoreAsset>().Filter(f =>
                                    !f.Hidden && ((f.AssetOwnerID != null) && (f.AssetOwnerID == currentSocietyId))).Select(s => s.ID).ToArray();

                                int[] ncaListIds = uofw.GetRepository<NonCoreAssetList>()
                                    .Filter(f => f.NonCoreAssetListState != null && f.NonCoreAssetListState.Code == "105" && f.ID != id)
                                    .Select(s => s.ID).ToArray();

                                if (ncaIds.Length == 0 || ncaListIds.Length == 0)
                                    return null;

                                return uofw.GetRepository<NonCoreAssetAndList>()
                                    .Filter(f =>
                                        (ncaIds.Contains(f.ObjLeftId) && ncaListIds.Contains(f.ObjRigthId) &&
                                         ((f.NonCoreAssetListItemState != null) &&
                                          (f.NonCoreAssetListItemState.Code == "104"))) && !f.Hidden);
                            })
                        )
                    )
                    .DefaultSettings((uofw, item, commonEditorViewModel) =>
                    {
                        if (item.NonCoreAssetListStateID != null)
                        {
                            string statusCode = uofw.GetRepository<NonCoreAssetListState>().Find(f => f.ID == item.NonCoreAssetListStateID).Code;

                            if (statusCode == "105")
                            {
                                commonEditorViewModel.SetReadOnlyAll();
                                commonEditorViewModel.ReadOnlyByMnemonic("NCAList_NonCoreAssets", true);
                                commonEditorViewModel.ReadOnly(ro => ro.NonCoreAssetListState, false);
                            }

                            //
                            if (statusCode.ToLower() == "arhive")
                            {
                                commonEditorViewModel.SetReadOnlyAll();
                            }
                        }
                    })
                )
                   .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<NonCoreAssetList>(nameof(NonCoreAssetList), "NCAListPreviousPeriod")
                .Service<INCAListPreviousPeriodService>()
                .IsReadOnly();

            context.CreateVmConfig<NonCoreAssetSale>()
           .Service<INonCoreAssetSaleService>()
           .Title("Реализация ННА")
           .ListView(x => x.Title("Реализации ННА"))
           .DetailView(x => x.Title("Реализация ННА"))
           .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<NonCoreAssetSaleAccept>()
             .Service<INonCoreAssetSaleAcceptService>()
             .Title("Одобрение реализации ННА")
             .ListView(x => x.Title("Одобрения реализаций ННА"))
             .DetailView(x => x.Title("Одобрение реализации ННА")
                 .Editors(edt => edt
                            .AddManyToManyRigthAssociation<NonCoreAssetListItemAndNonCoreAssetSaleAccept>("NonCoreAssetListItemAndNonCoreAssetSaleAccept", y => y.Mnemonic("NonCoreAssetAndList").TabName("[1]Одобрения реализации ННА"))))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<NonCoreAssetSaleOffer>()
             .Service<INonCoreAssetSaleOfferService>()
             .Title("Предложение по реализации ННА")
             .ListView(x => x.Title("Предложения по реализациям ННА"))
             .DetailView(x => x.Title("Предложение по реализации ННА")
                .Editors(edt => edt
                    .AddOneToManyAssociation<NonCoreAssetAndList>("NonCoreAssetAndList_SaleOffer",
                            y => y.Title("Строка перечня")
                                .IsLabelVisible(false)
                                .Visible(true)
                                .TabName("[1]Строки перечня")
                                .Create((uofw, entity, id) =>
                                {
                                    entity.Offer = uofw.GetRepository<NonCoreAssetSaleOffer>().Find(id);
                                })
                                .Delete((uofw, entity, id) =>
                                {
                                    entity.Offer = null;
                                })
                                .Filter((uofw, q, id, oid) => q.Where(w => w.ObjRigthId == id && !w.Hidden)))
                    ))
             .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<NonCoreAssetInventory>()
                .Service<INonCoreAssetInventoryService>()
                .Title("Реестр ННА")
                .ListView(x => x.Title("Реестры ННА").DataSource(ds => ds.Groups(gr => gr.Add(g => g.Year)))
                .Columns(c => c
                    .Add(t => t.Name, h => h.Visible(true).Order(1))
                    .Add(t => t.Year, h => h.Visible(true).Order(2))
                    .Add(t => t.NonCoreAssetInventoryType, h => h.Visible(true).Order(3))
                    .Add(t => t.NonCoreAssetInventoryState, h => h.Visible(true).Order(4))
                    .Add(t => t.DocumentDetails, h => h.Visible(false))
                    .Add(t => t.DateFrom, h => h.Visible(false))
                    .Add(t => t.DateTo, h => h.Visible(false))
               ))
                .DetailView(x => x.Title("Реестр ННА").Editors(edt => edt
                    .Add(ed => ed.Year, ac => ac.Visible(true).Title("Год").Order(1))
                    .Add(ed => ed.Name, ac => ac.Visible(true).Title("Наименование").Order(2))
                    .Add(ed => ed.DateFrom, ac => ac.Visible(true).Description("Дата начала согласования").Order(3))
                    .Add(ed => ed.DateTo, ac => ac.Visible(true).Description("Дата окончания согласования").Order(4))
                    .Add(ed => ed.NonCoreAssetInventoryType, ac => ac.Visible(true).Order(5))
                    .Add(ed => ed.NonCoreAssetInventoryState, ac => ac.Visible(true).Order(6))
                    .Add(ed => ed.DocumentDetails, ac => ac.Visible(true).Order(7))
                    .AddOneToManyAssociation<NonCoreAssetList>("NonCoreAssetInventory_List",
                        y => y.Title("Перечни")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("[2]Перечни ННА")
                            .Create((uofw, entity, id) =>
                            {
                                entity.NonCoreAssetInventory = uofw.GetRepository<NonCoreAssetInventory>().FindAsNoTracking(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.NonCoreAssetInventory = null;
                                entity.NonCoreAssetInventoryID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.NonCoreAssetInventoryID == id && !w.Hidden)))

                    .AddOneToManyAssociation<NonCoreAssetAndList>("NonCoreAssetInventory_NonCoreAssetAndList",
                        y => y.Title("Перечни")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .TabName("[3]Строки перечней")
                            .Create((uofw, entity, id) =>
                            {
                                entity.NonCoreAssetInventory = uofw.GetRepository<NonCoreAssetInventory>()
                                .FindAsNoTracking(id);
                                entity.NonCoreAssetInventoryID = id;
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.NonCoreAssetInventoryID = null;
                                entity.NonCoreAssetInventory = null;
                            })
                            .Filter((uofw, q, id, oid) =>
                                q.Where(w => w.NonCoreAssetInventoryID == id
                                && w.NonCoreAssetListItemState != null
                                && w.NonCoreAssetListItemState.Code == "104"
                                && !w.Hidden
                                && !w.IsNCAPreviousPeriod
                                && w.ObjRigth != null
                                && w.ObjRigth.NonCoreAssetListState != null
                                && w.ObjRigth.NonCoreAssetListState.Code != "Arhive"
                            )))

                 .AddOneToManyAssociation<NonCoreAsset>("NonCoreAssetInventory_NonCoreAssetEarly",
                        y => y.Title("ННА предыдущих периодов")
                            .IsLabelVisible(false)
                            .Visible(true)
                            .IsReadOnly(true)
                            .TabName("[4]ННА предыдущих периодов")
                            .FilterExtended((uow, q, id, oid) =>
                            {
                                var inv = uow.GetRepository<NonCoreAssetInventory>()
                                .Filter(f => f.ID == id).FirstOrDefault();
                                var dt = inv.DateFrom;

                                var items = uow.GetRepository<NonCoreAssetAndList>()
                                            .Filter(ff => ff.ObjRigth != null
                                            && ff.ObjRigth.NonCoreAssetInventoryID == id
                                            && ff.IsNCAPreviousPeriod);

                                return items.Join(q, e => e.ObjLeftId, o => o.ID, (e, o) => o);
                            }))
                )
                .DefaultSettings((uow, r, commonEditorViewModel) =>
                {
                    commonEditorViewModel.Visible(nca => nca.DictObjectStatus, false);
                    commonEditorViewModel.Visible(nca => nca.DictObjectState, false).ReadOnly(nca => nca.DictObjectState, true);
                    commonEditorViewModel.Visible(nca => nca.Code, false);
                })
                )
                .LookupProperty(x => x.Text(t => t.Name));

            #endregion Asset

            #region Document

            /// <see cref="CorpProp.Model.FileModel"/>

            #endregion Document

            #region Estate

            context.CreateVmConfig<CadastralValue>()
                .Service<ICadastralValueService>()
                .Title("Кадастровая стоимость")
                .ListView(x => x.Title("Кадастровая стоимость"))
                .DetailView(x => x.Title("Кадастровая стоимость"))
                .LookupProperty(x => x.Text(t => t.Value));

            context.CreateVmConfig<CadastralPart>()
                .Service<ICadastralPartService>()
                .Title("Часть кадастрового объекта")
                .ListView(x => x.Title("Части кадастрового объекта"))
                .DetailView(x => x.Title("Часть кадастрового объекта"))
                .LookupProperty(x => x.Text(t => t.Number));

            context.CreateVmConfig<EstateDeal>()
                .Title("Предмет сделки")
                .ListView(x => x.Title("Предметы сделок"))
                .DetailView(ed => ed.Title("Предмет сделки"))
                .LookupProperty(b => b.Text(t => t.Estate));

            context.CreateVmConfigOnBase<Base.UI.RegisterMnemonics.Entities.MnemonicEx, Base.UI.RegisterMnemonics.Entities.DeatilViewEx>("AdditionalPropertyEditor")
                .Service<Base.UI.RegisterMnemonics.Services.IMnemonicExCrudService<Base.UI.RegisterMnemonics.Entities.DeatilViewEx>>()
                .Title("Дополнительные характеристики")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Форма").Editors(e => e.Add(ed => ed.Editors, action => action.IsLabelVisible(false))));

            context.CreateVmConfig<Coordinate>()
           .Title("Координаты")
           .ListView(x => x.Title("Координаты"))
           .LookupProperty(x => x.Text(t => t.ID))
           .DetailView(x => x.Title("Координаты"));

            #endregion Estate

            #region FIAS

            context.CreateVmConfig<SibAddress>()
               .Title("Адрес")
               .ListView(x => x.Title("Адреса"))
               .DetailView(x => x.Title("Адрес"))
               .LookupProperty(x => x.Text(t => t.Name));

            #endregion FIAS

            context.CreateVmConfig<ExchangeRate>()
              .Title("Курс Валюты")
              .ListView(x => x.Title("Курсы Валют"))
              .DetailView(x => x.Title("Курс Валюты"))
              .LookupProperty(x => x.Text(t => t.Currency));

            context.CreateVmConfig<SourceInformation>()
              .Title("Источник информации Документа")
              .ListView(x => x.Title("Источники информации"))
              .DetailView(x => x.Title("Источник информации"))
              .LookupProperty(x => x.Text(t => t.Name));

            #region Mapping

            context.CreateVmConfig<EstateRulesCteation>()
              .Title("Правила создания ОИ при импорте ОБУ")
              .ListView(x => x.Title("Правила создания ОИ при импорте ОБУ"))
              .DetailView(x => x.Title("Правила создания ОИ при импорте ОБУ"))
              .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<AccountingEstates>()
                .Service<IAccountingEstatesService>()
                .Title("Сопоставление класса БУ с ОИ")
                .ListView(x => x.Title("Сопоставления класса БУ с ОИ"))
                .DetailView(x => x.Title("Сопоставление класса БУ с ОИ"))
                .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<Mapping>()
                .Title("Сопоставление объекта во внешней системе")
                .ListView(x => x.Title("Сопоставления объектов во внешней системе"))
                .DetailView(x => x.Title("Сопоставление объекта во внешней системе"))
                .LookupProperty(x => x.Text(t => t.ExternalClassName));

            context.CreateVmConfig<OKOFEstates>()
               .Service<IOKOFEstatesService>()
               .Title("Сопоставление ОКОФ с ОИ")
               .ListView(x => x.Title("Сопоставления ОКОФ с ОИ"))
               .DetailView(x => x.Title("Сопоставление ОКОФ с ОИ"))
               .LookupProperty(x => x.Text(t => t.ID));

            #endregion Mapping

            #region Securuty

            context.CreateVmConfig<SibRole>()
                .Title("Роль")
                .ListView(x => x.Title("Роли"))
                .DetailView(x => x.Title("Роль"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<SibUser>()
                .Service<ISibUserService>()
                .Title("Пользователь")
                .ListView(x => x.Title("Пользователь")
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                    .Columns(col =>
                        col.Add(c => c.FullName, ac => ac.Visible(true))
                        .Add(c => c.SocietyDept, ac => ac.Visible(true))
                        .Add(c => c.Society, ac => ac.Visible(true))
                        .Add(c => c.Phone, ac => ac.Visible(true))
                        .Add(c => c.Email, ac => ac.Visible(true))

                        .Add(c => c.Address, ac => ac.Visible(false))
                        .Add(c => c.BirthDate, ac => ac.Visible(false))
                        .Add(c => c.Image, ac => ac.Visible(false))
                        .Add(c => c.Gender, ac => ac.Visible(false))

                )
                .DataSource(ds => ds.Sort(s => s.Add(ss => ss.FullName)))
                )
                .DetailView(x => x.Title("Пользователь")
                .Editors(ed =>
                        ed.Add(d => d.LastName, ac => ac.Order(1))
                        .Add(d => d.FirstName, ac => ac.Order(2))
                        .Add(d => d.MiddleName, ac => ac.Order(3))
                        .Add(d => d.Phone, ac => ac.Order(4))
                        .Add(d => d.Mobile, ac => ac.Order(5))
                        .Add(d => d.Email, ac => ac.Order(6))
                        .Add(d => d.PostName, ac => ac.Order(7))
                        .Add(d => d.SocietyDept, ac => ac.Order(8))
                        .Add(d => d.Society, ac => ac.Order(9))
                        .Add(d => d.Boss, ac => ac.Order(10))
                        .Add(d => d.Vice, ac => ac.Order(11))
                        .Add(d => d.Description, ac => ac.Order(12))

                        .Add(d => d.FullName, ac => ac.Visible(false))
                        .Add(d => d.Address, ac => ac.Visible(false))
                        .Add(d => d.BirthDate, ac => ac.Visible(false))
                        .Add(d => d.Emails, ac => ac.Visible(false))
                        .Add(d => d.Gender, ac => ac.Visible(false))
                        .Add(d => d.Image, ac => ac.Visible(false))
                        .Add(d => d.Phones, ac => ac.Visible(false))

                        .AddManyToManyLeftAssociation<SibUserTerritory>("SibUser_SocietyTerritorys",
                        y => y.TabName("Территориальный признак").Visible(true).Order(1))

                        .AddOneToManyAssociation<Appraisal>("SibUser_Appraisals",
                                editor => editor
                                .TabName("[2]Оценки")
                                .IsReadOnly(true)
                                .Title("Оценки")
                                .IsLabelVisible(false)
                                .Order(1)
                                .Filter((uofw, q, id, oid) =>
                                     q.Where(w => w.ExecutorID == id)))

                ))
                .LookupProperty(x => x.Text(t => t.FullName));

            context.CreateVmConfig<CorpProp.Entities.Security.SibUserAccessWizard>()
               .Service<IAccessUserWizardService<SibUserAccessWizard>>()
               .Title("Мастер - создания пользователя")
               .WizzardDetailView(w => w.Steps(steps =>
               {
                   steps.Add("first", s => s
                       .StepProperties(prs => prs
                           .Add(p => p.Login)
                           .Add(p => p.Password)
                           .Add(p => p.IsActive)
                           .Add(p => p.LastName)
                           .Add(p => p.FirstName)
                           .Add(p => p.MiddleName)
                           .Add(p => p.Society)
                       ))
                    .Add("second", s => s
                       .StepProperties(prs => prs
                           .Add(p => p.Department)
                           .Add(p => p.Post)
                           .Add(p => p.Email)
                           .Add(p => p.Phone)
                           .Add(p => p.Mobile)
                           .Add(p => p.Description)
                       ))
                       ;
               }).HasSummary(true))
               .DetailView(x => x.Title("Пользователь"));

            context.CreateVmConfig<ObjectPermission>()
                .Service<IObjectPermissionService>()
                .Title("Разрешение на объект")
                .ListView(lv => lv.HiddenActions(new[] { LvAction.Link, LvAction.Unlink }))
                .DetailView(dv => dv.Editors(eds => eds
               .Add(ed => ed.AllowRead, ac => ac.OnChangeClientScript("corpProp.dv.editors.onChange.ObjectPermission_CheckBox(form,isChange)"))
               .Add(ed => ed.AllowWrite, ac => ac.OnChangeClientScript("corpProp.dv.editors.onChange.ObjectPermission_CheckBox(form,isChange)"))
               .Add(ed => ed.AllowDelete, ac => ac.OnChangeClientScript("corpProp.dv.editors.onChange.ObjectPermission_CheckBox(form,isChange)"))
                ));

            context.CreateVmConfig<Base.Security.Permission>()
                .Title("Разрешение на тип")
                .DetailView(dv => dv.Editors(ed => ed
                .Add(e => e.FullName, ac => ac.Title("Тип объекта"))
                .AddOneToManyAssociation<ObjectPermission>("Permission_ObjectPermissions",
                         y => y
                         .Title("Разрешения на объект")
                         .TabName("[1]Разрешения на объект")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.TypePermission = uofw.GetRepository<Base.Security.Permission>().Find(id);
                             entity.RoleID = entity.TypePermission?.RoleID;
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.TypePermission = null;
                             entity.Role = null;
                         })
                        .Filter((uofw, q, id, oid) => q.Where(w => w.TypePermissionID == id))
                        .Visible(true).Order(100))
                ));

            #endregion Securuty

            #region Subject

            context.CreateVmConfig<Appraiser>()
               .Service<IAppraiserService>()
               .Title("Оценочная организация (Деловой партнер)")
               .ListView(x => x.Title("Оценочная организация (Деловой партнер)")
                     .Columns(cols => cols
                              .Add(c => c.ShortName, ac => ac.Order(1))
                              .Add(c => c.INN, ac => ac.Order(2))
                              .Add(c => c.SDP, ac => ac.Order(3))
                              .Add(c => c.Phone, ac => ac.Order(4))
                              .Add(c => c.Email, ac => ac.Order(5))
                              .Add(c => c.FederalDistrict, ac => ac.Order(6))
                              .Add(c => c.Region, ac => ac.Order(7))
                              .Add(c => c.Country, ac => ac.Order(8))

                             )
                         )
               .DetailView(x => x.Title("Оценочная организация (Деловой партнер)")

                    .Editors(edt => edt
                        .Add(d => d.INN, ac => ac.Order(1).TabName("[0]Основные данные"))
                        .AddOneToManyAssociation<AppraisalOrgData>("Appraiser_AppraisalOrgData", e => e
                            .TabName("[6]Данные оценочной организации")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Appraiser = uofw.GetRepository<Appraiser>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Appraiser = null;
                            })
                            .Filter((uow, q, id, oid) => q.Where(f => f.Appraiser != null && f.Appraiser.ID == id))
                        )
                        .AddOneToManyAssociation<AppraiserDataFinYear>("Appraiser_AppraiserDataFinYear", e => e
                            .TabName("[6]Данные оценщика за финансовый год")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Appraiser = uofw.GetRepository<Appraiser>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Appraiser = null;
                            })
                            .Filter((uow, q, id, oid) => q.Where(f => f.Appraiser != null && f.Appraiser.ID == id))
                        )
                        .AddOneToManyAssociation<BankingDetail>("Subject_BankingDetail",
                         editor => editor
                         .TabName("[6]Реквизиты")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Subject = uofw.GetRepository<Subject>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Subject = null;
                         })
                        .Filter((uofw, q, id, oid) => q.Where(w => w.SubjectID == id)))
                        .AddOneToManyAssociation<Appraisal>("Appraiser_Appraisal", e => e
                            .TabName("[5]Оценки")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Appraiser = uofw.GetRepository<Appraiser>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Appraiser = null;
                            })
                        )
                    )
                )
               .LookupProperty(x => x.Text(t => t.ShortName));

            context.CreateVmConfig<Society>("Society")
                .Service<ISocietyService>()
                .Title("Общество группы")
                .DetailView(x => x.Title("Общество группы").Editors(edt => edt
                   .AddOneToManyAssociation<Shareholder>("Society_ShareholderView",
                         editor => editor
                         .TabName("[5]Акционеры")
                         .Mnemonic("Society_ShareholderView")
                         .IsReadOnly()
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = null;
                         })
                        .Filter((uofw, q, id, oid) =>
                                 q.Where(w => w.SocietyRecipient != null && w.SocietyRecipient.Oid == oid && !w.Hidden)
                        ))
                   .AddOneToManyAssociation<Shareholder>("Society_RecipientView",
                         editor => editor
                         .TabName("[6]Участие")
                         .Mnemonic("Society_RecipientView")
                         .IsReadOnly()
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietyShareholder != null && w.SocietyShareholder.Oid == oid && !w.Hidden)))
                   .AddOneToManyAssociation<Predecessor>("Society_Predecessor",
                         editor => editor
                         .TabName("[7]Правопредшественники")
                         .Mnemonic("Society_Predecessor")
                         .IsReadOnly(false)
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietySuccessor = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietySuccessor = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietySuccessor != null && w.SocietySuccessor.Oid == oid && !w.Hidden)))
                   .AddOneToManyAssociation<FileCard>("Society_FileCard", //Учредительные документы.
                         editor => editor
                         .TabName("[4]Документы")
                         .IsLabelVisible(false)
                         .Create(
                         (uofw, entity, id) =>
                         {
                             entity.SocietyID = id;
                             entity.Society = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Society = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietyID == id && !w.Hidden)))
                   ))
                .LookupProperty(x => x.Text(t => t.ShortName))
                .ListView(x => x.Title("Общества группы")
#if DEBUG
                        //.HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#else
                        .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#endif
                        .Columns(c =>
                                 c.Add(t => t.ID, h => h.Visible(false))
                                  .Add(t => t.IDEUP, h => h.Visible(true).Order(1))
                                  .Add(t => t.ConsolidationUnit, h => h.Visible(false))
                                  .Add(t => t.FullName, h => h.Visible(false))
                                  .Add(t => t.ShortName, h => h.Visible(true).Order(2))
                                  .Add(t => t.FullNameEng, h => h.Visible(false))
                                  .Add(t => t.ShortNameEng, h => h.Visible(false))
                                  .Add(t => t.DateRegistration, h => h.Visible(false))
                                  .Add(t => t.INN, h => h.Visible(false))
                                  .Add(t => t.OGRN, h => h.Visible(false))
                                  .Add(t => t.Country, h => h.Visible(true).Order(5))
                                  .Add(t => t.FederalDistrict, h => h.Visible(true).Order(6))
                                  .Add(t => t.Region, h => h.Visible(true).Order(7))
                                  .Add(t => t.City, h => h.Visible(false))
                                  .Add(t => t.AddressLegalString, h => h.Visible(false))
                                  .Add(t => t.AddressActualString, h => h.Visible(false))
                                  .Add(t => t.Phone, h => h.Visible(true).Order(8))
                                  .Add(t => t.Email, h => h.Visible(true).Order(9))
                                  .Add(t => t.IsSocietyControlled, h => h.Visible(true).Order(11))
                                  .Add(t => t.IsSocietyKey, h => h.Visible(true).Order(10))
                                  .Add(t => t.IsSocietyJoint, h => h.Visible(false))
                                  .Add(t => t.IsSocietyResident, h => h.Visible(false))
                                  .Add(t => t.IsSociety, h => h.Visible(false))
                                  .Add(t => t.DateInclusionInPerimeter, h => h.Visible(false))
                                  .Add(t => t.BaseInclusionInPerimeter, h => h.Visible(false))
                                  .Add(t => t.IsExclusionFromPerimeter, h => h.Visible(false))
                                  .Add(t => t.DateExclusionFromPerimeter, h => h.Visible(false))
                                  .Add(t => t.BaseExclusionFromPerimeter, h => h.Visible(false))
                                  .Add(t => t.ProductionBlock, h => h.Visible(false))
                                  .Add(t => t.BusinessSegment, h => h.Visible(false))
                                  .Add(t => t.BusinessDirection, h => h.Visible(false))
                                  .Add(t => t.ActualKindActivity, h => h.Visible(false))
                                  .Add(t => t.SoleExecutiveBodyName, h => h.Visible(true).Order(12))
                                  .Add(t => t.SoleExecutiveBodyPost, h => h.Visible(true).Order(13))
                                  .Add(t => t.SoleExecutiveBodyDateFrom, h => h.Visible(true).Order(14))
                                  .Add(t => t.SoleExecutiveBodyDateTo, h => h.Visible(true).Order(15))
                                  .Add(t => t.UnitOfCompany, h => h.Visible(true).Order(16))
                                  .Add(t => t.Curator, h => h.Visible(true).Order(17))
                                  .Add(t => t.ShareInEquity, h => h.Visible(true).Order(3))
                                  .Add(t => t.ShareInVotingRights, h => h.Visible(true).Order(4))
                                  .Add(t => t.SocietyPredecessorsCount, h => h.Visible(false))
                                  .Add(t => t.DirectShare, h => h.Visible(false))
                                  .Add(t => t.DirectShareInVotingRights, h => h.Visible(false))
                                  .Add(t => t.DirectParticipantCount, h => h.Visible(false))
                                  .Add(t => t.DirectParticipantList, h => h.Visible(false))
                               )
                        .Toolbar(factory => factory.Add("GetExportSocietyGraphsToolbar", "Toolbar"))
                )
                ;

            context.CreateVmConfigOnBase<Society>(nameof(Society), "NCAChangeOG")
                .Service<INCAChangeOGService>()
                .IsReadOnly();



            context.CreateVmConfig<SocietyCalculatedField>("SocietyCalculatedField")
                .Service<ISocietyCalculatedFieldService>()
                .IsReadOnly(true)
                .Title("Общество группы")
                .DetailView(x => x.Title("Общество группы").Editors(edt => edt
                   .AddOneToManyAssociation<Shareholder>("SocietyCalculatedField_ShareholderView",
                         editor => editor
                         .TabName("[5]Акционеры")
                         .Mnemonic("SocietyCalculatedField_ShareholderView")
                         .IsReadOnly(true)
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = null;
                         })
                        .Filter(
                             (uofw, q, id, oid) =>
                                 q.Where(w => w.SocietyRecipient != null && w.SocietyRecipient.Oid == oid && !w.Hidden)
                             ))
                   .AddOneToManyAssociation<Shareholder>("SocietyCalculatedField_RecipientView",
                         editor => editor
                         .TabName("[6]Участие")
                         .Mnemonic("SocietyCalculatedField_RecipientView")
                         .IsReadOnly(true)
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietyRecipient = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietyShareholder != null && w.SocietyShareholder.Oid == oid && !w.Hidden)))
                   .AddOneToManyAssociation<Predecessor>("SocietyCalculatedField_Predecessor",
                         editor => editor
                         .TabName("[7]Правопредшественники")
                         .Mnemonic("SocietyCalculatedField_Predecessor")
                         .IsReadOnly(true)
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SocietySuccessor = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SocietySuccessor = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietySuccessor != null && w.SocietySuccessor.Oid == oid && !w.Hidden)))
                   .AddOneToManyAssociation<FileCard>("SocietyCalculatedField_FileCard", //Учредительные документы.
                         editor => editor
                         .TabName("[4]Документы")
                         .IsReadOnly(true)
                         .IsLabelVisible(false)
                         .Create(
                         (uofw, entity, id) =>
                         {
                             entity.SocietyID = id;
                             entity.Society = uofw.GetRepository<Society>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Society = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SocietyID == id && !w.Hidden)))
                   ))
                .LookupProperty(x => x.Text(t => t.ShortName))
                .ListView(x => x.Title("Общества группы")
                .DataSource(ds => ds.Groups(gr => gr.Add(g => g.FederalDistrict).Add(g => g.Region)))
                        .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Edit, LvAction.Import, LvAction.Unlink, LvAction.Link, LvAction.MultiEdit })

                        .Columns(c =>
                                    c.Add(t => t.ID, h => h.Visible(false))
                                    .Add(t => t.IDEUP, h => h.Visible(true).Order(1))
                                    .Add(t => t.ConsolidationUnit, h => h.Visible(false))
                                    .Add(t => t.FullName, h => h.Visible(false))
                                    .Add(t => t.ShortName, h => h.Visible(true).Order(2))
                                    .Add(t => t.FullNameEng, h => h.Visible(false))
                                    .Add(t => t.ShortNameEng, h => h.Visible(false))
                                    .Add(t => t.DateRegistration, h => h.Visible(false))
                                    .Add(t => t.INN, h => h.Visible(false))
                                    .Add(t => t.OGRN, h => h.Visible(false))
                                    .Add(t => t.Country, h => h.Visible(true).Order(5))
                                    .Add(t => t.FederalDistrict, h => h.Visible(true).Order(6))
                                    .Add(t => t.Region, h => h.Visible(true).Order(7))
                                    .Add(t => t.City, h => h.Visible(false))
                                    .Add(t => t.AddressLegalString, h => h.Visible(false))
                                    .Add(t => t.AddressActualString, h => h.Visible(false))
                                    .Add(t => t.Phone, h => h.Visible(true).Order(8))
                                    .Add(t => t.Email, h => h.Visible(true).Order(9))
                                    .Add(t => t.IsSocietyControlled, h => h.Visible(true).Order(11))
                                    .Add(t => t.IsSocietyKey, h => h.Visible(true).Order(10))
                                    .Add(t => t.IsSocietyJoint, h => h.Visible(false))
                                    .Add(t => t.IsSocietyResident, h => h.Visible(false))
                                    .Add(t => t.IsSociety, h => h.Visible(false))
                                    .Add(t => t.DateInclusionInPerimeter, h => h.Visible(false))
                                    .Add(t => t.BaseInclusionInPerimeter, h => h.Visible(false))
                                    .Add(t => t.IsExclusionFromPerimeter, h => h.Visible(false))
                                    .Add(t => t.DateExclusionFromPerimeter, h => h.Visible(false))
                                    .Add(t => t.BaseExclusionFromPerimeter, h => h.Visible(false))
                                    .Add(t => t.ProductionBlock, h => h.Visible(false))
                                    .Add(t => t.BusinessSegment, h => h.Visible(false))
                                    .Add(t => t.BusinessDirection, h => h.Visible(false))
                                    .Add(t => t.ActualKindActivity, h => h.Visible(false))
                                    .Add(t => t.SoleExecutiveBodyName, h => h.Visible(true).Order(12))
                                    .Add(t => t.SoleExecutiveBodyPost, h => h.Visible(true).Order(13))
                                    .Add(t => t.SoleExecutiveBodyDateFrom, h => h.Visible(true).Order(14))
                                    .Add(t => t.SoleExecutiveBodyDateTo, h => h.Visible(true).Order(15))
                                    .Add(t => t.UnitOfCompany, h => h.Visible(true).Order(16))
                                    .Add(t => t.Curator, h => h.Visible(true).Order(17))
                                    .Add(t => t.ShareInEquity, h => h.Visible(true).Order(3))
                                    .Add(t => t.ShareInVotingRights, h => h.Visible(true).Order(4))
                                    .Add(t => t.SocietyPredecessorsCount, h => h.Visible(false))
                                    .Add(t => t.DirectShare, h => h.Visible(false))
                                    .Add(t => t.DirectShareInVotingRights, h => h.Visible(false))
                                    .Add(t => t.DirectParticipantCount, h => h.Visible(false))
                                    .Add(t => t.DirectParticipantList, h => h.Visible(false))
                                    .Add(t => t.CountInventoryObject, h => h.Visible(true).Order(100))
                                    .Add(t => t.InitialCostInventoryObject, h => h.Visible(true).Order(101))
                                    .Add(t => t.ResidualCostInventoryObject, h => h.Visible(true).Order(102))
                                    .Add(t => t.CountRealEstate, h => h.Visible(true).Order(103))
                                    .Add(t => t.CountRealEstateRight, h => h.Visible(true).Order(104))
                                    .Add(t => t.InitialCostRealEstate, h => h.Visible(true).Order(105))
                                    .Add(t => t.ResidualCostRealEstate, h => h.Visible(true).Order(106))
                                    .Add(t => t.CountRealEstateNotRight, h => h.Visible(true).Order(107))
                                    .Add(t => t.InitialCostRealEstateNotRight, h => h.Visible(true).Order(108))
                                    .Add(t => t.ResidualCostRealEstateNotRight, h => h.Visible(true).Order(109))
                                    .Add(t => t.CountMovableEstate, h => h.Visible(true).Order(110))
                                    .Add(t => t.InitialCostMovableEstate, h => h.Visible(true).Order(111))
                                    .Add(t => t.ResidualCostMovableEstate, h => h.Visible(true).Order(112))
                                    .Add(t => t.CountLandEstate, h => h.Visible(true).Order(113))
                                    .Add(t => t.InitialCostLandEstate, h => h.Visible(true).Order(114))
                                    .Add(t => t.ResidualCostLandEstate, h => h.Visible(true).Order(115))
                                    .Add(t => t.CadastralValueLandEstate, h => h.Visible(true).Order(116))
                                    .Add(t => t.CountRentalLandEstate, h => h.Visible(true).Order(117))
                               ));



            context.CreateVmConfig<SubjectObject.Subject>()
                .Service<ISubjectService>()
                .Title("Деловой партнер")
                .ListView(x => x.Title("Деловые партнеры")
#if DEBUG
                                //.HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#else
                                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#endif
                                .Columns(c =>
                                 c.Add(t => t.ID, h => h.Visible(false))
                                  .Add(t => t.ShortName, h => h.Visible(true))
                                  .Add(t => t.INN, h => h.Visible(true))
                                  .Add(t => t.AddressLegal, p => p.Visible(true))
                               )
                )
                .DetailView(x => x.Title("Деловой партнер")
                    .Editors(edt => edt
                        .AddOneToManyAssociation<DealParticipant>("Subject_DealParticipant",
                         editor => editor
                         .TabName("[3]Участник сделки")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Subject = uofw.GetRepository<Subject>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Subject = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SubjectID == id && !w.Hidden)))
                   .AddOneToManyAssociation<FileCard>("Subject_FileCard",
                         editor => editor
                         .TabName("[4]Документы")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Subject = uofw.GetRepository<Subject>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Subject = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SubjectID == id && !w.Hidden)))
                   .AddOneToManyAssociation<BankingDetail>("Subject_BankingDetail",
                         editor => editor
                         .TabName("[5]Реквизиты")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Subject = uofw.GetRepository<Subject>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Subject = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.SubjectID == id && !w.Hidden)))))
                .LookupProperty(x => x.Text(t => t.ShortName))
                //.SetToDataSourceResultEvent((q, req) => {
                //    DataSourceResult ds = null;
                //    var items = q as IQueryable<Subject>;
                //    if (items != null)
                //    {
                //        var items2 = items.ToList();
                //        ds = items2.ToDataSourceResult(req, obj => new
                //        {
                //            ID = obj.ID,
                //            ShortName = obj.ShortName,
                //            INN = obj.INN,
                //            AddressLegal = (obj.AddressLegal == null) ? "" : obj.AddressLegal.Name
                //        });
                //    }
                //    else
                //        ds = new DataSourceResult();
                //    return ds;
                //})
                ;

            context.CreateVmConfig<Society>("Society_Agents_NameOnly")
                .Service<ISocietyService>()
                .ListView(lv => lv.Columns(col => col.Clear().Add(c => c.Name))
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                );

            context.CreateVmConfig<Society>("Society_Subsidiaries_NameOnly")
                .Service<ISocietyService>()
                .ListView(lv => lv.Columns(col => col.Clear().Add(c => c.Name))
                    .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                );

            context.ModifyVmConfig<Society>().DetailView(dv => dv.Editors(e => e
                .AddManyToManyLeftAssociation<SocietyAgents>("Society_Agents", y => y.Mnemonic("Society_Agents_NameOnly").TabName("[8]Агенты/Управляемые")
                    .Title("Агенты")
                    .Visible(true)
                    .IsReadOnly(false)
                    .Order(1)
                    .IsLabelVisible(true)
                )
                .AddManyToManyLeftAssociation<SocietySubsidiaries>("Society_Subsidiaries", y => y.Mnemonic("Society_Subsidiaries_NameOnly").TabName("[8]Агенты/Управляемые")
                    .Title("Управляемые")
                    .Visible(true)
                    .IsReadOnly(false)
                    .Order(2)
                    .IsLabelVisible(true)
                ))
                .DefaultSettings((uow, society, commonEditorViewModel) =>
                {
                    var currentUser = getCurrentSibUser.Invoke(uow);
                    var isReadonly = !currentUser.IsFromCauk();

                    commonEditorViewModel.ReadOnlyByMnemonic("Society_Agents_NameOnly", isReadonly);
                    commonEditorViewModel.ReadOnlyByMnemonic("Society_Subsidiaries_NameOnly", isReadonly);

                    var currIDEUP = society.IDEUP;
                    var mainUserSocietyIDEUP = currentUser.Society?.IDEUP;
                    var isSubsidiaries = uow.GetRepository<SocietySubsidiaries>()
                        .Filter(x => !x.Hidden && x.ObjLeft != null && x.ObjLeft.IDEUP == mainUserSocietyIDEUP
                               && x.ObjRigth != null && x.ObjRigth.IDEUP == currIDEUP).Any();

                    var isPredecessorReadonly = (mainUserSocietyIDEUP == society.IDEUP || isSubsidiaries);

                    commonEditorViewModel.ReadOnlyByMnemonic("Society_Predecessor", !isPredecessorReadonly);
                })
            ).Service<ISocietyService>();

            context.CreateVmConfig<BankingDetail>()
           .Service<IBankingDetailService>()
           .Title("Банковские реквизиты")
           .ListView(x => x.Title("Банковские реквизиты"))
           .DetailView(x => x.Title("Банковские реквизиты"));

            #endregion Subject

            #region Request, Response

            RequestInitializer.Init(context);

            #endregion Request, Response

            #region ManyToMany

            context.CreateVmConfig<SibUserTerritory>()
             .Service<ISibUserTerritoryService>()
             .Title("Территориальная принадлежность")
             .ListView(x => x.Title("Территориальное распределение")
             .Columns(cols => cols
                .Add(col => col.ObjLeft, ac => ac.Title("Пользователь"))
                .Add(col => col.ObjRigth, ac => ac.Title("Общество группы"))
             ))
             .DetailView(x => x.Title("Территориальная принадлежность")
             .Editors(eds => eds
                .Add(ed => ed.ObjLeft, ac => ac.Title("Пользователь"))
                .Add(ed => ed.ObjRigth, ac => ac.Title("Общество группы"))
             ))
             .LookupProperty(x => x.Text(t => t.ID));

            #endregion ManyToMany

            context.CreateVmConfig<HistoricalSettings>()
             .Title("Настройка историчности")
             .ListView(x => x.Title("Настройка историчности"))
             .DetailView(x => x.Title("Настройка историчности"))
             .LookupProperty(x => x.Text(t => t.ID));

            //кастомизация фильтра представлений
            context.ModifyVmConfig<Base.UI.Filter.MnemonicFilter>()
                .ListView(lv => lv
                .DataSource(ds => ds
                .Sort(sort => sort.Add(s => s.Title)))
                .Columns(cols => cols
                .Add(c => c.Filter, ac => ac.Visible(false))
                .Add(c => c.Description, ac => ac.Visible(true).Title("Фильтр"))
                ))
                .DetailView(dv => dv
                .Editors(eds => eds
                .Add(ed => ed.Description, ac => ac.Visible(false))
                ));

            context.ModifyVmConfig<Role>()
                    .ListView(x => x.Toolbar(factory => factory.Add("GetPermissionsToolbar", "Roles")));

            context.CreateVmConfig<ExportTemplate>()
                  .Title("Шаблон экспорта")
                  .DetailView(dv => dv.Title("Шаблон экспорта")
                  .Editors(eds => eds.Add<FileDB>(Guid.NewGuid().ToString(), ac => ac
                       .EditorTemplate("Sib_FileDB")
                       .Title("Загрузить")
                       .Order(8)))
                  )
                  .ListView(lv => lv.Title("Шаблон экспорта"))
                  .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<ExportTemplateItem>()
                  .Title("Строки шаблона")
                  .DetailView(dv => dv.Title("Строки шаблона"))
                  .ListView(lv => lv.Title("Строки шаблона"))
                  .LookupProperty(x => x.Text(c => c.ID));

            //файлы для хранения в БД
            context.CreateVmConfig<FileDB>()
                  .Title("Файл")
                  .DetailView(dv => dv.Title("Файл"))
                  .ListView(lv => lv.Title("Файлы"))
                  .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<ActionComment>("NNAComment")
               .Title("Введите примечание")
               .DetailView(x => x.Title("Введите примечание")
                   .Width(600)
                   .Height(240)
                   .Editors(e => e.Add(a => a.Message, v => v.IsRequired(true))));

            context.CreateVmConfigOnBase<PresetRegistor>("PresetRegistor", "UserPreset")
                .Service<IPresetRegistorService>()
                .Title("Пользовательские пресеты")
                .DetailView(dv => dv.Editors(eds => eds.Add(ed => ed.UserID, ac => ac.Visible(true))))
                .ListView(x => x.Title("Пользовательские пресеты")
                    .HiddenActions(new LvAction[] { LvAction.Create })
                    .DataSource(d => d.Filter(f => f.UserID != null))
                    .Columns(cols => cols
                        .Add(col => col.UserID, ac => ac.Visible(false).Order(1).Title("ИД Пользователя"))
                        .Add(col => col.UserLogin, ac => ac.Visible(true).Order(-100).Title("Логин пользователя"))
                        .Add(col => col.For, ac => ac.Title("Мнемоника"))
                    ));

            //инициализация модели - создание кофигураторов ListView и DetailView
            ModelInitializer.Init(context);

            // Инициирует запрос к Repository.
            DefaultDataHelper.InstanceSingletone.IsGetRepository(context.UnitOfWork, new FillDataStrategy());

            //наполнение БД дефолтными данными
            context.DataInitializer("CorpProp", "0.1", () =>
            {
                Seed(context.UnitOfWork);
                DocumentInitializer.Seed(context.UnitOfWork);
                DefaultDataHelper.InstanceSingletone.InitDefaulData(context.UnitOfWork, new FillDataStrategy());
                UsersAndRolesInitializer.Seed(context.UnitOfWork, _loginProvider, _presetRegistorService);
            });
        }

        public void Seed(IUnitOfWork uow)
        {
            var DictObjectStates = new List<DictObjectState>() {
                  new DictObjectState(){ Name = "Устаревший", Code ="Old", PublishCode ="OLD" }
                , new DictObjectState(){ Name = "Актуальный", Code ="NotOld", PublishCode ="NOTOLD" }
                , new DictObjectState(){ Name = "Временный", Code ="Temporary", PublishCode ="TEMPORARY" }
            };
            var DictObjectStatuses = new List<DictObjectStatus>() {
                  new DictObjectStatus(){ Name = "Запрос на добавление", Code ="AddRequest", PublishCode ="ADDREQUEST" }
                , new DictObjectStatus(){ Name = "Запрос на удаление", Code ="DelRequest", PublishCode ="DELREQUEST" }
                , new DictObjectStatus(){ Name = "Согласовано добавление", Code ="AddConfirm", PublishCode ="ADDCONFIRM" }
                , new DictObjectStatus(){ Name = "Согласовано удаление", Code ="DelConfirm", PublishCode ="DELCONFIRM" }
                , new DictObjectStatus(){ Name = "Запрос отклонен", Code ="RequestDeny", PublishCode ="REQUESTDENY" }
            };

            uow.GetRepository<DictObjectState>().CreateCollection(DictObjectStates);
            uow.GetRepository<DictObjectStatus>().CreateCollection(DictObjectStatuses);

            new NotificationInitializer().Seed(uow);
        }
    }
}