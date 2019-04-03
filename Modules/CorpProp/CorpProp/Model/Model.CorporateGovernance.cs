using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Services.Base;
using CorpProp.Services.CorporateGovernance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model
{
    public static class CorporateGovernanceModel
    {
        /// <summary>
        /// Инициализация моделей корп управления.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {

            CorpProp.Model.CorporateGovernance.AppraisalModel.CreateModelConfig(context);
            CorpProp.Model.CorporateGovernance.EstateAppraisalModel.CreateModelConfig(context);

            #region CorporateGovernance
           
            

            context.CreateVmConfig<IndicateEstateAppraisalView>()
               .Service<IIndicateEstateAppraisalService>()
               .Title("Индикация оценки активов")
               .ListView(x => x.Title("Индикация оценки активов")
               .DataSource(ds => ds.Groups(gr => gr.Groupable(true).Add(p => p.AppraiserShortName)
                                                  .Add(p => p.SibRegionTitle)
                                                  .Add(p => p.CustomerShortName)))
               .HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete, LvAction.Link })

               )
               .DetailView(x => x.Title("Индикация оценки активов"))
               .LookupProperty(x => x.Text(t => t.ShortDescriptionObjectAppraisal));

            context.CreateVmConfig<Investment>()
                .Service<IInvestmentService>()
                .Title("Акция / доля")
                .ListView(x => x.Title("Акции / доли"))
                .DetailView(x => x.Title("Акция / доля"))
                .LookupProperty(x => x.Text(t => t.IDEUP));

          

            context.CreateVmConfig<Predecessor>()
                .Service<IPredecessorService>()
                .Title("Правопредшественник")
                .ListView(x => x.Title("Правопредшественники")
                .HiddenActions(new[] { LvAction.Link, LvAction.Unlink }))
                .DetailView(x => x.Title("Правопредшественник"))
                .LookupProperty(x => x.Text(t => t.ShortName));

            context.CreateVmConfig<Predecessor>("Society_Predecessor")
                .Service<IPredecessorService>()
                .Title("Правопредшественник")
                .IsReadOnly(false)
                .ListView(x => x.Title("Правопредшественники")
                .HiddenActions(new[] { LvAction.Link, LvAction.Unlink })
                .Columns(ss =>
                    ss.Add(c => c.INN, action => action.Visible(true))
                     .Add(c => c.ShortName, action => action.Visible(true))
                     .Add(c => c.SocietySuccessor, action => action.Visible(false))
                     .Add(c => c.SuccessionType, action => action.Visible(false))
                     .Add(c => c.DateSuccession, action => action.Visible(false))
                     )
                )
                .DetailView(x => x.Title("Правопредшественник"))
                .LookupProperty(x => x.Text(t => t.ShortName));

            context.CreateVmConfig<Shareholder>()
                .Service<IShareholderService>()
                .Title("Акционер/Участник")
                .ListView(x => x.Title("Акционеры Участники"))
                .DetailView(x => x.Title("Акционер/Участник"))
                .LookupProperty(x => x.Text(t => t.SocietyShareholder));


            context.CreateVmConfig<Shareholder>("Society_ShareholderView")
                .Service<IShareholderService>()
                .Title("Акционер/Участник")
                .ListView(x => x
                .Title("Акционеры/Участники")
                .OnClientEditRow(
                 @"var dataItem = grid.dataItem(grid.select());
                 if (dataItem) {
                    if (dataItem.SocietyShareholderID > 0) {
                        id = dataItem.SocietyShareholderID;
                        mnemonic = 'Society';
                    }
                }")
                .Columns(col =>
                        col.Add(c => c.IDEUPShareholder, action => action.Visible(true).Order(1))
                        .Add(c => c.SocietyShareholder, action => action.Visible(true).Order(2))
                        .Add(c => c.NumberOrdinaryShares, action => action.Visible(true).Order(3))
                        .Add(c => c.NumberPreferredShares, action => action.Visible(true).Order(4))
                        .Add(c => c.CostNominalOrdinaryShares, action => action.Visible(true).Order(5))
                        .Add(c => c.CostActualOrdinaryShares, action => action.Visible(true).Order(6))
                        .Add(c => c.CostNominalPreferredShares, action => action.Visible(true).Order(7))
                        .Add(c => c.CostActualPreferredShares, action => action.Visible(true).Order(8))
                        .Add(c => c.DateFrom, action => action.Visible(true).Order(9))
                        .Add(c => c.DateTo, action => action.Visible(true).Order(10))
                        .Add(c => c.SocietyRecipient, action => action.Visible(false))
                    )
                )
                .DetailView(x => x.Title("Акционер/Участник"))
                .LookupProperty(x => x.Text(t => t.SocietyShareholder));

            context.CreateVmConfig<Shareholder>("Society_RecipientView")
               .Service<IShareholderService>()
               .Title("Акционер/Участник")
               .ListView(x => x.Title("Акционеры/Участники")
               .OnClientEditRow(
                 @"var dataItem = grid.dataItem(grid.select());
                 if (dataItem) {
                    if (dataItem.SocietyRecipientID > 0) {
                        id = dataItem.SocietyRecipientID;
                        mnemonic = 'Society';
                    }
                }")
               .Columns(col =>
                       col.Add(c => c.IDEUPRecipient, action => action.Visible(true).Order(1))
                       .Add(c => c.SocietyRecipient, action => action.Visible(true).Order(2))
                       .Add(c => c.NumberOrdinaryShares, action => action.Visible(true).Order(3))
                       .Add(c => c.NumberPreferredShares, action => action.Visible(true).Order(4))
                       .Add(c => c.CostNominalOrdinaryShares, action => action.Visible(true).Order(5))
                       .Add(c => c.CostActualOrdinaryShares, action => action.Visible(true).Order(6))
                       .Add(c => c.CostNominalPreferredShares, action => action.Visible(true).Order(7))
                       .Add(c => c.CostActualPreferredShares, action => action.Visible(true).Order(8))
                       .Add(c => c.DateFrom, action => action.Visible(true).Order(9))
                       .Add(c => c.DateTo, action => action.Visible(true).Order(10))
                       .Add(c => c.SocietyShareholder, action => action.Visible(false))
                   )
               )
               .DetailView(x => x.Title("Акционер/Участник"))
               .LookupProperty(x => x.Text(t => t.SocietyShareholder));

          

            context.CreateVmConfig<AppraiserDataFinYear>()
              .Service<IAppraiserDataFinYearService>()
              .Title("Данные оценщика за фининсовый год")
              .ListView(x => x.Title("Данные оценщика за фининсовый год"))
              .DetailView(x => x.Title("Данные оценщика за фининсовый год"));

            context.CreateVmConfig<AppraisalOrgData>()
           .Service<IAppraisalOrgDataService>()
           .Title("Данные оценочных организаций")
           .ListView(x => x.Title("Данные оценочных организаций"))
           .DetailView(x => x.Title("Данные оценочных организаций"));



            #endregion



        }
    }
}
