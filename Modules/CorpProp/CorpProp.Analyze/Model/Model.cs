//CorpProp.Analyze.Model
using Base;
using Base.UI;
using CorpProp.Analyze.Entities;
using CorpProp.Analyze.Entities.Accounting;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Analyze.Entities.Subject;
//using CorpProp.Analyze.Migration;
using CorpProp.Analyze.Services;
using CorpProp.Analyze.Services.Accounting;
using CorpProp.Analyze.Services.Subject;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Analyze.Model
{
    public static class AnalyzeModel
    {
        /// <summary>
        /// Инициализация моделей модуля Расширенная отчетность.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {
            #region Analyze
            context.CreateVmConfig<BankAccount>()
                .Service<IBankAccountService>()
                .ListView(b => b.Columns(f => f
                    .Add(account => account.BankName, a => a.Visible(true).Title("Наименование банка"))
                    .Add(account => account.BIK, a => a.Visible(true).Title("БИК банка"))
                    .Add(account => account.AccountType, a => a.Visible(true).Title("Вид счета"))
                    .Add(account => account.Currency, a => a.Visible(true).Title("Валюта"))
                    .Add(account => account.AvgOfDay, a => a.Visible(true).Title("Средний оборот за день"))
                    .Add(account => account.OperationCount, a => a.Visible(true).Title("Количество операций"))
                    .Add(account => account.Society, a => a.Visible(true).Title("ОГ"))
                )
                ).DetailView(b => b.Editors(f => f
                        .Add(account => account.BankName, a => a.Visible(true).Title("Наименование банка"))
                        .Add(account => account.BIK, a => a.Visible(true).Title("БИК банка"))
                        .Add(account => account.AccountType, a => a.Visible(true).Title("Вид счета"))
                        .Add(account => account.Currency, a => a.Visible(true).Title("Валюта"))
                        .Add(account => account.AvgOfDay, a => a.Visible(true).Title("Средний оборот за день"))
                        .Add(account => account.OperationCount, a => a.Visible(true).Title("Количество операций"))
                        .Add(account => account.Society, a => a.Visible(true).Title("ОГ"))
                )
                );

            context.CreateVmConfig<RecordBudgetLine>()
                .Service<IRecordBudgetLineService>()
                .Title("Исполнение бюджета");


            context.CreateVmConfig<FinancialIndicatorItem>()
                .Service<IFinancialIndicatorItemService>()
                .Title("Финансовые показатели")
                .ListView(builder => builder
                    .Columns(factory => factory
                        .Add(item => item.FinancialIndValue, ac => ac.Visible(true).Order(3).Title("Значение"))
                    ));

            context.CreateVmConfig<AnalyzeSociety>()
                .Service<IAnalyzeSocietyService>()
                .Title("Дополнительные сведения об ОГ");

            context.ModifyVmConfig<Society>()
                .DetailView(builder => builder.Editors(factory =>
                    factory.AddOneToManyAssociation<FinancialIndicatorItem>("Society_FinancialIndicatorItem",
                        y => y.TabName("[10]Финансовые показатели").Mnemonic("FinancialIndicatorItem")
                            .IsLabelVisible(false)
                            .Create((uofw, entity, id) => { entity.Owner = uofw.GetRepository<Society>().Find(id); })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Owner = null;
                                entity.OwnerID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.OwnerID == id && !w.Hidden)))
                ));


            context.CreateVmConfigOnBase<DictObject, BudgetLine>()
                .Service<IDictObjectService<BudgetLine>>()
                .Title("Строки бюджета")
                .ListView(x => x.Title("Строки бюджета"))
                .DetailView(x => x.Title("Строки бюджета"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<BudgetLine>("DictMenu", "BudgetLineMenu")
                .Service<IDictHistoryService<BudgetLine>>()
                .Title("Строки бюджета")
                .ListView(builder => builder.Title("Строки бюджета"))
                .DetailView(builder => builder.Title("Строки бюджета"))
                .LookupProperty(lp => lp.Text(t => t.Name));


            context.CreateVmConfigOnBase<DictObject, FinancialIndicator>()
                .Service<IDictObjectService<FinancialIndicator>>()
                .Title("Финансовые показатели")
                .ListView(x => x.Title("Финансовые показатели"))
                .DetailView(x => x.Title("Финансовые показатели"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<FinancialIndicator>("DictMenu", "FinancialIndicatorMenu")
                .Service<IDictHistoryService<FinancialIndicator>>()
                .Title("Финансовые показатели")
                .ListView(builder => builder.Title("Финансовые показатели"))
                .DetailView(builder => builder.Title("Финансовые показатели"))
                .LookupProperty(lp => lp.Text(t => t.Name));

            #endregion
        }
    }
}
