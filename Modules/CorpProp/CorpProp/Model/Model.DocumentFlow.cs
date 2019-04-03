using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.Base;
using CorpProp.Services.DocumentFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model
{
    public static class DocumentFlowModel
    {
        /// <summary>
        /// Инициализация моделей корп управления.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {

            #region DocumentFlow
            //Document
            context.CreateVmConfig<SibDeal>("SibDeal")
            .Title("Сделка")
            .DetailView(x => x.Title("Сделка").Editors(edt => edt                  
                   .Add(e => e.DateDoc, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DocParent, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.Name, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.ExternalSystemIdentifier, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.FullNumber, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.Number, h => h.Title("Номер в компании"))
                   .Add(e => e.Description, h => h.Title("Дополнительная информация"))
                   .Add(e => e.SourseInformation, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DocType, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DateRegistration, h => h.Visible(false).IsRequired(false))
                   .AddOneToManyAssociation<EstateDeal>("SibDeal_EstateDeals",
                         y => y.TabName("[5]Объекты сделки")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.SibDeal = uofw.GetRepository<SibDeal>().Find(id);
                             entity.SibDealID = id;
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.SibDeal = null;
                             entity.SibDealID = null;
                         })

                   .Filter((uofw, q, id, oid) => q.Where(w => w.SibDealID == id && !w.Hidden)))
                   ))
            .LookupProperty(x => x.Text(t => t.Number))
            .ListView(x => x.Title("Сделки")
                    .Columns(
                        c =>
                            c.Add(t => t.ID, h => h.Visible(false))
                            .Add(t => t.SourseInformation, h => h.Visible(false))
                            .Add(t => t.ExternalSystemIdentifier, h => h.Visible(false))
                            .Add(t => t.NumberContragent, h => h.Visible(false))
                            .Add(t => t.Currency, h => h.Visible(true))
                            .Add(t => t.SumDeal, h => h.Visible(true))
                            .Add(t => t.IsRequiresStateRegistration, h => h.Visible(true))
                            .Add(t => t.DateStateRegistration, h => h.Visible(true))
                            .Add(t => t.Description, h => h.Visible(true))

                            .Add(t => t.DateFrom, h => h.Visible(true))
                            .Add(t => t.DateTo, h => h.Visible(true))
                            .Add(t => t.DateDoc, h => h.Visible(true))
                            .Add(t => t.DateRegistration, h => h.Visible(true))
                            .Add(t => t.Number, h => h.Visible(true))
                            .Add(t => t.FullNumber, h => h.Visible(true))
                    )
            );

            context.CreateVmConfig<DealCurrencyConversion>("DealCurrencyConversion")
                .Service<DealCurrencyConversionService>()
                .Title("Сделка (приведение валют)")
                .DetailView(x => x.Title("Сделка (приведение валют)").Editors(edt => edt
                    .AddOneToManyAssociation<EstateDeal>("DealCurrencyConversion_EstateDeal",
                        b => b.TabName("[3]Предметы сделки")
                            .Create((uofw, entity, id) =>
                            {                                
                                entity.SibDeal = uofw.GetRepository<SibDeal>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.SibDeal = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.SibDealID == id)))))
                .LookupProperty(x => x.Text(t => t.FullNumber))
                .ListView(x => x.Title("Сделки")
                    .Columns(
                        c =>
                            c.Add(t => t.ID, h => h.Visible(false))
                                .Add(t => t.SourseInformation, h => h.Visible(false))
                                .Add(t => t.ExternalSystemIdentifier, h => h.Visible(false))
                                .Add(t => t.NumberContragent, h => h.Visible(false))
                                .Add(t => t.Currency, h => h.Visible(true))
                                .Add(t => t.SumDeal, h => h.Visible(true))
                                .Add(t => t.IsRequiresStateRegistration, h => h.Visible(true))
                                .Add(t => t.DateStateRegistration, h => h.Visible(true))
                                .Add(t => t.Description, h => h.Visible(true))

                                .Add(t => t.DateFrom, h => h.Visible(true))
                                .Add(t => t.DateTo, h => h.Visible(true))
                                .Add(t => t.DateDoc, h => h.Visible(true))
                                .Add(t => t.DateRegistration, h => h.Visible(true))
                                .Add(t => t.Number, h => h.Visible(true))
                                .Add(t => t.FullNumber, h => h.Visible(true))
                                .Add(t => t.SumDealInTargetedCurrency, h => h.Visible(true))
                    )
                );

           

            context.CreateVmConfig<DealParticipant>()
                .Title("Участник сделки")
                .ListView(x => x.Title("Участники сделки"))
                .DetailView(x => x.Title("Участник сделки"));

            context.CreateVmConfig<Doc>()
              .Title("Электронный документ АИС КС")
              .ListView(x => x.Title("Электронный документы АИС КС"))
              .DetailView(x => x.Title("Электронный документ АИС КС")
              .Editors(ed=>ed.AddManyToManyRigthAssociation<FileCardAndDoc>("Doc_FileCards", y => y.TabName("[3]Документы"))
              ))
              .LookupProperty(x => x.Text(t => t.FullNumber));

          



            /////////////////////////////
            context.CreateVmConfig<SibDeal>("SibDealTree")
            .Title("Сделка")
            .DetailView(x => x.Title("Сделка").Editors(edt => edt
                   .Add(e => e.DateDoc, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DocParent, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.Name, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.ExternalSystemIdentifier, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.FullNumber, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.Number, h => h.Title("Номер в компании"))
                   .Add(e => e.Description, h => h.Title("Дополнительная информация"))
                   .Add(e => e.SourseInformation, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DocType, h => h.Visible(false).IsRequired(false))
                   .Add(e => e.DateRegistration, h => h.Visible(false).IsRequired(false))))
            .LookupProperty(x => x.Text(t => t.Number))
            .ListView(x => x.Title("Сделки")
                    .Columns(
                        c =>
                            c.Add(t => t.ID, h => h.Visible(false))
                            .Add(t => t.SourseInformation, h => h.Visible(false))
                            .Add(t => t.ExternalSystemIdentifier, h => h.Visible(false))
                            .Add(t => t.NumberContragent, h => h.Visible(false))
                            .Add(t => t.Currency, h => h.Visible(true))
                            .Add(t => t.SumDeal, h => h.Visible(true))
                            .Add(t => t.IsRequiresStateRegistration, h => h.Visible(true))
                            .Add(t => t.DateStateRegistration, h => h.Visible(true))
                            .Add(t => t.Description, h => h.Visible(true))
                            .Add(t => t.ParentDeal, h => h.Visible(true))
                            .Add(t => t.DateFrom, h => h.Visible(true))
                            .Add(t => t.DateTo, h => h.Visible(true))
                            .Add(t => t.DateDoc, h => h.Visible(true))
                            .Add(t => t.DateRegistration, h => h.Visible(true))
                            .Add(t => t.Number, h => h.Visible(true))
                            .Add(t => t.FullNumber, h => h.Visible(true))
                    ).Type(ListViewType.TreeListView)
            );

            #endregion



        }
    }
}
