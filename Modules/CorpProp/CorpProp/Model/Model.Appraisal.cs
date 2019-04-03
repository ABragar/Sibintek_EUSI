using Base;
using Base.UI;
using CorpProp.Entities;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using CorpProp.Services.CorporateGovernance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model.CorporateGovernance
{
    public static class AppraisalModel
    {
        /// <summary>
        /// Создает конфигурацию модели оценки по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<Appraisal>()
                 .Service<IAppraisalService>()
                 .Title("Оценка")
                 .ListView_Default().ListView(builder => builder.Toolbar(factory => factory.Add("GetSibPermissions", "SibPermission")))
                 .DetailView_Default()
                 .LookupProperty(x => x.Text(t => t.ReportNumber))
                 ;

            context.CreateVmConfig<Appraisal>("SummaryAppraisal")
               .Service<IAppraisalService>()
               .Title("Оценка")
               .ListView_SummaryAppraisal()
               .DetailView_Default()
               .LookupProperty(x => x.Text(t => t.ReportNumber))
               ;

            context.CreateVmConfig<Appraisal>("SummaryAppraisalExecutor")
               .Service<IAppraisalService>()
               .Title("Оценка")
               .ListView_SummaryAppraisalExecutor()
               .DetailView_Default()
               .LookupProperty(x => x.Text(t => t.ReportNumber))
               ;

        }

        /// <summary>
        /// Конфигурация карточки оценки по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Appraisal> DetailView_Default(this ViewModelConfigBuilder<Appraisal> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                   .Add(ed => ed.ExecutorLastName, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.Appraisal_ExecutorInfo(form);").Order(1))
                   .Add(ed => ed.Customer, ac => ac.Order(2))
                   .Add(ed => ed.ExecutorDeptName, ac => ac.Order(3))
                   .Add(ed => ed.ExecutorFirstName, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.Appraisal_ExecutorInfo(form);").Order(4))
                   .Add(ed => ed.ExecutorMobile, ac => ac.Order(5))
                   .Add(ed => ed.ExecutorPhone, ac => ac.Order(6))
                   .Add(ed => ed.ExecutorMiddleName, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.Appraisal_ExecutorInfo(form);").Order(7))
                   .Add(ed => ed.Executor, ac => ac.OnChangeClientScript(@" corpProp.dv.editors.onChange.Appraisal_Executor(form, isChange);").Order(8))
                   .Add(ed => ed.ExecutorEmail, ac => ac.Order(9))
                   .Add(ed => ed.AprObject, ac => ac.Order(10))
                   .Add(ed => ed.SibRegion, ac => ac.Order(11))
                   .Add(ed => ed.AppraisalDate, ac => ac.Order(12))
                   .Add(ed => ed.AppType, ac => ac.Order(13))
                   .Add(ed => ed.MarketAppraisalCost, ac => ac.Order(14))
                   .AddEmpty(ac => ac.Order(15).TabName(CaptionHelper.DefaultTabName).Group("Объект оценки"))
                   .Add(ed => ed.Owner, ac => ac.Order(16))
                   .Add(ed => ed.EstateAppraisalCost, ac => ac.Order(17))
                   .Add(ed => ed.AppraisalNNA, ac => ac.Order(18))                  
                   .Add(ed => ed.CostWithVAT, ac => ac.Order(19))
                   .Add(ed => ed.AppraisalGoal, ac => ac.Order(20))
                   .Add(ed => ed.ReportNumber, ac => ac.Order(21))//.Group("Отчет"))                   
                   .Add(ed => ed.CostVAT, ac => ac.Order(22))
                   .Add(ed => ed.Appraiser, ac => ac.Order(23))
                   .Add(ed => ed.ReportDate, ac => ac.Order(24))                               
                   .Add(ed => ed.AppraisalPurpose, ac => ac.Order(25))
                   .Add(ed => ed.INNOfSubj, ac => ac.Order(26))
                   .Add(ed => ed.АppraiserPerson, ac => ac.Order(27))                   
                   .AddEmpty( ac => ac.Order(28).TabName(CaptionHelper.DefaultTabName).Group("Отчет"))
                   .Add(ed => ed.Deal, ac => ac.Order(29))
                   .AddEmpty(ac => ac.Order(30).TabName(CaptionHelper.DefaultTabName).Group("Отчет"))
                   .Add(ed => ed.AgreedPay, ac => ac.Order(31))
                   
                   .AddEmpty(ac => ac.Order(33).TabName(CaptionHelper.DefaultTabName).Group("Отчет"))
                   .Add(ed => ed.Description, ac => ac.Order(34))
                   .AddEmpty(ac => ac.Order(35).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))
                   .AddEmpty(ac => ac.Order(38).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))
                   .AddEmpty(ac => ac.Order(39).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))
                  
                   .AddEmpty(ac => ac.Order(42).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))
                   .AddEmpty(ac => ac.Order(43).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))
                   .Add(ed => ed.FileCard, ac => ac.Order(41))
                   .AddEmpty(ac => ac.Order(46).TabName(CaptionHelper.DefaultTabName).Group("{4}Проверка отчета"))














                  .AddOneToManyAssociation<EstateAppraisal>("Appraisal_EstateAppraisal",
                          y => y.TabName("[2]Состав объекта оценки")
                          .IsLabelVisible(false)
                          .Create((uofw, entity, id) =>
                          {                              
                              entity.Appraisal = uofw.GetRepository<Appraisal>().Find(id);
                          })
                          .Delete((uofw, entity, id) =>
                          {
                              entity.Appraisal = null;                              
                          })
                    .Filter((uofw, q, id, oid) => q.Where(w => w.AppraisalID == id)))
                    //.AddOneToManyAssociation<NonCoreAssetAppraisal>("Appraisal_NonCoreAssetAppraisal",
                    //      y => y.TabName("[4]Оценки ННА")
                    //      .IsLabelVisible(false)
                    //      .Create((uofw, entity, id) =>
                    //      {
                    //          entity.AppraisalID = id;
                    //      })
                    //.Filter((uofw, q, id, oid) => q.Where(w => w.AppraisalID == id)))
                    .AddManyToManyRigthAssociation<FileCardAndAppraisal>("Appraisal_FileCards", y => y.TabName("[3]Документы"))
              )
             
              );
        }

        /// <summary>
        /// Конфигурация реестра оценок по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Appraisal> ListView_Default(this ViewModelConfigBuilder<Appraisal> conf)
        {
            return
                conf.ListView(x => x
                .Title("Оценки")
                .Columns(
                                 c =>
                                     c
                                     .Add(t => t.ID, h => h.Visible(false))
                                     .Add(t => t.Customer, p => p.Visible(true).Order(1))
                                     .Add(t => t.AppraisalDate, h => h.Visible(true).Order(2))
                                     .Add(t => t.ReportDate, p => p.Visible(true).Order(3))
                                     .Add(t => t.ReportNumber, p => p.Visible(true).Order(4))
                                     .Add(t => t.ShortDescriptionObjects, p => p.Visible(true).Order(5))
                                     .Add(t => t.AppType, p => p.Visible(true).Order(6))
                                     .Add(t => t.AppraisalGoal, p => p.Visible(true).Order(6))
                                     .Add(t => t.AppraisalPurpose, p => p.Visible(true).Order(7))
                                     .Add(t => t.SibRegion, p => p.Visible(true).Order(8))
                                     .Add(t => t.CostWithVAT, p => p.Visible(true).Order(9))
                                     .Add(t => t.CostVAT, p => p.Visible(true).Order(10))
                                     .Add(t => t.ExecutorFullName, p => p.Visible(true).Order(11))
                                     //.Add(t => t.ExecutorString, p => p.Visible(true).Order(12))
                                     .Add(t => t.Appraiser, p => p.Visible(true).Order(13))
                                     .Add(t => t.АppraiserPerson, p => p.Visible(false).Order(14))
                                     .Add(t => t.ApproachCode, h => h.Visible(false))
                                     .Add(t => t.Approach, h => h.Visible(false))                                   
                                     .Add(t => t.Description, p => p.Visible(false))                                    
                                     .Add(t => t.Owner, p => p.Visible(false))
                                    
                                     .Add(t => t.Appraiser, p => p.Visible(false))

                                     .Add(t => t.Executor, p => p.Visible(false))
                                     .Add(t => t.ExecutorDeptName, p => p.Visible(false))
                                     .Add(t => t.ExecutorEmail, p => p.Visible(false))
                                     .Add(t => t.ExecutorFirstName, p => p.Visible(false))
                                     .Add(t => t.ExecutorLastName, p => p.Visible(false))
                                     .Add(t => t.ExecutorMiddleName, p => p.Visible(false))
                                     .Add(t => t.ExecutorMobile, p => p.Visible(false))
                                     .Add(t => t.ExecutorPhone, p => p.Visible(false))

                                     )
               );

        }

        /// <summary>
        /// Конфигурация сводного реестра оценок.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Appraisal> ListView_SummaryAppraisal(this ViewModelConfigBuilder<Appraisal> conf)
        {
            return
                conf.ListView(x => x
                .Title("Сводный реестр оценок")
                .DataSource(ds => ds                
                .Groups(gr => gr
                .Groupable(true)
                .Add(p => p.AppraisalDate)                
                ))
                .Columns(c => c
                    .Clear()
                    .Add(t => t.Executor, p => p.Visible(true).Order(1).ClientGroupHeaderTemplate("Ф.И.О. исполнителя: #= value # (Оценок: #= count#)"))
                    .Add(t => t.Customer, p => p.Visible(true).Order(2))
                    .Add(t => t.AppraisalDate, h => h.Visible(true).Order(3))
                    .Add(t => t.ReportDate, p => p.Visible(true).Order(4))
                    .Add(t => t.ReportNumber, p => p.Visible(true).Order(5))
                    .Add(t => t.AppType, p => p.Visible(true).Order(6))
                    .Add(t => t.AprObject, p => p.Visible(true).Order(7))
                    .Add(t => t.AppraisalGoal, p => p.Visible(true).Order(8))
                    .Add(t => t.AppraisalPurpose, p => p.Visible(true).Order(9))
                    .Add(t => t.SibRegion, p => p.Visible(true).Order(10))
                    .Add(t => t.AppRegion, p => p.Visible(true).Order(10))
                    .Add(t => t.CostWithVAT, p => p.Visible(true).Order(11))
                    .Add(t => t.CostVAT, p => p.Visible(true).Order(12))
                    .Add(t => t.MarketAppraisalCost, p => p.Visible(true).Order(13))
                    .Add(t => t.EstateAppraisalCost, p => p.Visible(true).Order(14))
                    .Add(t => t.Owner, p => p.Visible(true).Order(15))
                    .Add(t => t.ExecutorLastName, p => p.Visible(true).Order(16))
                    .Add(t => t.ExecutorFirstName, p => p.Visible(true).Order(17))
                    .Add(t => t.ExecutorMiddleName, p => p.Visible(true).Order(18))
                    .Add(t => t.ExecutorPhone, p => p.Visible(true).Order(19))
                    .Add(t => t.ExecutorEmail, p => p.Visible(true).Order(20))
                    .Add(t => t.ExecutorMobile, p => p.Visible(true).Order(21))
                    .Add(t => t.ExecutorDeptName, p => p.Visible(true).Order(22))
                    .Add(t => t.Appraiser, p => p.Visible(true).Order(23))
                    .Add(t => t.INNOfSubj, p => p.Visible(true).Order(24))
                    .Add(t => t.АppraiserPerson, p => p.Visible(true).Order(25))
                    .Add(t => t.ReportCheck, p => p.Visible(true).Order(26))
                    .Add(t => t.Description, p => p.Visible(true).Order(27))
                    .Add(t => t.CustomerKindActivity, p => p.Visible(true).Order(28))
                    .Add(t => t.CustomerCountry, p => p.Visible(true).Order(29))
                    .Add(t => t.CustomerEK, p => p.Visible(true).Order(30))
                    .Add(t => t.CustomerRegion, p => p.Visible(true).Order(31))
                    .Add(t => t.IsSocietyKey, p => p.Visible(true).Order(32))
                    .Add(t => t.IsSocietyJoint, p => p.Visible(true).Order(33))
                    .Add(t => t.IsSocietyResident, p => p.Visible(true).Order(34))
                    .Add(t => t.IsSocietyControlled, p => p.Visible(true).Order(35))


                    .Add(t => t.ID, h => h.Visible(false))
                    .Add(t => t.ShortDescriptionObjects, p => p.Visible(false))
                    .Add(t => t.ExecutorFullName, p => p.Visible(false))
                    
                    )
               );

        }
        /// <summary>
        /// Конфигурация сводного реестра оценок по исполнителям.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Appraisal> ListView_SummaryAppraisalExecutor(this ViewModelConfigBuilder<Appraisal> conf)
        {
            return
                conf
                .ListView(x => x
                .Title("Сводный реестр оценок по исполнителям")
                .DataSource(ds => ds                
                .Aggregate(ag=>ag.Add(g=>g.Executor, Base.UI.ViewModal.AggregateType.Count))         
                .Groups(gr => gr
                    .Groupable(true)
                    .Add(p => p.Executor)
                ))
                 .Columns(c => c
                    .Clear()
                    .Add(t => t.Executor, p => p.Visible(true).Order(1).ClientGroupHeaderTemplate("Ф.И.О. исполнителя: #= value # (Оценок: #= count#)"))
                    .Add(t => t.Customer, p => p.Visible(true).Order(2))
                    .Add(t => t.AppraisalDate, h => h.Visible(true).Order(3))
                    .Add(t => t.ReportDate, p => p.Visible(true).Order(4))
                    .Add(t => t.ReportNumber, p => p.Visible(true).Order(5))
                    .Add(t => t.AppType, p => p.Visible(true).Order(6))
                    .Add(t => t.AprObject, p => p.Visible(true).Order(7))
                    .Add(t => t.AppraisalGoal, p => p.Visible(true).Order(8))
                    .Add(t => t.AppraisalPurpose, p => p.Visible(true).Order(9))
                    .Add(t => t.SibRegion, p => p.Visible(true).Order(10))
                    .Add(t => t.CostWithVAT, p => p.Visible(true).Order(11))
                    .Add(t => t.CostVAT, p => p.Visible(true).Order(12))
                    .Add(t => t.MarketAppraisalCost, p => p.Visible(true).Order(13))
                    .Add(t => t.EstateAppraisalCost, p => p.Visible(true).Order(14))
                    .Add(t => t.Owner, p => p.Visible(true).Order(15))
                    .Add(t => t.ExecutorLastName, p => p.Visible(true).Order(16))
                    .Add(t => t.ExecutorFirstName, p => p.Visible(true).Order(17))
                    .Add(t => t.ExecutorMiddleName, p => p.Visible(true).Order(18))
                    .Add(t => t.ExecutorPhone, p => p.Visible(true).Order(19))
                    .Add(t => t.ExecutorEmail, p => p.Visible(true).Order(20))
                    .Add(t => t.ExecutorMobile, p => p.Visible(true).Order(21))
                    .Add(t => t.ExecutorDeptName, p => p.Visible(true).Order(22))
                    .Add(t => t.Appraiser, p => p.Visible(true).Order(23))
                    .Add(t => t.INNOfSubj, p => p.Visible(true).Order(24))
                    .Add(t => t.АppraiserPerson, p => p.Visible(true).Order(25))
                    .Add(t => t.ReportCheck, p => p.Visible(true).Order(26))
                    .Add(t => t.Description, p => p.Visible(true).Order(27))
                    .Add(t => t.CustomerKindActivity, p => p.Visible(true).Order(28))
                    .Add(t => t.CustomerCountry, p => p.Visible(true).Order(29))
                    .Add(t => t.CustomerEK, p => p.Visible(true).Order(30))
                    .Add(t => t.CustomerRegion, p => p.Visible(true).Order(31))
                    .Add(t => t.IsSocietyKey, p => p.Visible(true).Order(32))
                    .Add(t => t.IsSocietyJoint, p => p.Visible(true).Order(33))
                    .Add(t => t.IsSocietyResident, p => p.Visible(true).Order(34))
                    .Add(t => t.IsSocietyControlled, p => p.Visible(true).Order(35))


                    .Add(t => t.ID, h => h.Visible(false))
                    .Add(t => t.ShortDescriptionObjects, p => p.Visible(false))
                    .Add(t => t.ExecutorFullName, p => p.Visible(false))
                   
                    )
               );

        }

    }
}
