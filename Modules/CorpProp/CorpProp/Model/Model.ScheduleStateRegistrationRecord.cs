using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Security;
using CorpProp.Extentions;

namespace CorpProp.Model.Law
{
    public static class ScheduleStateRegistrationRecordModel
    {
        /// <summary>
        /// Создает конфигурацию модели строки ГГР по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<ScheduleStateRegistrationRecord>()
              .Service<IScheduleStateRegistrationRecordService>()
              .IsNotify()
              .Title("Строка графика государственной регистрации права")              
              .DetailView_Default()
              .ListView_Default()
              .LookupProperty(x => x.Text(t => t.ID));

           
            context.CreateVmConfigOnBase<ScheduleStateRegistrationRecord>(nameof(ScheduleStateRegistrationRecord), "ScheduleStateRegistrationRecordPivot")
                .ListView(x=>x.Type(ListViewType.Pivot));

            context.CreateVmConfigOnBase<ScheduleStateRegistrationRecord>(nameof(ScheduleStateRegistrationRecord), "GGR")
                .ListView(x => x
                .Type(ListViewType.Pivot)
                .DataSource(ds => ds.Filter(f => f.ScheduleStateRegistration != null))
               );

            context.CreateVmConfigOnBase<ScheduleStateRegistrationRecord>(nameof(ScheduleStateRegistrationRecord), "SSRCheckRight")
             .ListView(x => x                
                .DataSource(ds => ds
                .Filter(f => f.NumberEGRP != null && f.NumberEGRP != "" && 
                ( (f.RightAfter != null && f.RightAfter.RegNumber != f.NumberEGRP) || f.RightAfter == null)))
            );

        }

        /// <summary>
        /// Конфигурация карточки строки ГГР по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ScheduleStateRegistrationRecord> DetailView_Default(this ViewModelConfigBuilder<ScheduleStateRegistrationRecord> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(ed => ed
                      .Add(e=>e.AccountingObject , ac=>ac.OnChangeClientScript(" corpProp.dv.editors.onChange.SSRR_AccountingObject(form, isChange);"))
                      .Add(e => e.RegistrationBasis, ac => ac.OnChangeClientScript(" corpProp.dv.editors.onChange.SSRR_RegistrationBasis(form, isChange);"))
                      .AddManyToManyRigthAssociation<FileCardAndScheduleStateRegistrationRecord>("FileCardAndScheduleStateRegistrationRecord", edt => edt.TabName("Документы"))
                  )
                  .DefaultSettings((uow, r, commonEditorViewModel) =>
                  {
                      commonEditorViewModel.ReadOnly(ro => ro.ScheduleStateRegistration);

                      if (r.ScheduleStateRegistration != null && r.ScheduleStateRegistration.ScheduleStateRegistrationStatus != null
                      && (r.ScheduleStateRegistration.ScheduleStateRegistrationStatus.Code == "102" ||
                      r.ScheduleStateRegistration.ScheduleStateRegistrationStatus.Code == "106"))
                      {
                          commonEditorViewModel.SetReadOnlyAll();
                          commonEditorViewModel.ReadOnly(ro => r.Description, false);

                                  commonEditorViewModel.ReadOnly(ro => ro.DateActualFilingDocument, false);
                                  commonEditorViewModel.ReadOnly(ro => ro.DateActualRegistration, false);
                                  commonEditorViewModel.ReadOnly(ro => ro.RightAfter, false);
                                  commonEditorViewModel.ReadOnly(ro => ro.NumberEGRP, false);
                                  commonEditorViewModel.ReadOnly(ro => ro.DateRegDoc, false);
                      }

                      //if (r.AccountingObject != null)
                      //{
                      //    commonEditorViewModel.ReadOnly(ro => ro.InventoryNumber);
                      //}

                      if (r.DateActualFilingDocument != null && r.DateActualRegistration != null)
                          commonEditorViewModel.ReadOnly(ro => ro.DateActualRegistration).ReadOnly(ro => ro.DateActualFilingDocument);
                  })
             );
        }

        /// <summary>
        /// Конфигурация реестра строк ГГР по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ScheduleStateRegistrationRecord> ListView_Default(this ViewModelConfigBuilder<ScheduleStateRegistrationRecord> conf)
        {
            return
                conf.ListView(x => x
                .Title("Строка графика государственной регистрации права")
                .IsMultiSelect(true)
               );

        }

    }
}
