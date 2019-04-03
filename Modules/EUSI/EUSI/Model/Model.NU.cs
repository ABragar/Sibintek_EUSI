using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.NSI;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Entities.NU;
using EUSI.Services.Estate;
using EUSI.Services.NU;
using System;
using System.Linq;

namespace EUSI.Model
{
    public static class NUModel
    {
        public static void Init(IInitializerContext context)
        {
            context.CreateVmConfig<DeclarationRow>(nameof(DeclarationRow))
                  .Service<ITypeObjectService<DeclarationRow>>()
                  .Title("Строка декларации")
                  .IsReadOnly();

            context.CreateVmConfigOnBase<DeclarationRow>(nameof(DeclarationRow), "DeclarationOI_OKMOGroups")
                .Title("Сумма налога, подлежащая уплате в бюджет")
                .DetailView(dv => dv.Title("Сумма налога, подлежащая уплате в бюджет")
                .Editors( eds => eds.Clear()
                .Add(ed => ed.OKTMO)
                .Add(ed => ed.KBK)
                .Add(ed => ed.Sum)                
                ))
                .ListView(lv => lv.Title("Сумма налога, подлежащая уплате в бюджет")
                .DataSource(ds => ds.Filter(f => f.TypeRow=="1"))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.OKTMO)
                .Add(ed => ed.KBK)
                .Add(ed => ed.Sum)
                ))
                .IsReadOnly();

