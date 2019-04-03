using Base;
using Base.Extensions;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.NSI;
using EUSI.Entities.Audit;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Services.Estate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUSI.Model
{
    public static class EstateRegistrationModel
    {
        private const string EstateRegistration_ERControlDateAttributes = nameof(EstateRegistration_ERControlDateAttributes);

        public static void Init(IInitializerContext context, IViewModelConfigService viewModelConfigService)
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml
            var Source = "Source";
            var ER = "ER";

            context.CreateVmConfig<ERControlDateAttributes>(EstateRegistration_ERControlDateAttributes)
                .DetailView(dv => dv.Editors(e => e
                    .Add(ed => ed.DateCDS, ac => ac.Visible(true))
                    .Add(ed => ed.DateСreation, ac => ac.Visible(true))
                    .Add(ed => ed.DateToVerify, ac => ac.Visible(true))
                    .Add(ed => ed.DateVerification, ac => ac.Visible(true))
                    .Add(ed => ed.DateRejection, ac => ac.Visible(true))
                    .Add(ed => ed.DateToСlarify, ac => ac.Visible(true))
                    .Add(ed => ed.ERRemainingTime, ac => ac.Visible(true))
                    )
                   .DefaultSettings((uow, obj, model) =>
                   {
                       if (obj.ID == 0)
                           return;
                       var erState = uow.GetRepository<EstateRegistration>()
                       .FilterAsNoTracking(f => !f.Hidden && f.ERControlDateAttributesID == obj.ID)
                       .Include(i => i.State)
                       .FirstOrDefault()?.State?.Code;

                       if (erState != "DIRECTED")
                           model.SetReadOnlyAll(true);
                       else
                           model.ReadOnly(r => r.DateCDS, false);
                   })
                  );

            context.CreateVmConfig<EstateRegistrationRow>(nameof(EstateRegistrationRow))
                  .Service<IERRowService>()
                  .Title("Строка заявки на регистрацию");

            context.CreateVmConfig<EstateRegistrationRow>("ER_OS")
                 .Service<IERRowService>()
                 .Title("Основные средства (кроме аренды)")
                 .DetailView(dv => dv.Title("Основное средство (кроме аренды)")
                 .Editors(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeFederalDistrict(form, isChange);")
                            .CascadeFrom(x => x.SibCountry, x => x.SibFederalDistrict.CountryID, false))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeRegion(form, isChange);")
                            .CascadeFrom(x => x.SibFederalDistrict, x => x.SibRegion.FederalDistrictID, false))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeSibCity(form, isChange);")
                            .CascadeFrom(x => x.SibRegion, x => x.SibCityNSI.SibRegionID, false))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true).Mnemonic("EstateTypeFiltered").SetCustomsParams(@" customParams: editor.getForm().getPr('ID') "))

                 .Add(ed => ed.EUSINumber, ac => ac.Visible(false))
                 .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(false))
                 .Add(ed => ed.StartDateUse, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleRegDate, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleYearOfIssue, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleCategory, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleDieselEngine, ac => ac.Visible(false))
                 .Add(ed => ed.VehiclePowerMeasure, ac => ac.Visible(false))
                 .Add(ed => ed.VehiclePower, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleSerialNumber, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleEngineSize, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleModel, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleRegNumber, ac => ac.Visible(false))
                 .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                 )
                 .DefaultSettings((uow, obj, model) =>
                 {
                     DefaultSettingsERows(uow, obj, model);
                 })
                 )
                 .ListView(lv => lv.Title("Основные средства (кроме аренды)")
                 .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink })
                 .Columns(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true))
                 ))
                 .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<EstateRegistrationRow>("ER_NMA")
                 .Service<IERRowService>()
                 .Title("Нематериальные активы")
                 .DetailView(dv => dv.Title("Нематериальный актив")
                 .Editors(eds => eds.Clear()
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(false).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeFederalDistrict(form, isChange);"))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeRegion(form, isChange);"))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeSibCity(form, isChange);"))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true).Mnemonic("EstateTypeFiltered").SetCustomsParams(@" customParams: editor.getForm().getPr('ID') "))
                 .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                 )
                 .DefaultSettings((uow, obj, editor) =>
                 {
                     DefaultSettingsERows(uow, obj, editor);
                 })
                 )
                 .ListView(lv => lv.Title("Нематериальные активы")
                 .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink })
                 .Columns(eds => eds.Clear()

                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(true))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true))
                 ))
                 .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<EstateRegistrationRow>("ER_NKS")
                 .Service<IERRowService>()
                 .Title("Незавершенное капитальное строительство")
                 .DetailView(dv => dv.Title("Незавершенное капитальное строительство")
                 .Editors(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.StartDateUse, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).Title("Инвентарный номер"))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeFederalDistrict(form, isChange);"))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeRegion(form, isChange);"))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeSibCity(form, isChange);"))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true).Mnemonic("EstateTypeFiltered").SetCustomsParams(@" customParams: editor.getForm().getPr('ID') "))
                 .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                 )
                 .DefaultSettings((uow, obj, editor) =>
                 {
                     DefaultSettingsERows(uow, obj, editor);
                 })
                 )
                 .ListView(lv => lv.Title("Незавершенное капитальное строительство")
                 .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink })
                 .Columns(eds => eds.Clear()

                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.StartDateUse, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).Title("Инвентарный номер"))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true))
                 ))
                 .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<EstateRegistrationRow>("ER_ArendaOS")
                 .Service<IERRowService>()
                 .Title("Аренда ОС")
                 .DetailView(dv => dv.Title("Аренда ОС")
                 .Editors(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeFederalDistrict(form, isChange);"))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeRegion(form, isChange);"))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeSibCity(form, isChange);"))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true).Mnemonic("EstateTypeFiltered").SetCustomsParams(@" customParams: editor.getForm().getPr('ID') "))

                 .Add(ed => ed.EUSINumber, ac => ac.Visible(false))
                 .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(false))
                 .Add(ed => ed.StartDateUse, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleRegDate, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleYearOfIssue, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleCategory, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleDieselEngine, ac => ac.Visible(false))
                 .Add(ed => ed.VehiclePowerMeasure, ac => ac.Visible(false))
                 .Add(ed => ed.VehiclePower, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleSerialNumber, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleEngineSize, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleModel, ac => ac.Visible(false))
                 .Add(ed => ed.VehicleRegNumber, ac => ac.Visible(false))
                 .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                 )
                 .DefaultSettings((uow, obj, model) =>
                 {
                     DefaultSettingsERows(uow, obj, model);
                 })
                 )
                 .ListView(lv => lv.Title("Аренда ОС")
                 .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink })
                 .Columns(eds => eds.Clear()

                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                 .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.CadastralNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                 .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true))
                 .Add(ed => ed.SibRegion, ac => ac.Visible(true))
                 .Add(ed => ed.SibCityNSI, ac => ac.Visible(true))
                 .Add(ed => ed.Address, ac => ac.Visible(true))
                 .Add(ed => ed.EstateType, ac => ac.Visible(true))
                 ))
                 .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<EstateRegistrationRow>("ER_VGP")
                 .Service<IERRowService>()
                 .Title("Внутригрупповое перемещение")
                 .DetailView(dv => dv.Title("Внутригрупповое перемещение")
                 .Editors(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                 .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                 )
                  .DefaultSettings((uow, obj, model) =>
                  {
                      DefaultSettingsERows(uow, obj, model);
                  })
                 )
                 .ListView(lv => lv.Title("Внутригрупповое перемещение")
                 .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink })
                 .Columns(eds => eds.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                 ))
                 .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfigOnBase<EstateRegistrationRow>("ER_OS", "ClaimObject")
                .DetailView(dv => dv
                .Title("Объект заявки")
                .Editors(eds => eds
                .Add(ed => ed.Position, ac => ac.Visible(false))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(false).IsReadOnly(true))
                .Add(ed => ed.NameEstateByDoc, ac => ac.IsReadOnly(false))
                .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                ).AlwaysEdit(true)
                .DefaultSettings((uow, obj, model) =>
                 {
                     DefaultSettingsClaim(uow, obj, model);
                 }))
                .ListView(lv => lv.Columns(cols => cols.Add(c => c.EstateTypeStr, ac => ac.Visible(true))))
                .LookupProperty(lp => lp.Text(t => t.EstateTypeStr));

            context.CreateVmConfig<EstateRegistrationRow>("ER_Union")
                .Service<IERRowService>()
                .Title("Объединение")
                .DetailView(dv => dv.Title("Объединение")
                .Editors(eds => eds.Clear()
                    .Add(ed => ed.Position, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                    .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                    .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                    .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER).Title("Инвентарный номер"))
                    .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                    .Add(ed => ed.NameEUSI, ac => ac.Visible(true))
                    .Add(ed => ed.CadastralNumber, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                    .Add(ed => ed.SibCountry, ac => ac.Visible(true))
                    .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeFederalDistrict(form, isChange);"))
                    .Add(ed => ed.SibRegion, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeRegion(form, isChange);"))
                    .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).OnChangeClientScript(@" eusi.dv.editors.onChange.ER_ChangeSibCity(form, isChange);"))
                    .Add(ed => ed.Address, ac => ac.Visible(true))
                    .Add(ed => ed.EstateType, ac => ac.Visible(true).Mnemonic("EstateTypeFiltered").SetCustomsParams(@" customParams: editor.getForm().getPr('ID') "))

                    .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(false))
                    .Add(ed => ed.StartDateUse, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleRegDate, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleYearOfIssue, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleCategory, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleDieselEngine, ac => ac.Visible(false))
                    .Add(ed => ed.VehiclePowerMeasure, ac => ac.Visible(false))
                    .Add(ed => ed.VehiclePower, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleSerialNumber, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleEngineSize, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleModel, ac => ac.Visible(false))
                    .Add(ed => ed.VehicleRegNumber, ac => ac.Visible(false))
                    .Add(ed => ed.Comment, ac => ac.Visible(true).IsReadOnly())
                    )
                    .DefaultSettings((uow, obj, model) =>
                    {
                        DefaultSettingsERows(uow, obj, model);
                    })
                )
                .ListView(lv => lv.Title("Объекты заявки")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete, LvAction.Link, LvAction.Unlink, LvAction.Edit })
                .Columns(cols => cols.Clear()
                 .Add(ed => ed.Position, ac => ac.Visible(true))
                 .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).Title("Инвентарный номер"))
                 .Add(ed => ed.NameEstateByDoc, ac => ac.Visible(true))
                ));

            context.CreateVmConfigOnBase<EstateRegistrationRow>("ER_Union", "ER_Division")
               .Service<IERRowService>()
               .Title("Разукрупнение")
               .DetailView(dv => dv.Title("Разукрупнение"))
               .ListView(lv => lv.Title("Объекты заявки"));

            context.CreateVmConfig<EstateRegistration>(nameof(EstateRegistration))
                  .Service<IEstateRegistrationService>()
                  .Title("Заявка на регистрацию")
                  .DetailView_Default()
                  .ListView_Default()
                  .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<ERImportWizard>(nameof(ERImportWizard))
                .Service<Base.Service.IBaseObjectService<ERImportWizard>>()
                .Title("Параметры импорта заявки на регистрацию ОИ")
                .DetailView(dv => dv.Title("Параметры импорта заявки на регистрацию ОИ")
                .Editors(eds => eds.Add(ed => ed.FileCard, ac => ac.Mnemonic("FileCardTree"))))
                .LookupProperty(x => x.Text(t => t.ID));
        }

        /// <summary>
        /// Конфигурация карточки детального просмотра заявки на регистрацию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<EstateRegistration> DetailView_Default(this ViewModelConfigBuilder<EstateRegistration> conf)
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml
            var Source = "Source";
            var ER = "ER";

            return conf.DetailView(dv => dv
                   .Title("Заявка на регистрацию")
                   .Editors(eds => eds.Clear()
                   .Add(ed => ed.Urgently, ac => ac.Visible(true).IsReadOnly(true))
                   .Add(ed => ed.State, ac => ac.Visible(true).IsReadOnly(true))
                   .Add(ed => ed.Number, ac => ac.Visible(true).IsReadOnly(true))
                   .Add(ed => ed.NumberCDS, ac => ac.Visible(true).IsReadOnly(false))
                   .Add(ed => ed.Originator, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.ContactEmail, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.ContactPhone, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.ERType, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.Society, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.ERReceiptReason, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly(true).AddParam(Source, ER))
                   .Add(ed => ed.FileCard, ac => ac.Visible(true).IsReadOnly(true))
                   .Add(ed => ed.Comment, ac => ac.Visible(true))
                   .Add(ed => ed.NotActual, ac => ac.Visible(true))
                   .Add(ed => ed.ERContractNumber, ac => ac.Visible(true))
                   .Add(ed => ed.ERContractDate, ac => ac.Visible(true))
                   .Add(ed => ed.ERControlDateAttributes, ac => ac.Visible(false))

                   .AddManyToManyRigthAssociation<FileCardAndEstateRegistrationObject>("FileCard_EstateRegistrationObject",
                         edt => edt
                         .TabName("[002]Прилагаемые документы")
                         .Mnemonic("EstateRegistration_Files")
                         )

                    .AddOneToManyAssociation<EstateRegistrationRow>("ER_OS",
                      editor => editor
                      .TabName("[003]Основные средства (кроме аренды)")
                      .Mnemonic("ER_OS")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.OS;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.OS))
                      )

                   .AddOneToManyAssociation<EstateRegistrationRow>("ER_NMA",
                      editor => editor
                      .TabName("[004]Нематериальные активы")
                      .Mnemonic("ER_NMA")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.NMA;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.NMA)))

                   .AddOneToManyAssociation<EstateRegistrationRow>("ER_NKS",
                      editor => editor
                      .TabName("[005]Незавершенное кап. стр.")
                      .Mnemonic("ER_NKS")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.NKS;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.NKS)))

                   .AddOneToManyAssociation<EstateRegistrationRow>("ER_ArendaOS",
                      editor => editor
                      .TabName("[006]Аренда ОС")
                      .Mnemonic("ER_ArendaOS")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.ArendaOS;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.ArendaOS)))

                   .AddOneToManyAssociation<EstateRegistrationRow>("ER_VGP",
                      editor => editor
                      .TabName("[007]Внутригрупповые перемещения")
                      .Mnemonic("ER_VGP")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.VGP;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.VGP)))

                    .Add(x => x.ClaimObject, s => s.TabName("[008]Объединение").IsReadOnly(true).Mnemonic("ClaimObject").Visible(false))

                    .AddOneToManyAssociation<EstateRegistrationRow>("ER_Union",
                      editor => editor
                      .TabName("[008]Объединение")
                      .Mnemonic("ER_Union")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.Union;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.Union)))

                  .AddOneToManyAssociation<EstateRegistrationRow>("ER_Division",
                      editor => editor
                      .TabName("[009]Разукрупнение")
                      .Mnemonic("ER_Division")
                      .Create((uow, obj, id) =>
                      {
                          obj.EstateRegistrationID = id;
                          obj.EstateRegistration = uow.GetRepository<EstateRegistration>().Find(id);
                          obj.RowType = EstateRegistrationRowType.Division;
                      })
                      .Delete((uow, obj, id) =>
                      {
                          obj.EstateRegistration = null;
                          obj.EstateRegistrationID = null;
                      })
                      .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.EstateRegistrationID == id
                      && w.RowType == EstateRegistrationRowType.Division)))

                   .AddManyToManyRigthAssociation<EstateAndEstateRegistrationObject>("ER_Estates",
                         edt => edt
                         .TabName("[010]Объекты имущества")
                         .IsReadOnly())
                   .AddManyToManyRigthAssociation<AccountingObjectAndEstateRegistrationObject>("ER_AccObjs",
                         edt => edt
                         .TabName("[011]Объекты ОС/НМА")
                         .IsReadOnly())

                   .AddOneToManyAssociation<CustomDiffItem>("ER_CustomDiffItems",
                      editor => editor
                      .TabName("[012]Аудит изменений")
                      .FilterExtended((uofw, q, id, oid) =>
                      {
                          var t = typeof(EstateRegistration).GetTypeName();
                          return q.Where(w => w.EntityID == id && w.EntityType == t);
                      }))

                    .AddPartialEditor(EstateRegistration_ERControlDateAttributes, nameof(EstateRegistration.ERControlDateAttributesID), nameof(IBaseObject.ID)
                                    , ac => ac.TabName("[013]Контроль исполнения"))

                   )
                   .DefaultSettings((uow, obj, editor) =>
                   {
                       if (obj.ID == 0)
                           return;

                       editor.Visible("ER_OS", false);
                       editor.Visible("ER_NMA", false);
                       editor.Visible("ER_NKS", false);
                       editor.Visible("ER_ArendaOS", false);
                       editor.Visible("ER_VGP", false);
                       editor.Visible("ER_Union", false);
                       editor.Visible("ER_Division", false);
                       editor.Visible(x => x.ClaimObject, false);
                       if (obj.ERTypeID != null)
                       {
                           var code = uow.GetRepository<EstateRegistrationTypeNSI>()
                           .FilterAsNoTracking(f => f.ID == obj.ERTypeID.Value)
                           .FirstOrDefault()?.Code;

                           switch (code)
                           {
                               case "AccountingObject":
                                   editor.Visible("ER_OS", true);
                                   break;

                               case "NonCoreAsset":
                               case "NMA":
                                   editor.Visible("ER_NMA", true);
                                   break;

                               case "NKS":
                                   editor.Visible("ER_NKS", true);
                                   break;

                               case "OSArenda":
                                   editor.Visible("ER_ArendaOS", true);
                                   break;

                               case "OSVGP":
                                   editor.Visible("ER_VGP", true);
                                   break;

                               case "Union":
                                   if (obj.ERReceiptReasonID != null)
                                   {
                                       var rr = uow.GetRepository<ERReceiptReason>()
                                           .FilterAsNoTracking(f => f.ID == obj.ERReceiptReasonID.Value)
                                           .FirstOrDefault()?.Code;
                                       if (rr == "Union")
                                       {
                                           editor.Visible("ER_Union", true);
                                           editor.Visible(x => x.ClaimObject, true);
                                       }
                                       else
                                           editor.Visible("ER_Division", true);
                                   }
                                   break;

                               default:
                                   break;
                           }
                       }

                       if (obj.StateID != null)
                       {
                           var state = uow.GetRepository<EstateRegistrationStateNSI>()
                           .FilterAsNoTracking(f => f.ID == obj.StateID.Value)
                           .FirstOrDefault()?.Code;

                           if (state != "DIRECTED")
                               editor.SetReadOnlyAll(true);
                       }
                   })
            );
        }

        /// <summary>
        /// Конфигурация реестра заявок на регистрацию по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<EstateRegistration> ListView_Default(this ViewModelConfigBuilder<EstateRegistration> conf)
        {
            return conf.ListView(lv => lv
                   .Title("Заявки на регистрацию")
                   .HiddenActions(new[] { LvAction.Create, LvAction.Link, LvAction.Unlink })
                   .Columns(eds => eds.Clear()
                   .Add(ed => ed.State, ac => ac.Visible(true))
                   .Add(ed => ed.Number, ac => ac.Visible(true))
                   .Add(ed => ed.Originator, ac => ac.Visible(true))
                   .Add(ed => ed.ContactEmail, ac => ac.Visible(true))
                   .Add(ed => ed.ContactPhone, ac => ac.Visible(true))
                   .Add(ed => ed.Date, ac => ac.Visible(true))
                   .Add(ed => ed.ERType, ac => ac.Visible(true))
                   .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                   .Add(ed => ed.Society, ac => ac.Visible(true))
                   .Add(ed => ed.ERReceiptReason, ac => ac.Visible(true))
                   .Add(ed => ed.Contragent, ac => ac.Visible(true))
                   .Add(ed => ed.Urgently, ac => ac.Visible(true))
                   .Add(ed => ed.NumberCDS, ac => ac.Visible(true))
                   .Add(ed => ed.TransferBUSDate, ac => ac.Visible(true))
                   .Add(ed => ed.TransferBUS, ac => ac.Visible(true))
                   .Add(ed => ed.Comment, ac => ac.Visible(true))
                   .Add(ed => ed.NotActual, ac => ac.Visible(true))
                   .Add(ed => ed.ERContractNumber, ac => ac.Visible(false))
                   .Add(ed => ed.ERContractDate, ac => ac.Visible(false))
                   .Add(ed => ed.PrimaryDocNumber, ac => ac.Visible(false))
                   .Add(ed => ed.PrimaryDocDate, ac => ac.Visible(false))
                   .Add(ed => ed.QuickClose, ac => ac.Visible(false))
                   ).ColumnsFrom<ERControlDateAttributes>(EstateRegistration_ERControlDateAttributes, nameof(EstateRegistration.ERControlDateAttributesID), nameof(IBaseObject.ID))
                   .IsMultiSelect(true)
            );
        }

        public static void DefaultSettingsERows(Base.DAL.IUnitOfWork uow, EstateRegistrationRow obj, CommonEditorVmSett<EstateRegistrationRow> model)
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml
            var Source = "Source";
            var ER = "ER";

            var erTypes = new List<string>() { "osvgp", "union" };
            var isVgpOrUnion = uow.GetRepository<EstateRegistration>()
                .FilterAsNoTracking(f => f.ID == obj.EstateRegistrationID &&
                    f.ERType != null && !String.IsNullOrEmpty(f.ERType.Code)
                    && erTypes.Contains(f.ERType.Code.ToLower()))
                .Any();

            //настройки карточки в зависимости от типа
            if (obj.EstateDefinitionTypeID != null)
            {
                var estType = uow.GetRepository<EstateDefinitionType>()
                .Filter(f => f.ID == obj.EstateDefinitionTypeID.Value)
                .FirstOrDefault();
                var et = CorpProp.Helpers.TypesHelper.GetTypeByName(estType.Code);
                model.ChangeEditor("EUSINumber", true, false, isVgpOrUnion);
                model.ChangeEditor("NameEUSI", true, false, true);
                model.ChangeEditor("EstateDefinitionType", true, true, true);
                model.ChangeEditor("EstateType", true, false, true, condition: () => !et.IsNMA());
                model.ChangeEditor("IntangibleAssetType", true, true, true, Source, ER, condition: () => et.IsNMA());
                model.ChangeEditor("NameEstateByDoc", true, true, true);
                model.ChangeEditor("CadastralNumber", false, false, true, condition: () => et.IsReals() || et.IsLand());
                model.ChangeEditor("StartDateUse", false, true, true, Source, ER, condition: () => et.IsNKS());
                model.ChangeEditor("SibCountry", true, false, true);
                model.ChangeEditor("SibFederalDistrict", true, false, true, condition: () => !et.IsOthers() && !et.IsNMA());
                model.ChangeEditor("SibRegion", true, false, true, condition: () => !et.IsOthers() && !et.IsNMA());
                model.ChangeEditor("SibCityNSI", true, false, true, condition: () => et.IsReals() || et.IsLand() || et.IsNKS());
                model.ChangeEditor("Address", true, false, true, condition: () => et.IsReals() || et.IsLand() || et.IsNKS());
                model.ChangeEditor("VehicleRegDate", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleYearOfIssue", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleCategory", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleDieselEngine", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehiclePowerMeasure", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehiclePower", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleSerialNumber", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleEngineSize", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleModel", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleRegNumber", false, false, true, condition: () => et.IsTS());
            }

            var id = obj.EstateRegistrationID ?? -1;
            if (uow.GetRepository<EstateRegistration>()
                   .FilterAsNoTracking(f => f.ID == id && f.State != null
                   && f.State.Code != "DIRECTED").Any())
                model.SetReadOnlyAll(true);
        }

        public static void DefaultSettingsClaim(Base.DAL.IUnitOfWork uow, EstateRegistrationRow obj, CommonEditorVmSett<EstateRegistrationRow> model)
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml
            var Source = "Source";
            var ER = "ER";

            //настройки карточки в зависимости от типа
            if (obj.EstateDefinitionTypeID != null)
            {
                var estType = uow.GetRepository<EstateDefinitionType>()
                .Filter(f => f.ID == obj.EstateDefinitionTypeID.Value)
                .FirstOrDefault();
                var et = CorpProp.Helpers.TypesHelper.GetTypeByName(estType.Code);
                model.ChangeEditor("EUSINumber", false, false, false);
                model.ChangeEditor("NameEUSI", true, false, true);
                model.ChangeEditor("EstateDefinitionType", true, true, true, condition: () => !et.IsNMA());
                model.ChangeEditor("EstateType", true, false, true, condition: () => !et.IsNMA());
                model.ChangeEditor("IntangibleAssetType", true, true, true, Source, ER, condition: () => et.IsNMA());
                model.ChangeEditor("NameEstateByDoc", true, false, true);
                model.ChangeEditor("CadastralNumber", false, false, true, condition: () => et.IsReals() || et.IsLand());
                model.ChangeEditor("StartDateUse", false, false, true, Source, ER, condition: () => et.IsNKS());
                model.ChangeEditor("SibCountry", true, false, true);
                model.ChangeEditor("SibFederalDistrict", true, false, true, condition: () => !et.IsOthers() && !et.IsNMA());
                model.ChangeEditor("SibRegion", true, false, true, condition: () => !et.IsOthers() && !et.IsNMA());
                model.ChangeEditor("SibCityNSI", true, false, true, condition: () => et.IsReals() || et.IsLand() || et.IsNKS());
                model.ChangeEditor("Address", true, false, true, condition: () => et.IsReals() || et.IsLand() || et.IsNKS());
                model.ChangeEditor("VehicleRegDate", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleYearOfIssue", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleCategory", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleDieselEngine", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehiclePowerMeasure", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehiclePower", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleSerialNumber", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleEngineSize", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleModel", false, false, true, condition: () => et.IsTS());
                model.ChangeEditor("VehicleRegNumber", false, false, true, condition: () => et.IsTS());
            }

            var id = obj.ID;
            if (uow.GetRepository<EstateRegistration>()
                   .FilterAsNoTracking(f => !f.Hidden
                   && f.ClaimObjectID == id && f.State != null
                   && f.State.Code != "DIRECTED").Any())
                model.SetReadOnlyAll(true);
        }
    }
}