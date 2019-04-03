using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.CorporateGovernance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model.CorporateGovernance
{
    public static class EstateAppraisalModel
    {
        /// <summary>
        /// Создает конфигурацию модели объекта оценки по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<EstateAppraisal>()
               .Service<IEstateAppraisalService>()
               .Title("Объект оценки")
               .ListView_Default()
               .DetailView_Default()
               .LookupProperty(x => x.Text(t => t.ShortDescriptionObjectAppraisal));

            context.CreateVmConfig<EstateAppraisal>("SummaryEstateAppraisal")
              .Service<IEstateAppraisalService>()
              .Title("Объект оценки")
              .ListView_SummaryEstateAppraisal()
              .DetailView_Default()
              .LookupProperty(x => x.Text(t => t.ShortDescriptionObjectAppraisal));

        }

        /// <summary>
        /// Конфигурация карточки объекта оценки по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<EstateAppraisal> DetailView_Default(this ViewModelConfigBuilder<EstateAppraisal> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)

               .Editors(editors => editors
               .Add(d => d.AOOwner, ac => ac.Order(1).TabName("[0]Основные данные"))
               .Add(d => d.ShortDescriptionObjectAppraisal, ac => ac.Order(2).TabName("[0]Основные данные"))
               .Add(d => d.AppType, ac => ac.Order(3).TabName("[0]Основные данные"))
               .Add(d => d.AppraisalType, ac => ac.Order(4).TabName("[0]Основные данные"))
               .Add(d => d.AccountingObject, ac => ac.Order(5).TabName("[0]Основные данные"))
               .Add(d => d.AOResidualCost, ac => ac.Order(6).TabName("[0]Основные данные"))
               .Add(d => d.BalanceCost, ac => ac.Order(7).TabName("[0]Основные данные"))
               .Add(d => d.MarketPriceWithoutVAT, ac => ac.Order(8).TabName("[0]Основные данные"))
               .Add(d => d.DateOfAppaisal, ac => ac.Order(9).TabName("[0]Основные данные"))
               .Add(d => d.RegionOfAppaisal, ac => ac.Order(10).TabName("[0]Основные данные"))
               .Add(d => d.Appraisal, ac => ac.Order(11).TabName("[0]Основные данные").Visible(false))
              

               //.AddManyToManyRigthAssociation<EstateAndEstateAppraisal>("EstateAppraisal_Estates", y => y.TabName("[1]Объекты имущества").Visible(false))
              ));
        }

        /// <summary>
        /// Конфигурация реестра объектов оценок по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<EstateAppraisal> ListView_Default(this ViewModelConfigBuilder<EstateAppraisal> conf)
        {
            return
                conf.ListView(x => x
                .Title("Объекты оценки")
                .Columns(c => c
                    .Add(t => t.ID, h => h.Visible(false))
                    .Add(t => t.AOOwner, p => p.Visible(true).Title("Балансодержатель объекта"))
                    .Add(t => t.AccountingObject, p => p.Visible(true).Title("Объект БУ"))
                    .Add(t => t.ShortDescriptionObjectAppraisal, p => p.Visible(true).Title("Краткое описание объекта оценки"))
                    .Add(t => t.AppType, p => p.Visible(true).Title("Тип объекта оценки"))
                    .Add(t => t.RegionOfAppaisal, p => p.Visible(true).Title("Субъект РФ"))
                    .Add(t => t.AOResidualCost, p => p.Visible(true).Title("Балансовая стоимость по РСБУ, руб."))
                    .Add(t => t.MarketPriceWithoutVAT, p => p.Visible(true).Title("Рыночная стоимость, без НДС"))
                    .Add(t => t.DateOfAppaisal, p => p.Visible(true).Title("Дата оценки"))
                   

                ));

        }

        /// <summary>
        /// Конфигурация сводного реестра объектов оценки.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<EstateAppraisal> ListView_SummaryEstateAppraisal(this ViewModelConfigBuilder<EstateAppraisal> conf)
        {
            return
                conf.ListView(x => x
                .Title("Объекты оценок")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Groups(gr => gr
                .Groupable(true)
                .ShowFooter(true)
                .Add(p => p.SocietyOfAppaisal)

                ))
                .Columns(c => c
                    .Add(t => t.ID, h => h.Visible(false))
                    .Add(t => t.RegionOfAppaisal, p => p.Visible(true).Title("Субъект РФ"))
                    .Add(t => t.AppraiserOfAppaisal, p => p.Visible(true).Title("Оценочная организация"))
                    .Add(t => t.AOOwner, p => p.Visible(true).Title("Балансодержатель объекта"))
                    .Add(t => t.DateOfAppaisal, p => p.Visible(true).Title("Дата оценки"))
                    .Add(t => t.SocietyOfAppaisal, p => p.Visible(true).Title("ОГ заказчик оценки"))
                    .Add(t => t.Appraisal, p => p.Visible(true).Title("Оценка"))
                    .Add(t => t.AppType, p => p.Visible(true).Title("Тип объекта оценки"))
             
         
                    )
               );

        }


    }
}