            context.CreateVmConfigOnBase<DeclarationRow>(nameof(DeclarationRow), "DeclarationCalcIO_OKMOGroups")
              .Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .DetailView(dv => dv.Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .Editors(eds => eds.Clear()
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.KBK)
                 .Add(ed => ed.Sum, ac=>ac.Title("Сумма авансового платежа поналогу, подлежащая уплате в бюджет"))
              ))
              .ListView(lv => lv.Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .DataSource(ds => ds.Filter(f => f.TypeRow == "1"))
              .Columns(cols => cols.Clear()
                  .Add(ed => ed.OKTMO)
                  .Add(ed => ed.KBK)
                  .Add(ed => ed.Sum, ac => ac.Title("Сумма авансового платежа поналогу, подлежащая уплате в бюджет"))
              ))
              .IsReadOnly();

            context.CreateVmConfigOnBase<DeclarationRow>(nameof(DeclarationRow), "DeclarationVehicle_OKMOGroups")
              .Title("Сумма налога, подлежащая уплате в бюджет")
              .DetailView(dv => dv.Title("Сумма налога, подлежащая уплате в бюджет")
              .Editors(eds => eds.Clear()
                 .Add(ed => ed.KBK)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.PrepaymentSumFirstQuarter)
                 .Add(ed => ed.PrepaymentSumSecondQuarter)
                 .Add(ed => ed.PrepaymentSumThirdQuarter)
                 .Add(ed => ed.Sum)
              ))
              .ListView(lv => lv.Title("Сумма налога, подлежащая уплате в бюджет")
              .DataSource(ds => ds.Filter(f => f.TypeRow == "1"))
              .Columns(cols => cols.Clear()
                 .Add(ed => ed.KBK)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.PrepaymentSumFirstQuarter)
                 .Add(ed => ed.PrepaymentSumSecondQuarter)
                 .Add(ed => ed.PrepaymentSumThirdQuarter)
                 .Add(ed => ed.Sum)
              ))
              .IsReadOnly();
            context.CreateVmConfigOnBase<DeclarationRow>("DeclarationVehicle_OKMOGroups", "DeclarationLand_OKMOGroups");


            context.CreateVmConfigOnBase<DeclarationRow>(nameof(DeclarationRow), "DeclarationOI_EstateInfo")
                .Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .DetailView(dv => dv.Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .Editors(eds => eds.Clear()
                   .Add(ed => ed.CadastralNumber)
                   .Add(ed => ed.ConditionalNumber)
                   .Add(ed => ed.InventoryNumber)
                   .Add(ed => ed.OKOF)
                   .Add(ed => ed.ResidualCost_3112)              
                ))
                .ListView(lv => lv.Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .DataSource(ds => ds.Filter(f => f.TypeRow == "2"))
                .Columns(cols => cols.Clear()
                   .Add(ed => ed.CadastralNumber)
                   .Add(ed => ed.ConditionalNumber)
                   .Add(ed => ed.InventoryNumber)
                   .Add(ed => ed.OKOF)
                   .Add(ed => ed.ResidualCost_3112)
                ))
                .IsReadOnly();
            context.CreateVmConfigOnBase<DeclarationRow>(nameof(DeclarationRow), "DeclarationCalcOI_EstateInfo")
                .Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .DetailView(dv => dv.Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .Editors(eds => eds.Clear()
                   .Add(ed => ed.CadastralNumber)
                   .Add(ed => ed.ConditionalNumber)
                   .Add(ed => ed.InventoryNumber)
                   .Add(ed => ed.OKOF)
                   .Add(ed => ed.ResidualCost_End)
                ))
                .ListView(lv => lv.Title("Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                .DataSource(ds => ds.Filter(f => f.TypeRow == "2"))
                .Columns(cols => cols.Clear()
                   .Add(ed => ed.CadastralNumber)
                   .Add(ed => ed.ConditionalNumber)
                   .Add(ed => ed.InventoryNumber)
                   .Add(ed => ed.OKOF)
                   .Add(ed => ed.ResidualCost_End)
                ))
                .IsReadOnly();

            context.CreateVmConfig<AccountingCalculatedField>("DeclarationOI_Avg")
               .Title("Сумма налога, подлежащая уплате в бюджет")
               .DetailView(dv => dv.Title("Сумма налога, подлежащая уплате в бюджет")
               .Editors(eds => eds.Clear()
                  .Add(ed => ed.EstateKindCode)
                  .Add(ed => ed.OKTMO)
                  .Add(ed => ed.ResidualCost_01)
                  .Add(ed => ed.ResidualCost_02)
                  .Add(ed => ed.ResidualCost_03)
                  .Add(ed => ed.ResidualCost_04)
                  .Add(ed => ed.ResidualCost_05)
                  .Add(ed => ed.ResidualCost_06)
                  .Add(ed => ed.ResidualCost_07)
                  .Add(ed => ed.ResidualCost_08)
                  .Add(ed => ed.ResidualCost_09)
                  .Add(ed => ed.ResidualCost_10)
                  .Add(ed => ed.ResidualCost_11)
                  .Add(ed => ed.ResidualCost_12)
                  .Add(ed => ed.ResidualCost_13)
                  .Add(ed => ed.ResidualCost_14)
                  .Add(ed => ed.AvgPriceYear)
                  .Add(ed => ed.TaxExemption)
                  .Add(ed => ed.TaxRate)
                  .Add(ed => ed.TaxSumYear)
                  .Add(ed => ed.PrepaymentSumYear)
               ))
               .ListView(lv => lv.Title("Сумма налога, подлежащая уплате в бюджет")
               .Columns(cols => cols.Clear()
                    .Add(ed => ed.EstateKindCode)
                  .Add(ed => ed.OKTMO)
                  .Add(ed => ed.ResidualCost_01)
                  .Add(ed => ed.ResidualCost_02)
                  .Add(ed => ed.ResidualCost_03)
                  .Add(ed => ed.ResidualCost_04)
                  .Add(ed => ed.ResidualCost_05)
                  .Add(ed => ed.ResidualCost_06)
                  .Add(ed => ed.ResidualCost_07)
                  .Add(ed => ed.ResidualCost_08)
                  .Add(ed => ed.ResidualCost_09)
                  .Add(ed => ed.ResidualCost_10)
                  .Add(ed => ed.ResidualCost_11)
                  .Add(ed => ed.ResidualCost_12)
                  .Add(ed => ed.ResidualCost_13)
                  .Add(ed => ed.ResidualCost_14)
                  .Add(ed => ed.AvgPriceYear)
                  .Add(ed => ed.TaxExemption)
                  .Add(ed => ed.TaxRate)
                  .Add(ed => ed.TaxSumYear)
                  .Add(ed => ed.PrepaymentSumYear)
               ))
               .IsReadOnly();

            context.CreateVmConfig<AccountingCalculatedField>("DeclarationCalcOI_Avg")
              .Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .DetailView(dv => dv.Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .Editors(eds => eds.Clear()
                 .Add(ed => ed.EstateKindCode)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.ResidualCost_01)
                 .Add(ed => ed.ResidualCost_02)
                 .Add(ed => ed.ResidualCost_03)
                 .Add(ed => ed.ResidualCost_04)
                 .Add(ed => ed.ResidualCost_05)
                 .Add(ed => ed.ResidualCost_06)
                 .Add(ed => ed.ResidualCost_07)
                 .Add(ed => ed.ResidualCost_08)
                 .Add(ed => ed.ResidualCost_09)
                 .Add(ed => ed.ResidualCost_10)                 
                 .Add(ed => ed.AvgPriceFirstQuarter)
                 .Add(ed => ed.AvgPriceSecondQuarter)
                 .Add(ed => ed.AvgPriceThirdQuarter)
                 .Add(ed => ed.TaxExemption)
                 .Add(ed => ed.TaxExemptionLow)
                 .Add(ed => ed.TaxRate)
                 .Add(ed => ed.PrepaymentSumFirstQuarter)
                 .Add(ed => ed.PrepaymentSumSecondQuarter)
                 .Add(ed => ed.PrepaymentSumThirdQuarter)                 
              ))
              .ListView(lv => lv.Title("Сумма авансового платежа, подлежащая уплате в бюджет")
              .Columns(cols => cols.Clear()
                 .Add(ed => ed.EstateKindCode)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.ResidualCost_01)
                 .Add(ed => ed.ResidualCost_02)
                 .Add(ed => ed.ResidualCost_03)
                 .Add(ed => ed.ResidualCost_04)
                 .Add(ed => ed.ResidualCost_05)
                 .Add(ed => ed.ResidualCost_06)
                 .Add(ed => ed.ResidualCost_07)
                 .Add(ed => ed.ResidualCost_08)
                 .Add(ed => ed.ResidualCost_09)
                 .Add(ed => ed.ResidualCost_10)
                 .Add(ed => ed.AvgPriceFirstQuarter)
                 .Add(ed => ed.AvgPriceSecondQuarter)
                 .Add(ed => ed.AvgPriceThirdQuarter)
                 .Add(ed => ed.TaxExemption)
                 .Add(ed => ed.TaxExemptionLow)
                 .Add(ed => ed.TaxRate)
                 .Add(ed => ed.PrepaymentSumFirstQuarter)
                 .Add(ed => ed.PrepaymentSumSecondQuarter)
                 .Add(ed => ed.PrepaymentSumThirdQuarter)
              ))
              .IsReadOnly();

            context.CreateVmConfig<AccountingCalculatedField>("DeclarationOI_CadastralCost")
              .Title("Сумма налога по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
              .DetailView(dv => dv.Title("Сумма налога по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
              .Editors(eds => eds.Clear()
                 .Add(ed => ed.EstateKindCode)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.CadastralNumber)                 
                 .Add(ed => ed.TaxBaseValue)
                 .Add(ed => ed.TaxExemptionLow)
                 .Add(ed => ed.TaxRate)
                 .Add(ed => ed.TaxSumYear)
                 .Add(ed => ed.PrepaymentSumYear)               
              ))
              .ListView(lv => lv.Title("Сумма налога по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
              .Columns(cols => cols.Clear()
                 .Add(ed => ed.EstateKindCode)
                 .Add(ed => ed.OKTMO)
                 .Add(ed => ed.CadastralNumber)
                 .Add(ed => ed.TaxBaseValue)
                 .Add(ed => ed.TaxExemptionLow)
                 .Add(ed => ed.TaxRate)
                 .Add(ed => ed.TaxSumYear)
                 .Add(ed => ed.PrepaymentSumYear)
              ))
              .IsReadOnly();

            context.CreateVmConfig<AccountingCalculatedField>("DeclarationCalcOI_CadastralCost")
             .Title("Сумма авансового платежа по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
             .DetailView(dv => dv.Title("Сумма авансового платежа по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
             .Editors(eds => eds.Clear()
                .Add(ed => ed.EstateKindCode)
                .Add(ed => ed.OKTMO)
                .Add(ed => ed.CadastralNumber)
                .Add(ed => ed.CadastralValue)
                .Add(ed => ed.TaxExemption)
                .Add(ed => ed.TaxExemptionLow)
                .Add(ed => ed.TaxRate)
                .Add(ed => ed.TaxSumYear)
                .Add(ed => ed.PrepaymentSumFirstQuarter)
                .Add(ed => ed.PrepaymentSumSecondQuarter)
                .Add(ed => ed.PrepaymentSumThirdQuarter)
             ))
             .ListView(lv => lv.Title("Сумма авансового платежа по объекту НИ, налоговая база которого определяется как кадастровая стоимость")
             .Columns(cols => cols.Clear()
                .Add(ed => ed.EstateKindCode)
                .Add(ed => ed.OKTMO)
                .Add(ed => ed.CadastralNumber)
                .Add(ed => ed.CadastralValue)
                .Add(ed => ed.TaxExemption)
                .Add(ed => ed.TaxExemptionLow)
                .Add(ed => ed.TaxRate)
                .Add(ed => ed.TaxSumYear)
                .Add(ed => ed.PrepaymentSumFirstQuarter)
                .Add(ed => ed.PrepaymentSumSecondQuarter)
                .Add(ed => ed.PrepaymentSumThirdQuarter)
             ))
             .IsReadOnly();


            context.CreateVmConfig<AccountingCalculatedField>("DeclarationVehicle_Calculate")
            .Title("Расчет суммы налога по транспортному средству")
            .DetailView(dv => dv.Title("Расчет суммы налога по транспортному средству")
            .Editors(eds => eds.Clear()               
               .Add(ed => ed.OKTMO)
               .Add(ed => ed.EstateKindCode, ac => ac.Title("Вид ТС (код)"))
               .Add(ed => ed.VehicleSerialNumber)
               .Add(ed => ed.VehicleSignNumber)
               .Add(ed => ed.TaxBaseValue)
               .Add(ed => ed.EcoKlass)
               .Add(ed => ed.VehicleYearOfIssue)
               .Add(ed => ed.VehicleMonthOwn)
               .Add(ed => ed.Share)
               .Add(ed => ed.TaxRate)
               .Add(ed => ed.VehicleTaxFactor)
               .Add(ed => ed.TaxSumYear)
               .Add(ed => ed.TaxExemptionFree)
               .Add(ed => ed.TaxExemptionFreeSum)
               .Add(ed => ed.TaxLow)
               .Add(ed => ed.TaxLowSum)
               .Add(ed => ed.TaxExemptionLow)
               .Add(ed => ed.TaxExemptionLowSum)
               .Add(ed => ed.PaymentTaxSum)
            ))
            .ListView(lv => lv.Title("Расчет суммы налога по транспортному средству")
            .Columns(cols => cols.Clear()
               .Add(ed => ed.OKTMO)
               .Add(ed => ed.EstateKindCode, ac => ac.Title("Вид ТС (код)"))
               .Add(ed => ed.VehicleSerialNumber)
               .Add(ed => ed.VehicleSignNumber)
               .Add(ed => ed.TaxBaseValue)
               .Add(ed => ed.EcoKlass)
               .Add(ed => ed.VehicleYearOfIssue)
               .Add(ed => ed.VehicleMonthOwn)
               .Add(ed => ed.Share)
               .Add(ed => ed.TaxRate)
               .Add(ed => ed.VehicleTaxFactor)
               .Add(ed => ed.TaxSumYear)
               .Add(ed => ed.TaxExemptionFree)
               .Add(ed => ed.TaxExemptionFreeSum)
               .Add(ed => ed.TaxLow)
               .Add(ed => ed.TaxLowSum)
               .Add(ed => ed.TaxExemptionLow)
               .Add(ed => ed.TaxExemptionLowSum)
               .Add(ed => ed.PaymentTaxSum)
            ))
            .IsReadOnly();

            context.CreateVmConfig<AccountingCalculatedField>("DeclarationLand_Calculate")
            .Title("Расчет налоговой базы и суммы земельного налога")
            .DetailView(dv => dv.Title("Расчет налоговой базы и суммы земельного налога")
            .Editors(eds => eds.Clear()
               .Add(ed => ed.CadastralNumber)
               .Add(ed => ed.OKTMO)
               .Add(ed => ed.EstateKindCode, ac => ac.Title("Категория земель (код)"))
               .Add(ed => ed.CadastralValue)
               .Add(ed => ed.Share)
               .Add(ed => ed.TaxBaseValue)
               .Add(ed => ed.TaxRate)
               .Add(ed => ed.TaxSumYear)
               .Add(ed => ed.TaxExemptionFreeLand)
               .Add(ed => ed.TaxExemptionFreeSumLand)
               .Add(ed => ed.TaxExemptionFree)
               .Add(ed => ed.TaxExemptionFreeSum)
               .Add(ed => ed.TaxLow)
               .Add(ed => ed.TaxLowSum)
               .Add(ed => ed.TaxExemptionLow)
               .Add(ed => ed.PaymentTaxSum)             
            ))
            .ListView(lv => lv.Title("Расчет налоговой базы и суммы земельного налога")
            .Columns(cols => cols.Clear()
               .Add(ed => ed.CadastralNumber)
               .Add(ed => ed.OKTMO)
               .Add(ed => ed.EstateKindCode, ac => ac.Title("Категория земель (код)"))
               .Add(ed => ed.CadastralValue)
               .Add(ed => ed.Share)
               .Add(ed => ed.TaxBaseValue)
               .Add(ed => ed.TaxRate)
               .Add(ed => ed.TaxSumYear)
               .Add(ed => ed.TaxExemptionFreeLand)
               .Add(ed => ed.TaxExemptionFreeSumLand)
               .Add(ed => ed.TaxExemptionFree)
               .Add(ed => ed.TaxExemptionFreeSum)
               .Add(ed => ed.TaxLow)
               .Add(ed => ed.TaxLowSum)
               .Add(ed => ed.TaxExemptionLow)
               .Add(ed => ed.PaymentTaxSum)
            ))
            .IsReadOnly();

            context.CreateVmConfig<Declaration>(nameof(Declaration))
                  .Service<IDeclarationService<Declaration>>()
                  .Title("Налоговая декларация")
                  .LookupProperty(x => x.Text(t => t.FileName))
                  .DetailViewOnBase(null)
                  .IsReadOnly();


            context.CreateVmConfig<DeclarationEstate>(nameof(DeclarationEstate))
                  .Service<IDeclarationService<DeclarationEstate>>()
                  .Title("Налоговая декларация по налогу на объекты имущества организаций")
                  .LookupProperty(x => x.Text(t => t.FileName))
                  .DetailViewOnBase(dv => dv.Editors(eds => eds                
                  .AddOneToManyAssociation<DeclarationRow>("DeclarationOI_OKMOGroups",
                        editor => editor
                        .TabName("[002]Раздел 1. Сумма налога, подлежащая уплате в бюджет")
                        .Title("Сумма налога, подлежащая уплате в бюджет")
                        .Mnemonic("DeclarationOI_OKMOGroups")
                        .IsReadOnly(true)
                        .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                  .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationOI_Avg",
                        editor => editor
                        .Mnemonic("DeclarationOI_Avg")
                        .TabName("[003]Раздел 2. Определение налоговой базы и исчисление налога в отношении подлежащего налогообложению имущества российских организаций и иностранных организаций, осуществляющих деятельность в Российской Федерации через постоянные правительства")
                        .IsReadOnly(true)
                        .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && !w.IsCadastralCost)))

                  .AddOneToManyAssociation<DeclarationRow>("DeclarationOI_EstateInfo",
                        editor => editor
                        .Mnemonic("DeclarationOI_EstateInfo")
                        .TabName("[004]Раздел 2.1. Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                        .IsReadOnly(true)
                        .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                  .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationOI_CadastralCost",
                        editor => editor
                        .Mnemonic("DeclarationOI_CadastralCost")
                        .TabName("[005]Раздел 3. Исчисление суммы налога за налоговый период по объекту недвижимого имущества, налоговая база и отношение которого определяется как кадастровая стоимость")
                        .IsReadOnly(true)
                        .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && w.IsCadastralCost)))

                  ))
                  .IsReadOnly()
                  ;

            context.CreateVmConfig<DeclarationCalcEstate>(nameof(DeclarationCalcEstate))
                 .Service<IDeclarationService<DeclarationCalcEstate>>()
                 .Title("Налоговый расчет по авансовому платежу по налогу на имущество организаций")
                 .LookupProperty(x => x.Text(t => t.FileName))
                 .DetailViewOnBase(dv => dv.Editors(eds => eds
                 .AddOneToManyAssociation<DeclarationRow>("DeclarationCalcIO_OKMOGroups",
                       editor => editor
                       .TabName("[002]Раздел 1. Сумма авансового платежа, подлежащая уплате в бюджет, по данным налогоплательщика")
                       .Title("Сумма авансового платежа, подлежащая уплате в бюджет")
                       .Mnemonic("DeclarationCalcIO_OKMOGroups")
                       .IsReadOnly(true)
                       .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                 .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationCalcOI_Avg",
                       editor => editor
                       .Mnemonic("DeclarationCalcOI_Avg")
                       .TabName("[003]Раздел 2. Исчисление суммы авансового платежа по налогу в отношении подлежащего налогообложению имущества российских организаций и иностранных организаций, осуществляющих деятельность в Российской Федерации через постоянные правительства")
                       .IsReadOnly(true)
                       .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && !w.IsCadastralCost)))

                 .AddOneToManyAssociation<DeclarationRow>("DeclarationCalcOI_EstateInfo",
                       editor => editor
                       .Mnemonic("DeclarationCalcOI_EstateInfo")
                       .TabName("[004]Раздел 2.1. Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости")
                       .IsReadOnly(true)
                       .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                 .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationCalcOI_CadastralCost",
                       editor => editor
                       .Mnemonic("DeclarationCalcOI_CadastralCost")
                       .TabName("[005]Раздел 3. Исчисление суммы авансового платежа за отчетный период по объекту недвижимого имущества, налоговая база и отношение которого определяется как кадастровая стоимость")
                       .IsReadOnly(true)
                       .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && w.IsCadastralCost)))

                 ))
                 .IsReadOnly()
                 ;


            context.CreateVmConfig<DeclarationVehicle>(nameof(DeclarationVehicle))
                .Service<IDeclarationService<DeclarationVehicle>>()
                .Title("Налоговая декларация по транспортному налогу")
                .LookupProperty(x => x.Text(t => t.FileName))
                .DetailViewOnBase(dv => dv.Editors(eds => eds
                .AddOneToManyAssociation<DeclarationRow>("DeclarationVehicle_OKMOGroups",
                      editor => editor
                      .TabName("[002]Раздел 1. Сумма налога, подлежащая уплате в бюджет")
                      .Title("Сумма налога, подлежащая уплате в бюджет")
                      .Mnemonic("DeclarationVehicle_OKMOGroups")
                      .IsReadOnly(true)
                      .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationVehicle_Calculate",
                      editor => editor
                      .Mnemonic("DeclarationVehicle_Calculate")
                      .TabName("[003]Раздел 2. Расчет суммы налога по каждому ТС")
                      .IsReadOnly(true)
                      .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && !w.IsCadastralCost)))
               
                ))
                .IsReadOnly()
                ;


            context.CreateVmConfig<DeclarationLand>(nameof(DeclarationLand))
                .Service<IDeclarationService<DeclarationLand>>()
                .Title("Налоговая декларация по земельному налогу")
                .LookupProperty(x => x.Text(t => t.FileName))
                .DetailViewOnBase(dv => dv.Editors(eds => eds
                .AddOneToManyAssociation<DeclarationRow>("DeclarationLand_OKMOGroups",
                      editor => editor
                      .TabName("[002]Раздел 1. Сумма земельного налога, подлежащая уплате в бюджет")
                      .Title("Сумма Сумма земельного налога, подлежащая уплате в бюджет")
                      .Mnemonic("DeclarationLand_OKMOGroups")
                      .IsReadOnly(true)
                      .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id)))

                .AddOneToManyAssociation<AccountingCalculatedField>("DeclarationLand_Calculate",
                      editor => editor
                      .Mnemonic("DeclarationLand_Calculate")
                      .TabName("[003]Раздел 2. Расчет налоговой базы и суммы земельного налога")
                      .IsReadOnly(true)
                      .Filter((uofw, q, id, oid) => q.Where(w => w.DeclarationID == id && !w.IsCadastralCost)))

                ))
                .IsReadOnly()
                ;

        }

       

    }

    public static class VMExtentions
    {

        public static ViewModelConfigBuilder<T> DetailViewOnBase<T>(this ViewModelConfigBuilder<T> builder, Action<DetailViewBuilder<T>> action) where T: Declaration
        {
            var i = 1;
            builder.DetailView(dv => dv.Editors(eds => eds.Clear()
                 .Add(ed => ed.FileName, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.FileCard, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.FileDate, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.KND, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.CorrectionNumb, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.PeriodCode, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.Year, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.AuthorityCode, ac => ac.Group("_").Order(i++))
                 .AddEmpty(ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.LocationCode, ac => ac.Group("_").Order(i++))
                 .Add(ed => ed.SubjectName, ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.Phone, ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.ReorgFormCode, ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.INN, ac => ac.Group("Налогоплательщик").Order(i++))
                 .AddEmpty(ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.ReorgINN, ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.KPP, ac => ac.Group("Налогоплательщик").Order(i++))
                 .AddEmpty(ac => ac.Group("Налогоплательщик").Order(i++))
                 .Add(ed => ed.ReorgKPP, ac => ac.Group("Налогоплательщик").Order(i++))

                 .Add(ed => ed.LastName, ac => ac.Group("Подписант").Order(i++))
                 .Add(ed => ed.RepresentOrg, ac => ac.Group("Подписант").Order(i++))
                 .Add(ed => ed.RepresentType, ac => ac.Group("Подписант").Order(i++))
                 .Add(ed => ed.FirstName, ac => ac.Group("Подписант").Order(i++))
                 .Add(ed => ed.RepresentDoc, ac => ac.Group("Подписант").Order(i++))
                 .AddEmpty(ac => ac.Group("Подписант").Order(i++))
                 .Add(ed => ed.MiddleName, ac => ac.Group("Подписант").Order(i++))
                 .AddEmpty(ac => ac.Group("Подписант").Order(i++))
                 .AddEmpty(ac => ac.Group("Подписант").Order(i++))
                 ));
            if (action != null)
                builder.DetailView(action);

            return builder;
        }
    }
}
