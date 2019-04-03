using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Services.Estate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUSI.Model
{
    public static class EstateModel
    {
        public static void Init(IInitializerContext context, IViewModelConfigService viewModelConfigService)
        {
            context.CreateVmConfigOnBase<EstateRegistration>(nameof(EstateRegistration), "EstateRegistrationRO")
                   .ListView(builder => builder.HiddenActions(new[] { LvAction.Create, LvAction.Delete }));

            var list = new List<Type>()
            {
                typeof(Estate),
                typeof(Aircraft),
                typeof(BuildingStructure),
                typeof(Cadastral),
                typeof(CarParkingSpace),
                typeof(IntangibleAsset),
                typeof(InventoryObject),
                typeof(Land),
                typeof(MovableEstate),
                typeof(NonCadastral),
                typeof(RealEstate),
                typeof(RealEstateComplex),
                typeof(Room),
                typeof(Ship),
                typeof(SpaceShip),
                typeof(UnfinishedConstruction),
                typeof(Vehicle)
            };
            foreach (var type in list)
            {
                var createyEstTaxesMethod = typeof(EstateModel).GetMethod("CreateEstateTaxesConfig").MakeGenericMethod(type);
                createyEstTaxesMethod.Invoke(null, new object[] { context, viewModelConfigService });

                var modifyEstConfigMethod = typeof(EstateModel).GetMethod("ModifyEstConfig").MakeGenericMethod(type);
                modifyEstConfigMethod.Invoke(null, new[] { context });
            }

            context.CreateVmConfigOnBase<Estate>("Estate", "EstateAndEstateRegistrationObject");

            ModifyModelPropertyComplex(context, viewModelConfigService);

            context.ModifyVmConfig<InventoryObject>("InventoryObjectTree")
                 .ListView(lv => lv.Columns(cols => cols
                 .Add(c => c.Number, ac => ac.Visible(false))
                 .Add(c => c.EUSINumber, ac => ac.Visible(true).Order(-200))
                 ));

            context.CreateVmConfigOnBase<Estate>(nameof(Estate), "ArchivedEstate")
                .Service<IArchivedEstateService<Estate>>()
                .Title("ОИ помеченные на удаление")
                .ListView(lv => lv.Title("ОИ помеченные на удаление"));

            //архивные конфиги
            foreach (var type in list)
            {
                var estArchiveConfigMethod = typeof(EstateModel).GetMethod("CreateEstArchiveConfig").MakeGenericMethod(type);
                estArchiveConfigMethod.Invoke(null, new[] { context });
            }
        }

        /// <summary>
        /// Дефолтное представление DetailView для любого
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<T> EUSI_DetailView_Default<T>(this ViewModelConfigBuilder<T> conf) where T : Estate
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml

            var Source = "Source";
            var ER = "ER";

            conf.DetailView(dv => dv
            .DefaultSettings((uow, obj, model) =>
            {
                if (obj.IsArchived == true || Base.Ambient.AppContext.SecurityUser.IsRoleCode("ResponsibleER"))
                {
                    model.SetReadOnlyAll();
                    if (obj.IsArchived == true)
                        model.ChangeEditor(nameof(Estate.Comment), false, true, true);
                }
            })
            .Editors(eds => eds

            //Общие
            .AddProp("EUSINumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true))
            .AddProp("NameEUSI", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER))
            .AddProp("EstateDefinitionType", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).AddParam(Source, ER), () => !typeof(T).IsNMA())
            .AddProp("EstateMovableNSI", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true), () => !typeof(T).IsNMA())
            .AddProp("IntangibleAssetType", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).AddParam(Source, ER), () => typeof(T).IsNMA())
            .AddProp("NameByDoc", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER))
            .AddProp("SPPCode", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).AddParam(Source, ER), () => typeof(T).IsNKS())
            .AddProp("CadastralNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(false).AddParam(Source, ER), () => (Type.Equals(typeof(T), typeof(Cadastral)) || typeof(T).IsSubclassOf(typeof(Cadastral))))
            .AddProp("StartDateUse", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(false).AddParam(Source, ER), () => typeof(T).IsNKS())
            .AddProp("SibCountry", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER))
            .AddProp("SibFederalDistrict", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER), () => !typeof(T).IsNMA() && !typeof(T).IsOthers())
            .AddProp("SibRegion", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset)) && !typeof(T).IsOthers()))
            .AddProp("SibCityNSI", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER), () => (Type.Equals(typeof(T), typeof(RealEstate))))
            .AddProp("Address", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER), () => (Type.Equals(typeof(T), typeof(RealEstate))))
            .AddProp("FactAddress", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset))))
            .AddProp("ShareRightNumerator", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset)) && !Type.Equals(typeof(T), typeof(UnfinishedConstruction))))
            .AddProp("ShareRightDenominator", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset)) && !Type.Equals(typeof(T), typeof(UnfinishedConstruction))))
            .AddProp("OutOfBalance", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly())
            .AddProp("IsArchived", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true))
            .AddProp("Comment", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true))

            //без вкладки, бросаем в общие
            .AddProp("EstateType", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).AddParam(Source, ER), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset))))
            .AddProp("IsNonCoreAsset", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true))
            .AddProp("YearCommissionings", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset)) && !Type.Equals(typeof(T), typeof(UnfinishedConstruction))))
            .AddProp("PlotsBlock", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (Type.Equals(typeof(T), typeof(Land))))
            .AddProp("PlotsNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (Type.Equals(typeof(T), typeof(Land))))
            .AddProp("PassportNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(IntangibleAsset)) && !Type.Equals(typeof(T), typeof(UnfinishedConstruction))))
            .AddProp("Manufacturer", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (Type.Equals(typeof(T), typeof(Vehicle)) || Type.Equals(typeof(T), typeof(MovableEstate)) || Type.Equals(typeof(T), typeof(InventoryObject)) || Type.Equals(typeof(T), typeof(Estate))))
            .AddProp("ContainmentVolume", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(Land)) && (typeof(T).IsSubclassOf(typeof(RealEstate)) || typeof(T).IsOthers())))
            .AddProp("ConditionalNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => (!Type.Equals(typeof(T), typeof(UnfinishedConstruction)) && (typeof(T).IsSubclassOf(typeof(RealEstate)) || Type.Equals(typeof(T), typeof(RealEstate)))))
            .AddProp("Parent", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).Title("Принадлежность ИК").Mnemonic("IK"))
            .AddEmpty(eb =>
            {
                eb.EditorTemplate("ParentsBranch").Visible(true)
                                                   .TabName(EUSI.Helpers.EstateTabs.General)
                                                   .Title("Структура принадлежности ИК");
            })
            .AddProp("OwnershipType", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true), () => typeof(T).IsSubclassOf(typeof(RealEstate)) || Type.Equals(typeof(T), typeof(RealEstate)))
            .AddProp("EncumbranceExist", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).OnChangeClientScript("eusi.dv.editors.onChange.Estate_EncumbranceExist(form, isChange);"))
            .AddProp("EncumbranceContractNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true))
            .AddProp("EstateStatus", ac => ac.TabName(EUSI.Helpers.EstateTabs.General).Visible(true).IsReadOnly())

            //Классификаторы
            .AddProp("OKTMO", ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false), () => !Type.Equals(typeof(T), typeof(IntangibleAsset)))
            .AddProp("OKOF2014", ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false))
            .AddProp("DepreciationGroup", ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false), () => !Type.Equals(typeof(T), typeof(IntangibleAsset)))
            .AddProp("PositionConsolidation", ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false))
            .AddProp("AddonOKOF", ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false), () => !Type.Equals(typeof(T), typeof(IntangibleAsset)))
            .AddProp(nameof(Estate.AddonOKOF2014), ac => ac.TabName(EUSI.Helpers.EstateTabs.Class2).Visible(true).IsRequired(false), () => !Type.Equals(typeof(T), typeof(IntangibleAsset)))

            //Специфические
            .AddProp("PermittedUseKind", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(RealEstate)) || typeof(T).IsSubclassOf(typeof(RealEstate)))
            .AddProp("AddonAttributeGroundCategory", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Land)))
            .AddProp("UsesKind", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(RealEstate)) || typeof(T).IsSubclassOf(typeof(RealEstate)))
            .AddProp("GroundCategory", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Land)))
            .AddProp("LandType", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Land)))
            .AddProp("LandPurpose", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Land)))
            .AddProp("RegDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Cadastral)) || typeof(T).IsSubclassOf(typeof(Cadastral))) //Кадастровые
            .AddProp(nameof(Cadastral.RightRegNumber), ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => typeof(T).IsReals() || typeof(T).IsLand() || typeof(T).IsNKS())
            .AddProp(nameof(Cadastral.RightRegDate), ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => typeof(T).IsReals() || typeof(T).IsLand() || typeof(T).IsNKS())
            .AddProp(nameof(Cadastral.RightRegEndDate), ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => typeof(T).IsReals() || typeof(T).IsLand() || typeof(T).IsNKS())
            .AddProp("Area", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(RealEstate)) || typeof(T).IsSubclassOf(typeof(RealEstate)))
            .AddProp("BuildingLength", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !Type.Equals(typeof(T), typeof(Land)) && (typeof(T).IsOthers() || (Type.Equals(typeof(T), typeof(RealEstate)) || typeof(T).IsSubclassOf(typeof(RealEstate)))))
            .AddProp("PipelineLength", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !Type.Equals(typeof(T), typeof(Land)) && (typeof(T).IsOthers() || (Type.Equals(typeof(T), typeof(RealEstate)) || typeof(T).IsSubclassOf(typeof(RealEstate)))))

            //Специфические для ТС
            .AddProp("RegDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).IsRequired(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle))) //ТС
            .AddProp("DeRegDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("VehicleClass", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("TaxVehicleKindCode", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("YearOfIssue", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).IsRequired(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("VehicleCategory", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).IsRequired(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("DieselEngine", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("EngineType", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("VehicleLabel", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("SibMeasure", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsRequired(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("Power", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).IsRequired(true).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("SerialNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("EngineSize", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("Model", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("AverageCost", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("RegNumber", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsReadOnly(false).AddParam(Source, ER), () => Type.Equals(typeof(T), typeof(Vehicle)))
            .AddProp("VehicleTaxFactor", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsRequired(false), () => Type.Equals(typeof(T), typeof(Vehicle)))

            .AddProp("IsSocial", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true))
            .AddProp("IsCultural", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !typeof(T).IsNKS() && typeof(T).IsReals())
            .AddProp("Deposit", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true))
            .AddProp("LicenseArea", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true).IsRequired(false))
            .AddProp("WellCategory", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !typeof(T).IsLand() && (typeof(T).IsOthers() || typeof(T).IsReals()))
            .AddProp("Bush", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !Type.Equals(typeof(T), typeof(Land)) && (typeof(T).IsOthers() || typeof(T).IsReals()))
            .AddProp("Well", ac => ac.TabName(EUSI.Helpers.EstateTabs.Special3).Visible(true), () => !Type.Equals(typeof(T), typeof(Land)) && (typeof(T).IsOthers() || typeof(T).IsReals()))

            //Стоимостные
            .AddProp("DepreciationMethodRSBU", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true).IsRequired(false), () => !typeof(T).IsLand() && !typeof(T).IsNKS())
            .AddProp("DepreciationMethodNU", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true).IsRequired(false), () => !typeof(T).IsLand() && !typeof(T).IsNKS())
            .AddProp("DepreciationMethodMSFO", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true), () => !typeof(T).IsLand() && !typeof(T).IsNKS())
            .AddProp("DepreciationMultiplierForNU", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true).IsRequired(false), () => !typeof(T).IsNKS())
            .AddProp("CadastralValue", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true), () => (typeof(T).IsReals() || typeof(T).IsLand()) && !typeof(T).IsNKS())
            .AddProp("MarketCost", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true), () => typeof(T).IsReals() || typeof(T).IsLand() || typeof(T).IsTS())
            .AddProp("UsefulForRSBU", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true).IsRequired(false), () => !typeof(T).IsLand() && !typeof(T).IsNKS())
            .AddProp("UsefulForNU", ac => ac.TabName(EUSI.Helpers.EstateTabs.Cost4).Visible(true).IsRequired(false), () => !typeof(T).IsLand() && !typeof(T).IsNKS())

            //Договоры
            .AddProp("LessorSubject", ac => ac.TabName(EUSI.Helpers.EstateTabs.Contract5).Visible(true))
            .AddProp("ProprietorSubject", ac => ac.TabName(EUSI.Helpers.EstateTabs.Contract5).Visible(true))//где виден то?

            //Налоги
            .AddPartialEditor($"{typeof(T).Name}_{nameof(EstateTaxes)}", nameof(Estate.ID), nameof(EstateTaxes.TaxesOfID), peb => peb.TabName(EUSI.Helpers.EstateTabs.Tax6).IsLabelVisible(false), true)

            //fix Вид ТС
            .ChangeProp("VehicleType", ac => ac.IsRequired(false))

      ).DefaultSettings((uow, r, commonEditorViewModel) =>
                    {
                        InventoryObject inv = r as InventoryObject;
                        if (inv != null)
                        {
                            commonEditorViewModel.Required(p => (p as InventoryObject).EncumbranceContractNumber,
                                inv.EncumbranceExist);
                        }
                    })
                )
      .Config.DetailView.Editors
               .AddManyToMany(""
               , typeof(EstateAndEstateRegistrationObject)
               , typeof(IManyToManyRightAssociation<>)
               , Base.UI.Editors.ManyToManyAssociationType.Left
               , y => y.TabName("[018]Заявки ЕУСИ").Visible(true).Order(1).Mnemonic("EstateRegistrationRO"))
     ;
            return conf;
        }

        public static ViewModelConfigBuilder<T> EUSI_ListView_Default<T>(this ViewModelConfigBuilder<T> conf) where T : Estate
        {
            return conf.ListView(lv => lv
                   .Columns(eds => eds.Clear()
                   .AddProp("EUSINumber", ac => ac.Visible(true))
                   .AddProp("NameEUSI", ac => ac.Visible(true))
                   .AddProp("IsNonCoreAsset", ac => ac.Visible(true))
                   //.AddProp("InventoryNumber", ac => ac.Visible(true))
                   .AddProp("OKOF2014", ac => ac.Visible(true))
                   .AddProp("Owner", ac => ac.Visible(true))
                   .AddProp("WhoUse", ac => ac.Visible(true))
                   .AddProp("Name", ac => ac.Visible(true))
                   .AddProp("DealProps", ac => ac.Visible(true))
                   //.AddProp("InventoryNumber2", ac => ac.Visible(true))
                   .AddProp("OKTMO", ac => ac.Visible(true))
                   .AddProp("NameByDoc", ac => ac.Visible(true))
                   .AddProp("SibCountry", ac => ac.Visible(true))
                   .AddProp("SibFederalDistrict", ac => ac.Visible(true))
                   .AddProp("SibRegion", ac => ac.Visible(true))
                   .AddProp("SibCityNSI", ac => ac.Visible(true))
                   .AddProp("Address", ac => ac.Visible(true))
                   .AddProp("PositionConsolidation", ac => ac.Visible(true))
                   .AddProp("DepreciationMethodRSBU", ac => ac.Visible(true))

                   .AddProp("EstateDefinitionType", ac => ac.Visible(false))
                   .AddProp("EstateMovableNSI", ac => ac.Visible(false))
                   .AddProp("IntangibleAssetType", ac => ac.Visible(false))
                   .AddProp("SPPCode", ac => ac.Visible(false))
                   .AddProp("CadastralNumber", ac => ac.Visible(false))
                   .AddProp("StartDateUse", ac => ac.Visible(false))
                   .AddProp("FactAddress", ac => ac.Visible(false))
                   .AddProp("ShareRightNumerator", ac => ac.Visible(false))
                   .AddProp("ShareRightDenominator", ac => ac.Visible(false))
                   .AddProp("EstateType", ac => ac.Visible(false))
                   .AddProp("IsNonCoreAsset", ac => ac.Visible(false))
                   .AddProp("YearCommissionings", ac => ac.Visible(false))
                   .AddProp("PlotsBlock", ac => ac.Visible(false))
                   .AddProp("PlotsNumber", ac => ac.Visible(false))
                   .AddProp("PassportNumber", ac => ac.Visible(false))
                   .AddProp("Manufacturer", ac => ac.Visible(false))
                   .AddProp("ContainmentVolume", ac => ac.Visible(false))
                   .AddProp("ConditionalNumber", ac => ac.Visible(false))
                   .AddProp("Parent", ac => ac.Visible(false))
                   .AddProp("OwnershipType", ac => ac.Visible(false))
                   .AddProp("EncumbranceExist", ac => ac.Visible(false))
                   .AddProp("EncumbranceContractNumber", ac => ac.Visible(false))
                   .AddProp("DepreciationGroup", ac => ac.Visible(false))
                   .AddProp("AddonOKOF", ac => ac.Visible(false))
                   .AddProp("PermittedUseKind", ac => ac.Visible(false))
                   .AddProp("AddonAttributeGroundCategory", ac => ac.Visible(false))
                   .AddProp("UsesKind", ac => ac.Visible(false))
                   .AddProp("GroundCategory", ac => ac.Visible(false))
                   .AddProp("LandType", ac => ac.Visible(false))
                   .AddProp("LandPurpose", ac => ac.Visible(false))
                   .AddProp("RegDate", ac => ac.Visible(false))
                   .AddProp(nameof(Cadastral.RightRegDate), ac => ac.Visible(false))
                   .AddProp(nameof(Cadastral.RightRegEndDate), ac => ac.Visible(false))
                   .AddProp("Area", ac => ac.Visible(false))
                   .AddProp("BuildingLength", ac => ac.Visible(false))
                   .AddProp("PipelineLength", ac => ac.Visible(false))
                   .AddProp("RegDate", ac => ac.Visible(false))
                   .AddProp("DeRegDate", ac => ac.Visible(false))
                   .AddProp("VehicleClass", ac => ac.Visible(false))
                   .AddProp("TaxVehicleKindCode", ac => ac.Visible(false))
                   .AddProp("YearOfIssue", ac => ac.Visible(false))
                   .AddProp("VehicleCategory", ac => ac.Visible(false))
                   .AddProp("DieselEngine", ac => ac.Visible(false))
                   .AddProp("VehicleLabel", ac => ac.Visible(false))
                   .AddProp("SibMeasure", ac => ac.Visible(false))
                   .AddProp("Power", ac => ac.Visible(false))
                   .AddProp("SerialNumber", ac => ac.Visible(false))
                   .AddProp("EngineSize", ac => ac.Visible(false))
                   .AddProp("Model", ac => ac.Visible(false))
                   .AddProp("AverageCost", ac => ac.Visible(false))
                   .AddProp("RegNumber", ac => ac.Visible(false))
                   .AddProp("RightRegNumber", ac => ac.Visible(false))
                   .AddProp("IsSocial", ac => ac.Visible(false))
                   .AddProp("IsCultural", ac => ac.Visible(false))
                   .AddProp("Deposit", ac => ac.Visible(false))
                   .AddProp("LicenseArea", ac => ac.Visible(false))
                   .AddProp("WellCategory", ac => ac.Visible(false))
                   .AddProp("Bush", ac => ac.Visible(false))
                   .AddProp("Well", ac => ac.Visible(false))
                   .AddProp("DepreciationMethodNU", ac => ac.Visible(false))
                   .AddProp("DepreciationMethodMSFO", ac => ac.Visible(false))
                   .AddProp("CadastralValue", ac => ac.Visible(false))
                   .AddProp("MarketCost", ac => ac.Visible(false))
                   .AddProp("LessorSubject", ac => ac.Visible(false))
                   .AddProp("ProprietorSubject", ac => ac.Visible(false))
                   .AddProp("TaxRateType", ac => ac.Visible(false))
                   .AddProp("PeriodNU", ac => ac.Visible(false))
                   .AddProp("ReportPeriodNU", ac => ac.Visible(false))
                   .AddProp("TermOfPymentTaxRate", ac => ac.Visible(false))
                   .AddProp("TermOfPymentTaxRateTS", ac => ac.Visible(false))
                   .AddProp("TermOfPymentTaxRateLand", ac => ac.Visible(false))
                   .AddProp("DecisionsDetails", ac => ac.Visible(false))
                   .AddProp("DecisionsDetailsTS", ac => ac.Visible(false))
                   .AddProp("DecisionsDetailsLand", ac => ac.Visible(false))
                   .AddProp("Benefit", ac => ac.Visible(false))
                   .AddProp("IsEnergy", ac => ac.Visible(false))
                   .AddProp("EnergyLabel", ac => ac.Visible(false))
                   .AddProp("EnergyDocsExist", ac => ac.Visible(false))
                   .AddProp("BenefitApplyForEnergy", ac => ac.Visible(false))
                   .AddProp("BenefitDocsExist", ac => ac.Visible(false))
                   .AddProp("IsInvestmentProgramm", ac => ac.Visible(false))
                   .AddProp("TaxCadastralIncludeDate", ac => ac.Visible(false))
                   .AddProp("TaxCadastralIncludeDoc", ac => ac.Visible(false))
                   .AddProp("TaxBase", ac => ac.Visible(false))
                   .AddProp("TaxExemptionReason", ac => ac.Visible(false))
                   .AddProp("TaxExemptionReasonTS", ac => ac.Visible(false))
                   .AddProp("TaxExemptionReasonLand", ac => ac.Visible(false))
                   .AddProp("TaxRateWithExemption", ac => ac.Visible(false))
                   .AddProp("TaxRateWithExemptionTS", ac => ac.Visible(false))
                   .AddProp("TaxRateWithExemptionLand", ac => ac.Visible(false))
                   .AddProp("TaxExemptionStartDate", ac => ac.Visible(false))
                   .AddProp("TaxExemptionStartDateTS", ac => ac.Visible(false))
                   .AddProp("TaxExemptionStartDateLand", ac => ac.Visible(false))
                   .AddProp("TaxExemptionEndDate", ac => ac.Visible(false))
                   .AddProp("TaxExemptionEndDateTS", ac => ac.Visible(false))
                   .AddProp("TaxExemptionEndDateLand", ac => ac.Visible(false))
                   .AddProp("OutOfBalance", ac => ac.Visible(false))
                   .AddProp("IsArchived", ac => ac.Visible(false))
                   .AddProp("EstateStatus", ac => ac.Visible(true))
                   .AddProp("Comment", ac => ac.Visible(false))

             ).ColumnsFrom<EstateTaxes>($"{typeof(T).Name}_{nameof(EstateTaxes)}", nameof(Estate.ID), nameof(EstateTaxes.TaxesOfID))
            );
        }

        public static void CreateEstateTaxesConfig<T>(this IInitializerContext context, IViewModelConfigService viewModelConfigService) where T : Estate
        {
            var mnemonic = $"{typeof(T).Name}_{nameof(EstateTaxes)}";
            //Добавил проверку, на случай если конфиг уже создан в corpprop

            ViewModelConfigBuilder<EstateTaxes> builder = null;
            if (!viewModelConfigService.GetAll().Any(c => c.Mnemonic == mnemonic))
                builder = context.CreateVmConfig<EstateTaxes>(mnemonic);
            else
                builder = context.ModifyVmConfig<EstateTaxes>(mnemonic);

            builder.DetailView(dv => dv.Editors(ed =>
                ed
                    .AddProp("TaxRateType", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(false),
                        () => !typeof(T).IsNMA())
                    .AddProp("DecisionsDetails", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("DecisionsDetailsTS", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsTS())
                    .AddProp("DecisionsDetailsLand", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsLand())
                    .AddProp("Benefit", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("IsEnergy", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsOthers())
                    .AddProp("EnergyLabel", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsOthers())
                    .AddProp("EnergyDocsExist", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsOthers())
                    .AddProp("BenefitApplyForEnergy", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsOthers())
                    .AddProp("BenefitDocsExist", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsNMA())
                    .AddProp("IsInvestmentProgramm", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsNMA())
                    .AddProp("TaxCadastralIncludeDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsReals() || typeof(T).IsLand())
                    .AddProp("TaxCadastralIncludeDoc", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsReals() || typeof(T).IsLand())
                    .AddProp("TaxBase", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("TaxExemptionReason", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("TaxExemptionReasonTS", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsTS())
                    .AddProp("TaxExemptionReasonLand", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsLand())
                    .AddProp("TaxRateWithExemption", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("TaxRateWithExemptionTS", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsTS())
                    .AddProp("TaxRateWithExemptionLand", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true))
                    .AddProp("TaxExemptionStartDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("TaxExemptionStartDateTS", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsTS())
                    .AddProp("TaxExemptionStartDateLand", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsLand())
                    .AddProp("TaxExemptionEndDate", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => !typeof(T).IsLand() && !typeof(T).IsNMA())
                    .AddProp("TaxExemptionEndDateTS", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsTS())
                    .AddProp("TaxExemptionEndDateLand", ac => ac.TabName(EUSI.Helpers.EstateTabs.Tax6).Visible(true),
                        () => typeof(T).IsLand())
            ));
        }

        public static void CreateEstArchiveConfig<T>(this IInitializerContext context) where T : Estate
        {
            context.CreateVmConfigOnBase<T>(typeof(T).Name, typeof(T).Name + "Archive")
                .Service<IGetAllService<T>>();
        }

        /// <summary>
        /// Изменяет все конфиги по имуществу для заданного типа объекта.
        /// </summary>
        /// <typeparam name="T">Тип ОИ.</typeparam>
        /// <param name="context">Контекст.</param>
        public static void ModifyEstConfig<T>(this IInitializerContext context) where T : Estate
        {
            var configs = context.GetVmConfigs()
              .Where(w => w.Value != null && Type.Equals(w.Value.TypeEntity, typeof(T)))
              .DefaultIfEmpty()
              .Select(s => s.Value)
              .ToList();

            foreach (var conf in configs)
            {
                context.ModifyVmConfig<T>(conf.Mnemonic)
                 .EUSI_DetailView_Default()
                 .EUSI_ListView_Default()
                 .LookupProperty(x => x.Text(t => t.EUSINumber))
                 .ChangePropNames();

                foreach (var editor in conf.DetailView.Editors
                       .Where(e =>
                       Type.Equals(e.EditorType, typeof(AccountingObject))
                       && (e is OneToManyAssociationEditor || e is Base.UI.Editors.ManyToManyAssociationEditor))
                       )
                {
                    editor.Title = editor.Title.Replace("ОБУ", "ОС/НМА");
                    editor.TabName = editor.TabName.Replace("ОБУ", "ОС/НМА");
                    editor.Title = editor.Title.Replace("Объекты БУ", "ОС/НМА");
                    editor.TabName = editor.TabName.Replace("Объекты БУ", "ОС/НМА");
                }
            }
        }

        /// <summary>
        /// Изменение модели ИК.
        /// </summary>
        /// <param name="context"></param>
        public static void ModifyModelPropertyComplex(IInitializerContext context, IViewModelConfigService viewModelConfigService)
        {
            var configs = context.GetVmConfigs()
                .Where(w => w.Value != null && Type.Equals(w.Value.TypeEntity, typeof(PropertyComplexIO)))
                .DefaultIfEmpty()
                .Select(s => s.Value)
                .ToList();

            foreach (var conf in configs)
            {
                context.ModifyVmConfig<PropertyComplexIO>(conf.Mnemonic)
                    .DetailView(dv => dv.Editors(eds => eds
                        .Add(ed => ed.NameTIS, ac => ac.Visible(true).TabName(CorpProp.Helpers.EstateTabs.GeneralInfo))
                        .Add(ed => ed.Country, ac => ac.Visible(true).Mnemonic("SibCountryPropertyComplexIO").TabName(CorpProp.Helpers.EstateTabs.Address6).Order(10).OnChangeClientScript(@"eusi.dv.editors.onChange.ER_ChangeIKCountry(form, isChange);"))
                        .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).Mnemonic("SibFederalDistrictPropertyComplexIO").TabName(CorpProp.Helpers.EstateTabs.Address6).Order(11))
                        .Add(a => a.SibRegion, h => h.Title("Субъект РФ").TabName(EstateTabs.Address6).Visible(true).IsReadOnly(false).IsRequired(true).Mnemonic("SibRegionPropertyComplexIO").Order(12))
                        .Add(ed => ed.Address, ac => ac.Visible(true).IsRequired(true).TabName(CorpProp.Helpers.EstateTabs.Address6).Order(13))
                        ))
                    .ListView(lv => lv.Columns(col => col.Add(c => c.NameTIS, ac => ac.Visible(true))));
            }

            var ikConf = context.GetVmConfigs()
                .Where(w => w.Value != null && Type.Equals(w.Value.TypeEntity, typeof(InventoryObject))
                && w.Key == "IK")
                .DefaultIfEmpty()
                .Select(s => s.Value)
                .FirstOrDefault();

            if (ikConf != null)
                ikConf.LookupProperty = new LookupProperty() { Text = "NameEUSI" };
        }

        /// <summary>
        /// Изменяет значения DetailView и ListView атрибутов свойств объектов имущества.
        /// </summary>
        public static ViewModelConfigBuilder<T> ChangePropNames<T>(this ViewModelConfigBuilder<T> conf) where T : Estate
        {
            //в ЕУСИ изменим наименования

            var props = new Dictionary<string, string>()
            {
                {"AddonAttributeGroundCategory", "Доп. признак категории земель"},
                {"AddonOKOF", "Доп. Код ОКОФ"},
                {nameof(Estate.AddonOKOF2014), "Доп. Код ОКОФ2"},
                {"Address", "Адрес"},
                {"Area", "Площадь объекта недвижимости, кв.м."},
                {"AverageCost", "Средняя стоимость ТС, руб."},
                {"Benefit", "Льготируемый объект"},
                {"BenefitApplyForEnergy", "Применение льготы для энегроэффективного оборудования"},
                {"BenefitDocsExist", "Наличие документов, подтверждающих  применение льготы"},
                {"BuildingLength", "Длина линейного сооружения, м."},
                {"Bush", "№ куста"},
                {"CadastralNumber", "Кадастровый номер"},
                {"CadastralValue", "Кадастровая стоимость, руб."},
                {"ConditionalNumber", "Условный номер"},
                {"ContainmentVolume", "Объем резервуара, куб.м."},
                {"DealProps", "Реквизиты договора"},
                {"DecisionsDetails", "Реквизиты решения органа субъектов/муниципальных образований по налогу на имущество"},
                {"DecisionsDetailsLand", "Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу"},
                {"DecisionsDetailsTS", "Реквизиты решения органа субъектов/муниципальных образований  по транспортному налогу"},
                {"Deposit", "Месторождение (номер)"},
                {"DepreciationGroup", "Амортизационная группа НУ"},
                {"DepreciationMethodMSFO", "Метод амортизации (МСФО)"},
                {"DepreciationMethodNU", "Метод амортизации (НУ)"},
                {"DepreciationMethodRSBU", "Метод амортизации (РСБУ)"},
                {"DeRegDate", "Дата снятия с учета ТС в государственных органах"},
                {"DieselEngine", "Дизельный двигатель"},
                {"EncumbranceContractNumber", "Номер и дата документа обременения"},
                {"EncumbranceExist", "Обременение"},
                {"EnergyDocsExist", "Наличие документов, подтверждающих энергоэффективность оборудования"},
                {"EnergyLabel", "Класс энергетической эффективности"},
                {"EngineSize", "Объем двигателя, л."},
                {"EstateDefinitionType", "Тип Объекта имущества"},
                {"EstateMovableNSI", "Признак движимое/недвижимое имущество по данным БУ"},
                {"EstateType", "Класс КС"},
                {"EUSINumber", "Номер ЕУСИ"},
                {"FactAddress", "Фактическое местонахождение объекта"},
                {"GroundCategory", "Код категории земель"},
                {"IntangibleAssetType", "Вид НМА"},
                {"InventoryNumber", "Инвентарный номер"},
                {"InventoryNumber2", "Инвентарный номер в старой БУС"},
                {"IsCultural", "Отнесение к категории памятников истории и культуры"},
                {"IsEnergy", "Энергоэффективное оборудование"},
                {"IsInvestmentProgramm", "Имущество, созданное по инвестиционной программе (в соответствии с программой развития регионов)"},
                {"IsNonCoreAsset", "Признак ННА"},
                {"IsSocial", "Признак объекта социально-культурного или бытового назначения"},
                {"LandPurpose", "Назначение ЗУ"},
                {"LandType", "Тип ЗУ"},
                {"LessorSubject", "Арендатор (Лизингополучатель) / Пользователь по договору"},
                {"LicenseArea", "Лицензионный участок"},
                {"Manufacturer", "Изготовитель"},
                {"MarketCost", "Рыночная стоимость, руб."},
                {"Model", "Марка ТС"},
                {"Name", "Наименование объекта (в соответствии с учетной системой)"},
                {"NameByDoc", "Наименование объекта (в соответствии с документами)"},
                {"NameEUSI", "Наименование ЕУСИ"},
                {"OKOF2014", "ОКОФ"},
                {"OKTMO", "Код ОКТМО"},
                {"OwnershipType", "Форма собственности"},
                {"Parent", "Принадлежность ИК"},
                {"PassportNumber", "Номер паспорта объекта"},
                {"PermittedUseKind", "Вид разрешенного использования"},
                {"PipelineLength", "Длина трубопровода, м."},
                {"PlotsBlock", "Номер квартала"},
                {"PlotsNumber", "Номер выдела"},
                {"PositionConsolidation", "Позиция консолидации"},
                {"Power", "Налоговая база. \"Транспортный налог (мощность ТС, валовая вместимость, паспортная стат. тяга, единица ТС)\""},
                {"ProprietorSubject", "Арендодатель (Лизингодатель)/Собственник по договору"},
                { "RegNumber", "Номер госрегистрации ТС"},
                { nameof(Cadastral.RightRegNumber), "Номер записи гос. регистрации"},
                { nameof(Cadastral.RightRegDate), "Дата гос. регистрации права"},
                { nameof(Cadastral.RightRegEndDate), "Дата гос. регистрации прекращения права"},
                { "SerialNumber", "Серийный номер/Заводской номер/ВИН"},
                { "ShareRightDenominator", "Доля в праве (знаменатель доли)"},
                { "ShareRightNumerator", "Доля в праве (числитель доли)"},
                { "SibCityNSI", "Город/Населенный пункт"},
                { "SibCountry", "Страна"},
                { "SibFederalDistrict", "Федеральный округ РФ"},
                { "SibMeasure", "Единицы измерения налоговой базы. Транспортный налог"},
                { "SibRegion", "Субъект РФ/Регион"},
                { "SPPCode", "Код БИП"},
                { "StartDateUse", "Дата начала использования (НКС)"},
                { "TaxBase", "Выбор базы налогообложения (ЕУСИ)"},
                { "TaxCadastralIncludeDate", "Дата включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения"},
                { "TaxCadastralIncludeDoc", "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения"},
                { "TaxExemptionEndDate", "Дата окончания действия льготных условий налогообложения. Налог на имущество (ЕУСИ)"},
                { "TaxExemptionEndDateLand", "Дата окончания действия льготных условий налогообложения. Земельный налог (ЕУСИ)"},
                { "TaxExemptionEndDateTS", "Дата окончания действия льготных условий налогообложения. Транспортный налог (ЕУСИ)"},
                { "TaxExemptionReason", "Причина налоговой льготы. Налог на имущество (ЕУСИ)"},
                { "TaxExemptionReasonLand", "Причина налоговой льготы. Земельный налог (ЕУСИ)"},
                { "TaxExemptionReasonTS", "Причина налоговой льготы. Транспортный налог (ЕУСИ)"},
                { "TaxExemptionStartDate", "Дата начала действия льготных условий налогообложения. Налог на имущество (ЕУСИ)"},
                { "TaxExemptionStartDateLand", "Дата начала действия льготных условий налогообложения. Земельный налог (ЕУСИ)"},
                { "TaxExemptionStartDateTS", "Дата начала действия льготных условий налогообложения. Транспортный налог (ЕУСИ)"},
                { "TaxRateType", "Наименование налога"},
                { "TaxRateWithExemption", "Налоговая ставка с учетом применяемых льгот, %. Налог на имущество (ЕУСИ)"},
                { "TaxRateWithExemptionLand", "Налоговая ставка с учетом применяемых льгот, %. Земельный налог (ЕУСИ)"},
                { "TaxRateWithExemptionTS", "Налоговая ставка с учетом применяемых льгот, % (ЕУСИ). Налог на имущество (ЕУСИ)"},
                { "TaxVehicleKindCode", "Код вида ТС"},
                { "UsesKind", "Разрешенное использование"},
                { "VehicleCategory", "Категория ТС"},
                { "VehicleClass", "Единый классификатор транспортных средств"},
                { "VehicleLabel", "Класс ТС"},
                { "VehicMarketCost", "Рыночная стоимость, руб."},
                { "Well", "№ скважины"},
                { "WellCategory", "Категория скважины"},
                { "YearCommissionings", "Год постройки"},
                { "YearOfIssue", "Год выпуска ТС"},
                { "OutOfBalance", "За балансом" }
            };

            if (typeof(T).IsSubclassOf(typeof(Cadastral)) || Type.Equals(typeof(Cadastral), (typeof(T))))
                props.Add("RegDate", "Дата постановки на государственный кадастровый учет");

            if (Type.Equals(typeof(Vehicle), (typeof(T))))
                props.Add("RegDate", "Дата регистрации ТС в государственных органах");

            foreach (var ed in conf.Config.DetailView.Editors)
            {
                if (!String.IsNullOrEmpty(ed.PropertyName) && props.Any(f => f.Key == ed.PropertyName))
                    ed.Title = props[ed.PropertyName];
            }

            foreach (var col in conf.Config.ListView.Columns)
            {
                if (!String.IsNullOrEmpty(col.PropertyName) && props.Any(f => f.Key == col.PropertyName))
                    col.Title = props[col.PropertyName];
            }

            return conf;
        }

        /// <summary>
        /// Возвращает признак принадлежности типа к категории "Прочие".
        /// </summary>
        /// <param name="tt"></param>
        /// <returns></returns>
        public static bool IsOthers(this Type tt)
        {
            var Others = new List<Type> { typeof(Estate), typeof(InventoryObject), typeof(MovableEstate) };
            return Others.Contains(tt);
        }

        /// <summary>
        /// Возвращает признак принадлежности типа к категории "Недвижимость (кроме земельных участков)".
        /// </summary>
        /// <param name="tt"></param>
        /// <returns></returns>
        public static bool IsReals(this Type tt)
        {
            return
                (!Type.Equals(tt, typeof(Land))
                && !Type.Equals(tt, typeof(UnfinishedConstruction))
                && (Type.Equals(tt, typeof(RealEstate)) || tt.IsSubclassOf(typeof(RealEstate))));
        }

        public static bool IsTS(this Type tt)
        {
            return Type.Equals(tt, typeof(Vehicle));
        }

        public static bool IsLand(this Type tt)
        {
            return Type.Equals(tt, typeof(Land));
        }

        public static bool IsNKS(this Type tt)
        {
            return Type.Equals(tt, typeof(UnfinishedConstruction));
        }

        public static bool IsNMA(this Type tt)
        {
            return Type.Equals(tt, typeof(IntangibleAsset));
        }
    }
}